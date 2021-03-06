﻿using Microsoft.AspNetCore.Identity;
using ProjetoClinicaASPNETCore.Data.Models;
using ProjetoClinicaASPNETCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoClinicaASPNETCore.Data.Interfaces
{
    public interface IConsultaRepository
    {
        void Add<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        Task<Consulta> GetConsultaById(int id);
        IEnumerable<Consulta> GetConsultas();
        IEnumerable<Consulta> GetConsultasByOwnerId(string userId);
        IEnumerable<Consulta> GetConsultaByDateAndVet(string date, int vetId);
        IEnumerable<Consulta> GetConsultaByDateAndVetAndTime(string date, int vetId, string time);
        void CreateConsulta(FormularioViewModel fVM);
    }
}
