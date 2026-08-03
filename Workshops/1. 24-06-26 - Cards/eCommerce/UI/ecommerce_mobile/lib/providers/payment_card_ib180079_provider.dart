import 'package:ecommerce_mobile/models/payment_card_ib180079.dart';
import 'package:ecommerce_mobile/providers/base_provider.dart';

class PaymentCardIB180079Provider extends BaseProvider<PaymentCardIB180079> {
  PaymentCardIB180079Provider() : super("PaymentCardIB180079");

  @override
  PaymentCardIB180079 fromJson(data) {
    return PaymentCardIB180079.fromJson(data);
  }
}
