using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public enum Status
{
	Regular,
	Requested,
	Exported
}


public class ResumeEntity
{
	[Key]
	public int ResumeId { get; set; }
	public string Creation_Date { get; set; }
	public string Last_Edited { get; set; }
	public Status Status { get; set; }
	public string Name { get; set; }

	public Guid EmployeeId { get; set; }
	[JsonIgnore]
	public virtual EmployeeEntity Employee { get; set; }

	public int? WorkspaceId { get; set; }
	[JsonIgnore]
	public virtual WorkspaceEntity? Workspace { get; set; }

	public int TemplateId { get; set; }
	[JsonIgnore]
	public virtual TemplateEntity Template { get; set; }

	public virtual List<SectorEntity> Sectors { get; set; }

}

