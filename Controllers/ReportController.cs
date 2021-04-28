using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using System.IO;
using System.Text;

namespace automotriz_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        [HttpPost]
        [Route("enganche")]
        public FileContentResult GenerateReport([FromBody] string url){
            // TODO HTTP RESPONSE
            var client = new RestClient("https://api.pdfshift.io/v3/convert/pdf")
            {
                Authenticator = new HttpBasicAuthenticator("api", "<key>")
            };

            var request = new RestRequest(Method.POST);

            var json = new
            {
                source = url.Trim()
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