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
    public string Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public CoverType Type { get; set; }

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
        int baseDateRate = 1250;

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

        var premiumPerDay = baseDateRate * multiplier;
        var insuranceDurationInDays = endDate.DayNumber - startDate.DayNumber;
        var totalPremium = 0m;

        for (var i = 0; i < insuranceDurationInDays; i++)
        {
            if (i < 30)
            {
                totalPremium += premiumPerDay;
            }

            else if (i < 180 && coverType == CoverType.Yacht)
            {
                totalPremium += premiumPerDay - premiumPerDay * 0.05m;
            }

            else if (i < 180)
            {
                totalPremium += premiumPerDay - premiumPerDay * 0.02m;
            }

            else if (i < 365 && coverType == CoverType.Yacht)
            {
                totalPremium += premiumPerDay - premiumPerDay * 0.03m;
            }

            else if (i < 365)
            {
                totalPremium += premiumPerDay - premiumPerDay * 0.01m;
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
