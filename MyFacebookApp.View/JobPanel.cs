using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;
using MyFacebookApp.Model;

namespace MyFacebookApp.View
{
	public partial class JobPanel : AppScreenPanel, IBackable
	{
		private int					m_LastChosenContactIndex;

		internal JobPanel(AppEngine i_AppEngine) : base(i_AppEngine)
		{
			InitializeComponent();
			FacebookView.CreateThread(fetchInitialDetails);
		}

		private void findAJobButton_Click(object sender, EventArgs e)
		{
			FacebookObjectCollection<AppUser>	hitechWorkerContacts = new FacebookObjectCollection<AppUser>();
			
			listBoxJobs.Items.Clear();
			try
			{
				r_AppEngine.Job.TestIsHitechWorker += r_AppEngine.Job.DoesWorksAtKnownHitechCompany;
				r_AppEngine.Job.TestIsHitechRelated += r_AppEngine.Job.DoesWorksAtPotentiallyHitechRelatedCompany;
				foreach (AppUser currentContact in r_AppEngine.Job)
				{
					hitechWorkerContacts.Add(currentContact);
				}

				if (hitechWorkerContacts != null && hitechWorkerContacts.Count > 0)
				{
					FacebookView.CreateThread(() => 
					{
						addAllContactsToListBox(hitechWorkerContacts);
					});
					FriendsDisplayer displayer = new FriendsDisplayer(hitechWorkerContacts, flowLayoutPanelContactPhotos);
					displayer.FriendOnClickDelegate += contactPic_Click;

					FacebookView.CreateThread(displayer.Display);
				}
				else
				{
					MessageBox.Show("Couldnt fetch work experience.");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			listBoxJobs.SelectedIndexChanged += new EventHandler(contactInfo_Click);
		}

		private void addAllContactsToListBox(FacebookObjectCollection<AppUser> i_HitechWorkerContacts)
		{
			bool hasShownMessageBox = false;

			foreach (AppUser currentContact in i_HitechWorkerContacts)
			{
				addContactToListBoxJobs(currentContact, ref hasShownMessageBox);
			}
		}

		private void addContactToListBoxJobs(AppUser i_CurrentContact, ref bool io_HasShownMessageBox)
		{
			string contactFullName = string.Empty;
			string contactFirstName = string.Empty;
			string contactLastName = string.Empty;
			string workPlace = string.Empty;

			try
			{
				contactFirstName = i_CurrentContact.FirstName;
				contactLastName = i_CurrentContact.LastName;
				workPlace = i_CurrentContact.GetWorkPlace()?.Name;
			}
			catch (Exception ex)
			{
				if (!io_HasShownMessageBox)
				{
					MessageBox.Show(ex.Message);
					io_HasShownMessageBox = true;
				}
			}
			finally
			{
				contactFullName = string.Format("{0} {1}", contactFirstName, contactLastName);
				listBoxJobs.Invoke(new Action(() =>
				{
					listBoxJobs.Items.Add(
										new ContactItem(new KeyValuePair<string, string>(
										contactFullName,
										string.Format("{0} works at", contactFullName, workPlace))));
				}));
			}
		}

		private void contactInfo_Click(object sender, EventArgs e)
		{
			PictureBox	lastChosenContactPhoto = flowLayoutPanelContactPhotos.Controls[m_LastChosenContactIndex] as PictureBox;
			ContactItem contactClicked;
			PictureBox	contactPicture;
			string		contactName = string.Empty;

			if (lastChosenContactPhoto != null)
			{
				lastChosenContactPhoto.BorderStyle = BorderStyle.None;
			}
			
			m_LastChosenContactIndex = listBoxJobs.SelectedIndex;
			contactClicked = listBoxJobs.Items[m_LastChosenContactIndex] as ContactItem;
			if (contactClicked != null)
			{
				contactName = contactClicked.Contact.Key;
				foreach (Control currentPicture in flowLayoutPanelContactPhotos.Controls)
				{
					contactPicture = currentPicture as PictureBox;
					if (contactPicture != null)
					{
						if (contactPicture.Name.Equals(contactName))
						{
							contactPicture.BorderStyle = BorderStyle.Fixed3D;
							contactPicture.Focus();
						}
					}
				}
			}
		}

		private void contactPic_Click(object sender, EventArgs e)
		{
			PictureBox	clickedContact = sender as PictureBox;
			ContactItem currentContactInfo;
			string		contactName = clickedContact?.Name;
			FriendsDisplayer.AppUserEventArgs userClicked = e as FriendsDisplayer.AppUserEventArgs;

			if (userClicked != null)
			{
				try
				{ 
					r_AppEngine.ContactUser(userClicked.User);
					MessageBox.Show(string.Format("Message has been sent successfully to {0}", contactName));
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}

			if (clickedContact != null)
			{
				foreach (object currentItem in listBoxJobs.Items)
				{
					currentContactInfo = currentItem as ContactItem;
					if (currentContactInfo != null)
					{
						if (currentContactInfo.Contact.Key.Equals(contactName))
						{
							listBoxJobs.Invoke(new Action(() => listBoxJobs.SetSelected(listBoxJobs.Items.IndexOf(currentItem), true)));
							break;
						}
					}
				}
			}
		}

		protected override void fetchInitialDetails()
		{
			fetchWorkExperience();
		}

		private void fetchWorkExperience()
		{
			try
			{
				WorkExperience[] workExperiences = r_AppEngine.WorkExperiences;

				if (!listBoxUserWorkExperience.InvokeRequired)
				{
					workExperienceBindingSource.DataSource = workExperiences;
				}
				else
				{
					listBoxUserWorkExperience.Invoke(new Action(() => workExperienceBindingSource.DataSource = workExperiences));
				}

				if (workExperienceBindingSource.Count == 0)
				{
					MessageBox.Show("No Work Experiences History.");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private class ContactItem
		{
			public KeyValuePair<string, string> Contact { get; private set; }

			public ContactItem(KeyValuePair<string, string> i_Contact)
			{
				Contact = i_Contact;
			}

			public override string ToString()
			{
				return Contact.Value;
			}
		}
	}
}
