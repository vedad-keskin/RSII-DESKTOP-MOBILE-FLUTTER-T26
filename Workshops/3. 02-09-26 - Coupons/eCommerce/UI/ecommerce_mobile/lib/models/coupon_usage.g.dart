// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'coupon_usage.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

CouponUsage _$CouponUsageFromJson(Map<String, dynamic> json) => CouponUsage(
  id: (json['id'] as num).toInt(),
  couponId: (json['couponId'] as num).toInt(),
  userId: (json['userId'] as num).toInt(),
  usedAt: DateTime.parse(json['usedAt'] as String),
  discountAmount: (json['discountAmount'] as num).toDouble(),
  originalAmount: (json['originalAmount'] as num?)?.toDouble(),
  finalAmount: (json['finalAmount'] as num?)?.toDouble(),
);

Map<String, dynamic> _$CouponUsageToJson(CouponUsage instance) =>
    <String, dynamic>{
      'id': instance.id,
      'couponId': instance.couponId,
      'userId': instance.userId,
      'usedAt': instance.usedAt.toIso8601String(),
      'discountAmount': instance.discountAmount,
      'originalAmount': instance.originalAmount,
      'finalAmount': instance.finalAmount,
    };
