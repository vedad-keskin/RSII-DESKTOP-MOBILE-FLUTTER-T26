import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:ecommerce_mobile/models/chat.dart';
import 'package:ecommerce_mobile/models/search_result.dart';
import 'package:ecommerce_mobile/models/user.dart';
import 'package:ecommerce_mobile/providers/auth_provider.dart';
import 'package:ecommerce_mobile/providers/chat_provider.dart';
import 'package:ecommerce_mobile/providers/user_provider.dart';
import 'package:ecommerce_mobile/screens/chat_details_screen.dart';
import 'package:ecommerce_mobile/utils/utils_widgets.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class ChatScreen extends StatefulWidget {
  const ChatScreen({super.key});

  @override
  State<ChatScreen> createState() => _ChatScreenState();
}

class _ChatScreenState extends State<ChatScreen> {
  late ChatProvider _chatProvider;
  late UserProvider _userProvider;

  SearchResult<Chat>? result;
  List<User> users = [];

  bool isLoading = true;

  int? selectedUserId;
  final TextEditingController _nameController = TextEditingController();

  int get myId =>
      int.tryParse(AuthProvider.accessTokenDecoded?['Id'] ?? '0') ?? 0;

  @override
  void initState() {
    super.initState();
    _chatProvider = context.read<ChatProvider>();
    _userProvider = context.read<UserProvider>();
    initData();
  }

  @override
  void dispose() {
    _nameController.dispose();
    super.dispose();
  }

  Future<void> initData() async {
    try {
      var usersResult = await _userProvider.get();
      var allUsers = usersResult.items ?? [];

      await loadChats();

      setState(() {
        users = allUsers.where((u) => u.id != myId).toList();
        isLoading = false;
      });
    } on Exception catch (e) {
      if (mounted) {
        alertBox(context, 'Error', e.toString());
      }
    }
  }

  Future<void> loadChats() async {
    try {
      var filter = <String, dynamic>{'user1Id': myId};
      if (selectedUserId != null) {
        filter['user2Id'] = selectedUserId;
      }

      var data = await _chatProvider.get(filter: filter);
      setState(() {
        result = data;
      });
    } on Exception catch (e) {
      if (mounted) {
        alertBox(context, 'Error', e.toString());
      }
    }
  }

  Future<void> createChat() async {
    if (_nameController.text.trim().isEmpty) {
      alertBox(context, 'Error', 'Naziv razgovora je obavezan.');
      return;
    }
    if (selectedUserId == null) {
      alertBox(context, 'Error', 'Odaberite korisnika.');
      return;
    }

    try {
      await _chatProvider.insert({
        'name': _nameController.text.trim(),
        'user1Id': myId,
        'user2Id': selectedUserId,
      });
      _nameController.clear();
      await loadChats();
    } on Exception catch (e) {
      if (mounted) {
        alertBox(context, 'Error', e.toString());
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return MasterScreen(
      title: "Poruke",
      child: Column(
        children: [
          _buildForm(),
          SizedBox(height: 20),
          isLoading ? CircularProgressIndicator() : buildChatList(),
        ],
      ),
    );
  }

  // Form structure from user_review_screen + TextField from add_review_screen
  Padding _buildForm() {
    return Padding(
      padding: const EdgeInsets.all(8.0),
      child: Column(
        children: [
          TextField(
            controller: _nameController,
            decoration: const InputDecoration(
              labelText: 'Naziv razgovora',
              border: OutlineInputBorder(),
            ),
          ),
          SizedBox(height: 10),
          DropdownButtonFormField<int>(
            value: selectedUserId,
            hint: const Text('Odaberi korisnika'),
            items: users.map((user) {
              return DropdownMenuItem<int>(
                value: user.id,
                child: Text("${user.firstName ?? ''} ${user.lastName ?? ''}"),
              );
            }).toList(),
            onChanged: (value) async {
              setState(() {
                selectedUserId = value;
              });
              await loadChats();
            },
          ),
          SizedBox(height: 10),
          ElevatedButton(
            onPressed: createChat,
            child: Text("Kreiraj razgovor"),
          ),
        ],
      ),
    );
  }

  Expanded buildChatList() {
    return Expanded(
      child: ListView.builder(
        itemCount: result?.items?.length ?? 0,
        itemBuilder: (context, index) {
          var chat = result!.items![index];

          return Card(
            margin: const EdgeInsets.only(bottom: 8),
            child: ListTile(
              title: Text(chat.name),
              subtitle: Padding(
                padding: const EdgeInsets.only(top: 8),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      "${chat.otherUserFirstName} ${chat.otherUserLastName}",
                    ),
                    Text(chat.createdAt.toString()),
                    Text(chat.lastMessageContent ?? "Nema poruka"),
                  ],
                ),
              ),
              onTap: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (context) =>
                        ChatDetailsScreen(chatId: chat.id),
                  ),
                );
              },
            ),
          );
        },
      ),
    );
  }
}
