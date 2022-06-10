using ae_resume_api.Models;
using System;


namespace ae_resume_api.Models
{
	public class WorkspaceModel
	{
		public int WorkspaceId { get; set; }
		public string ProposalNumber{ get; set; }
		public string Division { get; set; }
		public string Name { get; set; }
		public string EmployeeId { get; set; }
		public DateTime CreationDate { get; set; }
		public List<ResumeModel>? Resumes { get; set; }

	}

}

