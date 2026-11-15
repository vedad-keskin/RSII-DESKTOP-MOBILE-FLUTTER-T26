import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:ecommerce_mobile/models/coupon.dart';
import 'package:ecommerce_mobile/models/search_result.dart';
import 'package:ecommerce_mobile/providers/coupon_provider.dart';
import 'package:ecommerce_mobile/utils/utils_widgets.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

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
    super.initState();
    _couponProvider = context.read<CouponProvider>();
    initTable();
  }

  @override
  void dispose() {
    _codeController.dispose();
    super.dispose();
  }

  Future<void> initTable() async {
    final code = _codeController.text;
    setState(() => isLoading = true);
    try {
      var data = await _couponProvider.get(filter: {'code': code});
      if (!mounted || code != _codeController.text) return;

      setState(() {
        result = data;
        isLoading = false;
      });
    } on Exception catch (e) {
      if (!mounted || code != _codeController.text) return;
      setState(() {
        result = null;
        isLoading = false;
      });
      alertBox(context, 'Greška', e.toString());
    }
  }

  @override
  Widget build(BuildContext context) {
    return MasterScreen(
      title: 'Kuponi',
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [
            _buildSearch(),
            isLoading ? const CircularProgressIndicator() : _buildTable(),
            if (!isLoading)
              Text(
                'Trenutno važećih kupona: ${result?.items?.where((e) => e.isCouponValid).length ?? 0}',
              ),
          ],
        ),
      ),
    );
  }

  Expanded _buildTable() {
    return Expanded(
      child: SingleChildScrollView(
        child: SingleChildScrollView(
          scrollDirection: Axis.horizontal,
          child: DataTable(
            columns: const [
              DataColumn(label: Text('Kod')),
              DataColumn(label: Text('Opis')),
              DataColumn(label: Text('Popust (%)')),
              DataColumn(label: Text('Važi od')),
              DataColumn(label: Text('Važi do')),
              DataColumn(label: Text('Korištenja')),
              DataColumn(label: Text('Trenutno važeći')),
            ],
            rows:
                result?.items
                    ?.map(
                      (e) => DataRow(
                        cells: [
                          DataCell(Text(e.code)),
                          DataCell(Text(e.description)),
                          DataCell(Text(e.discountPercentage.toString())),
                          DataCell(
                            Text(e.validFrom.toString().split(' ').first),
                          ),
                          DataCell(Text(e.validTo.toString().split(' ').first)),
                          DataCell(Text('${e.usageCount}/${e.maxUses}')),
                          DataCell(Text(e.isCouponValid ? 'Da' : 'Ne')),
                        ],
                      ),
                    )
                    .toList() ??
                [],
          ),
        ),
      ),
    );
  }

  Padding _buildSearch() {
    return Padding(
      padding: const EdgeInsets.all(8.0),
      child: TextField(
        controller: _codeController,
        decoration: const InputDecoration(label: Text('Kod kupona')),
        onChanged: (_) => initTable(),
      ),
    );
  }
}
