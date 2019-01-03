using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFacebookApp.Model
{
	public interface IMatchStrategy
	{
		bool IsPotentialMatchAvailable(AppUser i_PotentialMatch);

		bool DoesCorrespondingUserPreferences(AppUser i_PotentialMatch, params object[] i_Preferences);

		void ProcessMatchesCollection(ref FacebookObjectCollection<AppUser> io_MatchesCollection);


	}
}
