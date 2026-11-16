// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'coupon.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Coupon _$CouponFromJson(Map<String, dynamic> json) => Coupon(
  id: (json['id'] as num).toInt(),
  code: json['code'] as String,
  description: json['description'] as String,
  discountPercentage: (json['discountPercentage'] as num).toDouble(),
  validFrom: DateTime.parse(json['validFrom'] as String),
  validTo: DateTime.parse(json['validTo'] as String),
  maxUses: (json['maxUses'] as num).toInt(),
  usageCount: (json['usageCount'] as num).toInt(),
  isActive: json['isActive'] as bool,
  isCouponValid: json['isCouponValid'] as bool,
);

Map<String, dynamic> _$CouponToJson(Coupon instance) => <String, dynamic>{
  'id': instance.id,
  'code': instance.code,
  'description': instance.description,
  'discountPercentage': instance.discountPercentage,
  'validFrom': instance.validFrom.toIso8601String(),
  'validTo': instance.validTo.toIso8601String(),
  'maxUses': instance.maxUses,
  'usageCount': instance.usageCount,
  'isActive': instance.isActive,
  'isCouponValid': instance.isCouponValid,
};
