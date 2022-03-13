using System;
using ae_resume_api.Attributes;
using ae_resume_api.Facade;
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
		private readonly IAttributeService _attributeservice;

		public List<WorkspaceModel> Workspaces { get; set; } = new List<WorkspaceModel>();


		public AtrributesController(IAttributeService attributeservice)
		{
			_attributeservice = attributeservice;


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

		[HttpPost]
		[Route("NewWorkspace")]
		public async Task<IActionResult> NewWorkspace([FromBody] WorkspaceModel model)
		{
			// return await _attributeservice.NewWorkspace(model);

			Workspaces.Add(model);
            WorkspaceEntity entity = new WorkspaceEntity
            {
                WID = model.WID,
				Division = model.Division,
				CreationDate = model.CreationDate,
				ProposalNumber = model.ProposalNumber
            };
            //_databaseContext.Employees.Add(entity);

            return CreatedAtAction(
                nameof(GetWorkspace),
                new { WID = model.WID },
                model);
		}

		[HttpGet]
		[Route("GetWorkspace")]
		public async Task<ActionResult<WorkspaceModel>> GetWorkspace(int WID)
		{
			// return await _attributeservice.GetWorkspace(WID);

			var workspace = Workspaces.Find(x => x.WID == WID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if(workspace == null)
            {
                return NotFound();
            }
            return workspace;
            //return EmployeeEntityToModel(employee);
		}

		[HttpPost]
		[Route("CopyResume")]
		public async Task<HttpResponseMessage> CopyResume(int EID, int WID)
		{
			return await _attributeservice.CopyResume(EID, WID);
		}

		[HttpDelete]
		[Route("DeleteWorkspace")]
		public async Task<IActionResult> DeleteWorkspace(int WID)
		{
			// return await _attributeservice.DeleteWorkspace(WID);

			var workspace = Workspaces.Find(x => x.WID == WID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if (workspace == null)
            {
                return NotFound();
            }
            
            Workspaces.Remove(workspace);
            //_databaseContext.Employees.Remove(employee);
            //return NoContent();
            return Ok();
		}

		[HttpGet]
		[Route("GetResumes")]

		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetResumes(int WID)

		{
			// return await _attributeservice.GetResumes(WID);

			var workspace = Workspaces.Find(x => x.WID == WID);

			if (workspace == null)
            {
                return NotFound();
            }
            
            var resumes = workspace.Resumes;
            //_databaseContext.Employees.Remove(employee);
            //return NoContent();
            return Ok(resumes);

		}

		[HttpPost]
		[Route("CreateTemplateRequest")]
		public async Task<HttpResponseMessage> CreateTemplateRequest(int TemplateID, int EID)
		{
			return await _attributeservice.CreateTemplateRequest(TemplateID, EID);
		}
	}
}

