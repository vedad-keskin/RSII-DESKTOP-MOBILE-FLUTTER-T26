import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:ecommerce_mobile/models/chat.dart';
import 'package:ecommerce_mobile/models/user.dart';
import 'package:ecommerce_mobile/providers/chat_provider.dart';
import 'package:ecommerce_mobile/providers/user_provider.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../models/product_review.dart';
import '../models/search_result.dart';
import '../providers/auth_provider.dart';
import '../providers/product_review_provider.dart';
import '../utils/utils_widgets.dart';

class ChatScreen extends StatefulWidget {
  const ChatScreen({super.key});

  @override
  State<ChatScreen> createState() => _ChatScreenScreenState();
}

class _ChatScreenScreenState extends State<ChatScreen> {
  late ChatProvider _chatProvider;
  late UserProvider _userProvider;


  SearchResult<Chat>? result;
    SearchResult<User>? usersResult;


   int? selectedUserId;


  bool isLoading = true;



  @override
  void initState() {
    // TODO: implement initState
    super.initState();

    _chatProvider = context.read<ChatProvider>();
    _userProvider = context.read<UserProvider>();

    initData();
  }

  Future<void> initData() async {
    try {

      var users = await _userProvider.get(filter: {});



      var data = await _chatProvider.get(
        filter: {
          'user1Id':
              int.tryParse(AuthProvider.accessTokenDecoded?['Id'] ?? '0') ?? 0,
             'user2Id': selectedUserId
        },
      );
      setState(() {
        result = data;

        usersResult = users;



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
          isLoading ? CircularProgressIndicator() : _buildSearch(),
          SizedBox(height: 20,),
          isLoading ? CircularProgressIndicator() : buildChatList(),
        ],
      ),
    );
  }

  Padding _buildSearch() {
    return Padding(
          padding: const EdgeInsets.all(8.0),
          child: Row(
            children: [
              Expanded(
                child: DropdownButtonFormField(
                  initialValue: selectedUserId,
                  hint: const Text('Odaberi korisnika'),
                  items: usersResult!.items!.map((user) {
                    return DropdownMenuItem<int>(
                      value: user.id,
                      child: Text('${user.firstName} - ${user.lastName}'),
                    );
                  }).toList(),
                  onChanged:(value) {
                    
                    if(value == 0){
                      value = null;
                    }

                    setState(() {
                      selectedUserId = value;
                    });
                  },
                ),
              ),
              SizedBox(width: 10),
              ElevatedButton(onPressed: () async{
                await initData();
              }, child: Text("Search"))
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
              title: Row(
                children: [
                  Text("${chat.name}"),
                  SizedBox(width: 8),
                 
                  Text("${chat.otherUserFirstName} ${chat.otherUserLastName}"),
                  SizedBox(width: 8),

                Text("${chat.createdAt.hour}:${chat.createdAt.minute} ${chat.createdAt.day}.${chat.createdAt.month}.${chat.createdAt.year}"),

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
