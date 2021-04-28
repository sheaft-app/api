using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sheaft.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Infrastructure.Services;
using Sheaft.Mediatr.DeliveryMode.Queries;

namespace Queries.Delivery.Tests
{
    [TestClass]
    public class GetProducerDeliveries
    {
        private IAppDbContext _context;
        private IDeliveryQueries _queries;
        private Mock<ITableService> _capingMock;

        [TestInitialize]
        public void Initialize()
        {
            _capingMock = new Mock<ITableService>();
            _context = ContextHelper.GetInMemoryContext();
            _queries = new DeliveryQueries(_context, _capingMock.Object, null);
        }

        [TestMethod]
        [DataRow("3b5c008bb6a24f5c8cc8258b9e33105f", false, DeliveryKind.Farm)]
        [DataRow("6a209232b2184ae5bf8a1c26a7a74e8b", true, DeliveryKind.Farm)]
        [DataRow("3b5c008bb6a24f5c8cc8258b9e33105f", true, DeliveryKind.ProducerToStore)]
        public async Task Should_Return_No_Deliveries(string producerId, bool available, DeliveryKind kind)
        {
            var token = CancellationToken.None;

            await _context.AddAsync(new DeliveryMode(
                Guid.NewGuid(),
                kind,
                new Producer(Guid.Parse("3b5c008bb6a24f5c8cc8258b9e33105f"), "prod1", "fa", "la", "test@email.com", new UserAddress("x", null, "x", "x", CountryIsoCode.FR, null)),
                available,
                new DeliveryAddress("x", null, "x", "x", CountryIsoCode.FR, null, null),
                new List<DeliveryHours>
                {
                    new DeliveryHours(DayOfWeek.Wednesday, TimeSpan.FromHours(8), TimeSpan.FromHours(12))
                },
                "delivery1"), token);

            await _context.SaveChangesAsync(token);

            //test
            var results = await _queries.GetProducersDeliveriesAsync(
                new List<Guid> { Guid.Parse(producerId) }, 
                new List<DeliveryKind> { DeliveryKind.Farm }, 
                DateTime.UtcNow, 
                null, 
                token);

            //assert
            results.Should().NotBeNull().And.BeEmpty();
        }

        [TestMethod]
        [DataRow(0, 2020, 1, 1, 7, 59, 59, 1, 8)]
        [DataRow(0, 2020, 1, 1, 8, 0, 0, 8, 15)]
        [DataRow(24, 2019, 12, 31, 8, 0, 0, 8, 15)]
        [DataRow(24, 2019, 12, 31, 7, 0, 0, 1, 8)]
        public async Task Should_Return_Wednesday_DeliveryHours(int orderLockInHours, int year, int month, int day, int hour, int minute, int second, int expectedFirstDay, int expectedLastDay)
        {
            var token = CancellationToken.None;
            var currentDate = new DateTime(year, month, day, hour, minute, second);
            var producerId = Guid.NewGuid();

            var delivery = new DeliveryMode(
                Guid.NewGuid(),
                DeliveryKind.Farm,
                new Producer(producerId, "prod1", "fa", "la", "test@email.com", new UserAddress("x", null, "x", "x", CountryIsoCode.FR, null)),
                true,
                new DeliveryAddress("x", null, "x", "x", CountryIsoCode.FR, null, null),
                new List<DeliveryHours> { new DeliveryHours(DayOfWeek.Wednesday, TimeSpan.FromHours(8), TimeSpan.FromHours(12)) },
                "delivery1");

            delivery.SetLockOrderHoursBeforeDelivery(orderLockInHours);

            await _context.AddAsync(delivery, token);
            await _context.SaveChangesAsync(token);

            //test
            var results = await _queries.GetProducersDeliveriesAsync(
                new List<Guid> { producerId },
                new List<DeliveryKind> { DeliveryKind.Farm },
                currentDate,
                null,
                token);

            //assert
            var deliveryHoursResults = results
                .Should().NotBeNull().And.ContainSingle()
                .And.Subject.First().Deliveries.Should().HaveCount(1)
                .And.Subject.First().DeliveryHours.Should().HaveCount(2)
                .And.Subject.Should().OnlyContain(c => c.Day == DayOfWeek.Wednesday);

            deliveryHoursResults.And.Subject.First().ExpectedDeliveryDate.Day.Should().Be(expectedFirstDay);
            deliveryHoursResults.And.Subject.Last().ExpectedDeliveryDate.Day.Should().Be(expectedLastDay);
        }

