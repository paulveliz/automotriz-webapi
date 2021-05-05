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
                                    .Include(d => d.IdFinanciamientoNavigation)
                                    .Include(d => d.IdSolicitudNavigation)
                                    .Where(d => 
                                        d.IdClienteNavigation.Curp == curp
                                    ).ToListAsync();
            return response;
        }

        [HttpPost]
        [Route("abonar/{deudaId}")]
        public async Task<ActionResult<Deuda>> Abonar([FromRoute]int deudaId){
            // Descontar abono de financiado
            // Reducir mensualidad
            var deuda = await this.Db.Deudas
                                    .Include(d => d.IdFinanciamientoNavigation)
                                    .FirstOrDefaultAsync(d => d.Id == deudaId);
            if(deuda==null) return NotFound(new {msg = "La deuda proporcionada no existe."});
            deuda.IdFinanciamientoNavigation.Meses -= 1;
            deuda.IdFinanciamientoNavigation.CantidadAFinanciar -= deuda.IdFinanciamientoNavigation.Mensualidad;
            deuda.UltimoAbono = DateTime.Now;
            await this.Db.SaveChangesAsync();
            return Ok(deuda);
        }

    }
}