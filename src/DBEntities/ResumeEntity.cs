using System;
using System.ComponentModel.DataAnnotations;

public class ResumeEntity
{
	[Key]
	public int RID { get; set; }
	public string CreationDate { get; set; }
	public string LastEditedDate { get; set; }
	public List<SectorEntity> SectorList { get; set; }
}

