using AutoMapper;
using ProjetoClinicaASPNETCore.Data.DTOs;
using ProjetoClinicaASPNETCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Animal, AnimalDTO>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();
        }
    }
}
