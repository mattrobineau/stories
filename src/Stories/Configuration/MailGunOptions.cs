namespace Stories.Configuration
{
    public class MailgunOptions
    {
        public string Url { get; set; }
        public string ApiPublicKey { get; set; }
        public string APiPrivateKey { get; set; }
        public bool TestMode { get; set; }
    }
}
