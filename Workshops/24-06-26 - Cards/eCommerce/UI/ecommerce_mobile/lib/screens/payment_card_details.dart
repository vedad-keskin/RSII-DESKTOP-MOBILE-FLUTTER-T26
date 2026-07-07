import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:ecommerce_mobile/models/payment_card_ib180079.dart';
import 'package:ecommerce_mobile/providers/auth_provider.dart';
import 'package:ecommerce_mobile/providers/payment_card_ib180079_provider.dart';
import 'package:ecommerce_mobile/utils/utils_widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:provider/provider.dart';

class PaymentCardDetailsScreen extends StatefulWidget {
  final PaymentCardIB180079? card;

  const PaymentCardDetailsScreen({super.key, this.card});

  @override
  State<PaymentCardDetailsScreen> createState() =>
      _PaymentCardDetailsScreenState();
}

class _PaymentCardDetailsScreenState extends State<PaymentCardDetailsScreen> {
  final _formKey = GlobalKey<FormBuilderState>();
  Map<String, dynamic> _initalValue = {};

  late PaymentCardIB180079Provider _paymentCardProvider;

  DateTime? _selectedDate;

  @override
  void initState() {
    super.initState();

    _initalValue = {
      'cardNumber': widget.card?.cardNumber,
      'cvc': widget.card?.cvc,
      'initialBalance': widget.card?.initialBalance.toString(),
    };

    _selectedDate = widget.card?.exiprationDate;

    _paymentCardProvider = context.read<PaymentCardIB180079Provider>();
  }

  Future<void> _pickDate() async {
    var date = await showDatePicker(
      context: context,
      initialDate: _selectedDate ?? DateTime.now(),
      firstDate: DateTime.now(),
      lastDate: DateTime(2100),
    );

    if (date != null) {
      setState(() {
        _selectedDate = date;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return MasterScreen(
      title: widget.card == null ? "Dodaj karticu" : "Uredi karticu",
      child: Center(
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(
            children: [
              _buildForm(),
              _saveButton(context),
            ],
          ),
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

          if (_selectedDate == null) {
            alertBox(context, "Error", "Datum isteka je obavezan.");
            return;
          }

          try {
            if (_formKey.currentState!.validate()) {
              var userId =
                  int.tryParse(AuthProvider.accessTokenDecoded?['Id'] ?? '0') ??
                  0;

              if (widget.card != null) {
                Map<String, dynamic> request = Map.of(
                  _formKey.currentState!.value,
                );

                request['id'] = widget.card?.id;
                request['userId'] = userId;
                request['initialBalance'] = double.parse(
                  _formKey.currentState!.value['initialBalance'],
                );
                request['exiprationDate'] = _selectedDate!.toIso8601String();

                await _paymentCardProvider.update(widget.card!.id, request);

                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text("Kartica uspješno izmijenjena")),
                );

                Navigator.pop(context, 'reload');
              } else {
                Map<String, dynamic> request = Map.of(
                  _formKey.currentState!.value,
                );

                request['userId'] = userId;
                request['initialBalance'] = double.parse(
                  _formKey.currentState!.value['initialBalance'],
                );
                request['exiprationDate'] = _selectedDate!.toIso8601String();

                await _paymentCardProvider.insert(request);

                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text("Kartica uspješno dodana")),
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
          FormBuilderTextField(
            name: 'cardNumber',
            validator: (value) {
              if (value == null || value.isEmpty) {
                return mField;
              } else if (!RegExp(r'^\d{12}$').hasMatch(value)) {
                return "Broj kartice mora imati 12 cifara";
              } else {
                return null;
              }
            },
            decoration: InputDecoration(label: Text("Broj kartice")),
          ),
          const SizedBox(height: 10),
          FormBuilderTextField(
            name: 'cvc',
            validator: (value) {
              if (value == null || value.isEmpty) {
                return mField;
              } else if (!RegExp(r'^\d{3}$').hasMatch(value)) {
                return "CVC mora imati 3 cifre";
              } else {
                return null;
              }
            },
            decoration: InputDecoration(label: Text("CVC")),
          ),
          const SizedBox(height: 10),
          Row(
            children: [
              Expanded(
                child: Text(
                  _selectedDate == null
                      ? "Datum isteka nije odabran"
                      : "Datum isteka: ${_selectedDate!.toString().split(' ')[0]}",
                ),
              ),
              ElevatedButton(
                onPressed: _pickDate,
                child: Text("Odaberi datum"),
              ),
            ],
          ),
          const SizedBox(height: 10),
          FormBuilderTextField(
            name: 'initialBalance',
            validator: (value) {
              if (value == null || value.isEmpty) {
                return mField;
              } else if (double.tryParse(value) == null) {
                return numericField;
              } else if (double.parse(value) < 0) {
                return "Stanje mora biti >= 0";
              } else {
                return null;
              }
            },
            decoration: InputDecoration(label: Text("Početno stanje")),
          ),
        ],
      ),
    );
  }
}