        [TestMethod]
        [DataRow(0, 2020, 1, 1, 7, 59, 59, 1, 8, 4, 11)]
        [DataRow(0, 2020, 1, 1, 8, 0, 0, 8, 15, 4, 11)]
        [DataRow(24, 2019, 12, 31, 8, 0, 0, 8, 15, 4, 11)]
        [DataRow(24, 2019, 12, 31, 7, 0, 0, 1, 8, 4, 11)]
        [DataRow(0, 2020, 1, 8, 8, 0, 0, 15, 22, 11, 18)]
        public async Task Should_Return_Multiple_Days_DeliveryHours(int orderLockInHours, int year, int month, int day, int hour, int minute, int second, int expectedFirstDay, int expectedSecondDay, int expectedThirdDay, int expectedLastDay)
        {
            var token = CancellationToken.None;
            var currentDate = new DateTime(year, month, day, hour, minute, second);
            var producerId = Guid.NewGuid();

            var entity = new DeliveryMode(
                            Guid.NewGuid(),
                            DeliveryKind.Farm,
                            new Producer(producerId, "prod1", "fa", "la", "test@email.com", new UserAddress("x", null, "x", "x", CountryIsoCode.FR, null)),
                            true,
                            new DeliveryAddress("x", null, "x", "x", CountryIsoCode.FR, null, null),
                            new List<DeliveryHours> {
                    new DeliveryHours(DayOfWeek.Wednesday, TimeSpan.FromHours(8), TimeSpan.FromHours(12)),
                    new DeliveryHours(DayOfWeek.Saturday, TimeSpan.FromHours(10), TimeSpan.FromHours(16))
                            },
                            "delivery1");

            entity.SetLockOrderHoursBeforeDelivery(orderLockInHours);
            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            //test
            var results = await _queries.GetProducersDeliveriesAsync(
                new List<Guid> { producerId },
                new List<DeliveryKind> { DeliveryKind.Farm },
                currentDate,
                null,
                token);

            //assert
            var deliveryHoursResults = results
                .Should().NotBeNull().And.ContainSingle()
                .And.Subject.First().Deliveries.Should().HaveCount(1)
                .And.Subject.First().DeliveryHours.Should().HaveCount(4)
                .And.Subject.Should().OnlyContain(c => c.Day == DayOfWeek.Wednesday || c.Day == DayOfWeek.Saturday);

            deliveryHoursResults.And.Subject.ElementAt(0).ExpectedDeliveryDate.Day.Should().Be(expectedFirstDay);
            deliveryHoursResults.And.Subject.ElementAt(1).ExpectedDeliveryDate.Day.Should().Be(expectedSecondDay);
            deliveryHoursResults.And.Subject.ElementAt(2).ExpectedDeliveryDate.Day.Should().Be(expectedThirdDay);
            deliveryHoursResults.And.Subject.ElementAt(3).ExpectedDeliveryDate.Day.Should().Be(expectedLastDay);
        }

