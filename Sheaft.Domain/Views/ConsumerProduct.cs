using System;
using NetTopologySuite.Geometries;

namespace Sheaft.Domain.Views
{
    public class ConsumerProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal OnSalePricePerUnit { get; set; }
        public Guid ProducerId { get; set; }
        public Point Location { get; private set; }
        public string Tags { get; private set; }
    }
}