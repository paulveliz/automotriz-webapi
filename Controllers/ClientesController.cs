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
            return Ok(clientes);
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

    }
}