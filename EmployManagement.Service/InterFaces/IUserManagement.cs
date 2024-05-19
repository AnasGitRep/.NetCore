
using EmployManagementDataBase.Dto.Base;
using EmployManagementDataBase.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployManagement.Service.InterFaces
{
    public interface IUserManagement
    {
        Task<ResponseModel<RegisterModel>>CreateUserWithTokenAsync(RegisterModel model);
        Task<ResponseModel<LoginModel>>Login(LoginModel model);
        Task<ResponseModel<LoginModel>> TwoFactoreAuth(string Authtoken, string UserName);
    }
}
