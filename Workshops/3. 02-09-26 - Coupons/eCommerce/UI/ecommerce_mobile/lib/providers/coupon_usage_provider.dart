import 'package:ecommerce_mobile/models/coupon_usage.dart';
import 'package:ecommerce_mobile/providers/base_provider.dart';

class CouponUsageProvider extends BaseProvider<CouponUsage> {
  CouponUsageProvider() : super('CouponUsage');

  @override
  CouponUsage fromJson(data) => CouponUsage.fromJson(data);
}
