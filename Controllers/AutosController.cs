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
                                    .Include(a => a.IdModeloNavigation.IdMarcaNavigation)
                                    .Include(a => a.IdPlanFinanciamientoNavigation)
                                    .Take(1000)
                                    .OrderByDescending(d => d.Id)
                                    .ToListAsync();
            return Ok(autos);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Auto>> PorId(int id)
        {
            var auto = await this.Db.Autos
                                    .Include(a => a.IdModeloNavigation.IdMarcaNavigation)
                                    .Include(a => a.IdPlanFinanciamientoNavigation)
                                    .FirstOrDefaultAsync(a => a.Id == id);
            if(auto == null){
                return NotFound(new {
                    code = 404,
                    msg = $"El automovil con id: <{id}> no existe en el sistema."
                });
            }else{
                return auto;
            }
        }

        [HttpGet]
        [Route("marca/{id}")]
        public async Task<ActionResult<Auto>> PorMarcaId(int id)
        {
            var autos = await this.Db.Autos
                                .Include(a => a.IdModeloNavigation.IdMarcaNavigation)
                                .Include(a => a.IdPlanFinanciamientoNavigation)
                                .Where(a => a.IdModeloNavigation.IdMarca == id)
                                .OrderByDescending(a => a.Id)
                                .ToListAsync();
            if(autos == null){
                return NotFound(new {
                    code = 404,
                    msg = $"La marca id: <{id}> no existe en el sistema."
                });
            }else{
                return Ok(autos);
            }
        }

        [HttpGet]
        [Route("modelo/{id}")]
        public async Task<ActionResult<Auto>> PorModeloId(int id)
        {
            var autos = await this.Db.Autos
                                .Include(a => a.IdModeloNavigation.IdMarcaNavigation)
                                .Include(a => a.IdPlanFinanciamientoNavigation)
                                .Where(a => a.IdModeloNavigation.Id == id)
                                .OrderByDescending(a => a.Id)
                                .ToListAsync();
            if(autos == null){
                return NotFound(new {
                    code = 404,
                    msg = $"La marca id: <{id}> no existe en el sistema."
                });
            }else{
                return Ok(autos);
            }
        }

    }
}