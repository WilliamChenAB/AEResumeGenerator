using System;
using System.ComponentModel.DataAnnotations;

public class EmployeeEntity
{
	[Key]
	public int EID { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public string? Name { get; set; }
	public string? Email { get; set; }
}
