using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class TemplateSectorEntity
{
	[Key]
	public int TemplateId { get; set; }
	[JsonIgnore]
	public virtual TemplateEntity Template { get; set; }


	[Key]
	public int TypeId { get; set; }
	[JsonIgnore]
	public virtual SectorTypeEntity SectorType { get; set; }
}

