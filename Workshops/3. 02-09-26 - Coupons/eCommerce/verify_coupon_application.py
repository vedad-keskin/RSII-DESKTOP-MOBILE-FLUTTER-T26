"""Run with a local Development API: python verify_coupon_application.py http://localhost:5127"""
import base64
import datetime as dt
from decimal import Decimal, ROUND_HALF_EVEN
import json
from pathlib import Path
import subprocess
import sys
import uuid
from urllib.error import HTTPError
from urllib.request import Request, urlopen

BASE = (sys.argv[1] if len(sys.argv) > 1 else 'http://localhost:5126').rstrip('/')
APPLY_ROUTE = '/Coupon/Apply'
ROOT = Path(__file__).resolve().parent
CONNECTION = json.loads((ROOT / 'eCommerce.WebAPI/appsettings.Development.json').read_text())['ConnectionStrings']['DefaultConnection']


def sql(query):
    # Windows authentication through the SQL client already included in PowerShell.
    script = '''
$data = [Console]::In.ReadToEnd() | ConvertFrom-Json
$connection = [System.Data.SqlClient.SqlConnection]::new($data.connection)
try {
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = $data.query
    $result = $command.ExecuteScalar()
    ConvertTo-Json -InputObject $result -Compress
} finally { $connection.Dispose() }
'''
    result = subprocess.run(['powershell', '-NoProfile', '-Command', script],
        input=json.dumps({'connection': CONNECTION, 'query': query}),
        text=True, capture_output=True, check=True,
        creationflags=subprocess.CREATE_NO_WINDOW)
    if result.stderr:
        raise RuntimeError(result.stderr)
    return json.loads(result.stdout)


def api(method, path, body=None, token=None):
    headers = {'Content-Type': 'application/json'}
    if token:
        headers['Authorization'] = 'Bearer ' + token
    request = Request(BASE + path, method=method, headers=headers,
        data=None if body is None else json.dumps(body).encode())
    try:
        response = urlopen(request, timeout=20)
    except HTTPError as error:
        response = error
    raw = response.read()
    return response.status, json.loads(raw, parse_float=Decimal) if raw else None


def login(username):
    status, data = api('POST', '/Access/Login', {'username': username, 'password': 'Test123'})
    assert status == 200, (status, data)
    token = data['accesstoken']
    payload = token.split('.')[1]
    claims = json.loads(base64.urlsafe_b64decode(payload + '=' * (-len(payload) % 4)))
    return token, int(claims['Id'])


created = []
prefix = 'RECORDING' + ''.join(chr(65 + int(c, 16)) for c in uuid.uuid4().hex)
now = dt.datetime.now(dt.timezone.utc)


def coupon(suffix, **changes):
    body = dict(code=prefix + suffix, description='Temporary coupon application check',
        discountPercentage=12.5, validFrom=(now - dt.timedelta(days=1)).isoformat(),
        validTo=(now + dt.timedelta(days=1)).isoformat(), maxUses=5, isActive=True)
    body.update(changes)
    status, result = api('POST', '/Coupon', body)
    assert status == 200, (status, result)
    created.append(result['id'])
    return body['code'], result['id']


def snapshot():
    return sql('SELECT (SELECT COUNT(*) FROM Orders) AS Orders, '
        '(SELECT COUNT(*) FROM OrderItems) AS OrderItems, '
        '(SELECT SUM(CAST(StockQuantity AS bigint)) FROM Products) AS Stock '
        'FOR JSON PATH, WITHOUT_ARRAY_WRAPPER')


