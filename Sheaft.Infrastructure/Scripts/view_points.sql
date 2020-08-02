DROP VIEW UserPointsPerDepartment
GO
CREATE VIEW UserPointsPerDepartment
   WITH SCHEMABINDING
   AS
SELECT UserId, Name, Picture, RegionId, DepartmentId, Points, Position
    FROM (
        SELECT u.Id as UserId, case when u.Anonymous = 1 then null else u.FirstName end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, r.Id as RegionId, d.Id as DepartmentId, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank() 
          over (ORDER BY sum(totalPoints) DESC ) AS Position
        FROM dbo.Users u  
         join dbo.Departments d on d.Uid = u.DepartmentUid
         join dbo.Regions r on r.Uid = d.Uid
		group by r.Id, d.Id, u.Id, case when u.Anonymous = 1 then null else u.FirstName end, case when u.Anonymous = 1 then null else u.Picture end
        ) rs 
where Position <= 10
GO


DROP VIEW UserPointsPerRegion
GO
CREATE VIEW UserPointsPerRegion
   WITH SCHEMABINDING
   AS
SELECT UserId, Name, Picture, RegionId, Points, Position
    FROM (
        SELECT u.Id as UserId, case when u.Anonymous = 1 then null else u.FirstName end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, r.Id as RegionId, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank() 
          over (ORDER BY sum(totalPoints) DESC ) AS Position
        FROM dbo.Users u  
         join dbo.Departments d on d.Uid = u.DepartmentUid
         join dbo.Regions r on r.Uid = d.Uid
		group by r.Id, u.Id, case when u.Anonymous = 1 then null else u.FirstName end, case when u.Anonymous = 1 then null else u.Picture end
        ) rs 
where Position <= 10
GO


DROP VIEW UserPointsPerCountry
GO
CREATE VIEW UserPointsPerCountry
   WITH SCHEMABINDING
   AS
SELECT UserId, Name, Picture, Points, Position
    FROM (
        SELECT u.Id as UserId, case when u.Anonymous = 1 then null else u.FirstName end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank() 
          over (ORDER BY sum(totalPoints) DESC ) AS Position
        FROM dbo.Users u  
		group by u.Id, case when u.Anonymous = 1 then null else u.FirstName end, case when u.Anonymous = 1 then null else u.Picture end
        ) rs 
where Position <= 10
GO


DROP VIEW PointsPerDepartment
GO
CREATE VIEW PointsPerDepartment
   WITH SCHEMABINDING
   AS
SELECT RegionId, RegionName, Code, DepartmentId, DepartmentName, Points, Users, Position
    FROM (
        SELECT r.Id as RegionId, r.Name as RegionName, d.Name as DepartmentName, d.Code, d.Id as DepartmentId, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank() 
          over (ORDER BY sum(totalPoints) DESC ) AS Position
        FROM dbo.Users u  
         join dbo.Departments d on d.Uid = u.DepartmentUid
         join dbo.Regions r on r.Uid = d.Uid
		group by r.Id, r.Name, d.Id, d.Name, d.Code
        ) rs 
where Position <= 10
GO


DROP VIEW PointsPerRegion
GO
CREATE VIEW PointsPerRegion
   WITH SCHEMABINDING
   AS
SELECT RegionId, RegionName, Points, Users, Position
    FROM (
        SELECT r.Id as RegionId, r.Name as RegionName, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank() 
          over (ORDER BY sum(totalPoints) DESC ) AS Position
        FROM dbo.Users u  
         join dbo.Departments d on d.Uid = u.DepartmentUid
         join dbo.Regions r on r.Uid = d.Uid
		group by r.Id, r.Name
        ) rs 
where Position <= 10
GO


DROP VIEW PointsPerCountry
GO
CREATE VIEW PointsPerCountry
   WITH SCHEMABINDING
   AS
select sum(TotalPoints) as Points, count(distinct Uid) as Users from dbo.Users
GO


CREATE PROCEDURE UserPositionInDepartement
@DepartmentId uniqueidentifier,
@UserId uniqueidentifier
AS 
BEGIN
   SELECT Points, Position
   FROM (
      SELECT u.Id, sum(TotalPoints) as Points, Rank() 
            over (ORDER BY sum(TotalPoints) DESC ) AS Position
         FROM dbo.Users u 
         join dbo.Departments d on d.Uid = u.DepartmentUid
         where d.Id = @DepartmentId
         group by d.Id, u.Id
      ) rs 
   WHERE Id = @UserId
END
GO


CREATE PROCEDURE UserPositionInRegion
@RegionId uniqueidentifier,
@UserId uniqueidentifier
AS 
BEGIN
   SELECT Points, Position
   FROM (
      SELECT u.Id, sum(TotalPoints) as Points, Rank() 
            over (ORDER BY sum(TotalPoints) DESC ) AS Position
         FROM dbo.Users u 
         join dbo.Departments d on d.Uid = u.DepartmentUid
         join dbo.Regions r on r.Uid = d.Uid
         where r.Id = @RegionId
         group by r.Id, u.Id
      ) rs 
   WHERE Id = @UserId
END
GO


CREATE PROCEDURE UserPositionInCountry
@UserId uniqueidentifier
AS 
BEGIN
   SELECT Points, Position
   FROM (
      SELECT Id, TotalPoints as Points, Rank() 
            over (ORDER BY TotalPoints DESC ) AS Position
         FROM dbo.Users 
      ) rs 
   WHERE Id = @UserId
END
GO