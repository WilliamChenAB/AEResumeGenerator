using System;
using System.ComponentModel.DataAnnotations;

public class EmployeeEntity
{
	[Key]
	public int Id { get; set; }
	public string UserName { get; set; }
	public string Password { get; set; }
	public string? Name { get; set; }
	public string? Email { get; set; }
}
