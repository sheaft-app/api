CREATE VIEW ProductsSearch
as
    select
    p.Id as product_id
     , p.Name as product_name
     , p.Name as partialProductName
	 , CAST(p.QuantityPerUnit as float) as product_quantityPerUnit	
     , case when p.Unit = 0 then 'mL'
			when p.Unit = 1 then 'L'
			when p.Unit = 2 then 'g'
			when p.Unit = 3 then 'kg' end as product_unit														
     , CAST(p.OnSalePricePerUnit as float) as product_onSalePricePerUnit
     , CAST(p.OnSalePrice as float) as product_onSalePrice
     , CAST(p.Rating as float) as product_rating
     , p.RatingsCount as product_ratings_count
     , case when pa.Uid is not null then cast(1 as bit) else cast(0 as bit) end as packaged
     , r.Id as producer_id
     , r.Name as producer_name
     , r.Email as producer_email
     , r.Phone as producer_phone
     , ra.Zipcode as producer_zipcode
     , ra.City as producer_city
	 , dbo.GetProductImage(p.Id, p.image, r.Id, STRING_AGG(LOWER(case when t.Kind = 0 then t.Name end), ',')) as product_image
     , dbo.InlineMax(dbo.InlineMax(dbo.InlineMax(p.UpdatedOn, r.UpdatedOn), t.UpdatedOn), p.CreatedOn) as last_update
     , case when (dbo.InlineMax(p.RemovedOn, r.RemovedOn)) is null and p.Available = 1 then 0 else 1 end as removed
     , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as product_tags     
     , ra.Longitude as producer_longitude
     , ra.Latitude as producer_latitude
     , geography::STGeomFromText('POINT('+convert(varchar(20),ra.Longitude)+' '+convert(varchar(20),ra.Latitude)+')',4326) as producer_geolocation
  from dbo.Products p
    join dbo.Users r on r.Uid = p.ProducerUid and r.Kind = 0
    join dbo.UserAddresses ra on r.Uid = ra.UserUid
	join dbo.DeliveryModes dm on dm.ProducerUid = r.Uid and dm.Kind in (1, 2, 3, 4) 
    left join dbo.ProductTags pt on p.Uid = pt.ProductUid
    left join dbo.Packagings pa on pa.Uid = p.PackagingUid
    left join dbo.Tags t on t.Uid = pt.TagUid
  group by
    p.Id,
    p.Name,
   case when p.Unit = 0 then 'mL'
			when p.Unit = 1 then 'L'
			when p.Unit = 2 then 'g'
			when p.Unit = 3 then 'kg' end,
	CAST(p.QuantityPerUnit as float),	
	CAST(p.OnSalePricePerUnit as float),
    CAST(p.OnSalePrice as float),
    CAST(p.WholeSalePrice as float),
    CAST(p.Rating as float),
    p.RatingsCount,
	case when pa.Uid is not null then cast(1 as bit) else cast(0 as bit) end,
	r.Id,
    r.Name,
    r.Email,
	p.Image,
	r.Id,
    r.Phone,
    ra.Zipcode,
    ra.City,
    dbo.InlineMax(dbo.InlineMax(dbo.InlineMax(p.UpdatedOn, r.UpdatedOn), t.UpdatedOn), p.CreatedOn),
    case when (dbo.InlineMax(p.RemovedOn, r.RemovedOn)) is null and p.Available = 1 then 0 else 1 end,
    ra.Longitude,
    ra.Latitude