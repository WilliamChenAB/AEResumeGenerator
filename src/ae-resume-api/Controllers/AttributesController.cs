using System;
using ae_resume_api.Attributes;
using ae_resume_api.Facade;
using ae_resume_api.Admin;
using ae_resume_api.DBContext;
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


namespace ae_resume_api.Controllers
{
	[Route("Attributes")]
	[ApiController]
	public class AtrributesController : ControllerBase
	{
		readonly DatabaseContext _databaseContext;

		// Temp list
		public List<WorkspaceModel> Workspaces { get; set; } = new List<WorkspaceModel>();


		public AtrributesController(DatabaseContext dbContext)
		{
			_databaseContext = dbContext;

			// populate testing data
			Workspaces.Add(new WorkspaceModel { 
				WID = 1,
				Division = "Water",
				CreationDate = "01/01/2022",
				ProposalNumber = 1,
				Resumes = new List<ResumeModel>{
				new ResumeModel
				{
					RID = 2,
					CreationDate = "01/01/2020",
					LastEditedDate = "01/01/2020",
					SectorList = null
				},
				new ResumeModel{
					RID = 5,
					CreationDate = "02/02/2020",
					LastEditedDate = "02/02/2020",
					SectorList = new List<SectorModel>{
						new SectorModel{
							SID = 1,
							SectorType = 2,
							Content = "Test sector 1",
							CreationDate = "02/02/2020",
							LastEditedDate = "02/02/2020"
						},
						new SectorModel{
							SID = 5,
							SectorType = 3,
							Content = "Test sector 2",
							CreationDate = "02/02/2020",
							LastEditedDate = "02/02/2020"
						}
					}
				}
				}
			});
		}

		/// <summary>
		/// Create a new Workspace
		/// </summary>
		[HttpPost]
		[Route("NewWorkspace")]
		public async Task<IActionResult> NewWorkspace([FromBody] WorkspaceModel model)
		{			
			Workspaces.Add(model);
            WorkspaceEntity entity = new WorkspaceEntity
            {
                WID = model.WID,
				Division = model.Division,
				Creation_Date = model.CreationDate,
				Proposal_Number = model.ProposalNumber
            };

			// TODO: implement DB
            //_databaseContext.Workspace.Add(entity);
			// await _databaseContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetWorkspace),
                new { WID = model.WID },
                model);
		}

		/// <summary>
		/// Get a workspace
		/// </summary>
		[HttpGet]
		[Route("GetWorkspace")]
		public async Task<ActionResult<WorkspaceModel>> GetWorkspace(int WID)
		{			
			var workspace = Workspaces.Find(x => x.WID == WID);
			// TODO: implement DB
            //var workspace = await _databaseContext.Workspace.FindAsync(WID);

            if(workspace == null)
            {
                return NotFound();
            }
            return workspace;
            //return EmployeeEntityToModel(employee);
		}

		/// <summary>
		/// Copy a Resume to a Workspace
		/// </summary>
		[HttpPost]
		[Route("CopyResume")]
		public async Task<IActionResult> CopyResume(int RID, int WID)
		{
			var workspace = Workspaces.Find(x => x.WID == WID);

			// TODO: implement DB
            // var workspace = await _databaseContext.Workspace.FindAsync(WID);
			if(workspace == null)
            {
                return NotFound();
            }

			// TODO: implement DB
			//var resume = await _databaseContext.Resume.FindAsync(RID);			
			var resume = Workspaces.First().Resumes.Last();

			if(resume == null)
            {
				return NotFound();
            }

			resume.WID = workspace.WID;
			workspace.Resumes.Add(resume);			

            try
            {
				await _databaseContext.SaveChangesAsync();
            }
			catch (Exception ex)
            {
				return NotFound(ex.Message);
            }

			return Ok(resume);
			

		}

		/// <summary>
		/// Delete a workspace
		/// </summary>
		[HttpDelete]
		[Route("DeleteWorkspace")]
		public async Task<IActionResult> DeleteWorkspace(int WID)
		{			
			var workspace = Workspaces.Find(x => x.WID == WID);
			// TODO: implement DB
            //var workspace = await _databaseContext.Workspace.FindAsync(WID);

            if (workspace == null)
            {
                return NotFound();
            }
            
            Workspaces.Remove(workspace);
			// TODO: implement DB
            //_databaseContext.Workspace.Remove(workspace);
			// await _databaseContext.SaveChangesAsync();
            
            return Ok();
		}


		/// <summary>
		/// Get all Resumes from a Workspace
		/// </summary>
		[HttpGet]
		[Route("GetResumesForWorkspace")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetResumesForWorkspace(int WID)
		{			
			var workspace = Workspaces.Find(x => x.WID == WID);
			// TODO: implement DB
			//var workspace = await _databaseContext.Workspace.FindAsync(WID);
			//TODO: convert Workspace entity to model

			if (workspace == null)
            {
                return NotFound();
            }
            
            var resumes = workspace.Resumes;
            
            
            return Ok(resumes);

		}

		/// <summary>
		/// Create a Template request for a specific Employee
		/// </summary>
		[HttpPost]
		[Route("CreateTemplateRequest")]
		public async Task<IActionResult> CreateTemplateRequest(int TemplateID, int EID)
		{
			return null;
			// Create a blank resume in the employee with the template type
			var employee = await _databaseContext.Employee.FindAsync(EID);

			if (employee == null)
			{
				return NotFound();
			}

			// Create a blank resume that has all the sectors in the template
			var template = TemplateEntityToModel(await _databaseContext.Resume_Template.FindAsync(TemplateID));
			// TODO: I dont think we can just loop through all the sectortypes depending on how we are storing templates

			if (template == null)
			{
				NotFound();
			}

			ResumeEntity templateResume = new ResumeEntity();
			templateResume.TemplateID = TemplateID;
			templateResume.Status = Status.Requested.ToString();
			templateResume.EID = EID;
			foreach (var sectorType in template.SectorTypes)
			{
				SectorModel sector = new SectorModel();
				sector.SectorType = sectorType.TypeID;
				sector.CreationDate = DateTime.Now.ToString("yyyyMMdd");

				_databaseContext.Template_Type.Add(new TemplateSectorsEntity
				{
					TemplateID = TemplateID,
					TypeID = sectorType.TypeID
				});

				await _databaseContext.Resume.AddAsync(templateResume);

				try
				{
					await _databaseContext.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					return NotFound(ex.Message);
				}

				return Ok(employee);

			}
		}
		private static TemplateModel TemplateEntityToModel(TemplateEntity entity) =>
			new TemplateModel
			{
				TemplateID = entity.TemplateID,
				Title = entity.Title,
				Description = entity.Description
			};

	}
}

