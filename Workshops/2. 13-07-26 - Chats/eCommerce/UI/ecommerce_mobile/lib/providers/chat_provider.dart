import 'package:ecommerce_mobile/providers/base_provider.dart';

import '../models/chat.dart';

class ChatProvider extends BaseProvider<Chat> {
  ChatProvider() : super("Chat");

  @override
  Chat fromJson(data) {
    return Chat.fromJson(data);
  }
}
