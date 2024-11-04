using Quartz;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace scrubsAPI
{
    public class MissedInspectionNotification : IJob
    {
        private readonly ScrubsDbContext _dbContext;



        public MissedInspectionNotification(ScrubsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var currentDate = DateTime.UtcNow;


            var missedInspections = await _dbContext.Inspections
                .Where(i => i.nextVisitDate < currentDate && i.conclusion == Conclusion.Disease)
                .Include(i => i.patient)
                .Include(i => i.doctor)
                .ToListAsync();

            foreach (var inspection in missedInspections)
            {
                var subject = $"Пропущенный визит пациента {inspection.patient.name}";
                var body = $"Пациент {inspection.patient.name} не явился на запланированный визит на {inspection.nextVisitDate?.ToString("yyyy-MM-dd")}.";


                using (var client = new SmtpClient("localhost", 1025))
                {
                    var mailMessage = new MailMessage("noreply@TryNotToDie.com", inspection.doctor.email, subject, body);

                    client.Send(mailMessage);
                }

            }
        }
    }
}
