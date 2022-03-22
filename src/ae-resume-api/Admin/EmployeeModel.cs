
﻿using ae_resume_api.Facade;
using System;


namespace ae_resume_api.Admin
{
	public class EmployeeModel
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public int EID { get; set; }
		public string Access { get; set; }
		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? JobTitle { get; set; }
		public List<ResumeModel>? Resumes { get; set; }

	}
	
}

