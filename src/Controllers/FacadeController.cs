using System;
using ae_resume_api.Authentication;
using ae_resume_api.Admin;
using ae_resume_api.DBContext;
using ae_resume_api.Facade;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net;


namespace ae_resume_api.Controllers
{
	[Route("Facade")]
	[ApiController]
	public class FacadeController : ControllerBase
	{
		private readonly IFacadeService _facadeservice;

		public List<ResumeModel> Resumes = new List<ResumeModel>();
		public List<SectorModel> Sectors = new List<SectorModel>();

		public FacadeController(IFacadeService facadeservice)
		{
			_facadeservice = facadeservice;

			Resumes.Add(new ResumeModel { 
				RID = 3,
				CreationDate = "01/01/2020",
				LastEditedDate = "01/01/2020",
				SectorList = new List<SectorModel> {
					new SectorModel {
						SectorType = 1,
						SID = 1,
						Content = "my work experince",
						CreationDate = "01/01/2020",
						LastEditedDate = "01/01/2020"
					}
				}
			});

			Sectors.Add(new SectorModel {
						SectorType = 1,
						SID = 1,
						Content = "my work experince",
						CreationDate = "01/01/2020",
						LastEditedDate = "01/01/2020"
					});
		}

		[HttpPost]
        [Route("NewResume")]
		public async Task<IActionResult> NewResume([FromBody] ResumeModel model, int EID)
		{
			// return await _facadeservice.NewResume(model, EID);

			//return await _adminservice.NewSectorType(model);

            Resumes.Add(model);

			var targetList = model.SectorList
				.Select(x => new SectorEntity() { 
					SID = x.SID,
					SectorType = x.SectorType,
					Content = x.Content,
					CreationDate = x.CreationDate,
					LastEditedDate = x.LastEditedDate
				}).ToList();

			ResumeEntity entity = new ResumeEntity
            {
               RID = model.RID,
			   CreationDate = model.CreationDate,
			   LastEditedDate = model.LastEditedDate,
			   SectorList = targetList
            };
            //_databaseContext.Employees.Add(entity);

            return CreatedAtAction(
                nameof (GetResume),
                new { RID = model.RID },
                model);
		}

		[HttpPost]
		[Route("NewSector")]
		public async Task<IActionResult> NewSector(SectorModel model)
		{
			// return await _facadeservice.NewSector(model);

			Sectors.Add(model);

            SectorEntity entity = new SectorEntity
            {
               SID = model.SID,
			   CreationDate = model.CreationDate,
			   LastEditedDate = model.LastEditedDate,
			   Content = model.Content
            };
            //_databaseContext.Employees.Add(entity);

            return CreatedAtAction(
                nameof (GetSector),
                new { SID = model.SID },
                model);
		}

		[HttpGet]
		[Route("GetSector")]
		public async Task<ActionResult<SectorModel>> GetSector(int SID)
		{
			// return await _facadeservice.GetSector(SID);

			var sector = Sectors.Find(x => x.SID == SID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if(sector == null)
            {
                return NotFound();
            }
            return sector;
            //return EmployeeEntityToModel(employee);
		}

		[HttpDelete]
		[Route("DeleteResume")]
		public async Task<IActionResult> DeleteResume(int RID)
		{
			// return await _facadeservice.DeleteResume(RID);

			var resume = Resumes.Find(x => x.RID == RID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if (resume == null)
            {
                return NotFound();
            }
            
            Resumes.Remove(resume);
            //_databaseContext.Employees.Remove(employee);
            //return NoContent();
            return Ok();
		}

		[HttpDelete]
		[Route("DeleteSector")]
		public async Task<IActionResult> DeleteSector(int SID, int RID)
		{
			// return await _facadeservice.DeleteSector(SID, RID);

			// return await _facadeservice.DeleteResume(RID);

			var resume = Resumes.Find(x => x.RID == RID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if (resume == null)
            {
                return NotFound();
            }

            var sector = Sectors.Find(x => x.SID == SID);
			resume.SectorList.Remove(sector);
            //_databaseContext.Employees.Remove(employee);
            //return NoContent();
            return Ok();
		}

		[HttpPut]
		[Route("EditSector")]
		public async Task<IActionResult> EditSector(int SID, SectorModel model)
		{
			// return await _facadeservice.EditSector(SID, model);

			//return await _adminservice.EditSectorType(sectorTypeID, model);
            if(SID != model.SID)
            {
                return BadRequest();
            }
            var sector = Sectors.Find(x => x.SID == SID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if (sector == null)
            {
                return NotFound();
            }

            sector.SID = model.SID;
			sector.CreationDate = model.CreationDate;
			sector.LastEditedDate = model.CreationDate;
			sector.Content = model.Content;
			sector.SectorType = model.SectorType;

            return Ok(sector);
		}

		[HttpPut]
		[Route("EditResume")]
		public async Task<HttpResponseMessage> EditResume(int RID, int SID,  SectorModel model)
		{
			return await _facadeservice.EditResume(RID, SID, model);
		}

		[HttpGet]
		[Route("GetResume")]
		public async Task<ActionResult<ResumeModel>> GetResume(int RID)
		{
			// return await _facadeservice.GetSector(SID);

			var resume = Resumes.Find(x => x.RID == RID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if(resume == null)
            {
                return NotFound();
            }
            return resume;
            //return EmployeeEntityToModel(employee);
		}

		[HttpGet]
		[Route("ExportResume")]
		public async Task<HttpResponseMessage> ExportResume(int RID)
		{
			return await _facadeservice.ExportResume(RID);
		}

		[HttpGet]
		[Route("SearchResume")]
		public async Task<HttpResponseMessage> SearchResume(string filter)
		{
			return await _facadeservice.SearchResume(filter);
		}
	}
}

