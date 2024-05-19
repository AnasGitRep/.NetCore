
using EmployManagementDataBase.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployManagement.Service.InterFaces
{
    public interface IEmailService
    {
        Task<ResponseModel<string>> SendMailAsync(string recipientEmail, string recipientName, string subject,string body);
    }
}
