using System;
using System.ComponentModel.DataAnnotations;

public class TemplateEntity
{
	[Key]
	public int TemplateID { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
}

