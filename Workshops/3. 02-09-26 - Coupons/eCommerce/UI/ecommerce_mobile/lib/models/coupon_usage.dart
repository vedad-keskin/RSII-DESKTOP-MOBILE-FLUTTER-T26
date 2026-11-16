import 'package:json_annotation/json_annotation.dart';

part 'coupon_usage.g.dart';

@JsonSerializable()
class CouponUsage {
  final int id;
  final int couponId;
  final int userId;
  final DateTime usedAt;
  final double discountAmount;
  final double? originalAmount;
  final double? finalAmount;

  CouponUsage({
    required this.id,
    required this.couponId,
    required this.userId,
    required this.usedAt,
    required this.discountAmount,
    this.originalAmount,
    this.finalAmount,
  });

  factory CouponUsage.fromJson(Map<String, dynamic> json) =>
      _$CouponUsageFromJson(json);

  Map<String, dynamic> toJson() => _$CouponUsageToJson(this);
}
