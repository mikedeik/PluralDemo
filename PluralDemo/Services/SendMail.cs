using System;
namespace PluralDemo.Services
{
	public class SendMail
	{
		private string _mailTo = "test_to@byte.gr";
		private string _mailFrom = "mike@byte.gr";

		public void Send(string subject, string message)
		{
			Console.WriteLine($"Mail from {_mailTo} to {_mailFrom} with {nameof(SendMail)}");
			Console.WriteLine("subject :" + subject);
            Console.WriteLine("message :" + message);

        }
	}
}

