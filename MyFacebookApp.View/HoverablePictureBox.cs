using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyFacebookApp.View
{
	public class HoverablePictureBox : PictureBoxDecorator
	{
		public BorderStyle m_BorderStyle { get; set; } = BorderStyle.Fixed3D;
		public HoverablePictureBox(PictureBox i_DecoratedPictureBox) : base(i_DecoratedPictureBox)
		{
			
			if (m_DecoratedPictureBox != null)
			{
				m_DecoratedPictureBox.MouseEnter += new EventHandler(mouse_Enter);
				m_DecoratedPictureBox.MouseLeave += new EventHandler(mouse_Leave);
			}
			else
			{
				this.MouseEnter += new EventHandler(mouse_Enter);
				this.MouseLeave += new EventHandler(mouse_Leave);
			}
		}

		/*protected override void OnMouseEnter(EventArgs e)
		{
			this.BorderStyle = m_BorderStyle;
			base.OnMouseEnter(e);
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			this.BorderStyle = BorderStyle.None;
			base.OnMouseLeave(e);
		}*/

		/*currentAlbumPictureBox.MouseEnter += new EventHandler(album_Enter);
								currentAlbumPictureBox.MouseLeave += new EventHandler(album_Leave);*/
		private void mouse_Enter(object sender, EventArgs e)
		{
			PictureBox albumLeft = sender as PictureBox;

			if (albumLeft != null)
			{
				albumLeft.Invoke(new Action(() => albumLeft.BorderStyle = m_BorderStyle));
			}

		}

		private void mouse_Leave(object sender, EventArgs e)
		{
			PictureBox albumHovered = sender as PictureBox;

			if (albumHovered != null)
			{
				albumHovered.Invoke(new Action(() => albumHovered.BorderStyle = BorderStyle.None));
			}
		}
	}
}
