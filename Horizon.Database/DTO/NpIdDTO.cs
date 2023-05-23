using System;

namespace Horizon.Database.DTO
{
    public partial class NpIdDTO
    {
        public int AppId { get; set; }
        public byte[] data { get; set; }
        public byte term { get; set; }
        public byte[] dummy { get; set; }

        public byte[] opt { get; set; }
        public byte[] reserved { get; set; }

        public DateTime CreateDt { get; set; }
        public DateTime ModifiedDt { get; set; }
    }
}