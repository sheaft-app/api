namespace Sheaft.Domain.OrderManagement;

public interface IDetermineNextDeliveryDays
{
    IEnumerable<DateTime> For(IEnumerable<DayOfWeek>? deliveryDays, int? lockOrderHoursBeforeDelivery);
}

public class DetermineNextDeliveryDays : IDetermineNextDeliveryDays
{
    public IEnumerable<DateTime> For(IEnumerable<DayOfWeek>? deliveryDays, int? lockOrderHoursBeforeDelivery)
    {
        if (deliveryDays == null || !deliveryDays.Any())
            return new List<DateTime>();

        var list = new List<DateTime>();
        var currentDate = DateTimeOffset.Now.Date;
        foreach (var deliveryDay in deliveryDays.OrderBy(oh => oh))
        {
            var results = new List<DateTime>();
            var increment = 0;
            while (results.Count < 3 && increment < 365)
            {
                var diff = (int) deliveryDay + increment - (int) currentDate.DayOfWeek;
                var result = GetDeliveryDateIfMatch(lockOrderHoursBeforeDelivery ?? 0, diff, currentDate);
                if (result.HasValue)
                    results.Add(result.Value);

                increment += 7;
            }

            if (results.Any())
                list.AddRange(results);
        }

        return list.OrderBy(l => l);
    }

    private DateTime? GetDeliveryDateIfMatch(int lockOrderHoursBeforeDelivery, int diff, DateTime currentDate)
    {
        var targetDate = currentDate.AddDays(diff);
        if (currentDate.AddHours(lockOrderHoursBeforeDelivery) >= targetDate)
            return null;

        return targetDate;
    }
}