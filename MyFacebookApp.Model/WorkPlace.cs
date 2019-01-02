using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFacebookApp.Model
{
	public class WorkPlace
	{
		public string m_Name { get; set; }

		public override string ToString()
		{
			return m_Name.ToLower();
		}
	}
}
