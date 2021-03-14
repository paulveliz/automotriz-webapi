using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using automotriz_webapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace automotriz_webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutosController : ControllerBase
    {
        private readonly automotrizContext Db;

        public AutosController(automotrizContext db)
        {
            this.Db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auto>>> AutosExistentes()
        {
            var autos = await this.Db.Autos.Take(1000).ToListAsync();
            return Ok(autos);
        }
    }
}