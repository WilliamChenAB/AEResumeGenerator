using System;
using System.ComponentModel.DataAnnotations;

public class ResumeEntity
{
	[Key]
	public int RID { get; set; }
	public string Creation_Date { get; set; }
	public string Last_Edited { get; set; }
	public int EID { get; set; }
	
	public string Status { get; set; }

	public int TemplateID { get; set; }
}

