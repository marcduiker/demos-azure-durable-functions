using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.AzureOps.Activities.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Environment = System.Environment;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities
{
    public static class SendSlackNotification
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        [FunctionName(nameof(SendSlackNotification))]
        public static async Task Run(
            [ActivityTrigger] DurableActivityContext activityContext,
            ILogger logger)
        {
            var input = activityContext.GetInput<SendSlackNotificationInput>();

            try
            {
                string slackUri = Environment.GetEnvironmentVariable("SlackWebHook");
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
