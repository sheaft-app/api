create function [app].[InlineMax](@val1 datetime, @val2 datetime)
returns datetime
as
begin
  if @val1 > @val2
    return @val1
  return isnull(@val2,@val1)
end
GO