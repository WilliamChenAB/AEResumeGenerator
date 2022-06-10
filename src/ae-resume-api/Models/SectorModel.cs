using System;

namespace ae_resume_api.Models
{
	public class SectorModel
	{
		public int SectorId { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime LastEditedDate { get; set; }
		public string Content { get; set; }
        public int TypeId { get;  set; }
		public string TypeTitle { get; set; }
		public int ResumeId { get; set; }
		public string ResumeName { get; set; }
		public string Division { get; set; }
		public string Image { get; set; }
    }

}

