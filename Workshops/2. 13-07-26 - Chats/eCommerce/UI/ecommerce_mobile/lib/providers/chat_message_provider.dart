import 'package:ecommerce_mobile/models/chat.dart';
import 'package:ecommerce_mobile/models/chat_message.dart';
import 'package:ecommerce_mobile/providers/base_provider.dart';



class ChatMessageProvider extends BaseProvider<ChatMessage> {
  ChatMessageProvider() : super("ChatMessage");

  @override
  ChatMessage fromJson(data) {
    return ChatMessage.fromJson(data);
  }
}