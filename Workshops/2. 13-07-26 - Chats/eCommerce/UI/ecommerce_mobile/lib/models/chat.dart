import 'package:json_annotation/json_annotation.dart';

part 'chat.g.dart';

@JsonSerializable()
class Chat {
  final int id;
  final String name;
  final DateTime createdAt;
  final int user1Id;
  final int user2Id;
  final int otherUserId;
  final String otherUserFirstName;
  final String otherUserLastName;
  final String? lastMessageContent;
  final DateTime? lastMessageCreatedAt;



  Chat({
    required this.id,
    required this.name,
    required this.createdAt,
    required this.user1Id,
    required this.user2Id,
    required this.otherUserId,
    required this.otherUserFirstName,
    required this.otherUserLastName,
    this.lastMessageContent,
    this.lastMessageCreatedAt,
  });

  factory Chat.fromJson(Map<String, dynamic> json) =>
      _$ChatFromJson(json);

  Map<String, dynamic> toJson() => _$ChatToJson(this);
}
