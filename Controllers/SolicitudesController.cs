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
    public class SolicitudesController:ControllerBase
    {
        private readonly automotrizContext Db;

        public SolicitudesController(automotrizContext db)
        {
            this.Db = db;
        }

        private async Task<(PlanesFinanciamiento plan, Solicitude solicitud, decimal? ingresoAcumulable)> ObtenerSugerido(int porcentaje, Cliente cliente, Solicitude solicitud){
                var ingresoAcumulable = cliente.IngresosMensuales * porcentaje / 100;
                var planSugerido = await Db.PlanesFinanciamientos
                                    .Where(pl => pl.MinIngresoAcumulable <= ingresoAcumulable)
                                    .OrderByDescending(pl => pl.PrecioLimite)
                                    .ToListAsync();
                solicitud.IdCliente = cliente.Id;
                solicitud.Aprobado = true;
                solicitud.IdPlanFinanciamiento = planSugerido[0].Id;
                return (planSugerido[0], solicitud, ingresoAcumulable);
        }

        /* Crear nueva solicitud, ejecutar algoritmo de asignacion de plan sugerido. */
        [HttpPost]
        [Route("cliente/{clienteId}")]
        public async Task<IActionResult> NuevaSolicitud([FromRoute]int clienteId){
            var solicitud  = new Solicitude();
            var cliente = await this.Db.Clientes
                                    .Include(cl => cl.IdEstadoCivilNavigation)
                                    .Include(cl => cl.Hijos)
                                    .Include(cl => cl.Solicitudes)
                                    .FirstOrDefaultAsync(cl => cl.Id == clienteId);
            // Do algorithm for sugeridos
            if(cliente.IdEstadoCivilNavigation.Id == 1){
                // Está soltero.
                var sugeridoResponse = await ObtenerSugerido(80, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                return Ok(new {
                    id_solicitud = solicitudResponse.Entity.Id,
                    fecha_solicitud = solicitudResponse.Entity.Fecha,
                    cliente = new {
                        id_cliente = solicitudResponse.Entity.IdClienteNavigation.Id,
                        curp = solicitudResponse.Entity.IdClienteNavigation.Curp,
                        nombre_completo = solicitudResponse.Entity.IdClienteNavigation.NombreCompleto,
                        ingreso_acumulable_actual = sugeridoResponse.ingresoAcumulable,
                    },
                    resultados = new {
                        aprobado = solicitudResponse.Entity.Aprobado,
                        plan_sugerido = new {
                            id_plan = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.Id,
                            descripcion = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.Descripcion,
                            precio_inicial = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.PrecioInicial,
                            precio_limite = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.PrecioLimite,
                            min_ingreso_acumulable = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.MinIngresoAcumulable
                        }
                    }
                });
            }else if (cliente.IdEstadoCivilNavigation.Id == 2 && cliente.Hijos.Count == 0){
                // Está casado y no tiene hijos.
                var sugeridoResponse = await ObtenerSugerido(70, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                return Ok(new {
                    id_solicitud = solicitudResponse.Entity.Id,
                    fecha_solicitud = solicitudResponse.Entity.Fecha,
                    cliente = new {
                        id_cliente = solicitudResponse.Entity.IdClienteNavigation.Id,
                        curp = solicitudResponse.Entity.IdClienteNavigation.Curp,
                        nombre_completo = solicitudResponse.Entity.IdClienteNavigation.NombreCompleto,
                        ingreso_acumulable_actual = sugeridoResponse.ingresoAcumulable,
                    },
                    resultados = new {
                        aprobado = solicitudResponse.Entity.Aprobado,
                        plan_sugerido = new {
                            id_plan = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.Id,
                            descripcion = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.Descripcion,
                            precio_inicial = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.PrecioInicial,
                            precio_limite = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.PrecioLimite,
                            min_ingreso_acumulable = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.MinIngresoAcumulable
                        }
                    }
                });
            }else if (cliente.IdEstadoCivilNavigation.Id == 2 && cliente.Hijos.Count == 1){
                // Está casado y tiene 1 hijo.
                var sugeridoResponse = await ObtenerSugerido(60, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                return Ok(new {
                    id_solicitud = solicitudResponse.Entity.Id,
                    fecha_solicitud = solicitudResponse.Entity.Fecha,
                    cliente = new {
                        id_cliente = solicitudResponse.Entity.IdClienteNavigation.Id,
                        curp = solicitudResponse.Entity.IdClienteNavigation.Curp,
                        nombre_completo = solicitudResponse.Entity.IdClienteNavigation.NombreCompleto,
                        ingreso_acumulable_actual = sugeridoResponse.ingresoAcumulable,
                    },
                    resultados = new {
                        aprobado = solicitudResponse.Entity.Aprobado,
                        plan_sugerido = new {
                            id_plan = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.Id,
                            descripcion = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.Descripcion,
                            precio_inicial = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.PrecioInicial,
                            precio_limite = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.PrecioLimite,
                            min_ingreso_acumulable = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.MinIngresoAcumulable
                        }
                    }
                });
            }else if (cliente.IdEstadoCivilNavigation.Id == 2 && cliente.Hijos.Count == 2){
                // Está casado y tiene 2 hijos.
                var sugeridoResponse = await ObtenerSugerido(55, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                return Ok(new {
                    id_solicitud = solicitudResponse.Entity.Id,
                    fecha_solicitud = solicitudResponse.Entity.Fecha,
                    cliente = new {
                        id_cliente = solicitudResponse.Entity.IdClienteNavigation.Id,
                        curp = solicitudResponse.Entity.IdClienteNavigation.Curp,
                        nombre_completo = solicitudResponse.Entity.IdClienteNavigation.NombreCompleto,
                        ingreso_acumulable_actual = sugeridoResponse.ingresoAcumulable,
                    },
                    resultados = new {
                        aprobado = solicitudResponse.Entity.Aprobado,
                        plan_sugerido = new {
                            id_plan = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.Id,
                            descripcion = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.Descripcion,
                            precio_inicial = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.PrecioInicial,
                            precio_limite = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.PrecioLimite,
                            min_ingreso_acumulable = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.MinIngresoAcumulable
                        }
                    }
                });
            }else if (cliente.IdEstadoCivilNavigation.Id == 2 && cliente.Hijos.Count >= 3){
                // Está casado y tiene 3 hijos o más.
                var sugeridoResponse = await ObtenerSugerido(50, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                return Ok(new {
                    id_solicitud = solicitudResponse.Entity.Id,
                    fecha_solicitud = solicitudResponse.Entity.Fecha,
                    cliente = new {
                        id_cliente = solicitudResponse.Entity.IdClienteNavigation.Id,
                        curp = solicitudResponse.Entity.IdClienteNavigation.Curp,
                        nombre_completo = solicitudResponse.Entity.IdClienteNavigation.NombreCompleto,
                        ingreso_acumulable_actual = sugeridoResponse.ingresoAcumulable,
                    },
                    resultados = new {
                        aprobado = solicitudResponse.Entity.Aprobado,
                        plan_sugerido = new {
                            id_plan = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.Id,
                            descripcion = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.Descripcion,
                            precio_inicial = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.PrecioInicial,
                            precio_limite = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.PrecioLimite,
                            min_ingreso_acumulable = solicitudResponse.Entity.IdPlanFinanciamientoNavigation.MinIngresoAcumulable
                        }
                    }
                });
            }
            
            return NotFound();
            
        }



    }
}