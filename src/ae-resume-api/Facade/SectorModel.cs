using System;


namespace ae_resume_api.Facade
{
	public class SectorModel
	{
		public int SID { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime LastEditedDate { get; set; }
		public string? Content { get; set; }
		public int SectorType { get; set; }
        public int TypeID { get;  set; }	
		public string TypeTitle { get; set; }
		public int RID { get; set; }
		public string ResumeName { get; set; }
		public string Division { get; set; }
		public string Image { get; set; }
    }
	
}

