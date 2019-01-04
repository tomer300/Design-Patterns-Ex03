using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFacebookApp.Model
{
	public class FacebookMatchPreferences : IMatchPreferences
	{
		public bool PreferGirls { get; set; } = true;

		public bool PreferBoys { get; set; } = true;

		public string AgeRange { get; set; } = DateTime.Now.ToShortDateString();
	}
}
