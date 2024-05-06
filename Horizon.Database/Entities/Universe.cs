using System;

namespace Horizon.Database.Entities
{
    public class Universe
    {
        public int AppId { get; set; }
        public int UniverseID { get; set; }
        public string UniverseName { get; set; }
        public string UniverseDescription { get; set; }
        public string DNS { get; set; }
        public int Port { get; set; }
        public int Status { get; set; }
        public int UserCount { get; set; }
        public int MaxUsers { get; set; }
        public string UniverseBilling { get; set; }
        public string BillingSystemName { get; set; }
        public string ExtendedInfo { get; set; }
        public string SvoURL { get; set; }

        public DateTime CreateDt { get; set; }
        public DateTime ModifiedDt { get; set; }
    }
}
