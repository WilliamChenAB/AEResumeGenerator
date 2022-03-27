using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class SectorEntity
{
	[Key]
	public int SectorId { get; set; }
	public string Creation_Date { get; set; }
	public string Last_Edited { get; set; }
	public string Content { get; set; }
	public string Division { get; set; }
	public string? Image { get; set; }

	public int ResumeId { get; set; }
	[JsonIgnore]
	public virtual ResumeEntity Resume { get; set; }

	public int TypeId { get; set; }
	[JsonIgnore]
	public virtual SectorTypeEntity Type { get; set; }
}

