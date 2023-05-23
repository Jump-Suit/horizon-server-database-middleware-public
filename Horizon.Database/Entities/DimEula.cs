using System;
using System.Collections.Generic;

namespace Horizon.Database.Entities
{
    public partial class DimEula
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public int PolicyType { get; set; }
        public string EulaTitle { get; set; }
        public string EulaBody { get; set; }
        public DateTime CreateDt { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public DateTime FromDt { get; set; }
        public DateTime? ToDt { get; set; }
    }

    #region Policy Type
    public enum MediusPolicyType : int
    {
        Usage,
        Privacy,
    }
    #endregion
}
