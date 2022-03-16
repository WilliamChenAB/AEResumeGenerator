﻿using ae_resume_api.Facade;
using System;


namespace ae_resume_api.Attributes
{
	public class WorkspaceModel
	{
		public int WID { get; set; }
		public int ProposalNumber{ get; set; }
		public string Division { get; set; }
		public string CreationDate { get; set; }
		public List<ResumeModel> Resumes { get; set; }
	}
	
}
