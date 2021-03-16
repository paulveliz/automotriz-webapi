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
        public async Task<IActionResult> Existentes(){
            var planes = await Db.PlanesFinanciamientos
                                    .Include(pl => pl.Autos)
                                    .Include("Autos.IdModeloNavigation.IdMarcaNavigation")
                                    .OrderByDescending(p => p.Id)
                                    .ToListAsync();
            var planesProcessed = planes.Select(plan => new {
                id_plan = plan.Id,
                descripcion = plan.Descripcion,
                precio_inicial = plan.PrecioInicial,
                precio_limite = plan.PrecioLimite,
                autos = plan.Autos.Select(automovil => new {
                    id_auto = automovil.Id,
                    valor_comercial = automovil.ValorComecial,
                    url_imagen = automovil.UrlImagen,
                    marca = new {
                        id_marca = automovil.IdModeloNavigation.IdMarcaNavigation.Id,
                        nombre = automovil.IdModeloNavigation.IdMarcaNavigation.Nombre,
                        url_imagen = automovil.IdModeloNavigation.IdMarcaNavigation.UrlImagen
                    },
                    modelo = new {
                        id_modelo = automovil.IdModeloNavigation.Id,
                        nombre = automovil.IdModeloNavigation.Nombre
                    }
                })
            });
            return Ok(planesProcessed);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PlanesFinanciamiento>> PorId(int id){
            var plan = await Db.PlanesFinanciamientos
                                    .Include(pl => pl.Autos)
                                    .Include("Autos.IdModeloNavigation.IdMarcaNavigation")
                                    .FirstOrDefaultAsync(p => p.Id == id);
            if(plan == null){
                return NotFound(new {
                    code = 404,
                    msg = "Plan no encontrado."
                });
            }else{
                var planProcessed = new {
                    id_plan = plan.Id,
                    descripcion = plan.Descripcion,
                    precio_inicial = plan.PrecioInicial,
                    precio_limite = plan.PrecioLimite,
                    autos = plan.Autos.Select(automovil => new {
                        id_auto = automovil.Id,
                        valor_comercial = automovil.ValorComecial,
                        url_imagen = automovil.UrlImagen,
                        marca = new {
                            id_marca = automovil.IdModeloNavigation.IdMarcaNavigation.Id,
                            nombre = automovil.IdModeloNavigation.IdMarcaNavigation.Nombre,
                            url_imagen = automovil.IdModeloNavigation.IdMarcaNavigation.UrlImagen
                        },
                        modelo = new {
                            id_modelo = automovil.IdModeloNavigation.Id,
                            nombre = automovil.IdModeloNavigation.Nombre
                        }
                    })
                };
                return Ok(planProcessed);
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