
﻿using ae_resume_api.Models;
using System;


namespace ae_resume_api.Models
{
	public class EmployeeModel
	{
		public string EmployeeId { get; set; }
		public Access Access { get; set; }
		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? JobTitle { get; set; }
		public List<ResumeModel>? Resumes { get; set; }

	}

}

