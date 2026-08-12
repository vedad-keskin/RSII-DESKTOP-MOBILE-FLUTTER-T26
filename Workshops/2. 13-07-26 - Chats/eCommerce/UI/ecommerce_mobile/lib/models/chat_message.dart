import 'package:json_annotation/json_annotation.dart';

part 'chat_message.g.dart';

@JsonSerializable()
class ChatMessage {
  final int id;
  final int chatId;
  final int senderId;
  final String content;
  final DateTime createdAt;




  ChatMessage({
    required this.id,
    required this.content,
    required this.createdAt,
    required this.chatId,
    required this.senderId,
  });

  factory ChatMessage.fromJson(Map<String, dynamic> json) =>
      _$ChatMessageFromJson(json);

  Map<String, dynamic> toJson() => _$ChatMessageToJson(this);
}
