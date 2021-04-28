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
     , case when pa.Id is not null then cast(1 as bit) else cast(0 as bit) end as product_returnable
     , r.Id as producer_id
     , r.Name as producer_name
     , r.Name as partialProducerName
     , r.Email as producer_email
     , r.Phone as producer_phone
     , r.Address_Zipcode as producer_zipcode
     , r.Address_City as producer_city
     , p.Picture as product_image
     , p.Available as product_available
     , case when sum(case when c.Kind = 1 and c.Available = 1 then 1 end) > 0 then cast(1 as bit) else cast(0 as bit) end as product_searchable
     , case when p.Conditioning = 1 then 'BOX'
            when p.Conditioning = 2 then 'BULK'
            when p.Conditioning = 3 then 'BOUQUET'
            when p.Conditioning = 4 then 'BUNCH'
            when p.Conditioning = 5 then 'PIECE'
            when p.Conditioning = 6 then 'BASKET' end as product_conditioning
     , app.InlineMax(app.InlineMax(app.InlineMax(p.UpdatedOn, r.UpdatedOn), t.UpdatedOn), p.CreatedOn) as last_update
     , case when (app.InlineMax(p.RemovedOn, r.RemovedOn)) is not null then 1 else 0 end as removed
     , '[' + STRING_AGG('"' + LOWER(t.Name) + '"', ',') + ']' as product_tags
     , r.Address_Longitude as producer_longitude
     , r.Address_Latitude as producer_latitude
     , geography::STGeomFromText('POINT('+convert(varchar(20),r.Address_Longitude)+' '+convert(varchar(20),r.Address_Latitude)+')',4326) as producer_geolocation
from app.Products p
         join app.Users r on r.Id = p.ProducerId and r.Kind = 0
         left join app.CatalogProducts cp on cp.ProductId = p.Id
         left join app.Catalogs c on c.Id = cp.CatalogId
         left join app.ProductTags pt on p.Id = pt.ProductId
         left join app.Returnables pa on pa.Id = p.ReturnableId
         left join app.Tags t on t.Id = pt.TagId
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
    case when pa.Id is not null then cast(1 as bit) else cast(0 as bit) end,
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
    r.Address_Zipcode,
    r.Address_City,
    r.Address_Longitude,
    r.Address_Latitude,
    p.CreatedOn,
    p.UpdatedOn,
    p.RemovedOn,
    r.UpdatedOn,
    r.RemovedOn,
    r.CanDirectSell,
    t.UpdatedOn