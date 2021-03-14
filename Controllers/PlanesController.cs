using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using automotriz_webapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace automotriz_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanesController : ControllerBase
    {
        private readonly automotrizContext Db;

        public PlanesController(automotrizContext db)
        {
            this.Db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanesFinanciamiento>>> Existentes(){
            var planes = await Db.PlanesFinanciamientos
                                    .Take(1000)
                                    .OrderByDescending(p => p.Id)
                                    .ToListAsync();
            return planes;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PlanesFinanciamiento>> PorId(int id){
            var plan = await Db.PlanesFinanciamientos
                                    .Include(p => p.Autos)
                                    .FirstOrDefaultAsync(p => p.Id == id);
            if(plan == null){
                return NotFound(new {
                    code = 404,
                    msg = "Plan no encontrado."
                });
            }else{
                return Ok(plan);
            }
        }

        [HttpGet]
        [Route("{planId}/autos")]
        public async Task<ActionResult<IEnumerable<Auto>>> AutosPorPlanId(int planId){
            var planAutos = await this.Db.Autos
                                        .Where(pl => pl.IdPlanFinanciamiento == planId)
                                        .ToListAsync();
            if(planAutos != null && planAutos.Count>0){
                return Ok(planAutos);
            }
            else {
                return NotFound(new {
                    code = 404,
                    msg = "No se encontraron automoviles para el plan seleccionado."
                });
            }
        }


    }
}