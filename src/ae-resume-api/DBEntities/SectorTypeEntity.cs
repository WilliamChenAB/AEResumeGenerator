using System;
using System.ComponentModel.DataAnnotations;

public class SectorTypeEntity
{
	[Key]
	public int TypeId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }

	public virtual List<TemplateSectorEntity> TemplateSectors { get; set; }

}

