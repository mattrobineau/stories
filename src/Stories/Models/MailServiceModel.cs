using System.Collections.Generic;

namespace Stories.Models
{
    public class MailServiceModel
    {
        public string From { get; set; }
        public List<string> To { get; set; }
        public List<string> CC { get; set; } = new List<string>();
        public List<string> BCC { get; set; } = new List<string>();
        public string Subject { get; set; }

        /// <summary>
        /// Body of the message (text version)
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Body of the message. (HTML version)
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// File attachment. You must use multipart/form-data encoding when sending attachments
        /// </summary>
        public List<string> Attachment { get; set; } = new List<string>();

        public Dictionary<string, List<string>> Headers { get; set; } = new Dictionary<string, List<string>>();

        public Dictionary<string, List<string>> MimeHeaders { get; set; } = new Dictionary<string, List<string>>();
    }
}
