using System;
using System.ComponentModel.DataAnnotations;

public class SectorEntity
{
	[Key]
	public int SID { get; set; }
	public string? CreationDate { get; set; }
	public string? LastEditedDate { get; set; }
	public string Content { get; set; }
	public int SectorType { get; set; }
}

