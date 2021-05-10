using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Sheaft.Domain.Common
{
    public static class LocationProvider
    {
        public static Point CreatePoint(double latitude, double longitude)
        {
            // 4326 is most common coordinate system used by GPS/Maps
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            // see https://docs.microsoft.com/en-us/ef/core/modeling/spatial
            // Longitude and Latitude
            var newLocation = geometryFactory.CreatePoint(new Coordinate(longitude, latitude));

            return newLocation;
        }
    }
}