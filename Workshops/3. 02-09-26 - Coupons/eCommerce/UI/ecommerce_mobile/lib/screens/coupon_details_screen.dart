import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:ecommerce_mobile/models/coupon.dart';
import 'package:ecommerce_mobile/providers/coupon_provider.dart';
import 'package:ecommerce_mobile/utils/utils_widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:provider/provider.dart';

class CouponDetailsScreen extends StatefulWidget {

  final Coupon? coupon;

  const CouponDetailsScreen({super.key, this.coupon});

  @override
  State<CouponDetailsScreen> createState() => _CouponDetailsScreenState();
}

class _CouponDetailsScreenState extends State<CouponDetailsScreen> {



  final _formKey = GlobalKey<FormBuilderState>();


  Map<String, dynamic> _initalValue = {};

  late CouponProvider _couponProvider;

  @override
  void initState() {
    super.initState();

    _initalValue = {
      'code': widget.coupon?.code ?? '',
      'description': widget.coupon?.description ?? '',
      'discountPercentage': widget.coupon?.discountPercentage.toString() ?? '',
      'validFrom': widget.coupon?.validFrom,
      'validTo': widget.coupon?.validTo,
      'maxUses': widget.coupon?.maxUses.toString() ?? '',
      'isActive': widget.coupon?.isActive ?? true,
    };

    _couponProvider = context.read<CouponProvider>();
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return MasterScreen(
      title: widget.coupon != null ? 'Edit Coupon' : 'Create Coupon',
      child: Center(
        child: SingleChildScrollView(
          padding: const EdgeInsets.all(32.0),
          child: ConstrainedBox(
            constraints: const BoxConstraints(maxWidth: 700),
            child: Column(
              children: [

                _buildHeader(theme),

                const SizedBox(height: 24.0),
                Card(
                  elevation: 10,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12.0),
                    side: BorderSide(
                      color: theme.colorScheme.primaryContainer,
                      width: 2,
                    ),
                  ),
                  child: Padding(
                    padding: const EdgeInsets.all(16.0),
                    child: _buildForm(theme),
                  ),
                ),
                SizedBox(height: 24.0),

                _buildActions(theme),

              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildHeader(ThemeData theme) {
    return Row(
      children: [
        Container(
          padding: const EdgeInsets.all(10.0),
          decoration: BoxDecoration(
            color: theme.colorScheme.primaryContainer,
            borderRadius: BorderRadius.circular(8.0),
          ),
          child: Icon(
            Icons.category_outlined,
            color: theme.colorScheme.onPrimaryContainer,
          ),
        ),
        const SizedBox(width: 16.0),
        Expanded(child: Column(
          children: [
            Text(
              widget.coupon != null
                  ? widget.coupon!.code
                  : 'Create Coupon',
              style: theme.textTheme.headlineSmall,
            ),
            Text(
              widget.coupon != null
                  ? 'Edit the coupon details'
                  : 'Fill the form to create a new coupon',
              style: theme.textTheme.bodyMedium,
            ),
          ],
        )),
      ],
    );
  }

  Widget _buildForm(ThemeData theme) {
    return FormBuilder(
      key: _formKey,
      initialValue: _initalValue,
      child: Column(
        children: [
          Row(
            children: [
              Expanded(
                child: FormBuilderTextField(
                  name: 'code',
                  decoration: const InputDecoration(label: Text("Code")),
                  validator: (value) => value == null || value.isEmpty ? mField : null,
                ),
              ),
            ],
          ),
          const SizedBox(height: 16.0),
          FormBuilderTextField(
            name: 'description',
            decoration: const InputDecoration(label: Text("Description")),
            maxLines: 4,
          ),
          const SizedBox(height: 16.0),
          FormBuilderTextField(
            name: 'discountPercentage',
            decoration: const InputDecoration(label: Text("Discount Percentage")),
            keyboardType: const TextInputType.numberWithOptions(decimal: true),
            validator: (value) =>
                double.tryParse(value ?? '') == null ? numericField : null,
          ),
          const SizedBox(height: 16.0),
          FormBuilderDateTimePicker(
            name: 'validFrom',
            inputType: InputType.date,
            decoration: const InputDecoration(label: Text("Valid From")),
            validator: (value) => value == null ? mField : null,
          ),
          const SizedBox(height: 16.0),
          FormBuilderDateTimePicker(
            name: 'validTo',
            inputType: InputType.date,
            decoration: const InputDecoration(label: Text("Valid To")),
            validator: (value) => value == null ? mField : null,
          ),
          const SizedBox(height: 16.0),
          FormBuilderTextField(
            name: 'maxUses',
            decoration: const InputDecoration(label: Text("Max Uses")),
            keyboardType: TextInputType.number,
            validator: (value) =>
                int.tryParse(value ?? '') == null ? numericField : null,
          ),
          const SizedBox(height: 16.0),
          FormBuilderCheckbox(name: 'isActive',
           title: const Text("Is Active"), 
           subtitle: const Text("Check to activate the coupon"),), 
        ],
      ),
    );
  }

  Widget _buildActions(ThemeData theme) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.end,
      children: [
        TextButton(
          onPressed: () => Navigator.of(context).pop(),
          child: const Text("Cancel"),
        ),
        const SizedBox(width: 16.0),
        ElevatedButton(
          onPressed: _save,
          child: const Text("Save"),
        ),
      ],
    );
  }

  Future _save() async {
    if (_formKey.currentState?.saveAndValidate() ?? false) {


      Map<String, dynamic> formData = Map.of(_formKey.currentState!.value);

      try {
        formData['discountPercentage'] = double.parse(formData['discountPercentage']);
        formData['maxUses'] = int.parse(formData['maxUses']);
        formData['validFrom'] = (formData['validFrom'] as DateTime).toIso8601String();
        formData['validTo'] = (formData['validTo'] as DateTime).toIso8601String();

        if (widget.coupon != null) {
          formData['id'] = widget.coupon!.id;
          await _couponProvider.update(widget.coupon!.id, formData);

        } else {
          await _couponProvider.insert(formData);
        }

        if (!mounted) return;
        Navigator.of(context).pop("reload");
      } catch (e) {
        if (!mounted) return;
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text("Error saving coupon: $e")),
        );
      }
    }
  }
}
