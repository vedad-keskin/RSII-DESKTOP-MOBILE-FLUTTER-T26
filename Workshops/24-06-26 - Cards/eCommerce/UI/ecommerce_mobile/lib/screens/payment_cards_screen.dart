import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:ecommerce_mobile/models/payment_card_ib180079.dart';
import 'package:ecommerce_mobile/models/search_result.dart';
import 'package:ecommerce_mobile/providers/auth_provider.dart';
import 'package:ecommerce_mobile/providers/payment_card_ib180079_provider.dart';
import 'package:ecommerce_mobile/screens/payment_card_details.dart';
import 'package:ecommerce_mobile/utils/utils_widgets.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class PaymentCardsScreen extends StatefulWidget {
  const PaymentCardsScreen({super.key});

  @override
  State<PaymentCardsScreen> createState() => _PaymentCardsScreenState();
}

class _PaymentCardsScreenState extends State<PaymentCardsScreen> {
  late PaymentCardIB180079Provider _paymentCardProvider;
  SearchResult<PaymentCardIB180079>? result;
  bool isLoading = true;

  @override
  void initState() {
    super.initState();

    _paymentCardProvider = context.read<PaymentCardIB180079Provider>();

    initTable();
  }

  Future<void> initTable() async {
    try {
      var data = await _paymentCardProvider.get(
        filter: {
          'userId':
              int.tryParse(AuthProvider.accessTokenDecoded?['Id'] ?? '0') ?? 0,
        },
      );

      setState(() {
        result = data;
        isLoading = false;
      });
    } on Exception catch (e) {
      alertBox(context, 'Error', e.toString());
    }
  }

  @override
  Widget build(BuildContext context) {
    return MasterScreen(
      title: "Moje kartice",
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [
            _buildActions(),
            isLoading ? CircularProgressIndicator() : _buildTable(),
          ],
        ),
      ),
    );
  }

  Expanded _buildTable() {
    return Expanded(
      child: SizedBox(
        width: double.infinity,
        child: SingleChildScrollView(
          child: DataTable(
            columns: [
              DataColumn(label: Text("Broj kartice")),
              DataColumn(label: Text("Datum isteka")),
              DataColumn(label: Text("Početno stanje")),
            ],
            rows:
                result?.items
                    ?.map(
                      (e) => DataRow(
                        onSelectChanged: (value) async {
                          var refresh = await Navigator.of(context).push(
                            MaterialPageRoute(
                              builder: (context) =>
                                  PaymentCardDetailsScreen(card: e),
                            ),
                          );

                          if (refresh == "reload") {
                            initTable();
                          }
                        },
                        cells: [
                          DataCell(Text(e.cardNumber)),
                          DataCell(Text('${e.exiprationDate.day}.${e.exiprationDate.month}.${e.exiprationDate.year}')),
                          DataCell(Text('${e.initialBalance} KM')),
                        ],
                      ),
                    )
                    .toList() ??
                List.empty(),
          ),
        ),
      ),
    );
  }

  Padding _buildActions() {
    return Padding(
      padding: const EdgeInsets.all(8.0),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          ElevatedButton(
            onPressed: () async {
              var refresh = await Navigator.of(context).push(
                MaterialPageRoute(
                  builder: (context) => const PaymentCardDetailsScreen(
                    card: null,
                  ),
                ),
              );

              if (refresh == "reload") {
                initTable();
              }
            },
            child: Text("Dodaj karticu"),
          ),
        ],
      ),
    );
  }
}
