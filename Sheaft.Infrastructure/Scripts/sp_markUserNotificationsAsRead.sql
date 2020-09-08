CREATE PROCEDURE MarkUserNotificationsAsRead
@UserUid uniqueidentifier,
@ReadBefore datetimeoffset
AS 
BEGIN	
	declare @Uid bigint
	set @Uid = (select u.Uid from dbo.users u where u.Id = @UserUId)

    update dbo.Notifications set Unread = 0 where UserUid = @Uid and CreatedOn < @ReadBefore
END