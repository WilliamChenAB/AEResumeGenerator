using ae_resume_api.Admin;
using ae_resume_api.Attributes;
using ae_resume_api.Facade;

namespace ae_resume_api.Controllers
{
    internal static class ControllerHelpers
    {              
        public static SectorModel SectorEntityToModel(SectorEntity entity) =>
            new SectorModel
            {
                SID = entity.SID,
                CreationDate = Convert.ToDateTime(entity.Creation_Date),
                LastEditedDate = Convert.ToDateTime(entity.Last_Edited),
                Content = entity.Content,
                TypeID = entity.TypeID,
                TypeTitle = entity.TypeTitle,
                RID = entity.RID,
                ResumeName = entity.ResumeName
            };

        public static EmployeeModel EmployeeEntityToModel(EmployeeEntity entity) =>
            new EmployeeModel
            {
                EID = entity.EID,
                Email = entity.Email,
                Name = entity.Name,
                Username = entity.Username,
                Password = entity.Password,
            };


        public static TemplateModel TemplateEntityToModel(TemplateEntity entity) =>
            new TemplateModel
            {
                TemplateID = entity.TemplateID,
                Title = entity.Title,
                Description = entity.Description
            };

        public static SectorTypeModel SectorTypeEntityToModel(SectorTypeEntity entity) =>
            new SectorTypeModel
            {
                TypeID = entity.TypeID,
                Title = entity.Title,
                Description = entity.Description
            };

        public static WorkspaceModel WorkspaceEntityToModel(WorkspaceEntity entity) =>
            new WorkspaceModel
            {
                WID = entity.WID,
                CreationDate = Convert.ToDateTime(entity.Creation_Date),
                Division = entity.Division,
                ProposalNumber = entity.Proposal_Number,
                Name = entity.Name,
                EID = entity.EID
            };
        public static ResumeModel ResumeEntityToModel(ResumeEntity entity) =>
            new ResumeModel
            {
                WID = entity.WID,
                EID = entity.EID,
                CreationDate = Convert.ToDateTime(entity.Creation_Date),
                LastEditedDate = Convert.ToDateTime(entity.Last_Edited),
                Name = entity.Name,
                RID = entity.RID,
                TemplateID = entity.TemplateID,
                TemplateName = entity.TemplateName                    
            };
       



    }
}