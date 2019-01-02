using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyFacebookApp.View
{
	public class HoverablePictureBox : PictureBoxDecorator
	{
		public BorderStyle m_BorderStyle { get; set; }
		public HoverablePictureBox(PictureBox i_DecoratedPictureBox) : base(i_DecoratedPictureBox)
		{
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this.BorderStyle = m_BorderStyle;
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.BorderStyle = BorderStyle.None;
		}
	}
}
