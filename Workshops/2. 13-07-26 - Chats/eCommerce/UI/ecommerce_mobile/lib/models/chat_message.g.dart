// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'chat_message.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ChatMessage _$ChatMessageFromJson(Map<String, dynamic> json) => ChatMessage(
  id: (json['id'] as num).toInt(),
  content: json['content'] as String,
  createdAt: DateTime.parse(json['createdAt'] as String),
  chatId: (json['chatId'] as num).toInt(),
  senderId: (json['senderId'] as num).toInt(),
);

Map<String, dynamic> _$ChatMessageToJson(ChatMessage instance) =>
    <String, dynamic>{
      'id': instance.id,
      'chatId': instance.chatId,
      'senderId': instance.senderId,
      'content': instance.content,
      'createdAt': instance.createdAt.toIso8601String(),
    };
