using Clientix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clientix.REST
{
    public class API : BaseAPI
    {
        /// <summary>
        /// Метод получает всех докторов в базе данных
        /// </summary>
        /// <returns>Список докторов</returns>
        public List<Doctor> GetDoctors()
        {           
            return GetCollection<Doctor>("Users", new List<UrlParam>() { });
        }

        /// <summary>
        /// Метод получает все процедуры в базе данных
        /// </summary>
        /// <returns>Список процедур</returns>
        public List<Procedure> GetProcedures()
        {
            return GetCollection<Procedure>("Services", new List<UrlParam>() { });
        }

        /// <summary>
        /// Метод получает все визиты и пациентов в базе данных
        /// </summary>
        /// <returns>Список визитов и пациентов</returns>
        public List<Visit> GetVisits()
        {
            return GetCollection<Visit>("Appointments", new List<UrlParam>() { });
        }

        /// <summary>
        /// Метод получает информацию о пациентах
        /// </summary>
        /// <returns>Метод получает информацию о пациентах</returns>
        public List<Patient> GetPatients(DateTime modifedAt)
        {
            return GetCollection<Patient>("Clients", new List<UrlParam>() { new UrlParam("date", modifedAt.ToString("yyyy-MM-dd")) });
        }

       
    }
}
