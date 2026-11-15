
import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:ecommerce_mobile/main.dart';
import 'package:ecommerce_mobile/models/coupon.dart';
import 'package:ecommerce_mobile/models/search_result.dart';
import 'package:ecommerce_mobile/providers/coupon_provider.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../models/category.dart';

class CouponListScreen extends StatefulWidget {
  const CouponListScreen({super.key});

  @override
  State<CouponListScreen> createState() => _CouponListScreenState();
}

class _CouponListScreenState extends State<CouponListScreen> {

  late CouponProvider _couponProvider;

  SearchResult<Coupon>? result;
  
  bool isLoading = true;

  final TextEditingController _codeController = TextEditingController();

  @override
  void initState() {
    // TODO: implement initState
    super.initState();

    _couponProvider = context.read<CouponProvider>();

    initTable();
  }

  Future<void> initTable() async {

    try {
      var data = await _couponProvider.get(filter: {'code' : _codeController.text});

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
      title: "Coupon List",
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [

            _buildSearch(),

            isLoading ? CircularProgressIndicator() : _buildTable(),

            Text('Ukupno trenutno važećih kupona: ${result?.items?.where((x) => x.isCouponValid ).length ?? 0}')
          
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
                  DataColumn(label: Text("Kod")),
                  DataColumn(label: Text("Opis")),
                  DataColumn(label: Text("Popust")),
                  DataColumn(label: Text("Važi od")),
                  DataColumn(label: Text("Važi do")),
                  DataColumn(label: Text("Upotreba")),
                  DataColumn(label: Text("Važeći")),
           






                  DataColumn(label: Text("Delete")),
                ],
                  rows: result?.items
                  ?.map(
                    (e) => DataRow(
                      onSelectChanged: (value) async {

                        // EDIT TODO 

                        // var refresh = await Navigator.of(context)
                        //         .push(MaterialPageRoute(
                        //       builder: (context) => CategoryDetailsScreen(category: e),
                        //     ));
                        
                        // if (refresh == "reload") {
                        //   initTable();
                        // }
                      },
                      cells: [
                      DataCell(Text(e.code.toString())),
                      DataCell(Text(e.description.toString())),
                      DataCell(Text(e.discountPercentage.toString())),
                      DataCell(Text(e.validFrom.toString().split(' ').first  )),
                      DataCell(Text(e.validTo.toString().split(' ').first)),
                      DataCell(Text( '${e.usageCount}/${e.maxUses}'    )),
                      DataCell(Text(e.isCouponValid ? 'Da' : 'Ne'  )),









                      DataCell(
                        IconButton(
                          icon: Icon(Icons.delete),
                          onPressed: () async {
                            showDialog(
                              context: context,
                              builder: (context) => AlertDialog(
                                title: Text("Delete"),
                                content: Text("Are you sure you want to delete this coupon?"),
                                actions: [
                                  TextButton(
                                    onPressed: () {
                                      Navigator.pop(context);
                                    },
                                    child: Text("Cancel"),
                                  ),
                                  ElevatedButton(
                                    onPressed: () async {
                                      // try {
                                      //   await _categoryProvider.remove(e.id!);

                                      //    ScaffoldMessenger.of(
                                      //         context,
                                      //       ).showSnackBar(
                                      //         SnackBar(
                                      //           content: Text(
                                      //             "Category deleted successfully",
                                      //           ),
                                      //         ),
                                      //       );
                                            
                                      //       Navigator.pop(context);

                                      //       setState(() {
                                      //         initTable();
                                      //       });
                                      // } on Exception catch (e) {
                                      //   alertBoxMoveBack(context, "Error", e.toString());
                                      // }
                                    },
                                    child: Text("Yes"),
                                  ),
                                ],
                              ),
                            );
                          },
                        ),
                      ),
                    ]),
                  )
                  .toList() ?? List.empty(),
            ),
            ),
          ));
  }

  Padding _buildSearch() {
    return Padding(
          padding: const EdgeInsets.all(8.0),
          child: Row(
            children: [
              Expanded(
                child: Padding(
                  padding: const EdgeInsets.all(8.0),
                  child: TextField(
                    controller: _codeController,
                    decoration: InputDecoration(label: Text("Code")),


onChanged:(value) {
  
initTable();

},



                  ),
                ),
              ),
              ElevatedButton(onPressed: () async{
                   try {
                        var data = await _couponProvider.get(filter: {"code": _codeController.text});

                        setState(() {
                          result = data;
                          isLoading = false;
                        });
                      } on Exception catch (e) {
                        alertBox(context, 'Error', e.toString());
                      }
              }, child: Text("Search")),
              SizedBox(
                width: 10,
              ),
              ElevatedButton(onPressed: () async {

                // TODO NEW
                // var refresh = await Navigator.of(context).push(
                //   MaterialPageRoute(
                //     builder: (context) => const CategoryDetailsScreen(
                //       category: null,
                //     )
                //   )
                // );

                // if( refresh == "reload"){
                //   initTable();
                // }
              }, child: Text("New")),
            ],
          ),
        );
  }
}
