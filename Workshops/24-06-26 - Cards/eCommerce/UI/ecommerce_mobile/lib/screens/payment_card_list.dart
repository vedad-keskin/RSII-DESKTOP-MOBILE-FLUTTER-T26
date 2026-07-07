
import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:ecommerce_mobile/models/payment_card_ib180079.dart';
import 'package:ecommerce_mobile/models/search_result.dart';
import 'package:ecommerce_mobile/providers/auth_provider.dart';
import 'package:ecommerce_mobile/providers/payment_card_ib180079_provider.dart';
import 'package:ecommerce_mobile/utils/utils_widgets.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class PaymentCardList extends StatefulWidget {
  const PaymentCardList({super.key});

  @override
  State<PaymentCardList> createState() => _PaymentCardListState();
}

class _PaymentCardListState extends State<PaymentCardList> {

  late PaymentCardIB180079Provider _paymentCardProvider;

  SearchResult<PaymentCardIB180079>? result;


  bool isLoading = true;


  @override
  void initState() {
    // TODO: implement initState
    super.initState();

    _paymentCardProvider = context.read<PaymentCardIB180079Provider>();

    initTable();
  }

  Future<void> initTable() async {
    try {
      var data = await _paymentCardProvider.get(filter: {

       'userId': int.tryParse(AuthProvider.accessTokenDecoded?['Id'] ?? '0') ?? 0,

      });

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
            isLoading ? CircularProgressIndicator() : _buildTable()
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
                  DataColumn(label: Text("Card Number")),
                  DataColumn(label: Text("CVC")),
                  DataColumn(label: Text("Initial Balance")),
                  DataColumn(label: Text("Expiration Date")),
                ],
                  rows: result?.items
                  ?.map(
                    (e) => DataRow(
                      // onSelectChanged: (value) async {
                      //   var refresh = await Navigator.of(context)
                      //           .push(MaterialPageRoute(
                      //         builder: (context) => ProductDetailsScreen(product: e),
                      //       ));
                        
                      //   if (refresh == "reload") {
                      //     initTable();
                      //   }
                      // },
                      cells: [
                      DataCell(Text(e.cardNumber ?? 'N/A')),
                      DataCell(Text(e.cvc ?? 'N/A')),
                      DataCell(Text('${e.initialBalance ?? 0} KM')),
                      DataCell(Text('${e.exiprationDate?.day ?? ''}.${e.exiprationDate?.month ?? 0}.${e.exiprationDate?.year ?? 0}')),
               
                    ]),
                  )
                  .toList() ?? List.empty(),
            ),
            ),
          ));
  }


  
}
