using System;
using System.Collections.Generic;


namespace ae_resume_api.Facade
{
	public class ResumeModel
	{
		public int RID { get; set; }
		public string CreationDate { get; set; }
		public string LastEditedDate { get; set; }
		public List<SectorModel> SectorList { get; set; }
		public int TemplateID { get; set; }


	}
	
}

