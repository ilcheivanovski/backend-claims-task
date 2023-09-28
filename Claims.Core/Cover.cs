using Newtonsoft.Json;
using System.Xml.Linq;

namespace Claims.Core;

public class Cover
{
    public Cover()
    {
        Id = Guid.NewGuid().ToString();
    }
    [JsonProperty(PropertyName = "id")]
    public string Id { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public CoverType Type { get; private set; }

    public void Add(DateOnly startDate, DateOnly endDate, CoverType type)
    {
        Id = Guid.NewGuid().ToString();
        StartDate = startDate;
        EndDate = endDate;
        Type = type;
    }
    public decimal Premium
    {
        get
        {
            return ComputePremium(StartDate, EndDate, Type);
        }
        set { }
    }

    public static decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        decimal multiplier = 1;

        switch (coverType)
        {
            case CoverType.Yacht:
                multiplier = 1.1m; // 10%
                break;
            case CoverType.PassengerShip:
                multiplier = 1.2m; // 20%
                break;
            case CoverType.Tanker:
                multiplier = 1.5m; // 50%
                break;
            default:
                multiplier = 1.3m; // 30%
                break;
        }

        var premiumPerDay = 1250 * multiplier;
        var insuranceDurationInDays = endDate.DayNumber - startDate.DayNumber;
        var totalPremium = 0m;

        for (var i = 0; i < insuranceDurationInDays; i++)
        {
            if (i < 30)
            {
                totalPremium += premiumPerDay;
            }
            if (i >= 30 && i < 180 && coverType == CoverType.Yacht)
            {
                totalPremium += premiumPerDay - premiumPerDay * 0.05m;
            }
            else if (i < 180)
            {
                totalPremium += premiumPerDay - premiumPerDay * 0.02m;
            }
            if (i < 365 && coverType != CoverType.Yacht)
            {
                totalPremium += premiumPerDay - premiumPerDay * 0.03m;
            }
            else if (i < 365)
            {
                totalPremium += premiumPerDay - premiumPerDay * 0.08m;
            }
        }

        return totalPremium;
    }

}

public enum CoverType
{
    Yacht = 0,
    PassengerShip = 1,
    ContainerShip = 2,
    BulkCarrier = 3,
    Tanker = 4
}
