CREATE VIEW ProducersPerDepartment
AS
select DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName, sum(Active) AS Active, sum(Created) as Created from (
select d.Id as DepartmentId, d.Code as DepartmentCode, d.Name as DepartmentName, r.Id as RegionId, r.Code as RegionCode, r.Name as RegionName, case when count(p.Uid) > 0 then 1 else 0 end as Active, count(distinct(c.Uid)) as Created
from dbo.Departments d
join dbo.Regions r on r.Uid = d.RegionUid
left join dbo.UserAddresses ca on d.Uid = ca.DepartmentUid
left join dbo.Users c on c.Uid = ca.UserUid and c.Kind = 0
left join dbo.Products p on c.Uid = p.ProducerUid
group by c.Kind, d.Id, d.Code, d.Name, r.Id, r.Code, r.Name, c.RemovedOn
) cc
group by DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName
GO

CREATE VIEW StoresPerDepartment
AS
select DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName, sum(Active) AS Active, sum(Created) as Created from (
select d.Id as DepartmentId, d.Code as DepartmentCode, d.Name as DepartmentName, r.Id as RegionId, r.Code as RegionCode, r.Name as RegionName, case when count(p.Uid) > 0 then 1 else 0 end as Active, count(distinct(c.Uid)) as Created
from dbo.Departments d
join dbo.Regions r on r.Uid = d.RegionUid
left join dbo.UserAddresses ca on d.Uid = ca.DepartmentUid
left join dbo.Users c on c.Uid = ca.UserUid and c.Kind = 1
left join dbo.Products p on c.Uid = p.ProducerUid
group by c.Kind, d.Id, d.Code, d.Name, r.Id, r.Code, r.Name, c.RemovedOn
) cc
group by DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName
GO