using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Horizon.Database.DTO;
using Horizon.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Horizon.Database.Models;
using Horizon.Database.Services;
using Horizon.Database.Helpers;

namespace Horizon.Database.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UniversesController : ControllerBase
    {
        private Ratchet_DeadlockedContext db;
        public UniversesController(Ratchet_DeadlockedContext _db)
        {
            db = _db;
        }



        [Authorize("database")]
        [HttpPost, Route("addUniverse")]
        public async Task<dynamic> addUniverse([FromBody] UniverseDTO universeToAdd)
        {
            var universes = db.Universes.Where(x => x.AppId == universeToAdd.AppId).FirstOrDefault();

            if (universes.AppId != universeToAdd.AppId 
                && universes.UniverseID != universeToAdd.UniverseID)
            {
                var newUniverse = new Universe()
                {
                    AppId = universeToAdd.AppId,
                    UniverseName = universeToAdd.UniverseName,
                    UniverseDescription = universeToAdd.UniverseDescription,
                    DNS = universeToAdd.DNS,
                    Port = universeToAdd.Port,
                    Status = universeToAdd.Status,
                    UserCount = universeToAdd.UserCount,
                    MaxUsers = universeToAdd.MaxUsers,
                    UniverseBilling = universeToAdd.UniverseBilling,
                    BillingSystemName = universeToAdd.BillingSystemName,
                    ExtendedInfo = universeToAdd.ExtendedInfo,
                    SvoURL = universeToAdd.SvoURL,
                    CreateDt = DateTime.Now,
                };

                db.Universes.Add(universes);
                return universes;
            }
            else
            {
                return StatusCode(403, $"Universe already exists with appId {universeToAdd.AppId}");
            }
        }

        [Authorize("database")]
        [HttpGet, Route("getUniverses")]
        public async Task<dynamic> getUniverses(int appId)
        {
            var universes = db.Universes.Where(x => x.AppId == appId).ToList();

            return universes;
        }

        [Authorize("database")]
        [HttpGet, Route("getUniverseNews")]
        public async Task<dynamic> getUniverseNews(int appId)
        {
            var universeNews = db.UniverseNews.Where(x => x.AppId == appId).ToList();

            return universeNews;
        }
    }
}
