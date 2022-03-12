using System;
using System.ComponentModel.DataAnnotations;

public class WorkspaceEntity
{
	[Key]
	public int WID { get; set; }
	public int ProposalNumber { get; set; }
	public string Division { get; set; }
	public string CreationDate { get; set; }
	public List<ResumeEntity> Resumes { get; set; }
}
