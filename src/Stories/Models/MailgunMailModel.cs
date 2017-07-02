using System;
using System.Collections.Generic;

namespace Stories.Models
{
    public class MailgunMailModel : MailServiceModel
    {
        /// <summary>
        /// Attachment with inline disposition.
        /// https://documentation.mailgun.com/user_manual.html#sending-via-api
        /// </summary>
        public List<string> Inlines { get; set; } = new List<string>();

        /// <summary>
        /// Tag string.
        /// https://documentation.mailgun.com/user_manual.html#tagging
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// Id of the campaign the message belongs to.
        /// </summary>
        public string Campaign { get; set; }

        /// <summary>
        /// Enables/disabled DKIM signatures on per-message basis.
        /// </summary>
        public bool? Dkim { get; set; }

        /// <summary>
        /// Desired time of delivery.
        /// Messages can be scheduled for a max of 3 days in the future.
        /// See Date Format: https://documentation.mailgun.com/api-intro.html#date-format
        /// </summary>
        public DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// Toggles tracking on a per-message basis.
        /// See https://documentation.mailgun.com/user_manual.html#tracking-messages
        /// </summary>
        public bool? Tracking { get; set; }

        /// <summary>
        /// Toggles clicks tracking on a per-message basis. 
        /// Has higher priority than domain-level setting. 
        /// Pass yes, no or htmlonly.
        /// </summary>
        public string TrackingClicks { get; set; }

        /// <summary>
        /// Toggles opens tracking on a per-message basis. 
        /// Has higher priority than domain-level setting. 
        /// Pass yes or no.
        /// </summary>
        public bool?  TrackingOpens { get; set; }

        /// <summary>
        /// If set to True this requires the message only be sent over a TLS connection.
        /// If a TLS connection can not be established, Mailgun will not deliver the message.
        /// If set to False, Mailgun will still try and upgrade the connection, but if Mailgun can not, the message will be delivered over a plaintext SMTP connection.
        /// Default is false.
        /// </summary>
        public bool RequireTLS { get; set; } = false;

        /// <summary>
        /// If set to True, the certificate and hostname will not be verified when trying to establish a TLS connection and Mailgun will accept any certificate during delivery.
        /// If set to False, Mailgun will verify the certificate and hostname.If either one can not be verified, a TLS connection will not be established.
        /// The default is False.
        /// </summary>
        public bool SkipTLSVerification { get; set; } = false;

        /// <summary>
        /// Allows attaching a custom JSON data to the message.
        /// See https://documentation.mailgun.com/user_manual.html#manual-customdata
        /// </summary>
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
    }
}
