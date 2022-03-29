using ae_resume_api.Models;
using ae_resume_api.Models;
using ae_resume_api.Models;
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

        public static bool ResumeIsPersonal(ResumeEntity r)
        {
            return (r.Status == Status.Regular && r.WorkspaceId == null)
                || r.Status == Status.Requested;
        }

        public static SectorModel SectorEntityToModel(SectorEntity entity) =>
            new SectorModel
            {
                SectorId = entity.SectorId,
                CreationDate = parseDate(entity.Creation_Date),
                LastEditedDate = parseDate(entity.Last_Edited),
                Content = entity.Content,
                TypeId = entity.TypeId,
                TypeTitle = entity.Type.Title,
                ResumeId = entity.ResumeId,
                ResumeName = entity.Resume.Name,
                Division = entity.Division,
                Image = entity.Image
            };

        public static EmployeeModel EmployeeEntityToModel(EmployeeEntity entity) =>
            new EmployeeModel
            {
                EmployeeId = entity.EmployeeId.ToString(),
                Email = entity.Email,
                Name = entity.Name,
                JobTitle = entity.JobTitle,
                Access = entity.Access
            };


        public static TemplateModel TemplateEntityToModel(TemplateEntity entity) =>
            new TemplateModel
            {
                TemplateId = entity.TemplateId,
                Title = entity.Title,
                Description = entity.Description,
                LastEdited = parseDate(entity.Last_Edited),
                SectorTypes = entity.TemplateSectors.Select(x => SectorTypeEntityToModel(x.SectorType)).ToList()
            };

        public static SectorTypeModel SectorTypeEntityToModel(SectorTypeEntity entity) =>
            new SectorTypeModel
            {
                TypeId = entity.TypeId,
                Title = entity.Title,
                Description = entity.Description,
            };

        public static WorkspaceModel WorkspaceEntityToModel(WorkspaceEntity entity) =>
            new WorkspaceModel
            {
                WorkspaceId = entity.WorkspaceId,
                CreationDate = parseDate(entity.Creation_Date),
                Division = entity.Division,
                ProposalNumber = entity.Proposal_Number,
                Name = entity.Name,
                EmployeeId = entity.EmployeeId.ToString(),
                Resumes = entity.Resumes?.Select(x => ResumeEntityToModel(x)).ToList()
            };

        public static ResumeModel ResumeEntityToModel(ResumeEntity entity) =>
            new ResumeModel
            {
                WorkspaceId = entity.WorkspaceId,
                EmployeeId = entity.EmployeeId.ToString(),
                EmployeeName = entity.Employee.Name,
                CreationDate = parseDate(entity.Creation_Date),
                LastEditedDate = parseDate(entity.Last_Edited),
                Name = entity.Name,
                ResumeId = entity.ResumeId,
                TemplateId = entity.TemplateId,
                TemplateName = entity.Template?.Title,
                Status = entity.Status,
                SectorList = entity.Sectors.Select(x => SectorEntityToModel(x)).ToList()
            };

        public static Status ParseStatus(string status)
        {
            return (Status)Enum.Parse(typeof(Status), status);
        }
    }
}