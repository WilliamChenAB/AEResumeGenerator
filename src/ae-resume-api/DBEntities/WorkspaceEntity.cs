using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class WorkspaceEntity
{
	[Key]
	public int WorkspaceId { get; set; }
	public string Name { get; set; }
	public int Proposal_Number { get; set; }
	public string Division { get; set; }
	public string Creation_Date { get; set; }

	public Guid EmployeeId { get; set; }
	[JsonIgnore]
	public virtual EmployeeEntity Employee { get; set; }

	public virtual List<ResumeEntity> Resumes { get; set; }
}
