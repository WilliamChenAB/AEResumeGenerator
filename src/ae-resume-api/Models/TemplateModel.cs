using System;


namespace ae_resume_api.Models
{
	public class TemplateModel
	{
		public int? TemplateId { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public DateTime? LastEdited { get; set; }
		public List<SectorTypeModel>? SectorTypes { get; set; }

	}

}

