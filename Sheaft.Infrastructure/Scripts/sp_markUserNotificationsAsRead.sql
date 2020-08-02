CREATE PROCEDURE MarkUserNotificationsAsRead
@UserId uniqueidentifier,
@GroupId uniqueidentifier,
@ReadBefore datetimeoffset
AS 
BEGIN
	declare @userUid bigint
	set @userUid = (select u.Uid from dbo.users u where u.Id = @UserId)
	
	declare @groupUid bigint
	set @groupUid = (select c.Uid from dbo.companies c where c.Id = @GroupId)

    update dbo.Notifications set Unread = 0 where (UserUid = @userUid or (GroupUid is not null and GroupUid = @groupUid)) and CreatedOn < @ReadBefore
END