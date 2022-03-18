﻿using System;
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
		readonly DatabaseContext _databasecontext;

		public FacadeController(DatabaseContext databaseContext)
		{
			_databasecontext = databaseContext;

		}
		
		/// <summary>
        /// Add a Resume to an Employee
        /// </summary>
		[HttpPost]
        [Route("NewResume")]		
		public async Task<ActionResult<ResumeModel>> NewResume(int templateID, string resumeName, int EID)
		{	
			
			// Find the template record
			var template =  _databasecontext.Resume_Template.FindAsync(templateID);
			

			if( template == null )
            {
				return NotFound();
            }				

			// Find the Sector Types associated with that template
			var sectorTypes =  _databasecontext.Template_Type
				.Where(x => x.TemplateID == templateID);


			ResumeEntity entity = new ResumeEntity
            {               
			   Creation_Date = DateTime.Now.ToString("yyyMMdd"),
			   EID = EID,
			   TemplateID = templateID,		
			   Name = resumeName
            };

            var resume = _databasecontext.Resume.Add(entity);

			foreach (var sector in sectorTypes)
			{
				_databasecontext.Sector.Add(new SectorEntity { 
					Creation_Date = DateTime.Now.ToString("yyyMMdd"),
					EID = EID,
					TypeID = sector.TypeID,
					RID = resume.Entity.RID
				});
			}

			
			await _databasecontext.SaveChangesAsync();

            return CreatedAtAction(
                nameof (GetResume),
                new { RID = resume.Entity.RID },
				 resume.Entity);
		}

		/// <summary>
		/// Create a new Sector
		/// </summary>		
		[HttpPost]
		[Route("NewSector")]
		public async Task<ActionResult<SectorModel>> NewSector(SectorModel model)
		{
		
            SectorEntity entity = new SectorEntity
            {
               SID = model.SID,
			   Creation_Date = DateTime.Now.ToString("yyyMMdd"),
			   Last_Edited = DateTime.Now.ToString("yyyMMdd"),
			   Content = model.Content
            };

			// Sectors.Add(model);

			_databasecontext.Sector.Add(entity);
			await _databasecontext.SaveChangesAsync();


			return CreatedAtAction(
                nameof (GetSector),
                new { SID = model.SID },
                model);
		}

		/// <summary>
		/// Get a Sector based on SID
		/// </summary>	
		[HttpGet]
		[Route("GetSector")]
		public async Task<ActionResult<SectorModel>> GetSector(int SID)
		{
			

			//var sector = Sectors.Find(x => x.SID == SID);
			
            var sector = await _databasecontext.Sector.FindAsync(SID);

            if(sector == null)
            {
                return NotFound();
            }
            return ControllerHelpers.SectorEntityToModel(sector);            
		}

		/// <summary>
		/// Get all Sectors from an Employee
		/// </summary>	
		[HttpGet]
		[Route("GetAllSectorsForEmployee")]
		public async Task<ActionResult<IEnumerable<SectorModel>>> GetAllSectorsForEmployee(int EID)
		{
			
            var employee = await _databasecontext.Employee.FindAsync(EID);

			if(employee == null)
            {
				return NotFound();
            }

			List<SectorModel> sectorList = new List<SectorModel>();
			var sectors =  _databasecontext.Sector.Where(s => s.EID == EID).OrderBy(s => s.TypeID);

			

			foreach(var sector in sectors)
            {
				sectorList.Add(ControllerHelpers.SectorEntityToModel(sector));
            }
			
	
            return sectorList;            
		}

		/// <summary>
		/// Get all Sectors from an Employee
		/// </summary>	
		[HttpGet]
		[Route("GetAllSectorsForEmployeeByType")]
		public async Task<ActionResult<IEnumerable<SectorModel>>> GetAllSectorsForEmployeeByType(int EID, int TypeID)
		{
			
            var employee = await _databasecontext.Employee.FindAsync(EID);

			if(employee == null)
            {
				return NotFound();
            }

			List<SectorModel> sectorList = new List<SectorModel>();
			var sectors =  _databasecontext.Sector.Where(s => s.EID == EID && s.TypeID == TypeID)
				.OrderBy(s => s.TypeID);
			

			foreach(var sector in sectors)
            {
				sectorList.Add(ControllerHelpers.SectorEntityToModel(sector));
            }
			
	
            return sectorList;            
		}

		/// <summary>
		/// Delete a Resume
		/// </summary>	
		[HttpDelete]
		[Route("DeleteResume")]
		public async Task<IActionResult> DeleteResume(int RID)
		{
			

			//var resume = Resumes.Find(x => x.RID == RID);
			
            var resume = await _databasecontext.Resume.FindAsync(RID);

            if (resume == null)
            {
                return NotFound();
            }

			//Resumes.Remove(resume);			
			_databasecontext.Resume.Remove(resume);
            await _databasecontext.SaveChangesAsync();
            return Ok();
		}

		/// <summary>
		/// Delete a Sector from a Resume
		/// </summary>	
		[HttpDelete]
		[Route("DeleteSector")]
		public async Task<IActionResult> DeleteSector(int SID)
		{			
            //var sector = Sectors.Find(x => x.SID == SID);
			
			
			var sector = await _databasecontext.Sector.FindAsync(SID);
			if(sector == null)
            {
				return NotFound();
            }
			
			//resume.SectorList.Remove(sector);
			_databasecontext.Remove(SID);
			await _databasecontext.SaveChangesAsync();

            return Ok();
		}

		/// <summary>
		/// Edit a Sector
		/// </summary>
		[HttpPut]
		[Route("EditSector")]
		public async Task<IActionResult> EditSector(int SID, SectorModel model)
		{			
            if(SID != model.SID)
            {
                return BadRequest();
            }
            //var sector = Sectors.Find(x => x.SID == SID);
            var sector = await _databasecontext.Sector.FindAsync(SID);

            if (sector == null)
            {
                return NotFound();
            }

            sector.SID = model.SID;
			sector.Creation_Date = model.CreationDate.ToString("yyyMMdd");
			sector.Last_Edited = DateTime.Now.ToString("yyyMMdd");
			sector.Content = model.Content;
			sector.TypeID = model.SectorType;

			try
            {
                await _databasecontext.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                return NotFound(ex.Message);
			}

            return Ok(sector);
		}

		/// <summary>
		/// Add a Sector to a Resume
		/// </summary>
		[HttpPost]
		[Route("AddSectorToResume")]
		public async Task<IActionResult> AddSectorToResume(int RID, string content, int typeID)
        {
			
			var resume = await _databasecontext.Resume.FindAsync(RID);
			

			if (resume == null)
            {
                return NotFound();
            }

			//resume.SectorList.Add(model);
			SectorEntity sector = new SectorEntity();
			
			sector.Creation_Date = DateTime.Now.ToString("yyyMMdd");
			sector.Last_Edited = DateTime.Now.ToString("yyyMMdd");
			sector.Content = content;
			sector.TypeID = typeID;

			_databasecontext.Sector.Add(sector);
			await _databasecontext.SaveChangesAsync();

            return Ok(sector);


        }

		/// <summary>
		/// Edit a Sector on a Resume
		/// </summary>
		[HttpPut]
		[Route("EditResumeSector")]
		public async Task<IActionResult> EditResume(int RID, int SID, SectorModel model)
		{
			var resume = await _databasecontext.Resume.FindAsync(RID);

			if(resume == null)
            {
				return NotFound();
            }
							
			var sector = await _databasecontext.Sector.FindAsync(SID);

			if(sector == null)
            {
				return NotFound();
            }

			sector.SID = model.SID;
			sector.Creation_Date = model.CreationDate.ToString("yyyMMdd");
			sector.Last_Edited = model.LastEditedDate.ToString("yyyMMdd");
			sector.Content = model.Content;
			sector.TypeID = model.SectorType;
			sector.RID = RID;

			resume.Last_Edited = DateTime.Now.ToString("yyyMMdd");


			try
            {				
                await _databasecontext.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                return NotFound(ex.Message);
			}

            return Ok(resume);

		}


		/// <summary>
		/// Get a Resume
		/// </summary>
		[HttpGet]
		[Route("GetResume")]
		public async Task<ActionResult<ResumeModel>> GetResume(int RID)
		{
			
			///var resume = Resumes.Find(x => x.RID == RID);			
            var resume = await _databasecontext.Resume.FindAsync(RID);

            if(resume == null)
            {
                return NotFound();
            }

			// Get all sectors for this resume
			ResumeModel result = ControllerHelpers.ResumeEntityToModel(resume);

			var sectors = _databasecontext.Sector.Where(s => s.RID == RID);

            foreach (var sector in sectors)
            {
				result.SectorList.Add(ControllerHelpers.SectorEntityToModel(sector));
            }

			return result;            
		}


		/// <summary>
		/// Get all Resumes for an Employee
		/// </summary>
		[HttpGet]
		[Route("GetResumesForEmployee")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetResumesForEmployee(int EID)
		{					
			//var resumes = Resumes;
			
            
			var resumes = _databasecontext.Resume.Where(r => r.EID == EID);
			List<ResumeModel> result = new List<ResumeModel>();
            foreach (var resume in resumes)
            {
				result.Add(ControllerHelpers.ResumeEntityToModel(resume));
            }
            return result;            
		}

		/// <summary>
		/// Get presonal Resume for an Employee
		/// </summary>
		[HttpGet]
		[Route("GetPersonalResumesForEmployee")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetPersonalResumesForEmployee(int EID)
		{

			return BadRequest("Not implemented");
			
			
             var resumes =  _databasecontext.Resume.Where(r => r.EID == EID && r.WID == null);
			 

            if(resumes == null)
            {
                return NotFound();
            }

			List<ResumeModel> result = new List<ResumeModel>();
            foreach (var resume in resumes)
            {
				result.Add(ControllerHelpers.ResumeEntityToModel(resume));
            }

            return result;
            //return EmployeeEntityToModel(employee);
		}

		/// <summary>
		/// Export Resume
		/// </summary>
		[HttpGet]
		[Route("ExportResume")]
		public async Task<IActionResult> ExportResume(int RID)
		{
			return BadRequest("Not implemented");

			// return JsonResult(resume);
		}

		/// <summary>
		/// Search all Resumes
		/// </summary>
		[HttpGet]
		[Route("SearchResumes")]
		public async Task<IActionResult> SearchResume(string filter)
		{
			return BadRequest("Not implemented");
		}

		/// <summary>
		/// Search all Sectors
		/// </summary>
		[HttpGet]
		[Route("SearchSectors")]
		public async Task<IActionResult> SearchSectors(string filter)
        {
			return BadRequest("Not implemented");
        }


		/// <summary>
		/// Search all Employees
		/// </summary>
		[HttpGet]
		[Route("SearchEmployees")]
		public IEnumerable<EmployeeModel> SearchEmployees(string? filter)
        {
			// Ensure that null value returns all Employees
			if(filter == null)
            {
				filter = "";
            }

			// Current search only supports text fields
			// TODO: search by resume content
			//var employees = _databasecontext.Employee.AsQueryable().
			//	Where(e => e.Name.Contains(filter) ||
			//			   e.Email.Contains(filter)||
			//			   e.Access.Contains(filter)||
			//			   e.Username.Contains(filter));
			// Search all Employees by Employee joined with Resume and Sectors
			var employees = from e in _databasecontext.Employee
						  join r in _databasecontext.Resume on e.EID equals r.EID
						  join s in _databasecontext.Sector on r.EID equals s.EID
						  where e.Name.Contains(filter) ||
								e.Email.Contains(filter) ||
								e.Access.Contains(filter) ||
								e.Username.Contains(filter) ||
								r.Name.Contains(filter) ||
								r.TemplateName.Contains(filter) ||
								s.Content.Contains(filter) ||
								s.TypeTitle.Contains(filter)
						  select new EmployeeModel{
							  EID = e.EID,
							  Email = e.Email,
							  Access = e.Access,
							  Username = e.Username,							  
                          };
			
            return employees;
        }

		/// <summary>
		/// Search all of an Employees resume
		/// </summary>
		[HttpGet]
		[Route("SearchAllEmployeeResumes")]
		public IEnumerable<ResumeModel> SearchEmployeeResume(string? filter, int EID)
		{
			// Ensure that null value returns all Employees
			if (filter == null)
			{
				filter = "";
			}

			// Get all Resumes for that EID
			//var resumes = _databasecontext.Resume.AsQueryable().
			//	Where(r => r.EID == EID);
			var resumes = from r in _databasecontext.Resume
						  join s in _databasecontext.Sector on r.EID equals s.EID
						  where r.EID == EID && (
						  r.Name.Contains(filter) ||
						  r.Creation_Date.Contains(filter) ||
						  r.Status.Contains(filter) ||
						  r.Last_Edited.Contains(filter) ||
						  r.WID.ToString().Contains(filter) ||
						  r.TemplateID.ToString().Contains(filter) ||
						  s.Content.Contains(filter) ||
						  s.TypeTitle.Contains(filter) ||
						  s.TypeID.ToString().Contains(filter)
						  )
						  select new ResumeModel { 
							  EID = r.EID,
							  CreationDate = DateTime.Parse(r.Creation_Date),
							  LastEditedDate = DateTime.Parse(r.Last_Edited),
							  RID = r.RID,
							  Status = (Status)Enum.Parse(typeof(Status), r.Status),
							  WID = r.WID,
							  Name = r.Name,
							  TemplateID = r.TemplateID,
							  TemplateName = r.TemplateName,							  
						  };
			
            foreach (var resume in resumes)
            {
				resume.SectorList = _databasecontext.Sector
					.Where(s => s.RID == resume.RID)
					.Select(s => ControllerHelpers.SectorEntityToModel(s))
					.ToList();

            }

			return resumes;

		}

		/// <summary>
		/// Search all Workspaces
		/// </summary>
		[HttpGet]
		[Route("SearchWorkspaces")]
		public async Task<IActionResult> SearchWorkspaces(string filter)
        {
			return BadRequest("Not implemented");
        }
    }
}

