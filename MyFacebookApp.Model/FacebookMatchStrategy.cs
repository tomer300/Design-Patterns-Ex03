using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;
using static FacebookWrapper.ObjectModel.User;

namespace MyFacebookApp.Model
{
	class FacebookMatchStrategy : IMatchStrategy
	{
		public bool DoesCorrespondingUserPreferences(AppUser i_PotentialMatch, params object[] i_Preferences)
		{
			bool needToBeAdded = false;
		/*	const int CHOSE_BOYS = 0, CHOSE_GIRLS = 1, AGE_RANGE = 2;
				if (isUserWithinChosenAgeRange(i_PotentialMatch, i_Preferences[AGE_RANGE))
				{ if ((!i_ChoseBoys && !i_ChoseGirls) || (i_ChoseBoys && i_ChoseGirls))
					{
						needToBeAdded = true;
					}
					else
					{
						eGender? userGender = currentPotentialMatch.GetGender();
						if (userGender != null)
						{
							if (i_ChoseBoys && userGender == eGender.male || i_ChoseGirls && userGender == eGender.female)
							{
								needToBeAdded = true;
							}
						}
					}
				}*/
			
				return needToBeAdded;
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
			const int SINGLE_BOUND = 1, LOWER_BOUND = 0, UPPER_BOUND = 1;
			const char RANGE_DELIMITER = '-', ABOVE_DELIMITER = '+';
			string[] chosenRange = i_AgeRange.Split(RANGE_DELIMITER, ABOVE_DELIMITER);
			bool iswithinRange = false;
			int usersAge;
			string matchFullName = string.Format("{0} {1}", i_User.FirstName, i_User.LastName);

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
			const int MONTH = 0, DAY = 1, YEAR = 2;
			const char DATE_DELIMITER = '/';
			int age = 0;
			string[] birthDateArray = i_Birthday.Split(DATE_DELIMITER);
			DateTime birthDate;
			DateTime today = DateTime.Today;

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

		public bool IsPotentialMatchAvailable(AppUser i_PotentialMatch)
		{
			bool isSingle = false;
			eRelationshipStatus? userRelationshipStatus = i_PotentialMatch.GetRelationshipStatus();

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

		public void ProcessMatchesCollection(ref FacebookObjectCollection<AppUser> io_MatchesCollection)
		{
			throw new NotImplementedException();
		}
	}
}
