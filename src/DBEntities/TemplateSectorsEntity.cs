using System;
using System.ComponentModel.DataAnnotations;

public class TemplateSectorsEntity
{
	[Key]
	public int TemplateID { get; set; }
	[Key]
	public int TypeID { get; set; }
}