        [TestMethod]
        [DataRow(0, 2020, 1, 1, 7, 59, 59, 1, 8, 3, 10)]
        [DataRow(0, 2020, 1, 1, 8, 0, 0, 8, 15, 3, 10)]
        [DataRow(24, 2019, 12, 31, 8, 0, 0, 8, 15, 3, 10)]
        [DataRow(24, 2019, 12, 31, 7, 0, 0, 1, 8, 3, 10)]
        [DataRow(0, 2020, 1, 8, 8, 0, 0, 15, 22, 10, 17)]
        public async Task Should_Return_Multiple_Deliveries(int orderLockInHours, int year, int month, int day, int hour, int minute, int second, int expectedFarmDelivery_FirstDay, int expectedFarmDelivery_SecondDay, int expectedMarketDelivery_FirstDay, int expectedMarketDelivery_SecondDay)
        {
            var token = CancellationToken.None;
            var currentDate = new DateTime(year, month, day, hour, minute, second);
            var producerId = Guid.NewGuid();

            var entity1 = new DeliveryMode(
                Guid.NewGuid(),
                DeliveryKind.Farm,
                new Producer(producerId, "prod1", "fa", "la", "test@email.com", new UserAddress("x", null, "x", "x", CountryIsoCode.FR, null)),
                true,
                new DeliveryAddress("x", null, "x", "x", CountryIsoCode.FR, null, null),
                new List<DeliveryHours> { new DeliveryHours(DayOfWeek.Wednesday, TimeSpan.FromHours(8), TimeSpan.FromHours(12)) },
                "delivery1");

            entity1.SetLockOrderHoursBeforeDelivery(orderLockInHours);
            await _context.AddAsync(entity1, token);

            var entity2 = new DeliveryMode(
                Guid.NewGuid(),
                DeliveryKind.Market,
                new Producer(producerId, "prod1", "fa", "la", "test@email.com", new UserAddress("x", null, "x", "x", CountryIsoCode.FR, null)),
                true,
                new DeliveryAddress("x", null, "x", "x", CountryIsoCode.FR, null, null),
                new List<DeliveryHours> { new DeliveryHours(DayOfWeek.Friday, TimeSpan.FromHours(16), TimeSpan.FromHours(18)) },
                "delivery2");

            entity2.SetLockOrderHoursBeforeDelivery(orderLockInHours);
            await _context.AddAsync(entity2, token);

            await _context.SaveChangesAsync(token);

            //test
            var results = await _queries.GetProducersDeliveriesAsync(
                new List<Guid> { producerId },
                new List<DeliveryKind> { DeliveryKind.Farm, DeliveryKind.Market },
                currentDate,
                null,
                token);

            //assert
            var deliveriesResults = results
                .Should().NotBeNull().And.ContainSingle()
                .And.Subject.First().Deliveries.Should().HaveCount(2);

             var marketDelivery = deliveriesResults.And.Subject.First(c => c.Kind == DeliveryKind.Market);
             var farmDelivery = deliveriesResults.And.Subject.First(c => c.Kind == DeliveryKind.Farm);

            marketDelivery.DeliveryHours.Should().OnlyContain(c => c.Day == DayOfWeek.Friday);
            marketDelivery.DeliveryHours.ElementAt(0).ExpectedDeliveryDate.Day.Should().Be(expectedMarketDelivery_FirstDay);
            marketDelivery.DeliveryHours.ElementAt(1).ExpectedDeliveryDate.Day.Should().Be(expectedMarketDelivery_SecondDay);

            farmDelivery.DeliveryHours.Should().OnlyContain(c => c.Day == DayOfWeek.Wednesday);
            farmDelivery.DeliveryHours.ElementAt(0).ExpectedDeliveryDate.Day.Should().Be(expectedFarmDelivery_FirstDay);
            farmDelivery.DeliveryHours.ElementAt(1).ExpectedDeliveryDate.Day.Should().Be(expectedFarmDelivery_SecondDay);
        }

        [TestMethod]
        [DataRow(1, 0, 2021, 1, 1, 8, 12)]
        [DataRow(5, 3, 2021, 1, 1, 8, 12)]
        public async Task Should_Return_Deliveries_With_Capings(int maxPurchaseOrders, int currentOrders, int year, int month, int day, int from, int to)
        {
            var deliveryId = Guid.NewGuid();
            var producerId = Guid.NewGuid();
            var expectedDeliveryDate = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.FromHours(0));
            var expectedDeliveryDate2 = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.FromHours(0)).AddDays(7);
            var result = new List<CapingDeliveryDto>
            {
                new CapingDeliveryDto
                {
                    Count = currentOrders,
                    DeliveryId = deliveryId,
                    ProducerId = producerId,
                    From = TimeSpan.FromHours(from),
                    To = TimeSpan.FromHours(to),
                    PartitionKey = $"{producerId}-{deliveryId}-{expectedDeliveryDate.Year}{expectedDeliveryDate.Month}{expectedDeliveryDate.Day}",
                    RowKey = $"{TimeSpan.FromHours(from).TotalSeconds}-{TimeSpan.FromHours(to).TotalSeconds}",
                    ExpectedDate = expectedDeliveryDate
                },
                new CapingDeliveryDto
                {
                    Count = currentOrders,
                    DeliveryId = deliveryId,
                    ProducerId = producerId,
                    From = TimeSpan.FromHours(from),
                    To = TimeSpan.FromHours(to),
                    PartitionKey = $"{producerId}-{deliveryId}-{expectedDeliveryDate2.Year}{expectedDeliveryDate2.Month}{expectedDeliveryDate2.Day}",
                    RowKey = $"{TimeSpan.FromHours(from).TotalSeconds}-{TimeSpan.FromHours(to).TotalSeconds}",
                    ExpectedDate = expectedDeliveryDate2
                }
            };

            _capingMock.Setup(c => c.GetCapingDeliveriesInfosAsync(It.IsAny<IEnumerable<Tuple<Guid, Guid, DeliveryHourDto>>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Result<IEnumerable<CapingDeliveryDto>>.Success(result)));

            var token = CancellationToken.None;

