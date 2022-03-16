using System;
using System.ComponentModel.DataAnnotations;

public class SectorEntity
{

    [Key]
	public int SID { get; set; }
	public string? Creation_Date { get; set; }
	public string? Last_Edited { get; set; }
	public string Content { get; set; }	
    public int EID { get; internal set; }
    public int TypeID { get; set; }
	// TODO: Propogate changes
	public string TypeTitle { get; set; }
	public int RID { get; set; }
}

