CREATE PROCEDURE  [app].MarkUserNotificationsAsRead
@UserUid uniqueidentifier,
@ReadBefore datetimeoffset
AS 
BEGIN	
    update app.Notifications set Unread = 0 where UserId = @UserUid and CreatedOn < @ReadBefore
END