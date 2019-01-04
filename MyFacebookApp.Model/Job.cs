using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FacebookWrapper.ObjectModel;

namespace MyFacebookApp.Model
{
	public class Job : IEnumerable<AppUser>
	{
		private readonly FacebookObjectCollection<AppUser>	r_UserFriends;
		private HashSet<string>								m_HitechWorkPlaces;
		private HashSet<string>								m_HitechKeyWords;
		public Func<AppUser, bool> TestIsHitechWorker, TestIsHitechRelated;

		public IContactStrategy ContactStrategy { get; set; } = new FacebookContactStrategy();

		public Job(FacebookObjectCollection<AppUser> i_UserFriends)
		{
			r_UserFriends = i_UserFriends;
		}

		private HashSet<string> buildSetFromXMLFile<T>(string i_XMLFileContent)
		{
			HashSet<string> setFromFile = new HashSet<string>();

			if (i_XMLFileContent.Length > 0)
			{
				using (TextReader reader = new StringReader(i_XMLFileContent))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
					List<T> listFromFile = serializer.Deserialize(reader) as List<T>;

					foreach (T currentMember in listFromFile)
					{
						setFromFile.Add(currentMember.ToString());
					}
				}
			}

			return setFromFile;
		}

		public IEnumerator<AppUser> GetEnumerator()
		{
			foreach (AppUser possibleContact in r_UserFriends)
			{
				if ((TestIsHitechWorker != null && TestIsHitechWorker.Invoke(possibleContact)) || (TestIsHitechRelated != null && TestIsHitechRelated.Invoke(possibleContact)))
				{
					yield return possibleContact;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public bool DoesWorksAtPotentiallyHitechRelatedCompany(AppUser i_CurrentFriend)
		{
			Page workPlace;
			bool doesAtPotentiallyHitechRelatedCompany = false;
			
			if(m_HitechKeyWords == null)
			{
				m_HitechKeyWords = buildSetFromXMLFile<HitechKeyWord>(MyFacebookApp.Model.Properties.Resources.hitechKeyWords);
			}
			
			workPlace = i_CurrentFriend.GetWorkPlace();
			if (workPlace != null)
			{
				foreach (string wordInDescriptionOfWorkPlace in workPlace.Description?.Split(' '))
				{
					if (m_HitechKeyWords.Contains(wordInDescriptionOfWorkPlace))
					{
						doesAtPotentiallyHitechRelatedCompany = true;
					}
				}
			}

			return doesAtPotentiallyHitechRelatedCompany;
		}

		public bool DoesWorksAtKnownHitechCompany(AppUser i_CurrentFriend)
		{
			bool	doesWorksAtKnownHitechCompany = false;
			string	workPlace;

			if (m_HitechWorkPlaces == null)
			{
				m_HitechWorkPlaces = buildSetFromXMLFile<WorkPlace>(MyFacebookApp.Model.Properties.Resources.israeliHitechList);
			}

			workPlace = i_CurrentFriend.GetWorkPlace()?.Name.ToLower();
			if (m_HitechWorkPlaces.Contains(workPlace))
			{
				doesWorksAtKnownHitechCompany = true;
			}

			return doesWorksAtKnownHitechCompany;
		}

		public void ContactUser(AppUser i_LoggedUser, AppUser i_Contact)
		{
			string message = string.Format(
@"Hello {0}, your facebook friend {1} {2} has started his job-hunt in hitech!
You've been found suitable by our system which means you work at a hitech company.
If you think that {1} {2} has a place in your company, please contact him at: {3}.
Thank you,
Facebook App Team.", 
i_Contact.FirstName, 
i_LoggedUser.FirstName, 
i_LoggedUser.LastName, 
i_LoggedUser.Email);

			ContactStrategy.Contact(i_LoggedUser, i_Contact, message);
		}
	}
}