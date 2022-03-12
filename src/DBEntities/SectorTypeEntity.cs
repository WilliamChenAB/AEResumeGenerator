using System;
using System.ComponentModel.DataAnnotations;

public class SectorTypeEntity
{
	[Key]
	public int TypeID { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
}

