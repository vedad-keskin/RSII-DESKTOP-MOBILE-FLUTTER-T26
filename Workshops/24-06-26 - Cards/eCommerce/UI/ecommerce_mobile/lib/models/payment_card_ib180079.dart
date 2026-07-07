import 'package:json_annotation/json_annotation.dart';

part 'payment_card_ib180079.g.dart';

@JsonSerializable()
class PaymentCardIB180079 {
  final int id;
  final int userId;
  final String cardNumber;
  final String cvc;
  final DateTime exiprationDate;
  final double initialBalance;

  PaymentCardIB180079({
    required this.id,
    required this.userId,
    required this.cardNumber,
    required this.cvc,
    required this.exiprationDate,
    required this.initialBalance,
  });

  factory PaymentCardIB180079.fromJson(Map<String, dynamic> json) =>
      _$PaymentCardIB180079FromJson(json);

  Map<String, dynamic> toJson() => _$PaymentCardIB180079ToJson(this);
}
