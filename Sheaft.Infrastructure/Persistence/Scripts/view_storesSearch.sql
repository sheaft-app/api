CREATE VIEW  [app].StoresSearch
as
    select
     r.Id as store_id
     , r.Name as store_name 
     , r.Name as partialStoreName
     , r.Email as store_email
     , r.Picture as store_picture
     , r.Phone as store_phone
     , r.Address_Line1 as store_line1
     , r.Address_Line2 as store_line2
     , r.Address_Zipcode as store_zipcode
     , r.Address_City as store_city
     , app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)) as last_update
     , case when r.RemovedOn is null then 0 else 1 end as removed
     , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as store_tags     
     , r.Address_Longitude as store_longitude
     , r.Address_Latitude as store_latitude
     , geography::STGeomFromText('POINT('+convert(varchar(20),r.Address_Longitude)+' '+convert(varchar(20),r.Address_Latitude)+')',4326) as store_geolocation
   from app.Users r 
    left join app.StoreTags ct on r.Id = ct.StoreId
    left join app.Tags t on t.Id = ct.TagId	
	where r.Kind = 1 and r.OpenForNewBusiness = 1
   group by
	r.Id,
    r.Name,
    r.Email,
	r.Picture,
    r.Phone,
    r.Address_Line1,
    r.Address_Line2,
    r.Address_Zipcode,
    r.Address_City,
    app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)),
    case when r.RemovedOn is null then 0 else 1 end,
    r.Address_Longitude,
    r.Address_Latitude