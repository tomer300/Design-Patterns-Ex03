using System;
using System.Net.Mail;

namespace MyFacebookApp.Model
{
	public class FacebookContactStrategy : IContactStrategy
	{
		private readonly string r_AppMail = "facebookappnt@gmail.com";
		private readonly string r_UserName = "facebookappnt";
		private readonly string r_Password = "FacebookAppYear3";
		private readonly int r_Port = 587;

		public void Contact(AppUser i_LoggedUser, AppUser i_UserToContact, string i_Message)
		{
			try
			{
				MailMessage mail = new MailMessage();
				SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

				mail.From = new MailAddress(r_AppMail);
				mail.To.Add(i_UserToContact.Email);
				mail.Subject = string.Format("Help {0} {1} to find a job!", i_LoggedUser.FirstName, i_LoggedUser.LastName);
				mail.Body = i_Message;

				SmtpServer.Port = r_Port;
				SmtpServer.Credentials = new System.Net.NetworkCredential(r_UserName, r_Password);
				SmtpServer.EnableSsl = true;

				SmtpServer.Send(mail);
			}
			catch (Exception)
			{
				throw new Exception("Email sending failed!");
			}
		}
	}
}
