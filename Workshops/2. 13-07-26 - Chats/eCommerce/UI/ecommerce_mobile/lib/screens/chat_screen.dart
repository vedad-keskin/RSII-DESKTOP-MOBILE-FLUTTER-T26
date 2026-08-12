import 'package:ecommerce_mobile/layouts/master_screen.dart';
import 'package:ecommerce_mobile/models/chat.dart';
import 'package:ecommerce_mobile/models/user.dart';
import 'package:ecommerce_mobile/providers/chat_provider.dart';
import 'package:ecommerce_mobile/providers/user_provider.dart';
import 'package:ecommerce_mobile/screens/chat_details_screen.dart';
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
  State<ChatScreen> createState() => _ChatScreenState();
}

class _ChatScreenState extends State<ChatScreen> {





  late ChatProvider _chatProvider;
  late UserProvider _userProvider;

  SearchResult<Chat>? result;
  SearchResult<User>? userResult;

  bool isLoading = true;


  final TextEditingController _nameController = TextEditingController();


  int? selectedUserId;
  int? currentUserId = int.tryParse(AuthProvider.accessTokenDecoded?['Id'] ?? '0') ?? 0;



  @override
  void dispose() {
    _nameController.dispose();
    super.dispose();
  }

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

      var users = await _userProvider.get();


      var data = await _chatProvider.get(
        filter: {
          'user1Id': currentUserId,
          'user2Id': selectedUserId
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

  @override
  Widget build(BuildContext context) {
    return MasterScreen(
      title: "Poruke",
      child: Column(
        children: [
          isLoading ? CircularProgressIndicator() : _buildSearch(),
          isLoading ? CircularProgressIndicator() : _buildTextBox(),
          SizedBox(height: 20,),
          isLoading ? CircularProgressIndicator() : buildChatList(),
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
                    decoration: InputDecoration(label: Text("Naziv razgovora")),
                  ),
                ),
              ),
      
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
                  items: userResult!.items!
                  .where((x) => x.id != currentUserId  )
                  .map((entry) {
                    return DropdownMenuItem<int>(
                      value: entry.id,
                      child:  Text('${entry.firstName} - ${entry.lastName}'),
                    );
                  }).toList(),
                  onChanged:(value) async {
                    
                    if(value == 0){
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
              ElevatedButton(onPressed: () async{
                

             await _submit();




              }, child: Text("Kreiraj razgovor"))
            ],
          ),
        );
  }


  Future<void> _submit() async {
  

    if(_nameController.text.trim().isEmpty){

        alertBox(context, 'Upozorenje', "Naziv razgovora je obavezan");

    }else if(selectedUserId == null){

        alertBox(context, 'Upozorenje', "Korisnik je obavezan");



    }else{




   try {
      await context.read<ChatProvider>().insert({
        'name': _nameController.text.trim(),
         // Razgovor 1


        'user1Id': currentUserId,
        'user2Id': selectedUserId
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

          return Card(
            margin: const EdgeInsets.only(bottom: 8),
            child: ListTile(
              title: Row(
                children: [
                  Text(chat.name),
                  SizedBox(width: 8),

                  Text(chat.otherUserFirstName),
                  SizedBox(width: 3),

                  Text(chat.otherUserLastName),
                  SizedBox(width: 8),

                  Text("${chat.createdAt.hour}:${chat.createdAt.minute} ${chat.createdAt.day}.${chat.createdAt.month}.${chat.createdAt.year}"),
                  SizedBox(width: 8),
             
                ],
              ),
              subtitle: Padding(
                padding: const EdgeInsets.only(top: 8),
                child: Text(chat.lastMessageContent ?? "Nema poruke"),
              ),


              onTap: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (context) => ChatDetailsScreen(chatId: chat.id ,),
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
