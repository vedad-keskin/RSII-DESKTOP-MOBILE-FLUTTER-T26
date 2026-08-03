
import 'package:json_annotation/json_annotation.dart';

part 'payment_card_ib180079.g.dart';


@JsonSerializable()
class PaymentCardIB180079 {
  final int? id;
  final int? userId;
  final String? cardNumber;
  final String? cvc;
  final double? initialBalance;
  final DateTime? exiprationDate;

PaymentCardIB180079({
  this.id,
  this.userId,
  this.cardNumber,
  this.cvc,
  this.exiprationDate,
  this.initialBalance
});


factory PaymentCardIB180079.fromJson(Map<String,dynamic> json) => _$PaymentCardIB180079FromJson(json);

Map<String, dynamic> toJson() => _$PaymentCardIB180079ToJson(this);

}