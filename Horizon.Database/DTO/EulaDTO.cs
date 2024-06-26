﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Horizon.Database.DTO
{
    public class AddEulaDTO
    {
        public MediusPolicyTypeDTO EulaType { get; set; }
        public string EulaTitle { get; set; }
        public string EulaBody { get; set; }
        public DateTime? FromDt { get; set; }
        public DateTime? ToDt { get; set; }
        public int AppId { get; set; }
    }

    public class ChangeEulaDTO
    {
        public int Id { get; set; }
        public MediusPolicyTypeDTO EulaType { get; set; }
        public string EulaTitle { get; set; }
        public string EulaBody { get; set; }
        public DateTime? FromDt { get; set; }
        public DateTime? ToDt { get; set; }
        public int AppId { get; set; }

    }

    #region Policy Type
    public enum MediusPolicyTypeDTO : int
    {
        Usage,
        Privacy,
    }
    #endregion
}
