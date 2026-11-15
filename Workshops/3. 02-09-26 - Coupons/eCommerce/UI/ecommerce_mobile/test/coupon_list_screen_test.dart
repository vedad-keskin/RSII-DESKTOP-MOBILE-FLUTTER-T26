import 'dart:async';

import 'package:ecommerce_mobile/models/coupon.dart';
import 'package:ecommerce_mobile/models/search_result.dart';
import 'package:ecommerce_mobile/providers/coupon_provider.dart';
import 'package:ecommerce_mobile/screens/coupon_list_screen.dart';
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:provider/provider.dart';

class TestCouponProvider extends CouponProvider {
  final codes = <String>[];
  final pending = <Completer<SearchResult<Coupon>>>[];

  @override
  Future<SearchResult<Coupon>> get({dynamic filter}) {
    codes.add(filter['code'] as String);
    final request = Completer<SearchResult<Coupon>>();
    pending.add(request);
    return request.future;
  }
}

void main() {
  testWidgets(
    'coupon table filters immediately, ignores stale results, and handles errors',
    (tester) async {
      tester.view.physicalSize = const Size(400, 800);
      tester.view.devicePixelRatio = 1;
      addTearDown(tester.view.resetPhysicalSize);
      addTearDown(tester.view.resetDevicePixelRatio);

      final provider = TestCouponProvider();
      expect(provider.endpoint, 'Coupon');
      final welcome = provider.fromJson({
        'id': 1,
        'code': 'WELCOME',
        'description': 'Welcome discount',
        'discountPercentage': 20,
        'validFrom': '2026-01-01T00:00:00',
        'validTo': '2027-01-01T00:00:00',
        'maxUses': 3,
        'usageCount': 0,
        'isActive': true,
        'isCouponValid': true,
      });
      final used = Coupon.fromJson({
        ...welcome.toJson(),
        'id': 2,
        'code': 'USED',
        'discountPercentage': 12.5,
        'usageCount': 3,
        'isCouponValid': false,
      });
      SearchResult<Coupon> results(List<Coupon> coupons) =>
          SearchResult<Coupon>()..items = coupons;

      await tester.pumpWidget(
        ChangeNotifierProvider<CouponProvider>.value(
          value: provider,
          child: const MaterialApp(home: CouponListScreen()),
        ),
      );
      expect(provider.codes, ['']);
      provider.pending[0].complete(results([welcome, used]));
      await tester.pumpAndSettle();
      expect(find.text('WELCOME'), findsOneWidget);
      expect(find.text('USED'), findsOneWidget);
      expect(find.text('2026-01-01'), findsNWidgets(2));
      expect(find.text('0/3'), findsOneWidget);
      expect(find.text('3/3'), findsOneWidget);
      expect(find.text('Trenutno važećih kupona: 1'), findsOneWidget);
      expect(tester.takeException(), isNull);

      await tester.drag(
        find.byWidgetPredicate(
          (widget) =>
              widget is SingleChildScrollView &&
              widget.scrollDirection == Axis.horizontal,
        ),
        const Offset(-1500, 0),
      );
      await tester.pumpAndSettle();
      expect(find.text('Da').hitTestable(), findsOneWidget);
      expect(find.text('Ne').hitTestable(), findsOneWidget);

      await tester.enterText(find.byType(TextField), 'W');
      await tester.pump();
      await tester.enterText(find.byType(TextField), 'WE');
      await tester.pump();
      expect(provider.codes, ['', 'W', 'WE']);
      provider.pending[2].complete(results([welcome]));
      await tester.pumpAndSettle();
      provider.pending[1].complete(results([used]));
      await tester.pumpAndSettle();
      expect(find.text('WELCOME'), findsOneWidget);
      expect(find.text('USED'), findsNothing);

      await tester.enterText(find.byType(TextField), 'ZZZ');
      await tester.pump();
      provider.pending[3].complete(results([]));
      await tester.pumpAndSettle();
      expect(tester.widget<DataTable>(find.byType(DataTable)).rows, isEmpty);
      expect(find.text('Trenutno važećih kupona: 0'), findsOneWidget);

      await tester.enterText(find.byType(TextField), 'ERROR');
      await tester.pump();
      provider.pending[4].completeError(Exception('Request failed'));
      await tester.pumpAndSettle();
      expect(find.text('Exception: Request failed'), findsOneWidget);
      expect(find.byType(CircularProgressIndicator), findsNothing);
      await tester.tap(find.text('OK'));
      await tester.pumpAndSettle();

      await tester.enterText(find.byType(TextField), 'PENDING');
      await tester.pump();
      await tester.pumpWidget(const SizedBox());
      provider.pending[5].complete(results([welcome]));
      await tester.pumpAndSettle();
      expect(tester.takeException(), isNull);
    },
  );
}
