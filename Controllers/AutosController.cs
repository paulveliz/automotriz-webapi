using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using automotriz_webapi.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace automotriz_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutosController : ControllerBase
    {
        private readonly automotrizContext Db;

        public AutosController(automotrizContext db)
        {
            this.Db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auto>>> Existentes()
        {
            var autos = await this.Db.Autos
                                    .Take(1000)
                                    .OrderByDescending(d => d.Id)
                                    .ToListAsync();
            return Ok(autos);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Auto>> PorId(int id)
        {
            var auto = await this.Db.Autos.FirstOrDefaultAsync(a => a.Id == id);
            if(auto == null){
                return NotFound(new {
                    code = 404,
                    msg = $"El automovil con id: <{id}> no existe en el sistema."
                });
            }else{
                return auto;
            }
        }

    }
}