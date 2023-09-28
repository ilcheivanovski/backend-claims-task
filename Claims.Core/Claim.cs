using Newtonsoft.Json;

namespace Claims.Core
{
    public class Claim
    {
        public Claim()
        {

        }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }
        public string CoverId { get; private set; }
        public DateTime Created { get; private set; }
        public string Name { get; private set; }
        public ClaimType Type { get; private set; }
        public decimal DamageCost { get; private set; }

        public void Add(string coverId, string name, ClaimType type, decimal damageCost)
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.UtcNow;
            CoverId = coverId;
            Name = name;
            Type = type;
            DamageCost = damageCost;
        }
    }



    public enum ClaimType
    {
        Collision = 0,
        Grounding = 1,
        BadWeather = 2,
        Fire = 3
    }
}