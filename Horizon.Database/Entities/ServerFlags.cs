using System;
using System.Collections.Generic;

namespace Horizon.Database.Entities
{
    public partial class ServerFlags
    {
        public int Id { get; set; }
        public string ServerFlag { get; set; }
        public string Value { get; set; }
        public DateTime? FromDt { get; set; }
        public DateTime? ToDt { get; set; }
    }
}
