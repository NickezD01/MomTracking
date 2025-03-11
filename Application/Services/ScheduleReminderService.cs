using Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ScheduleReminderService : IScheduleReminderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        public ScheduleReminderService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task CheckAndSendReminders()
        {
            var today = DateTime.Now.Date;
            var schedules = await _unitOfWork.Schedule.GetAllAsync(s => s.AppointmentDate == today);
            foreach(var schedule in schedules)
            {
                var user = await _unitOfWork.UserAccounts.GetAsync(u => u.Id == schedule.AccountId);
                var emailContent = EmailContentBuilder.BuildNotiMail(user.FirstName, schedule.AppointmentDate);
                await _emailService.SendNotiMail(user.Email, emailContent);
            }
        }
    }
}
