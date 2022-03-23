using ae_resume_api.Admin;
using ae_resume_api.Attributes;
using ae_resume_api.Facade;
using System.Globalization;

namespace ae_resume_api.Controllers
{
    internal static class ControllerHelpers
    {

        private static readonly string DATE_TIME_FORMAT = "yyyyMMdd HH:mm:ss";

        public static DateTime parseDate(string dateTime)
        {
            return DateTime.ParseExact(dateTime, DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
        }

        public static string CurrentTimeAsString()
        {
            return DateTime.Now.ToString(DATE_TIME_FORMAT);
        }

        public static SectorModel SectorEntityToModel(SectorEntity entity) =>
            new SectorModel
            {
                SID = entity.SID,
                CreationDate = parseDate(entity.Creation_Date),
                LastEditedDate = parseDate(entity.Last_Edited),
                Content = entity.Content,
                TypeID = entity.TypeID,
                TypeTitle = entity.TypeTitle,
                RID = entity.RID,
                ResumeName = entity.ResumeName,
                Division = entity.Division,
                Image = entity.Image
            };

        public static EmployeeModel EmployeeEntityToModel(EmployeeEntity entity) =>
            new EmployeeModel
            {
                EID = entity.EID,
                Email = entity.Email,
                Name = entity.Name,
                JobTitle = entity.JobTitle
            };


        public static TemplateModel TemplateEntityToModel(TemplateEntity entity) =>
            new TemplateModel
            {
                TemplateID = entity.TemplateID,
                Title = entity.Title,
                Description = entity.Description,
                LastEdited = parseDate(entity.Last_Edited)
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
                CreationDate = parseDate(entity.Creation_Date),
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
                CreationDate = parseDate(entity.Creation_Date),
                LastEditedDate = parseDate(entity.Last_Edited),
                Name = entity.Name,
                RID = entity.RID,
                TemplateID = entity.TemplateID,
                TemplateName = entity.TemplateName,
                Status = (Status)Enum.Parse(typeof(Status), entity.Status)
            };

        public static Status ParseStatus(string status)
        {
            return (Status)Enum.Parse(typeof(Status), status);
        }




    }
}