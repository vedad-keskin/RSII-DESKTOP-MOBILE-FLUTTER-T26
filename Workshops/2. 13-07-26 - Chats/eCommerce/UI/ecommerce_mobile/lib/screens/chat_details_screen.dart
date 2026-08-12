import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:ecommerce_mobile/models/chat.dart';
import 'package:flutter/material.dart';

class ChatDetailsScreen extends StatefulWidget {
  final Chat chat;

  const ChatDetailsScreen({super.key, required this.chat});

  @override
  State<ChatDetailsScreen> createState() => _ChatDetailsScreenState();
}

class _ChatDetailsScreenState extends State<ChatDetailsScreen> {
  @override
  Widget build(BuildContext context) {
    return MasterScreen(
      title: widget.chat.name,
      child: Center(
        child: Text(widget.chat.name),
      ),
    );
  }
}
