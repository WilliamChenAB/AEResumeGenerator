using System;
using System.Collections.Generic;


namespace ae_resume_api.Models
{

	public class ResumeModel
	{
		public int ResumeId { get; set; }
		public string EmployeeId { get; set; }
		public string Name { get; set; }
		public string EmployeeName { get; set; }
		public int? WorkspaceId { get; set; }
		public Status Status { get; set; }
		public DateTime? CreationDate { get; set; }
		public DateTime? LastEditedDate { get; set; }
		public List<SectorModel> SectorList { get; set; }
		public int? TemplateId { get; set; }
		public string? TemplateName { get; set; }

	}

}

