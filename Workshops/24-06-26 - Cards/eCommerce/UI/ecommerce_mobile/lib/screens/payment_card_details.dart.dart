import 'dart:convert';
import 'dart:ui';
import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:ecommerce_mobile/models/payment_card_ib180079.dart';
import 'package:ecommerce_mobile/providers/auth_provider.dart';
import 'package:ecommerce_mobile/providers/payment_card_ib180079_provider.dart';
import 'package:ecommerce_mobile/utils/utils_widgets.dart';
import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:provider/provider.dart';

import '../models/asset.dart';

class PaymentCardDetailsScreen extends StatefulWidget {
  final PaymentCardIB180079? paymentCard;

  const PaymentCardDetailsScreen({super.key, this.paymentCard});

  @override
  State<PaymentCardDetailsScreen> createState() =>
      _PaymentCardDetailsScreenState();
}

class _PaymentCardDetailsScreenState extends State<PaymentCardDetailsScreen> {
  final _formKey = GlobalKey<FormBuilderState>();
  Map<String, dynamic> _initalValue = {};

  late PaymentCardIB180079Provider _paymentCardProvider;

  @override
  void initState() {
    // TODO: implement initState
    super.initState();

    _initalValue = {
      'cardNumber': widget.paymentCard?.cardNumber,
      'cvc': widget.paymentCard?.cvc,
      'initialBalance': widget.paymentCard?.initialBalance.toString(),
      'exiprationDate': widget.paymentCard?.exiprationDate,
    };

    _paymentCardProvider = context.read<PaymentCardIB180079Provider>();
  }

  @override
  Widget build(BuildContext context) {
    return MasterScreen(
      title: widget.paymentCard == null
          ? "New Payment Card"
          : "Update Payment Card",
      child: Center(
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(children: [_buildForm(), _saveButton(context)]),
        ),
      ),
    );
  }

  Padding _saveButton(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(32.0),
      child: ElevatedButton(
        onPressed: () async {
          _formKey.currentState?.save();

          try {
            if (_formKey.currentState!.validate()) {
              var userId =
                  int.tryParse(AuthProvider.accessTokenDecoded?['Id'] ?? '0') ??
                  0;

              if (widget.paymentCard != null) {
                // edit existing payment card

                Map<String, dynamic> request = Map.of(
                  _formKey.currentState!.value,
                );

                request['id'] = widget.paymentCard?.id;

                var initialBalance = double.parse(
                  _formKey.currentState!.value['initialBalance'],
                );

                request['initialBalance'] = initialBalance;
                request['userId'] = userId;

                var date = request['exiprationDate'] as DateTime?;

                request['exiprationDate'] = date?.toIso8601String();

                print(request);

                await _paymentCardProvider.update(
                  widget.paymentCard!.id!,
                  request,
                );

                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text("Payment card successfully modified")),
                );

                Navigator.pop(context, 'reload');
              } else {
                // create new payment card

                Map<String, dynamic> request = Map.of(
                  _formKey.currentState!.value,
                );

                var initialBalance = double.parse(
                  _formKey.currentState!.value['initialBalance'],
                );

                request['initialBalance'] = initialBalance;
                request['userId'] = userId;

                var date = request['exiprationDate'] as DateTime?;
                request['exiprationDate'] = date?.toIso8601String();

                print(request);

                await _paymentCardProvider.insert(request);

                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text("Payment card successfully added")),
                );

                Navigator.pop(context, 'reload');
              }
            }
          } on Exception catch (e) {
            alertBox(context, "Error", e.toString());
          }
        },
        child: Text("Save", style: const TextStyle(fontSize: 15)),
      ),
    );
  }

  Widget _buildForm() {
    return FormBuilder(
      key: _formKey,
      initialValue: _initalValue,
      child: Column(
        children: [
          Row(
            children: [
              Expanded(
                child: FormBuilderTextField(
                  name: 'cardNumber',
                  validator: (value) {
                    if (value == null) {
                      return mField;
                    } else if (!RegExp(r"^\d{12}$").hasMatch(value)) {
                      return "Broj kartice mora imati tačno 12 cifara";
                    } else {
                      return null;
                    }
                  },
                  decoration: InputDecoration(label: Text("Card Number")),
                ),
              ),
              const SizedBox(width: 20),

              Expanded(
                child: FormBuilderDateTimePicker(
                  name: 'exiprationDate',
                  inputType: InputType.date,
                  initialDate:
                      widget.paymentCard?.exiprationDate ?? DateTime.now(),
                  decoration: InputDecoration(label: Text("Expiration Date")),
                  firstDate: DateTime.now(),
                  validator: (value) {
                    if (value == null) {
                      return mField;
                    }
                    }
                ),
              ),
            ],
          ),
          Row(
            children: [
              Expanded(
                child: FormBuilderTextField(
                  name: 'cvc',
                  validator: (value) {
                    if (value == null) {
                      return mField;
                    } else if (!RegExp(r"^\d{3}$").hasMatch(value)) {
                      return "CVC mora imati tačno 3 cifre";
                    } else {
                      return null;
                    }
                  },
                  decoration: InputDecoration(label: Text("CVC")),
                ),
              ),

              const SizedBox(width: 20),
              Expanded(
                child: FormBuilderTextField(
                  name: 'initialBalance',
                  validator: (value) {
                    if (value == null) {
                      return mField;
                    } else if (double.tryParse(value) == null) {
                      return numericField;
                    } else if (double.tryParse(value)! < 0) {
                      return "Stanje mora biti veće ili jednako 0";
                    } else {
                      return null;
                    }
                  },
                  decoration: InputDecoration(label: Text("Initial Balance")),
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }
}
