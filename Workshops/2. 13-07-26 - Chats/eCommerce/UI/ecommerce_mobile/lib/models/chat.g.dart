// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'chat.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Chat _$ChatFromJson(Map<String, dynamic> json) => Chat(
      id: (json['id'] as num).toInt(),
      name: json['name'] as String? ?? '',
      createdAt: DateTime.parse(json['createdAt'] as String),
      user1Id: (json['user1Id'] as num).toInt(),
      user2Id: (json['user2Id'] as num).toInt(),
      otherUserId: (json['otherUserId'] as num).toInt(),
      otherUserFirstName: json['otherUserFirstName'] as String? ?? '',
      otherUserLastName: json['otherUserLastName'] as String? ?? '',
      lastMessageContent: json['lastMessageContent'] as String?,
      lastMessageCreatedAt: json['lastMessageCreatedAt'] == null
          ? null
          : DateTime.parse(json['lastMessageCreatedAt'] as String),
    );

Map<String, dynamic> _$ChatToJson(Chat instance) => <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'createdAt': instance.createdAt.toIso8601String(),
      'user1Id': instance.user1Id,
      'user2Id': instance.user2Id,
      'otherUserId': instance.otherUserId,
      'otherUserFirstName': instance.otherUserFirstName,
      'otherUserLastName': instance.otherUserLastName,
      'lastMessageContent': instance.lastMessageContent,
      'lastMessageCreatedAt': instance.lastMessageCreatedAt?.toIso8601String(),
    };
