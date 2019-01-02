using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyFacebookApp.View
{
	public class PictureBoxDecorator : PictureBox
	{
		protected PictureBox m_DecoratedPictureBox;

		public PictureBoxDecorator(PictureBox i_DecoratedPictureBox)
		{
			m_DecoratedPictureBox = i_DecoratedPictureBox;
		}
	}
}
