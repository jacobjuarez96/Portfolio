using BlazorApp.Shared;
using BlazorApp.Shared.Interfaces;
using BlazorApp.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Functions
{
    public class SendEmailFunction
    {
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;

        public SendEmailFunction(IEmailService emailService, ILoggerFactory loggerFactory)
        {
            _emailService = emailService;
            _logger = loggerFactory.CreateLogger<SendEmailFunction>();
        }

        [Function("SendEmail")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "sendemail")] HttpRequestData req)
        {
            var response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);

            try
            {
                //var test = req.Body;
                //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                //_logger.LogInformation($"Raw request body: {requestBody}");
                // Read and deserialize the contact form data from the request
                // var contactFormModel = await JsonSerializer.DeserializeAsync<ContactFormModel>(req.Body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var contactFormModel = await JsonSerializer.DeserializeAsync<ContactFormModel>(req.Body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (contactFormModel != null)
                {
                    bool success = await _emailService.SendEmail(contactFormModel);
                    if (success)
                    {
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                        response.WriteString("Email sent successfully.");
                    }
                    else
                    {
                        response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                        response.WriteString("Failed to send email.");
                    }
                }
                else
                {
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.WriteString("Invalid contact form data.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing email request: {ex.Message}");
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.WriteString("An error occurred while sending the email.");
            }

            return response;
        }
    }
}