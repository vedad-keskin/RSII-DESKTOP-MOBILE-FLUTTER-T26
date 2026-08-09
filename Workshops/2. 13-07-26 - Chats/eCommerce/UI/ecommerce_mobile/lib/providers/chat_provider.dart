import 'package:ecommerce_mobile/models/chat.dart';
import 'package:ecommerce_mobile/providers/base_provider.dart';



class ChatProvider extends BaseProvider<Chat> {
  ChatProvider() : super("Chat");

  @override
  Chat fromJson(data) {
    return Chat.fromJson(data);
  }
}