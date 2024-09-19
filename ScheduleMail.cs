using System;
using System.Net.Mail;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FunctionScheduleMail
{
    public class ScheduleMail
    {
        private readonly ILogger _logger;

        public ScheduleMail(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ScheduleMail>();
        }

        [Function("ScheduleMail")]
        public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var apiKey = Environment.GetEnvironmentVariable("apikey");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("email@email.com");
            var subject = "Scheduled Email";
            var to = new EmailAddress("receptor@email.com");
            var plainTextContent = "This is a scheduled email sent every minute.";
            var htmlContent = "<strong>This is a scheduled email sent every minute.</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
