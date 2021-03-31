DROP VIEW [app].[ProductsSearch]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [app].ProductsSearch
as
select
    p.Id as product_id
     , p.Name as product_name
     , p.Name as partialProductName
     , CAST(p.QuantityPerUnit as float) as product_quantityPerUnit
     , case when p.Unit = 1 then 'mL'
            when p.Unit = 2 then 'L'
            when p.Unit = 3 then 'g'
            when p.Unit = 4 then 'kg' end as product_unit
     , CAST(cp.OnSalePricePerUnit as float) as product_onSalePricePerUnit
     , CAST(cp.OnSalePrice as float) as product_onSalePrice
     , CAST(p.Rating as float) as product_rating
     , p.RatingsCount as product_ratings_count
     , case when pa.Uid is not null then cast(1 as bit) else cast(0 as bit) end as product_returnable
     , r.Id as producer_id
     , r.Name as producer_name
     , r.Name as partialProducerName
     , r.Email as producer_email
     , r.Phone as producer_phone
     , ra.Zipcode as producer_zipcode
     , ra.City as producer_city
     , p.Picture as product_image
     , p.Available as product_available
     , c.Available as product_searchable
     , case when p.Conditioning = 1 then 'BOX'
            when p.Conditioning = 2 then 'BULK'
            when p.Conditioning = 3 then 'BOUQUET'
            when p.Conditioning = 4 then 'BUNCH'
            when p.Conditioning = 5 then 'PIECE'
            when p.Conditioning = 6 then 'BASKET' end as product_conditioning
     , app.InlineMax(app.InlineMax(app.InlineMax(p.UpdatedOn, r.UpdatedOn), t.UpdatedOn), p.CreatedOn) as last_update
     , case when (app.InlineMax(p.RemovedOn, r.RemovedOn)) is not null or r.CanDirectSell = 0 then 1 else 0 end as removed
     , '[' + STRING_AGG('"' + LOWER(t.Name) + '"', ',') + ']' as product_tags
     , ra.Longitude as producer_longitude
     , ra.Latitude as producer_latitude
     , geography::STGeomFromText('POINT('+convert(varchar(20),ra.Longitude)+' '+convert(varchar(20),ra.Latitude)+')',4326) as producer_geolocation
from app.Catalogs c
         join app.CatalogProducts cp on cp.CatalogUid = c.Uid and c.Kind = 1 and c.IsDefault = 1 and c.Available = 1
         join app.Products p on p.Uid = cp.ProductUid
         join app.Users r on r.Uid = p.ProducerUid and r.Kind = 0
         join app.UserAddresses ra on r.Uid = ra.UserUid
         left join app.ProductTags pt on p.Uid = pt.ProductUid
         left join app.Returnables pa on pa.Uid = p.ReturnableUid
         left join app.Tags t on t.Uid = pt.TagUid
group by
    p.Id,
    p.Name,
    case when p.Unit = 1 then 'mL'
         when p.Unit = 2 then 'L'
         when p.Unit = 3 then 'g'
         when p.Unit = 4 then 'kg' end,
    CAST(p.QuantityPerUnit as float),
    CAST(cp.OnSalePricePerUnit as float),
    CAST(cp.OnSalePrice as float),
    CAST(p.Rating as float),
    p.RatingsCount,
    case when pa.Uid is not null then cast(1 as bit) else cast(0 as bit) end,
    r.Id,
    r.Name,
    r.Email,
    p.Picture,
    case when p.Conditioning = 1 then 'BOX'
         when p.Conditioning = 2 then 'BULK'
         when p.Conditioning = 3 then 'BOUQUET'
         when p.Conditioning = 4 then 'BUNCH'
         when p.Conditioning = 5 then 'PIECE'
         when p.Conditioning = 6 then 'BASKET' end,
    r.Id,
    r.Phone,
    p.Available,
    c.Available,
    ra.Zipcode,
    ra.City,
    app.InlineMax(app.InlineMax(app.InlineMax(p.UpdatedOn, r.UpdatedOn), t.UpdatedOn), p.CreatedOn),
    case when (app.InlineMax(p.RemovedOn, r.RemovedOn)) is not null or r.CanDirectSell = 0 then 1 else 0 end,
    ra.Longitude,
    ra.Latitude