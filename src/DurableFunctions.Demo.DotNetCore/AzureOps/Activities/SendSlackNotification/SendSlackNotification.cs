using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.SendSlackNotification.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Environment = System.Environment;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.SendSlackNotification
{
    public static class SendSlackNotification
    {
        public static string EnvironmentVariableSlackWebHook = "SlackWebHook";

        private static HttpClient _httpClient;
        public static HttpClient HttpClient {
            get { return _httpClient = _httpClient ?? new HttpClient(); }
            set => _httpClient = value;
        }
        
        [FunctionName(nameof(SendSlackNotification))]
        public static async Task Run(
            [ActivityTrigger] SendSlackNotificationInput input,
            ILogger logger)
        {
            try
            {
                string slackUri = Environment.GetEnvironmentVariable(EnvironmentVariableSlackWebHook);
                var slackMessage = MapInputToSlackMessage(input);

                await HttpClient.PostAsJsonAsync(slackUri, slackMessage);

            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }

        private static SlackMessage MapInputToSlackMessage(SendSlackNotificationInput input)
        {
            var result = new SlackMessage
            {
                Text = input.Message,
                Attachments = input.CreatedResources.Select(createdResource => new Attachment
                {
                    DateTime = DateTime.Now.Ticks,
                    Color = "#00FF00",
                    Fallback = $"Created resourcegroup with {createdResource.ResourceGroup}",
                    Fields = new[] {
                        new Field
                        {
                            Title = "Resouce",
                            Value = createdResource.Name,
                            Short = false
                        },
                        new Field
                        {
                            Title = "Resource ID",
                            Value = createdResource.Id,
                            Short = false
                        },
                        new Field
                        {
                            Title = "Resource Group Name",
                            Value = createdResource.ResourceGroup,
                            Short = false
                        },
                        new Field
                        {
                            Title = "Environment",
                            Value = createdResource.Environment,
                            Short = false
                        }
                        ,
                        new Field
                        {
                            Title = "Region",
                            Value = createdResource.Region,
                            Short = false
                        }
                    }
                        
                }).ToArray()
            };

            return result;
        }
    }
}
