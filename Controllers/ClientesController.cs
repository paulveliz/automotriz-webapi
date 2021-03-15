using System;
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
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly automotrizContext Db;

        public ClientesController(automotrizContext db)
        {
            Db = db;
        }

        /* Metodo para validar si la CURP existe en el sistema. */
        private bool ValidarCurp(string curp) => this.Db.Clientes.Any(cl => cl.Curp == curp);

        /* EP para obtener los clientes existentes en el sistema. */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> ObtenerExistentes(){
            var clientes = await Db.Clientes
                                    .Take(100)
                                    .OrderByDescending(c => c.Id)
                                    .ToListAsync();
            return Ok( clientes);
        }

        /* EP para Crear un nuevo cliente en el sistema. */
        [HttpPost]
        [Route("nuevo")]
        public async Task<ActionResult<Cliente>> NuevoCliente([FromBody]Cliente cliente){
            // Validamos el body
            if(!TryValidateModel(cliente)){
                return BadRequest();
            }

            if(ValidarCurp(cliente.Curp)){
                return UnprocessableEntity(new {
                    code = 422,
                    msg = $"La curp <{cliente.Curp}>  ya existe en el sistema."
                });
            }
            
            var nuevoCliente = this.Db.Clientes.Add(cliente);
            await this.Db.SaveChangesAsync();
            return Ok(nuevoCliente.Entity);
        }

        [HttpGet]
        [Route("{clienteId}/solicitudes")]
        public async Task<IActionResult> ObtenerSolicitudesDeCliente(int clienteId){
            var cliente = await Db.Clientes
                                .Include(cl => cl.Solicitudes)
                                .Include("Solicitudes.IdPlanFinanciamientoNavigation")
                                .FirstOrDefaultAsync(cl => cl.Id == clienteId);

            if(cliente != null && cliente.Solicitudes.Count > 0){
                var sProcessed = cliente.Solicitudes
                                        .Select(newSol =>
                    new {
                        id_solicitud = newSol.Id,
                        id_cliente = newSol.IdCliente,
                        fecha_solicitud = newSol.Fecha,
                        aprobado = newSol.Aprobado,
                        plan_financiamiento_sugerido = (newSol.IdPlanFinanciamientoNavigation != null) ? new {
                            id_plan = newSol.IdPlanFinanciamientoNavigation.Id,
                            descipcion = newSol.IdPlanFinanciamientoNavigation.Descripcion,
                            precio_inicial = newSol.IdPlanFinanciamientoNavigation.PrecioInicial,
                            precio_limite = newSol.IdPlanFinanciamientoNavigation.PrecioLimite
                        } : null

                    }
                );
                return Ok(new {
                    cliente = cliente.Curp,
                    total_solicitudes = cliente.Solicitudes.Count,
                    solicitudes_aprobadas = cliente.Solicitudes.Where(sl => sl.Aprobado == true).Count(),
                    solicitudes_no_aprobadas = cliente.Solicitudes.Where(sl => sl.Aprobado == false).Count(),
                    solicitudes = sProcessed
                });
            }else{
                return NotFound(new {
                    code = 404,
                    msg = "No se encontraron registros para el cliente solicitado."
                });
            }
        }

        [HttpPost]
        [Route("{clienteId}/hijos/nuevo")]
        public async Task<IActionResult> AddHijos([FromRoute]int clienteId, [FromBody]IEnumerable<Hijo> hijos){
            var clienteExists = Db.Clientes.Any(cl => cl.Id == clienteId);
            if(!clienteExists) return BadRequest(new {
                        code = 400,
                        msg = "El cliente no existe en el sistema."
                    });
            foreach (var hijo in hijos)
            {
                if(hijo.IdCliente != clienteId){
                    return BadRequest(new {
                        code = 400,
                        msg = "Los hijos deben depender del cliente seleccionado."
                    });
                }
            }
            await Db.AddRangeAsync(hijos);
            var aff = await Db.SaveChangesAsync();
            if(aff > 0){
                var cliente = await Db.Clientes
                                    .Include(cl => cl.Hijos)
                                    .FirstOrDefaultAsync(cl => cl.Id == clienteId);
                var hijosCliente = cliente.Hijos.Select(hc =>
                    new {
                        id_hijo = hc.Id,
                        nombre_completo = hc.NombreCompleto,
                        fecha_nacimiento = hc.FechaNacimiento,
                        edad = hc.Edad,
                        trabaja = hc.Trabaja,
                    }
                );
                return Ok(new {
                    cliente = cliente.Curp,
                    cantidad_hijos = hijos.Count(),
                    hijos = hijosCliente
                });
            }else{
                return NotFound("No se pudieron crear los hijos.");
            }

        }

        [HttpGet]
        [Route("{clienteId}/hijos")]
        public async Task<IActionResult> ObtenerHijos([FromRoute]int clienteId){
            var cliente = await Db.Clientes
                    .Include(cl => cl.Hijos)
                    .FirstOrDefaultAsync(cl => cl.Id == clienteId);
            if(cliente == null) return BadRequest(new {
                code = 400,
                msg = "El cliente proporcionado no existe en el sistema."
            });
            var hijosCliente = cliente.Hijos.Select(hc =>
                new {
                    id_hijo = hc.Id,
                    nombre_completo = hc.NombreCompleto,
                    fecha_nacimiento = hc.FechaNacimiento,
                    edad = hc.Edad,
                    trabaja = hc.Trabaja,
                }
            );
            return Ok(new {
                cliente = cliente.Curp,
                cantidad_hijos = hijosCliente.Count(),
                hijos = hijosCliente
            });
        }

    }
}