import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:ecommerce_mobile/models/chat.dart';
import 'package:ecommerce_mobile/models/chat_message.dart';
import 'package:ecommerce_mobile/models/user.dart';
import 'package:ecommerce_mobile/providers/chat_message_provider.dart';
import 'package:ecommerce_mobile/providers/chat_provider.dart';
import 'package:ecommerce_mobile/providers/user_provider.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../models/product_review.dart';
import '../models/search_result.dart';
import '../providers/auth_provider.dart';
import '../providers/product_review_provider.dart';
import '../utils/utils_widgets.dart';

class ChatDetailsScreen extends StatefulWidget {
  final int chatId;

  const ChatDetailsScreen({super.key, required this.chatId});

  @override
  State<ChatDetailsScreen> createState() => _ChatDetailsScreenState();
}

class _ChatDetailsScreenState extends State<ChatDetailsScreen> {
  late ChatMessageProvider _chatMessageProvider;
  // late UserProvider _userProvider;

  SearchResult<ChatMessage>? result;
  // SearchResult<User>? userResult;

  bool isLoading = true;

  final TextEditingController _nameController = TextEditingController();

  int? selectedUserId;
  int? currentUserId =
      int.tryParse(AuthProvider.accessTokenDecoded?['Id'] ?? '0') ?? 0;

  @override
  void dispose() {
    _nameController.dispose();
    super.dispose();
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();

    _chatMessageProvider = context.read<ChatMessageProvider>();
    // _userProvider = context.read<UserProvider>();

    initData();
  }

  Future<void> initData() async {
    try {
      // var users = await _userProvider.get();

      var data = await _chatMessageProvider.get(
        filter: {'chatId': widget.chatId, 'pageSize': 1000},
      );
      setState(() {
        result = data;

        // userResult = users;

        isLoading = false;
      });
    } on Exception catch (e) {
      alertBox(context, 'Error', e.toString());
    }
  }

  @override
  Widget build(BuildContext context) {
    return MasterScreen(
      title: "Poruke",
      child: Column(
        children: [
          // isLoading ? CircularProgressIndicator() : _buildSearch(),
          // SizedBox(height: 20,),
          isLoading ? CircularProgressIndicator() : buildChatList(),
          isLoading ? CircularProgressIndicator() : _buildTextBox(),
        ],
      ),
    );
  }

  Padding _buildTextBox() {
    return Padding(
      padding: const EdgeInsets.all(3.0),
      child: Row(
        children: [
          Expanded(
            child: Padding(
              padding: const EdgeInsets.all(3.0),
              child: TextField(
                controller: _nameController,
                decoration: InputDecoration(label: Text("Poruka")),
              ),
            ),
          ),

          SizedBox(width: 10),
          ElevatedButton(
            onPressed: () async {
              await _submit();
            },
            child: Text("Pošalji"),
          ),
        ],
      ),
    );
  }



  Future<void> _submit() async {
    if (_nameController.text.trim().isEmpty) {
      alertBox(context, 'Upozorenje', "Poruka je obavezna");
    } else {
      try {
        await context.read<ChatMessageProvider>().insert({
          'chatId': widget.chatId,
          'senderId': currentUserId,
          'content': _nameController.text.trim(),
        });
        if (mounted) {
          _nameController.clear();
          await initData();
        }
      } on Exception catch (e) {
        if (mounted) {
          alertBox(context, 'Upozorenje', e.toString());
        }
      }
    }
  }

  Expanded buildChatList() {
    return Expanded(
      child: ListView.builder(
        itemCount: result?.items?.length ?? 0,
        itemBuilder: (context, index) {
          var chat = result!.items![index];

          var isMessageMine = chat.senderId == currentUserId;

          return Padding(
            padding: EdgeInsets.only(
              left: isMessageMine ? 80 : 8,
              right: isMessageMine ? 8 : 80,
            ),
            child: Card(
              margin: const EdgeInsets.only(bottom: 8),
              child: ListTile(
                title: Row(children: [Text(chat.content), SizedBox(width: 8)]),
                subtitle: Padding(
                  padding: const EdgeInsets.only(top: 8),
                  child: Text(
                    "${chat.createdAt.hour}:${chat.createdAt.minute} ${chat.createdAt.day}.${chat.createdAt.month}.${chat.createdAt.year}",
                  ),
                ),

                trailing: isMessageMine
                    ? IconButton(
                        icon: Icon(Icons.delete),
                        onPressed: () async {
                          // _cartProvider.removeFromCart(cartItem.product);

                          try {
                            await context.read<ChatMessageProvider>().remove(
                              chat.id,
                            );
                            if (mounted) {
                              await initData();
                            }
                          } on Exception catch (e) {
                            if (mounted) {
                              alertBox(context, 'Upozorenje', e.toString());
                            }
                          }
                        },
                      )
                    : null,
              ),
            ),
          );
        },
      ),
    );
  }
}
