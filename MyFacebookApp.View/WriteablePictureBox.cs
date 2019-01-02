using System.Drawing;
using System.Windows.Forms;

namespace MyFacebookApp.View
{
	internal class WriteablePictureBox : PictureBoxDecorator
	{
		public WriteablePictureBox(PictureBox i_DecoratedPictureBox, string i_FirstName, string i_LastName) : base(i_DecoratedPictureBox)
		{
			FirstName = i_FirstName;
			LastName = i_LastName;
		}

		public string FirstName { get; private set; }

		public string LastName { get; private set; }

		/*public DetailedProfilePicture(PictureBox i_FriendsPicture, string i_FirstName, string i_LastName)
		{
			FriendProfilePicture = i_FriendsPicture;
			FirstName = i_FirstName;
			LastName = i_LastName;
			FriendProfilePicture.Paint += writeNameOnFriendPicture;
		}*/

		protected override void OnPaint(PaintEventArgs pe)
		{
			writeNameOnFriendPicture(pe);
			base.OnPaint(pe);
		}
		private void writeNameOnFriendPicture(PaintEventArgs ePaint)
		{
			float fontSize = 12;
			SizeF firstNameSize = ePaint.Graphics.MeasureString(FirstName, new Font("Franklin Gothic Heavy", fontSize));
			SizeF lastNameSize = ePaint.Graphics.MeasureString(LastName, new Font("Franklin Gothic Heavy", fontSize));
			PointF locationToDraw = new PointF();

			ePaint.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			locationToDraw.X = (this.Width / 2) - (firstNameSize.Width / 2);
			locationToDraw.Y = (this.Height / (float)1.4) - (firstNameSize.Height / (float)2);
			ePaint.Graphics.DrawString(FirstName, new Font("Franklin Gothic Heavy", fontSize), Brushes.White, locationToDraw);
			locationToDraw.X = (this.Width / 2) - (lastNameSize.Width / 2);
			locationToDraw.Y = (this.Height / (float)1.1) - (lastNameSize.Height / (float)2);
			ePaint.Graphics.DrawString(LastName, new Font("Franklin Gothic Heavy", fontSize), Brushes.White, locationToDraw);
		}
	}
}
