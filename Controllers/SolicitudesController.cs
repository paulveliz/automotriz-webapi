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

        /* Crear nueva solicitud, ejecutar algoritmo de asignacion de plan sugerido. */
        [HttpPost]
        [Route("/cliente/{clienteId}")]
        public async Task<IActionResult> NuevaSolicitud([FromRoute]int clienteId, [FromBody]Solicitude solicitud){
            if(!TryValidateModel(solicitud)) return BadRequest();
            var cliente = await this.Db.Clientes
                                    .Include(cl => cl.Hijos)
                                    .Include(cl => cl.Solicitudes)
                                    .FirstOrDefaultAsync(cl => cl.Id == clienteId);
            // Do algorithm for sugeridos
            var response = await Db.Solicitudes.AddAsync(solicitud);
            return Ok(response.Entity);
        }



    }
}