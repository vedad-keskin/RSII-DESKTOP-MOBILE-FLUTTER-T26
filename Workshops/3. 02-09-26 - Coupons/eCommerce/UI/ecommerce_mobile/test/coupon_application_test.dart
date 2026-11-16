import 'package:ecommerce_mobile/models/cart.dart';
import 'package:ecommerce_mobile/models/product.dart';
import 'package:ecommerce_mobile/providers/cart_provider.dart';
import 'package:ecommerce_mobile/providers/coupon_provider.dart';
import 'package:ecommerce_mobile/screens/cart_list_screen.dart';
import 'package:ecommerce_mobile/utils/api_client_exception.dart';
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:provider/provider.dart';

class RecordingCouponProvider extends CouponProvider {
  String? code;
  List<Map<String, dynamic>>? items;
  bool reject = false;

  @override
  Future<Map<String, dynamic>> apply(
    String code,
    List<Map<String, dynamic>> items,
  ) async {
    this.code = code;
    this.items = items;
    if (reject) throw ApiClientException('You have already used this coupon.');
    return {'originalAmount': 100, 'discountAmount': 20, 'finalAmount': 80};
  }
}

void main() {
  testWidgets(
    'apply displays amounts, reports rejection and resets on cart change',
    (tester) async {
      tester.view.physicalSize = const Size(400, 800);
      tester.view.devicePixelRatio = 1;
      addTearDown(tester.view.resetPhysicalSize);
      addTearDown(tester.view.resetDevicePixelRatio);
      final product = Product(id: 42, name: 'Test product', price: 50);
      final cart = CartProvider();
      cart.cart.items.add(
        CartItem()
          ..product = product
          ..quantity = 2,
      );
      final coupons = RecordingCouponProvider();

      await tester.pumpWidget(
        MultiProvider(
          providers: [
            ChangeNotifierProvider<CartProvider>.value(value: cart),
            ChangeNotifierProvider<CouponProvider>.value(value: coupons),
          ],
          child: const MaterialApp(home: Scaffold(body: CartListScreen())),
        ),
      );
      await tester.enterText(find.byType(TextField), ' SALE ');
      await tester.tap(find.text('Primijeni'));
      await tester.pumpAndSettle();
      expect(coupons.code, 'SALE');
      expect(coupons.items, [
        {'productId': 42, 'quantity': 2},
      ]);
      expect(find.text('\$100.00'), findsOneWidget);
      expect(find.text('Discount amount: \$20.00'), findsOneWidget);
      expect(find.text('Final amount: \$80.00'), findsOneWidget);
      expect(cart.cart.items.single.quantity, 2);

      coupons.reject = true;
      await tester.tap(find.text('Primijeni'));
      await tester.pumpAndSettle();
      expect(find.text('You have already used this coupon.'), findsOneWidget);
      expect(find.text('Final amount: \$80.00'), findsNothing);
      await tester.tap(find.text('OK'));
      await tester.pumpAndSettle();

      coupons.reject = false;
      await tester.tap(find.text('Primijeni'));
      await tester.pumpAndSettle();
      cart.addToCart(product);
      await tester.pumpAndSettle();
      expect(find.text('Final amount: \$80.00'), findsNothing);
      expect(find.text('\$150.00'), findsOneWidget);
    },
  );
}
