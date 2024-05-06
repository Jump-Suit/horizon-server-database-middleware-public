using System;

namespace Horizon.Database.Entities
{
    public class UniverseNews
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string News { get; set; }
        public DateTime CreateDt { get; set; }
        public DateTime ModifiedDt { get; set; }
    }
}
