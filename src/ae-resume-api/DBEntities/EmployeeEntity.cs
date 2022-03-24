using System;
using System.ComponentModel.DataAnnotations;

public enum Access
{
	Employee,
	ProjectAdmin,
	SystemAdmin
}

public class EmployeeEntity
{
	[Key]
	public string EID { get; set; }
	public Access Access { get; set; }
	public string? Name { get; set; }
	public string? Email { get; set; }
	public string? JobTitle { get; set; }
}
