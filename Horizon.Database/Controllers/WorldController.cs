using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Horizon.Database.DTO;
using Horizon.Database.Models;
using Horizon.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorldController : ControllerBase
    {
        private Ratchet_DeadlockedContext db;
        public WorldController(Ratchet_DeadlockedContext _db)
        {
            db = _db;
        }

        [Authorize("database")]
        [HttpGet, Route("getChannels")]
        public async Task<dynamic> getChannels()
        {
            var channels = db.Channels.ToList();

            return channels;
        }

        [Authorize("database")]
        [HttpGet, Route("getLocations")]
        public async Task<dynamic> getLocations()
        {
            var locations = db.Locations.ToList();

            return locations;
        }

        [Authorize("database")]
        [HttpGet, Route("getLocations/{appId}")]
        public async Task<dynamic> getLocations(int appId)
        {
            var app_id_group = (from a in db.DimAppIds
                                where a.AppId == appId
                                select a.GroupId).FirstOrDefault();

            var app_ids_in_group = (from a in db.DimAppIds
                                    where (a.GroupId == app_id_group && a.GroupId != null) || a.AppId == appId
                                    select a.AppId).ToList();

            var locations = db.Locations.Where(x => app_ids_in_group.Contains(x.AppId)).ToList();

            return locations.GroupBy(x => x.Id).Select(x => x.FirstOrDefault());
        }

    }
}
