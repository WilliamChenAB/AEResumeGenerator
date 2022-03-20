using System;


namespace ae_resume_api.Admin
{
	public class TemplateModel
	{
		public int? TemplateID{ get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public int EID { get; set; }
		public DateTime? LastEdited { get; set; }
		public List<SectorTypeModel>? SectorTypes { get; set; }

	}
	
}

