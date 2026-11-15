import 'package:json_annotation/json_annotation.dart';

part 'coupon.g.dart';

@JsonSerializable()
class Coupon {
  final int id;
  final String code;
  final String description;
  final double discountPercentage;
  final DateTime validFrom;
  final DateTime validTo;
  final int maxUses;
  final int usageCount;
  final bool isActive;
  final bool isCouponValid;


  Coupon({
    required this.id,
    required this.code,
    required this.description,
    required this.discountPercentage,
    required this.validFrom,
    required this.validTo,
    required this.maxUses,
    required this.usageCount,
    required this.isActive,
    required this.isCouponValid,
  });

  factory Coupon.fromJson(Map<String, dynamic> json) =>
      _$CouponFromJson(json);

  Map<String, dynamic> toJson() => _$CouponToJson(this);


}
