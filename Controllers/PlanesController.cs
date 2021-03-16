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
        public async Task<IActionResult> AutosPorPlanId(int planId){
                var autos = await this.Db.Autos
                                            .Include(a => a.IdModeloNavigation)
                                            .Include(a => a.IdModeloNavigation.IdMarcaNavigation)
                                            .Include(a => a.IdPlanFinanciamientoNavigation)
                                            .Where(pl => pl.IdPlanFinanciamiento == planId)
                                            .ToListAsync();
            if(autos != null && autos.Count>0){
                                var autosProcessed = autos.Select(auto => new {
                    id_auto = auto.Id,
                    valor_comercial = auto.ValorComecial,
                    url_imagen = auto.UrlImagen,
                    marca = new {
                        id_marca = auto.IdModeloNavigation.IdMarcaNavigation.Id,
                        nombre = auto.IdModeloNavigation.IdMarcaNavigation.Nombre,
                        url_imagen = auto.IdModeloNavigation.IdMarcaNavigation.UrlImagen
                    },
                    modelo = new {
                        id_modelo = auto.IdModeloNavigation.Id,
                        nombre = auto.IdModeloNavigation.Nombre
                    },
                    plan_financiamiento = new {
                        id_plan = auto.IdPlanFinanciamientoNavigation.Id,
                        descripcion = auto.IdPlanFinanciamientoNavigation.Descripcion,
                        precio_inicial = auto.IdPlanFinanciamientoNavigation.PrecioInicial,
                        precio_limite = auto.IdPlanFinanciamientoNavigation.PrecioLimite
                    }
                });
                return Ok(autosProcessed);
            } else {
                return NotFound(new {
                    code = 404,
                    msg = "No se encontraron automoviles para el plan seleccionado."
                });
            }
        }


    }
}