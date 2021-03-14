using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using automotriz_webapi.Models;
using Microsoft.AspNetCore.Mvc;

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
        
        [HttpGet]
        [Route("sugeridos/{clienteId}")]
        public async Task<ActionResult<IEnumerable<PlanesFinanciamiento>>> ObtenerSugeridos(int clienteId){
            return Ok();
        }



    }
}