using System;


namespace ae_resume_api.Facade
{
	public class SectorModel
	{
		public int SID { get; set; }
		public string? CreationDate { get; set; }
		public string? LastEditedDate { get; set; }
		public string Content { get; set; }
		public int SectorType { get; set; }
	}
	
}

