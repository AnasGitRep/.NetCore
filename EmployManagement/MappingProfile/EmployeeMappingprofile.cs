using AutoMapper;
using EmployManagement.Dto.Master;
using EmployManagement.Models.Master;

namespace EmployManagement.MappingProfile
{
    public class EmployeeMappingprofile:Profile
    {

        public EmployeeMappingprofile()
        {
            CreateMap<Employe, EmployeeDto>().
            ForMember(x => x.DepatmentName, option => option.MapFrom(src => src.Depatment.name)).
            ForMember(x => x.Manager, option => option.MapFrom(src => src.Manager.name));

            CreateMap<EmployeeDto, Employe>();
        }
    }
}
