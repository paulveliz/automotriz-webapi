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
        public async Task<IActionResult> Existentes()
        {
            var autos = await this.Db.Autos
                                    .Include(a => a.IdModeloNavigation)
                                    .Include(a => a.IdModeloNavigation.IdMarcaNavigation)
                                    .Include(a => a.IdPlanFinanciamientoNavigation)
                                    .OrderByDescending(d => d.Id)
                                    .ToListAsync();
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
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> PorId(int id)
        {
            var auto = await this.Db.Autos
                                    .Include(a => a.IdModeloNavigation)
                                    .Include(a => a.IdModeloNavigation.IdMarcaNavigation)
                                    .Include(a => a.IdPlanFinanciamientoNavigation)
                                    .FirstOrDefaultAsync(a => a.Id == id);
            if(auto == null){
                return NotFound(new {
                    code = 404,
                    msg = $"El automovil con id: <{id}> no existe en el sistema."
                });
            }else{
                var autoProcessed = new {
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
                };
                return Ok(autoProcessed);
            }
        }

        [HttpGet]
        [Route("marca/{id}")]
        public async Task<ActionResult<Auto>> PorMarcaId(int id)
        {
            var autos = await this.Db.Autos
                                .Include(a => a.IdModeloNavigation)
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
            }
        }

        [HttpGet]
        [Route("modelo/{id}")]
        public async Task<ActionResult<Auto>> PorModeloId(int id)
        {
            var autos = await this.Db.Autos
                                .Include(a => a.IdModeloNavigation)
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
            }
        }

    }
}