            var delivery = new DeliveryMode(
                deliveryId,
                DeliveryKind.Farm,
                new Producer(producerId, "prod1", "fa", "la", "test@email.com", new UserAddress("x", null, "x", "x", CountryIsoCode.FR, null)),
                true,
                new DeliveryAddress("x", null, "x", "x", CountryIsoCode.FR, null, null),
                new List<DeliveryHours>
                {
                    new DeliveryHours(DayOfWeek.Friday, TimeSpan.FromHours(from), TimeSpan.FromHours(to))
                },
                "delivery1");

            delivery.SetMaxPurchaseOrdersPerTimeSlot(maxPurchaseOrders);

            await _context.AddAsync(delivery, token);
            await _context.SaveChangesAsync(token);

            //test
            var results = await _queries.GetProducersDeliveriesAsync(
                new List<Guid> { producerId },
                new List<DeliveryKind> { DeliveryKind.Farm },
                expectedDeliveryDate.AddDays(-1),
                null,
                token);

            //assert
            var deliveriesResults = results
                .Should().NotBeNull().And.ContainSingle()
                .And.Subject.First().Deliveries.Should().HaveCount(1);

            deliveriesResults.And.Subject.First().DeliveryHours.Should().HaveCount(2);
        }

        [TestMethod]
        [DataRow(1, 2, 2021, 1, 1, 8, 12)]
        [DataRow(5, 5, 2021, 1, 1, 8, 12)]
        public async Task Should_Return_No_Deliveries_With_Capings(int maxPurchaseOrders, int currentOrders, int year, int month, int day, int from, int to)
        {
            var deliveryId = Guid.NewGuid();
            var producerId = Guid.NewGuid();
            var expectedDeliveryDate = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.FromHours(0));
            var expectedDeliveryDate2 = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.FromHours(0)).AddDays(7);
            var result = new List<CapingDeliveryDto>
            {
                new CapingDeliveryDto
                {
                    Count = currentOrders,
                    DeliveryId = deliveryId,
                    ProducerId = producerId,
                    From = TimeSpan.FromHours(from),
                    To = TimeSpan.FromHours(to),
                    PartitionKey = $"{producerId}-{deliveryId}-{expectedDeliveryDate.Year}{expectedDeliveryDate.Month}{expectedDeliveryDate.Day}",
                    RowKey = $"{TimeSpan.FromHours(from).TotalSeconds}-{TimeSpan.FromHours(to).TotalSeconds}",
                    ExpectedDate = expectedDeliveryDate
                },
                new CapingDeliveryDto
                {
                    Count = currentOrders,
                    DeliveryId = deliveryId,
                    ProducerId = producerId,
                    From = TimeSpan.FromHours(from),
                    To = TimeSpan.FromHours(to),
                    PartitionKey = $"{producerId}-{deliveryId}-{expectedDeliveryDate2.Year}{expectedDeliveryDate2.Month}{expectedDeliveryDate2.Day}",
                    RowKey = $"{TimeSpan.FromHours(from).TotalSeconds}-{TimeSpan.FromHours(to).TotalSeconds}",
                    ExpectedDate = expectedDeliveryDate2
                }
            };

            _capingMock.Setup(c => c.GetCapingDeliveriesInfosAsync(It.IsAny<IEnumerable<Tuple<Guid, Guid, DeliveryHourDto>>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Result<IEnumerable<CapingDeliveryDto>>.Success(result)));

            var token = CancellationToken.None;

            var delivery = new DeliveryMode(
                deliveryId,
                DeliveryKind.Farm,
                new Producer(producerId, "prod1", "fa", "la", "test@email.com", new UserAddress("x", null, "x", "x", CountryIsoCode.FR, null)),
                true,
                new DeliveryAddress("x", null, "x", "x", CountryIsoCode.FR, null, null),
                new List<DeliveryHours>
                {
                    new DeliveryHours(DayOfWeek.Friday, TimeSpan.FromHours(from), TimeSpan.FromHours(to))
                },
                "delivery1");

            delivery.SetMaxPurchaseOrdersPerTimeSlot(maxPurchaseOrders);

            await _context.AddAsync(delivery, token);
            await _context.SaveChangesAsync(token);

            //test
            var results = await _queries.GetProducersDeliveriesAsync(
                new List<Guid> { producerId },
                new List<DeliveryKind> { DeliveryKind.Farm },
                expectedDeliveryDate.AddDays(-1),
                null,
                token);

            //assert
            var deliveriesResults = results
                .Should().NotBeNull().And.ContainSingle()
                .And.Subject.First().Deliveries.Should().HaveCount(0);
        }
    }
}
