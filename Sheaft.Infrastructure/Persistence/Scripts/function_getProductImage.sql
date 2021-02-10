create function [app].[GetProductImage](@productId uniqueidentifier, @image nvarchar(max), @companyId uniqueidentifier, @tags nvarchar(max))
returns nvarchar(max)
as
begin
  if @image is not null 
    return 'https://content.sheaft.com/pictures/companies/' + Lower(convert(nvarchar(50), @companyId)) + '/products/' + Lower(convert(nvarchar(50), @productId)) + '/' + @image + '_medium.jpg'

  declare @tag nvarchar(max)
  select @tag = LOWER(value)
  from STRING_SPLIT(@tags, ',')

  if @tag = 'fruits et légumes' 
   return 'https://content.sheaft.com/pictures/products/categories/fruitsvegetables.jpg'

  if @tag = 'oeufs et produits laitiers' 
	return 'https://content.sheaft.com/pictures/products/categories/dairy.jpg'

  if @tag = 'poissons' 
	return 'https://content.sheaft.com/pictures/products/categories/fish.jpg'

  if @tag = 'épicerie' 
	return 'https://content.sheaft.com/pictures/products/categories/grocery.jpg'

  if @tag = 'viandes' 
	return 'https://content.sheaft.com/pictures/products/categories/meat.jpg'

  if @tag = 'boissons' 
	return 'https://content.sheaft.com/pictures/products/categories/drinks.jpg'

  return ''
end
GO
