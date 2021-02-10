CREATE VIEW  [app].ProducersSearch
as
	select
     r.Id as producer_id
     , r.Name as producer_name   
     , r.Name as partialProducerName
     , r.Email as producer_email
     , r.Picture as producer_picture
     , r.Phone as producer_phone
     , ra.Line1 as producer_line1
     , ra.Line2 as producer_line2
     , ra.Zipcode as producer_zipcode
     , ra.City as producer_city
     , app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)) as last_update
     , case when r.RemovedOn is null then 0 else 1 end as removed
     , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as producer_tags     
     , ra.Longitude as producer_longitude
     , ra.Latitude as producer_latitude
     , geography::STGeomFromText('POINT('+convert(varchar(20),ra.Longitude)+' '+convert(varchar(20),ra.Latitude)+')',4326) as producer_geolocation
     , count(p.Id) as producer_products_count
    from app.Users r 
    join app.UserAddresses ra on r.Uid = ra.UserUid
    left join app.ProducerTags ct on r.Uid = ct.ProducerUid
    left join app.Tags t on t.Uid = ct.TagUid
    left join app.Products p on p.ProducerUid = r.Uid	
	where r.Kind = 0 and r.OpenForNewBusiness = 1
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
    app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)),
    case when r.RemovedOn is null then 0 else 1 end,
    ra.Longitude,
    ra.Latitude