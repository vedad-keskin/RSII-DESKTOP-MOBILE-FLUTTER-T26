import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:flutter/material.dart';

class ChatDetailsScreen extends StatelessWidget {
  final int chatId;

  const ChatDetailsScreen({super.key, required this.chatId});

  @override
  Widget build(BuildContext context) {
    return MasterScreen(
      title: "Poruke",
      child: Center(child: Text("chat details TBD (chatId: $chatId)")),
    );
  }
}
