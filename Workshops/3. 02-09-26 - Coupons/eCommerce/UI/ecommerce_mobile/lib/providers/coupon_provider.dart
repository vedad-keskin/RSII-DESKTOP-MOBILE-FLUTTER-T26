import 'package:ecommerce_mobile/models/coupon.dart';
import 'package:ecommerce_mobile/providers/base_provider.dart';

class CouponProvider extends BaseProvider<Coupon> {
  CouponProvider() : super('Coupon');

  @override
  Coupon fromJson(data) {
    return Coupon.fromJson(data);
  }
}
