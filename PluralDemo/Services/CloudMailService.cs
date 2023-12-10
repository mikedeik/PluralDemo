namespace PluralDemo.Services {
    public class CloudMailService : ISendMail {
        private string _mailTo = string.Empty;
        private string _mailFrom = string.Empty;

        public CloudMailService(IConfiguration configuration) {
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }


        public void Send(string subject, string message) {
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo} with {nameof(CloudMailService)}");
            Console.WriteLine("subject :" + subject);
            Console.WriteLine("message :" + message);

        }
    }
}