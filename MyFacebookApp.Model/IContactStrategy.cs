using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFacebookApp.Model
{
	public interface IContactStrategy
	{
		void Contact(AppUser i_User, AppUser i_UserToContact, string i_Message);
	}
}
