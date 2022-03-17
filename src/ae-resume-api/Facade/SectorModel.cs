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
		// TODO: Propogate changes
		public string TypeTitle { get; set; }
    }
	
}

