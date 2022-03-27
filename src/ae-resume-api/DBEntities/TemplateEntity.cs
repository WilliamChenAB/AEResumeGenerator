using System;
using System.ComponentModel.DataAnnotations;

public class TemplateEntity
{
	[Key]
	public int TemplateId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Last_Edited { get; set; }

	public virtual List<TemplateSectorEntity> TemplateSectors { get; set; }

}