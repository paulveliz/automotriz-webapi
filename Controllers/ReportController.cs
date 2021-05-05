using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using System.IO;
using System.Text;
using automotriz_webapi.Models;
using System.Collections;
using System.Collections.Generic;
using System;

namespace automotriz_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly automotrizContext Db;
        public ReportController(automotrizContext db)
        {
            this.Db = db;
        }

        [HttpPost]
        [Route("enganche")]
        public async Task<FileContentResult> GenerateReport([FromBody](ReportUrl data, Financiamiento financiamiento, Deuda deuda) req)
        {
            // request: ReportUrl data && Financiamiento financiamiento.
            var data = req.data;
            var financiamiento = req.financiamiento;
            var deuda = req.deuda;
            // System.Console.WriteLine(data.url);
            // TODO HTTP RESPONSE
            var client = new RestClient("https://api.pdfshift.io/v3/convert/pdf")
            {
                Authenticator = new HttpBasicAuthenticator("api", "dc0eae382d9640f38826da5d20cbd8d1")
            };

            var request = new RestRequest(Method.POST);

            var json = new
            {
                // like: http://localhost:4200/#/reporte/enganche/automovil/1/plan/1/cliente/11
                source = data.url
            };

            request.AddJsonBody(json);

            var response = client.Execute(request);
            if (!response.IsSuccessful)
            {
                // Check why status is not int 2xx.
            }
            else
            {
                // Crear financiamiento
                var newFinanciamiento = await this.Db.Financiamientos.AddAsync(financiamiento);
                await this.Db.SaveChangesAsync();
                deuda.IdFinanciamiento = newFinanciamiento.Entity.Id;
                await this.Db.Deudas.AddAsync(deuda);
                await this.Db.SaveChangesAsync();
                // WriteAllBytes("wikipedia.pdf", response.RawBytes);
                var file = File(response.RawBytes, "application/pdf", "new.pdf");
                return file;
            }
            return null;
        }
    }

}