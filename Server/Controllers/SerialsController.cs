﻿using EyE.Server.Controllers.Common;
using EyE.Server.Data;
using EyE.Shared.Enums;
using EyE.Shared.Helpers;
using EyE.Shared.Models.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EyE.Server.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class SerialsController : IMDbController<SerialModel>
    {
        public SerialsController(
            ApplicationDbContext db,
            IHttpClientFactory clientFactory)
            : base(db, clientFactory)
        {
        }

        public override DbSet<SerialModel> GetItems()
        {
            return Db.Serials;
        }
    }
}
