using System;
using System.Collections.Generic;
using FacebookWrapper.ObjectModel;
using static FacebookWrapper.ObjectModel.User;

namespace MyFacebookApp.Model
{
	public class Match : MatchBase
	{
		public Match(FacebookObjectCollection<AppUser> i_UserFriends) : base(i_UserFriends)
		{
			MatchPreferences = new FacebookMatchPreferences();
		}

		protected override void ProcessMatchesCollection(ref FacebookObjectCollection<AppUser> io_PotentialMatches)
		{
			List<AppUser> sortedList = new List<AppUser>(io_PotentialMatches);

			sortedList.Sort((user1, user2) => user1.FirstName.CompareTo(user2.FirstName));
			io_PotentialMatches.Clear();
			foreach(AppUser currUser in sortedList)
			{
				io_PotentialMatches.Add(currUser);
			}
		}
		
		protected override bool DoesCandidateCorrespondUserPreferences(AppUser i_CurrentPotentialMatch)
		{
			bool needToBeAdded = false;
			FacebookMatchPreferences preferences = MatchPreferences as FacebookMatchPreferences;

			if (preferences == null)
			{
				throw new InvalidCastException("Couldnt cast MatchPreferences To FacebookMatchPreferences.");
			}

			if (isUserWithinChosenAgeRange(i_CurrentPotentialMatch, preferences.AgeRange))
			{
				if ((!preferences.PreferBoys && !preferences.PreferGirls) || (preferences.PreferBoys && preferences.PreferGirls))
				{
					needToBeAdded = true;
				}
				else
				{
					eGender? userGender = i_CurrentPotentialMatch.GetGender();
					if (userGender != null)
					{
						if ((preferences.PreferBoys && userGender == eGender.male) || (preferences.PreferGirls && userGender == eGender.female))
						{
							needToBeAdded = true;
						}
					}
				}
			}

			return needToBeAdded;
		}

		protected override bool IsPotentialMatchAvailable(AppUser i_CurrentPotentialMatch)
		{
			bool isSingle = false;
			eRelationshipStatus? userRelationshipStatus = i_CurrentPotentialMatch.GetRelationshipStatus();

			if (userRelationshipStatus != null)
			{
				if (userIsReadyForRelationship(userRelationshipStatus))
				{
					isSingle = true;
				}
			}

			return isSingle;
		}

		private bool userIsReadyForRelationship(eRelationshipStatus? i_UserRelationshipStatus)
		{
			bool isReadyForRelationship = false;

			if (i_UserRelationshipStatus != eRelationshipStatus.InACivilUnion &&
				i_UserRelationshipStatus != eRelationshipStatus.InADomesticPartnership &&
				i_UserRelationshipStatus != eRelationshipStatus.InARelationship &&
				i_UserRelationshipStatus != eRelationshipStatus.Married)
			{
				isReadyForRelationship = true;
			}

			return isReadyForRelationship;
		}

		private bool isUserWithinChosenAgeRange(AppUser i_User, string i_AgeRange)
		{
			/*the age range options are: 
										18-20
										21-25
										26-30
										31-35
										36-40
										41-45
										46-50
										50+
			*/
			const int	SINGLE_BOUND = 1, LOWER_BOUND = 0, UPPER_BOUND = 1;
			const char	RANGE_DELIMITER = '-', ABOVE_DELIMITER = '+';
			string[]	chosenRange = i_AgeRange.Split(RANGE_DELIMITER, ABOVE_DELIMITER);
			bool		iswithinRange = false;
			int			usersAge;
			string		matchFullName = string.Format("{0} {1}", i_User.FirstName, i_User.LastName);

			usersAge = calculateAge(i_User.Birthday, matchFullName);
			if (chosenRange.Length == SINGLE_BOUND)
			{
				if (usersAge > int.Parse(chosenRange[LOWER_BOUND]))
				{
					iswithinRange = true;
				}
			}
			else
			{
				if (usersAge >= int.Parse(chosenRange[LOWER_BOUND]) && usersAge <= int.Parse(chosenRange[UPPER_BOUND]))
				{
					iswithinRange = true;
				}
			}

			return iswithinRange;
		}

		private int calculateAge(string i_Birthday, string i_MatchFullName)
		{
			const int	MONTH = 0, DAY = 1, YEAR = 2;
			const char	DATE_DELIMITER = '/';
			int			age = 0;
			string[]	birthDateArray = i_Birthday.Split(DATE_DELIMITER);
			DateTime	birthDate;
			DateTime	today = DateTime.Today;

			if (birthDateArray != null)
			{
				try
				{
					birthDate = new DateTime(int.Parse(birthDateArray[YEAR]), int.Parse(birthDateArray[MONTH]), int.Parse(birthDateArray[DAY]));
					age = today.Year - birthDate.Year;
					if (birthDate > today.AddYears(-age))
					{
						age--;
					}
				}
				catch (Exception)
				{
					throw new Exception(string.Format("{0} has no birth year.", i_MatchFullName));
				}
			}
			else
			{
				throw new FormatException("Invalid formatted string in calculate age");
			}

			return age;
		}
	}
}