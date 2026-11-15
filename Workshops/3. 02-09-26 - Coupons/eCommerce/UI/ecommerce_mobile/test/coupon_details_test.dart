import 'package:ecommerce_mobile/models/coupon.dart';
import 'package:ecommerce_mobile/providers/coupon_provider.dart';
import 'package:ecommerce_mobile/screens/coupon_details_screen.dart';
import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:provider/provider.dart';

class RecordingCouponProvider extends CouponProvider {
  Map<String, dynamic>? request;
  int? updatedId;

  @override
  Future<Coupon> insert(dynamic request) async {
    this.request = Map<String, dynamic>.of(request);
    return Coupon.fromJson({
      ...this.request!,
      'id': request['id'] ?? 7,
      'usageCount': 0,
      'isCouponValid': true,
    });
  }

  @override
  Future<Coupon> update(int id, [dynamic request]) {
    updatedId = id;
    return insert(request);
  }
}

void main() {
  testWidgets('coupon form validates and sends add/edit values', (
    tester,
  ) async {
    tester.view.physicalSize = const Size(400, 800);
    tester.view.devicePixelRatio = 1;
    addTearDown(tester.view.resetPhysicalSize);
    addTearDown(tester.view.resetDevicePixelRatio);

    final existing = Coupon(
      id: 7,
      code: 'SUMMER',
      description: 'Summer discount',
      discountPercentage: 12.5,
      validFrom: DateTime(2026, 6, 1),
      validTo: DateTime(2026, 9, 1),
      maxUses: 20,
      usageCount: 0,
      isActive: false,
      isCouponValid: false,
    );
    for (final coupon in [null, existing]) {
      final provider = RecordingCouponProvider();
      final navigator = GlobalKey<NavigatorState>();
      await tester.pumpWidget(
        ChangeNotifierProvider<CouponProvider>.value(
          value: provider,
          child: MaterialApp(navigatorKey: navigator, home: const Scaffold()),
        ),
      );
      final returned = navigator.currentState!.push<String>(
        MaterialPageRoute(builder: (_) => CouponDetailsScreen(coupon: coupon)),
      );
      await tester.pumpAndSettle();
      final form = tester.state<FormBuilderState>(find.byType(FormBuilder));
      expect(form.fields['code']!.value, coupon?.code ?? '');
      expect(
        form.fields['discountPercentage']!.value,
        coupon?.discountPercentage.toString() ?? '',
      );
      expect(form.fields['validFrom']!.value, coupon?.validFrom);
      expect(form.fields['isActive']!.value, coupon?.isActive ?? true);

      form.patchValue({'discountPercentage': 'invalid'});
      await tester.ensureVisible(find.text('Save'));
      await tester.tap(find.text('Save'));
      await tester.pumpAndSettle();
      expect(provider.request, isNull);

      form.patchValue({
        'code': 'WINTER',
        'description': 'Updated discount',
        'discountPercentage': '25.5',
        'maxUses': '30',
        'validFrom': DateTime(2026, 11, 1),
        'validTo': DateTime(2026, 12, 1),
      });
      await tester.pumpAndSettle();
      await tester.ensureVisible(find.text('Save'));
      await tester.tap(find.text('Save'));
      await tester.pumpAndSettle();

      expect(await returned, 'reload');
      expect(provider.updatedId, coupon?.id);
      expect(provider.request!['discountPercentage'], 25.5);
      expect(provider.request!['maxUses'], 30);
      expect(provider.request!['validFrom'], '2026-11-01T00:00:00.000');
      expect(provider.request!['validTo'], '2026-12-01T00:00:00.000');
      expect(provider.request!['isActive'], coupon?.isActive ?? true);
      expect(provider.request!.containsKey('id'), coupon != null);
      expect(provider.request!['id'], coupon?.id);
      expect(provider.request!.containsKey('usageCount'), false);
      expect(provider.request!.containsKey('isCouponValid'), false);
      expect(tester.takeException(), isNull);
    }
  });
}
