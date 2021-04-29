using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using System.IO;
using System.Text;
using automotriz_webapi.Models;

namespace automotriz_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        [HttpPost]
        [Route("enganche")]
        public FileContentResult GenerateReport([FromBody]ReportUrl data){
            // System.Console.WriteLine(data.url);
            // TODO HTTP RESPONSE
            var client = new RestClient("https://api.pdfshift.io/v3/convert/pdf")
            {
                Authenticator = new HttpBasicAuthenticator("api", "4a274c5975a8475ea957379e2a84e03a")
            };

            var request = new RestRequest(Method.POST);

            var json = new
            {
                // like: http://localhost:4200/#/reporte/enganche/automovil/1/plan/1/cliente/11
                delay = 10000,
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
                // WriteAllBytes("wikipedia.pdf", response.RawBytes);
               var file = File(response.RawBytes, "application/pdf", "new.pdf");
               return file;
            }
            return null;
        }
    }
    
}