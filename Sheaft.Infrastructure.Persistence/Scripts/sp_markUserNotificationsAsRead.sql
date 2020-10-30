CREATE PROCEDURE MarkUserNotificationsAsRead
@UserUid uniqueidentifier,
@ReadBefore datetimeoffset
AS 
BEGIN	
	declare @Uid bigint
	set @Uid = (select u.Uid from app.users u where u.Id = @UserUId)

    update app.Notifications set Unread = 0 where UserUid = @Uid and CreatedOn < @ReadBefore
END