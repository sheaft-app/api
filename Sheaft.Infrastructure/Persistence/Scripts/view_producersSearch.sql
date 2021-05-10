CREATE VIEW  [app].ProducersSearch
as
	select
     r.Id as producer_id
     , r.Name as producer_name   
     , r.Name as partialProducerName
     , r.Email as producer_email
     , r.Picture as producer_picture
     , r.Phone as producer_phone
     , r.Address_Line1 as producer_line1
     , r.Address_Line2 as producer_line2
     , r.Address_Zipcode as producer_zipcode
     , r.Address_City as producer_city
     , app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)) as last_update
     , case when r.RemovedOn is null then 0 else 1 end as removed
     , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as producer_tags     
     , r.Address_Longitude as producer_longitude
     , r.Address_Latitude as producer_latitude
     , geography::STGeomFromText('POINT('+convert(varchar(20),r.Address_Longitude)+' '+convert(varchar(20),r.Address_Latitude)+')',4326) as producer_geolocation
     , count(p.Id) as producer_products_count
    from app.Users r 
    left join app.ProducerTags ct on r.Id = ct.ProducerId
    left join app.Tags t on t.Id = ct.TagId
    left join app.Products p on p.ProducerId = r.Id	
	where r.Kind = 0 and r.OpenForNewBusiness = 1
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