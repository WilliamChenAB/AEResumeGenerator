using System;
using System.Collections.Generic;


namespace ae_resume_api.Facade
{
	public enum Status { Requested, inProgress, Submitted}

	public class ResumeModel
	{
		public int RID { get; set; }
		public int EID { get; set; }
		// TODO: propogate changes
		public string Name { get; set; }
		public int WID { get; set; }
		public Status Status { get; set; }
		public string CreationDate { get; set; }
		public string LastEditedDate { get; set; }
		public List<SectorModel> SectorList { get; set; }
		public int TemplateID { get; set; }

	}

}

