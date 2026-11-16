import 'dart:convert';

import 'package:ecommerce_mobile/models/category.dart';
import 'package:ecommerce_mobile/models/coupon.dart';
import 'package:ecommerce_mobile/providers/base_provider.dart';
import 'package:http/http.dart' as http;

class CouponProvider extends BaseProvider<Coupon> {
  CouponProvider() : super("Coupon");

  @override
  Coupon fromJson(data) {
    return Coupon.fromJson(data);
  }



  Future<Map<String, dynamic>> apply(String code,List<Map<String, dynamic>> items) async {


    final uri = Uri.parse('${BaseProvider.baseUrl}Coupon/Apply');

    final headers = createHeaders();

    final body = jsonEncode({ 'code' : code,  'items': items});

    final response = await http.post(uri, headers: headers, body: body);

    validateResponse(response);

    
    return jsonDecode(response.body) as Map<String, dynamic>;
  }














}
