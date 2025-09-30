using AutoMapper;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Mappings;

public class SalaryMappingProfile : Profile
{
    public SalaryMappingProfile()
    {
        CreateMap<Salary, SalaryDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.Name));
        CreateMap<CreateSalaryDto, Salary>();
        CreateMap<UpdateSalaryDto, Salary>();
    }
}
