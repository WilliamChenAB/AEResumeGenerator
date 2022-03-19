using ae_resume_api.Admin;
using ae_resume_api.Attributes;
using ae_resume_api.Facade;
using System.Globalization;

namespace ae_resume_api.Controllers
{
    internal static class ControllerHelpers
    {              
        public static SectorModel SectorEntityToModel(SectorEntity entity) =>
            new SectorModel
            {
                SID = entity.SID,
                CreationDate = DateTime.ParseExact(entity.Creation_Date, "yyyyMMdd", CultureInfo.InvariantCulture),
                LastEditedDate = DateTime.ParseExact(entity.Last_Edited, "yyyyMMdd", CultureInfo.InvariantCulture),
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
                Password = entity.Password
            };


        public static TemplateModel TemplateEntityToModel(TemplateEntity entity) =>
            new TemplateModel
            {
                TemplateID = entity.TemplateID,
                Title = entity.Title,
                Description = entity.Description,
                LastEdited = DateTime.ParseExact(entity.Last_Edited, "yyyyMMdd", CultureInfo.InvariantCulture),
                EID = entity.EID
            };

        public static SectorTypeModel SectorTypeEntityToModel(SectorTypeEntity entity) =>
            new SectorTypeModel
            {
                TypeID = entity.TypeID,
                Title = entity.Title,
                Description = entity.Description,
                EID = entity.EID
            };

        public static WorkspaceModel WorkspaceEntityToModel(WorkspaceEntity entity) =>
            new WorkspaceModel
            {
                WID = entity.WID,
                CreationDate = DateTime.ParseExact(entity.Creation_Date, "yyyyMMdd", CultureInfo.InvariantCulture),
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
                CreationDate = DateTime.ParseExact(entity.Creation_Date, "yyyyMMdd", CultureInfo.InvariantCulture),
                LastEditedDate = DateTime.ParseExact(entity.Last_Edited, "yyyyMMdd", CultureInfo.InvariantCulture),
                Name = entity.Name,
                RID = entity.RID,
                TemplateID = entity.TemplateID,
                TemplateName = entity.TemplateName                    
            };
       



    }
}