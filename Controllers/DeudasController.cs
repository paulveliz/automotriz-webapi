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
    public class DeudasController : ControllerBase
    {
        private readonly automotrizContext Db;
        public DeudasController(automotrizContext db)
        {
            this.Db = db;
        }

        [HttpGet]
        [Route("{curp}")]
        public async Task<IEnumerable<Deuda>> ObtenerDeudasClienteCurp([FromRoute]string curp){
            var response = await this.Db.Deudas
                                    .Include(d => d.IdClienteNavigation)
                                    .Where(d => 
                                        d.IdClienteNavigation.Curp == curp
                                    ).ToListAsync();
            return response;
        }

        [HttpPost]
        [Route("nueva")]
        public async Task<Deuda> CrearNueva([FromBody]Deuda deuda){
            var result = await this.Db.AddAsync(deuda);
            return result.Entity;
        }

    }
}