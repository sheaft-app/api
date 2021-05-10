CREATE VIEW  [app].ProducersPerDepartment
AS
select DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName, sum(Active) AS Active, sum(Created) as Created from (
select c.Id as UserId, d.Id as DepartmentId, d.Code as DepartmentCode, d.Name as DepartmentName, r.Id as RegionId, r.Code as RegionCode, r.Name as RegionName, case when count(p.Id) > 0 then 1 else 0 end as Active, count(distinct(c.Id)) as Created
from app.Departments d
join app.Regions r on r.Id = d.RegionId
left join app.Users c on d.Id = c.Address_DepartmentId and c.Kind = 0
left join app.Products p on c.Id = p.ProducerId
group by c.Id, c.Kind, d.Id, d.Code, d.Name, r.Id, r.Code, r.Name, c.RemovedOn
) cc
group by DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName
GO

CREATE VIEW  [app].StoresPerDepartment
AS
select DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName, sum(Active) AS Active, sum(Created) as Created from (
select c.Id as UserId, d.Id as DepartmentId, d.Code as DepartmentCode, d.Name as DepartmentName, r.Id as RegionId, r.Code as RegionCode, r.Name as RegionName, case when count(p.Id) > 0 then 1 else 0 end as Active, count(distinct(c.Id)) as Created
from app.Departments d
join app.Regions r on r.Id = d.RegionId
left join app.Users c on d.Id = c.Address_DepartmentId and c.Kind = 1
left join app.Products p on c.Id = p.ProducerId
group by c.Id, c.Kind, d.Id, d.Code, d.Name, r.Id, r.Code, r.Name, c.RemovedOn
) cc
group by DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName
GO