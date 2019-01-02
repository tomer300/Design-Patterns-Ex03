namespace MyFacebookApp.Model
{
	public class HitechKeyWord
	{
		public string m_KeyWord { get; set; }

		public override string ToString()
		{
			return m_KeyWord.ToLower();
		}
	}
}
