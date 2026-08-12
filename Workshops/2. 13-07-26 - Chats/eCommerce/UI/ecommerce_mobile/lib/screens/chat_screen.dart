import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:ecommerce_mobile/models/chat.dart';
import 'package:ecommerce_mobile/models/user.dart';
import 'package:ecommerce_mobile/providers/chat_provider.dart';
import 'package:ecommerce_mobile/providers/user_provider.dart';
import 'package:ecommerce_mobile/screens/chat_details_screen.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../models/search_result.dart';
import '../providers/auth_provider.dart';
import '../utils/utils_widgets.dart';

class ChatScreen extends StatefulWidget {
  const ChatScreen({super.key});

  @override
  State<ChatScreen> createState() => _ChatScreenState();
}

class _ChatScreenState extends State<ChatScreen> {
  late ChatProvider _chatProvider;
  late UserProvider _userProvider;

  SearchResult<Chat>? result;
  SearchResult<User>? userResult;

  bool isLoading = true;

  int? selectedUserId;

  final TextEditingController _nameController = TextEditingController();

  int get _currentUserId =>
      int.tryParse(AuthProvider.accessTokenDecoded?['Id'] ?? '0') ?? 0;

  @override
  void initState() {
    // TODO: implement initState
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
      var users = await _userProvider.get();

      var data = await _chatProvider.get(
        filter: {
          'user1Id': _currentUserId,
          'user2Id': selectedUserId,
        },
      );
      setState(() {
        result = data;
        userResult = users;
        isLoading = false;
      });
    } on Exception catch (e) {
      alertBox(context, 'Error', e.toString());
    }
  }

  Future<void> _createChat() async {
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
        'user1Id': _currentUserId,
        'user2Id': selectedUserId,
      });
      _nameController.clear();
      await initData();
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
          isLoading ? CircularProgressIndicator() : _buildSearch(),
          SizedBox(height: 20),
          isLoading ? CircularProgressIndicator() : buildChatList(),
        ],
      ),
    );
  }

  Padding _buildSearch() {
    return Padding(
      padding: const EdgeInsets.all(8.0),
      child: Column(
        children: [
          TextField(
            controller: _nameController,
            decoration: InputDecoration(label: Text("Naziv razgovora")),
          ),
          SizedBox(height: 10),
          Row(
            children: [
              Expanded(
                child: DropdownButtonFormField(
                  initialValue: selectedUserId,
                  hint: const Text('Odaberi korisnika'),
                  items: userResult!.items!
                      .where((u) => u.id != _currentUserId)
                      .map((entry) {
                    return DropdownMenuItem<int>(
                      value: entry.id,
                      child: Text('${entry.firstName} - ${entry.lastName}'),
                    );
                  }).toList(),
                  onChanged: (value) async {
                    if (value == 0) {
                      value = null;
                    }

                    setState(() {
                      selectedUserId = value;
                    });
                    await initData();
                  },
                ),
              ),
              SizedBox(width: 10),
              ElevatedButton(
                onPressed: () async {
                  await _createChat();
                },
                child: Text("Kreiraj razgovor"),
              ),
            ],
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
              onTap: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (context) => ChatDetailsScreen(chat: chat),
                  ),
                );
              },
              title: Row(
                children: [
                  Text(chat.name),
                  SizedBox(width: 8),
                  Text(chat.otherUserFirstName),
                  SizedBox(width: 3),
                  Text(chat.otherUserLastName),
                  SizedBox(width: 8),
                  Text(
                    "${chat.createdAt.hour}:${chat.createdAt.minute} ${chat.createdAt.day}.${chat.createdAt.month}.${chat.createdAt.year}",
                  ),
                  SizedBox(width: 8),
                ],
              ),
              subtitle: Padding(
                padding: const EdgeInsets.only(top: 8),
                child: Text(chat.lastMessageContent ?? "Nema poruka"),
              ),
            ),
          );
        },
      ),
    );
  }
}
