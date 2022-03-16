using System;
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

		public List<ResumeModel> Resumes = new List<ResumeModel>();
		public List<SectorModel> Sectors = new List<SectorModel>();

		public FacadeController(DatabaseContext databaseContext)
		{
			_databasecontext = databaseContext;

			Resumes.Add(new ResumeModel { 
				RID = 3,
				EID = 1,
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
			Resumes.Add(new ResumeModel
			{
				RID = 5,
				EID = 2,
				CreationDate = "02/02/2020",
				LastEditedDate = "02/02/2020",
				SectorList = new List<SectorModel> {
					new SectorModel {
						SectorType = 4,
						SID = 4,
						Content = "my work experince",
						CreationDate = "02/02/2020",
						LastEditedDate = "02/02/2020"
					},
					new SectorModel {
						SectorType = 4,
						SID = 7,
						Content = "More work experince",
						CreationDate = "02/02/2020",
						LastEditedDate = "02/02/2020"
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
		
		/// <summary>
        /// Add a Resume to an Employee
        /// </summary>
		[HttpPost]
        [Route("NewResume")]		
		public async Task<ActionResult<ResumeModel>> NewResume(string templateName, int EID)
		{	
			
			// Find the template record
			var template =  _databasecontext.Resume_Template.FirstOrDefault(t => t.Title == templateName);
			

			if( template == null )
            {
				return NotFound();
            }				

			// Find the Sector Types associated with that template
			var sectorTypes =  _databasecontext.Template_Type
				.Where(x => x.TemplateID == template.TemplateID);


			ResumeEntity entity = new ResumeEntity
            {               
			   Creation_Date = DateTime.Now.ToString("yyyMMdd"),
			   EID = EID,
			   TemplateID = template.TemplateID			  		   
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
		
			Sectors.Add(model);

            SectorEntity entity = new SectorEntity
            {
               SID = model.SID,
			   Creation_Date = model.CreationDate,
			   Last_Edited = model.LastEditedDate,
			   Content = model.Content
            };
			// TODO: Implement DB connection
			//_databaseContext.Sector.Add(entity);
			// await _databasecontext.SaveChangesAsync();


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
			

			var sector = Sectors.Find(x => x.SID == SID);
			// TODO: Implement DB connection
            //var sector = await _databaseContext.Sector.FindAsync(SID);

            if(sector == null)
            {
                return NotFound();
            }
            return sector;            
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
				sectorList.Add(SectorEntityToModel(sector));
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
				sectorList.Add(SectorEntityToModel(sector));
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
			

			var resume = Resumes.Find(x => x.RID == RID);
			// TODO: Implement DB connection
            //var eresumemployee = await _databaseContext.Resume.FindAsync(RID);

            if (resume == null)
            {
                return NotFound();
            }
            
            Resumes.Remove(resume);
			// TODO: Implement DB connection
            //_databaseContext.Employee.Remove(employee);
            // await _databasecontext.SaveChangesAsync();
            return Ok();
		}

		/// <summary>
		/// Delete a Sector from a Resume
		/// </summary>	
		[HttpDelete]
		[Route("DeleteSector")]
		public async Task<IActionResult> DeleteSector(int SID, int RID)
		{			
			var resume = Resumes.Find(x => x.RID == RID);
            //var resume = await _databaseContext.Resume.FindAsync(RID);

            if (resume == null)
            {
                return NotFound();
            }

            var sector = Sectors.Find(x => x.SID == SID);
			
			 //var sector = await _databaseContext.Sector.FindAsync(SID);			 
			resume.SectorList.Remove(sector);
			// await _databasecontext.SaveChangesAsync();

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
            var sector = Sectors.Find(x => x.SID == SID);
            //var sector = await _databaseContext.Sector.FindAsync(SID);

            if (sector == null)
            {
                return NotFound();
            }

            sector.SID = model.SID;
			sector.CreationDate = model.CreationDate;
			sector.LastEditedDate = model.LastEditedDate;
			sector.Content = model.Content;
			sector.SectorType = model.SectorType;

			try
            {
                //await _databasecontext.SaveChangesAsync();
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
		[HttpPut]
		[Route("AddSectorToResume")]
		public async Task<IActionResult> AddSectorToResume(int RID, SectorModel model)
        {
			// TODO: implement DB
			// var resume = await _databasecontext.Resume.FindAsync(x => x.RID == RID);
			var resume = Resumes.First();

			if (resume == null)
            {
                return NotFound();
            }

			resume.SectorList.Add(model);

			try
            {
				// TODO: implement DB
                //await _databasecontext.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                return NotFound(ex.Message);
			}

            return Ok(resume);


        }

		/// <summary>
		/// Edit a Sector on a Resume
		/// </summary>
		[HttpPut]
		[Route("EditResumeSector")]
		public async Task<IActionResult> EditResume(int RID, int SID, SectorModel model)
		{			
			// TODO: implement DB
			// var resume = await _databasecontext.Resume.FindAsync(x => x.RID == RID);
			var resume = Resumes.First();

			if (resume == null)
            {
                return NotFound();
            }

			var sector = resume.SectorList.Find(x => x.SID == SID);

			if(sector == null)
            {
				return NotFound();
            }

			sector.SID = model.SID;
			sector.CreationDate = model.CreationDate;
			sector.LastEditedDate = model.LastEditedDate;
			sector.Content = model.Content;
			sector.SectorType = model.SectorType;

			resume.LastEditedDate = DateTime.Now.ToLongDateString();


			try
            {
				// TODO: implement DB
                //await _databasecontext.SaveChangesAsync();
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
			
			var resume = Resumes.Find(x => x.RID == RID);
			// TODO: implement DB
            //var resume = await _databaseContext.Resume.FindAsync(RID);

            if(resume == null)
            {
                return NotFound();
            }
            return resume;            
		}


		/// <summary>
		/// Get all Resumes for an Employee
		/// </summary>
		[HttpGet]
		[Route("GetResumesForEmployee")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetResumesForEmployee(int EID)
		{					
			var resumes = Resumes;
			// TODO: implement DB
            // var employee = await _databaseContext.Employee.FindAsync(EID);
			// var resumes = employee.Resumes;

            if(resumes == null)
            {
                return NotFound();
            }
            return resumes;
            //return EmployeeEntityToModel(employee);
		}

		/// <summary>
		/// Get all Resumes for an Employee
		/// </summary>
		[HttpGet]
		[Route("GetPersonalResumesForEmployee")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetPersonalResumesForEmployee(int EID)
		{					
			var resumes = Resumes;
			// TODO: implement DB
            // var employee = await _databaseContext.Employee.FindAsync(EID);
			// var resumes = employee.Resumes;

            if(resumes == null)
            {
                return NotFound();
            }
            return resumes;
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
			var employees = _databasecontext.Employee.AsQueryable().
				Where(e => e.Name.Contains(filter) ||
						   e.Email.Contains(filter)||
						   // e.Access.Contains(filter)||
						   e.Username.Contains(filter));
			List<EmployeeModel>? result = new List<EmployeeModel>();
			foreach (var employee in employees)
			{
				EmployeeModel e =  AdminController.EmployeeEntityToModel(employee);
				result.Add(e);
			}

            List<EmployeeModel> employeeModels = new List<EmployeeModel>(result);
            return employeeModels;
        }

		/// <summary>
		/// Search all Resumes of a specifc Employee
		/// </summary>
		[HttpGet]
		[Route("SearchEmployeeResume")]
		public async Task<IActionResult> SearchEmployeeResume(string filter)
        {
			return BadRequest("Not implemented");
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

		 /// <summary>
        /// Translate the Employee entity to model used
        /// </summary>        
        public static SectorModel SectorEntityToModel(SectorEntity entity) =>
            new SectorModel
            {                
				SID = entity.SID,
				CreationDate = entity.Creation_Date,
				LastEditedDate = entity.Last_Edited,
				Content = entity.Content,
				TypeID = entity.TypeID
				//TypeName = entity.TypeName
            };
	}
}

