// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'payment_card_ib180079.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

PaymentCardIB180079 _$PaymentCardIB180079FromJson(Map<String, dynamic> json) =>
    PaymentCardIB180079(
      id: (json['id'] as num?)?.toInt(),
      userId: (json['userId'] as num?)?.toInt(),
      cardNumber: json['cardNumber'] as String?,
      cvc: json['cvc'] as String?,
      exiprationDate: json['exiprationDate'] == null
          ? null
          : DateTime.parse(json['exiprationDate'] as String),
      initialBalance: (json['initialBalance'] as num?)?.toDouble(),
    );

Map<String, dynamic> _$PaymentCardIB180079ToJson(
  PaymentCardIB180079 instance,
) => <String, dynamic>{
  'id': instance.id,
  'userId': instance.userId,
  'cardNumber': instance.cardNumber,
  'cvc': instance.cvc,
  'initialBalance': instance.initialBalance,
  'exiprationDate': instance.exiprationDate?.toIso8601String(),
};
