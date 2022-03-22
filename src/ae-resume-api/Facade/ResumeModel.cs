using System;
using System.Collections.Generic;


namespace ae_resume_api.Facade
{
	public enum Status { Requested, InProgress, Submitted, Exported}

	public class ResumeModel
	{
		public int RID { get; set; }
		public int EID { get; set; }
		public string Name { get; set; }
		public string EmployeeName { get; set; }
		public int? WID { get; set; }
		public Status Status { get; set; }
		public DateTime? CreationDate { get; set; }
		public DateTime? LastEditedDate { get; set; }
		public List<SectorModel>? SectorList { get; set; }
		public int TemplateID { get; set; }
		public string TemplateName { get; set; }

	}

}

