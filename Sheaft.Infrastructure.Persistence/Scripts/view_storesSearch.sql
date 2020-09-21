CREATE VIEW StoresSearch
as
    select
     r.Id as store_id
     , r.Name as store_name 
     , r.Name as partialStoreName
     , r.Email as store_email
     , r.Picture as store_picture
     , r.Phone as store_phone
     , ra.Line1 as store_line1
     , ra.Line2 as store_line2
     , ra.Zipcode as store_zipcode
     , ra.City as store_city
     , dbo.InlineMax(r.CreatedOn, dbo.InlineMax(r.UpdatedOn, t.UpdatedOn)) as last_update
     , case when r.RemovedOn is null then 0 else 1 end as removed
     , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as store_tags     
     , ra.Longitude as store_longitude
     , ra.Latitude as store_latitude
     , geography::STGeomFromText('POINT('+convert(varchar(20),ra.Longitude)+' '+convert(varchar(20),ra.Latitude)+')',4326) as store_geolocation
   from dbo.Users r 
    join dbo.UserAddresses ra on r.Uid = ra.UserUid
    left join dbo.StoreTags ct on r.Uid = ct.StoreUid
    left join dbo.Tags t on t.Uid = ct.TagUid	
	where r.Kind = 1 and r.OpenForNewBusiness = 1
   group by
	r.Id,
    r.Name,
    r.Email,
	r.Picture,
    r.Phone,
    ra.Line1,
    ra.Line2,
    ra.Zipcode,
    ra.City,
    dbo.InlineMax(r.CreatedOn, dbo.InlineMax(r.UpdatedOn, t.UpdatedOn)),
    case when r.RemovedOn is null then 0 else 1 end,
    ra.Longitude,
    ra.Latitude