try:
    token1, user1 = login('customer1')
    token2, user2 = login('customer2')
    status, products = api('GET', '/Products?PageSize=1', token=token1)
    assert status == 200 and products['items'], (status, products)
    product = products['items'][0]
    items = [{'productId': product['id'], 'quantity': 2}]
    original = Decimal(str(product['price'])) * 2
    expected_discount = (original * Decimal('0.125')).quantize(Decimal('.01'), rounding=ROUND_HALF_EVEN)
    before = snapshot()
    code, coupon_id = coupon('VALID')
    request = {'code': code, 'items': items, 'userId': user2, 'originalAmount': 0.01}

    status, _ = api('POST', APPLY_ROUTE, request)
    assert status == 401, status
    for invalid in [{'code': ''}, {'items': []}, {'items': None},
                    {'items': [{'productId': product['id'], 'quantity': 0}]},
                    {'items': [{'productId': product['id'], 'quantity': -1}]},
                    {'items': [{'productId': 2147483647, 'quantity': 1}]},
                    {'code': prefix + 'MISSING'}]:
        status, error = api('POST', APPLY_ROUTE, {**request, **invalid}, token1)
        assert status == 400, (invalid, status, error)

    for suffix, changes in [
        ('INACTIVE', {'isActive': False}),
        ('EXPIRED', {'validFrom': (now - dt.timedelta(days=3)).isoformat(), 'validTo': (now - dt.timedelta(days=1)).isoformat()}),
        ('FUTURE', {'validFrom': (now + dt.timedelta(days=1)).isoformat(), 'validTo': (now + dt.timedelta(days=2)).isoformat()}),
    ]:
        invalid_code, _ = coupon(suffix, **changes)
        status, error = api('POST', APPLY_ROUTE, {**request, 'code': invalid_code}, token1)
        assert status == 400, (status, error)

    assert sql(f'SELECT COUNT(*) FROM CouponUsages WHERE CouponId IN ({",".join(map(str, created))})') == 0
    status, applied = api('POST', APPLY_ROUTE, request, token1)
    assert status == 200, (status, applied)
    assert Decimal(str(applied['originalAmount'])) == original
    assert Decimal(str(applied['discountAmount'])) == expected_discount
    assert Decimal(str(applied['finalAmount'])) == original - expected_discount
    assert sql(f'SELECT COUNT(*) FROM CouponUsages WHERE CouponId={coupon_id} AND UserId={user1} AND DiscountAmount={expected_discount}') == 1
    status, error = api('POST', APPLY_ROUTE, request, token1)
    assert status == 400 and 'already used' in error['message'], (status, error)
    status, result = api('POST', APPLY_ROUTE, request, token2)
    assert status == 200, (status, result)
    assert sql(f'SELECT COUNT(*) FROM CouponUsages WHERE CouponId={coupon_id}') == 2

    limited_code, limited_id = coupon('LIMIT', maxUses=1)
    limited_request = {**request, 'code': limited_code}
    assert api('POST', APPLY_ROUTE, limited_request, token1)[0] == 200
    status, error = api('POST', APPLY_ROUTE, limited_request, token2)
    assert status == 400 and 'maximum' in error['message'], (status, error)
    assert sql(f'SELECT COUNT(*) FROM CouponUsages WHERE CouponId={limited_id}') == 1
    assert api('DELETE', f'/Coupon/{coupon_id}', token=token1)[0] == 400

    if APPLY_ROUTE == '/CouponUsage':
        usage_id = applied['id']
        status, page = api('GET', f'/CouponUsage?CouponId={coupon_id}', token=token1)
        assert status == 200 and len(page['items']) == 1, (status, page)
        assert all(row['userId'] == user1 for row in page['items'])
        assert api('GET', f'/CouponUsage/{usage_id}', token=token2)[0] == 404
        assert api('PUT', f'/CouponUsage/{usage_id}', {'id': usage_id}, token1)[0] == 400
        assert api('DELETE', f'/CouponUsage/{usage_id}', token=token1)[0] == 400

    assert snapshot() == before, 'Applying a coupon changed orders, order items or stock.'
    print('PASS: amounts, recorded user/discount, all coupon rules, rejected inputs, and unchanged checkout data.')
finally:
    if created:
        ids = ','.join(map(str, created))
        sql(f'DELETE FROM CouponUsages WHERE CouponId IN ({ids}); DELETE FROM Coupons WHERE Id IN ({ids}); SELECT 1;')
        print('Temporary test coupons and usages removed.')
