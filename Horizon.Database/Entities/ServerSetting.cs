using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Horizon.Database.Entities
{
    public class ServerSetting
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
