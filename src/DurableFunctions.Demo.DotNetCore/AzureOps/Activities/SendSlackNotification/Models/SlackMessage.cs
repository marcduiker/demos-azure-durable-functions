using Newtonsoft.Json;

namespace DurableFunctions.Demo.DotNetCore.AzureOps.Activities.SendSlackNotification.Models
{
    public sealed class SlackMessage
    {
        public string Text { get; set; }

        public Attachment[] Attachments { get; set; }
    }

    public sealed class Attachment
    {
        public string Fallback { get; set; }

        public string Text { get; set; }

        public string Color { get; set; }

        public Field[] Fields { get; set; }

        [JsonProperty("ts")]
        public long DateTime { get; set; }
    }

    public sealed class Field
    {
        public string Title { get; set; }

        public object Value { get; set; }

        public bool Short { get; set; }
    }

    /*
     *  "fallback": "Required plain-text summary of the attachment.",
            "color": "#36a64f",
            "pretext": "Optional text that appears above the attachment block",
            "author_name": "Bobby Tables",
            "author_link": "http://flickr.com/bobby/",
            "author_icon": "http://flickr.com/icons/bobby.jpg",
            "title": "Slack API Documentation",
            "title_link": "https://api.slack.com/",
            "text": "Optional text that appears within the attachment",
            "fields": [
                {
                    "title": "Priority",
                    "value": "High",
                    "short": false
                }
            ],
            "image_url": "http://my-website.com/path/to/image.jpg",
            "thumb_url": "http://example.com/path/to/thumb.png",
            "footer": "Slack API",
            "footer_icon": "https://platform.slack-edge.com/img/default_application_icon.png",
            "ts": 123456789

    */
}
