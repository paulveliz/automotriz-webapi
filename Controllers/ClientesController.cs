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

        private bool ValidarCurp(string curp) => this.Db.Clientes.Any(cl => cl.Curp == curp);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> ObtenerExistentes(){
            var clientes = await Db.Clientes
                                    .Take(100)
                                    .OrderByDescending(c => c.Id)
                                    .ToListAsync();
            return Ok(clientes);
        }

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


    }
}