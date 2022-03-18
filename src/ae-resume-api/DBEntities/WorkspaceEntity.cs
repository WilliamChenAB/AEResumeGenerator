using System;
using System.ComponentModel.DataAnnotations;

public class WorkspaceEntity
{
	[Key]
	public int WID { get; set; }
	public int Proposal_Number { get; set; }
	public string Division { get; set; }
	public string Creation_Date { get; set; }
	public int EID { get; set; }
	public string Name { get; set; }

}
