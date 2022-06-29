using Clientix.Models;
using Clientix.REST;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ZabotaSDK;
using ZabotaSDK.Data;

namespace Clientix
{
    /* Атрибут для детектирования этого класса как экспортируемого */
    [Export(typeof(IPlugin))]
    public class Clientix : IPlugin2
    {
        /* Ссылка на экземпляр "ядерного класса", содержит методы для работы с данными, логированием */
        private ICore _Core;

        /* Ссылка на экземляр реализации API для МИС Clientix*/
        private API _API;

        /* Имя плагина */
        public string Name => "Clientix";

        /* Описание плагина */
        public string Description => "Clientix Exporter";

        /* Автор плагина */
        public string AuthorName => "Yakovchenko Sergey";

        /* Обязательное поле, см. файл z20.ini. Предназначено для определения типа интеграции. Можно назвать Medods */
        public string Type => "Clientix";

        private void ExportEntity<T>(string name, List<T> data)
        {
            DataTable table = _Core.CreateDataTable(name, data);

            /* Отправка данных на экспорт, способ сохранения определит исполняемый модуль (по умолчанию данные будут сохранены в директорию out, в формате *.csv) */
            _Core.ExportDataTable(table);
        }

        /* Экспорт пациентов */
        public void ExportPatients()
        {
            /* Таблица кэширования */
            ZabotaCacheTable<Patient> cache = new ZabotaCacheTable<Patient>(_Core, "patients", x => x.PatientId);
            DateTime dateFrom = cache.LastUpdate.AddDays(-1);

            List<Patient> data = _API.GetPatients(dateFrom);
            data = data.Where(x => (x.Surname == null) || (!x.Surname.Contains("Звонок"))).ToList();

            /* Метод перезаписывает только новые данные в кеше */
            cache.Update(data);

            /* Отправка данных на экспорт, способ сохранения определит исполняемый модуль (по умолчанию данные будут сохранены в директорию out, в формате *.csv) */
            _Core.ExportDataTable(cache.GetDataTable());
        }

        private List<StatusType> GetStatusTypes()
        {
            /* Список фиксированных статусов данной МИС */
            return new List<StatusType> {
                new StatusType(1, "cancelled", true),
                new StatusType(2, "in_progress"),
                new StatusType(3, "scheduled"),
                new StatusType(4, "finished"),
                new StatusType(5, "missed"),
                new StatusType(6, "confirmed"),
                new StatusType(7, "cancelled_by_sms"),
                new StatusType(8, "sms_confirmation_sent")
            };
        }

        /* Метод экспорта визитов */
        public void ExportVisits()
        {
            /* Не всегда данные по API предоставляются в удобном виде, поэтому используются преобразования для формирования необходимых данных */

            List<StatusType> statusTypes = GetStatusTypes();

            List<Treatment> treatments = new List<Treatment>();
            List<VisitStatus> statuses = new List<VisitStatus>();
            List<Visit> visits = _API.GetVisits();

            for (int i = 0; i < visits.Count; i++)
            {
                var visit = visits[i];
                visit.VisitState = "N";

                if ((visit.PatientId == null) || (visit.VisitDate == null))
                {
                    visits.RemoveAt(i);
                    i--;
                    continue;
                }

                if (visit.Treatments != null)
                {
                    foreach (var treatment in visit.Treatments)
                    {
                        if ((visit.DoctorId == null) && (treatment.Doctors != null))
                        {
                            visit.DoctorId = treatment.Doctors.Select(x => x.DoctorID).DefaultIfEmpty(0).First();
                            if (visit.DoctorId == 0)
                            {
                                visit.DoctorId = null;
                            }
                        }

                        treatment.PatientId = visit.PatientId;
                        treatment.DoctorId = visit.DoctorId;
                        treatment.TreatmentDate = visit.VisitDate;
                        treatment.CustId = visit.CustId;

                        treatments.Add(treatment);
                    }
                }



                if (string.IsNullOrEmpty(visit.Status))
                    continue;

                if (statusTypes.Where(x => x.Description.Equals(visit.Status)).Count() == 0)
                    continue;

                StatusType statusType = statusTypes.Where(x => x.Description.Equals(visit.Status)).First();
                if (statusType.IsCancel)
                    visit.VisitState = "C";


                VisitStatus visitStatus = new VisitStatus(visit.VisitId, statusType.StatusId);
                statuses.Add(visitStatus);                
            }

            visits = visits.Where(x => x.DoctorId.HasValue).ToList();


            ExportEntity("patients_visits", visits);
            ExportEntity("patients_treatment", treatments);
            ExportEntity("visit_statuses", statuses);
        }

        /* Метод экспортирования данных, вызывается после Initialize */
        public void Export(ICore core)
        {
            ExportEntity("status_types", GetStatusTypes());

            ExportEntity("doctors", _API.GetDoctors());
            ExportEntity("procedures", _API.GetProcedures());
            ExportPatients();
            ExportVisits();        

        }

        /* Метод инициализации плагина, всегда вызывается первым */
        public bool Initialize(ICore core)
        {
            _Core = core;

            _API = new API
            {
                AccountID = core.GetSettingsValue("Database", "AccountID", ""),
                UserID = core.GetSettingsValue("Database", "UserID", ""),
                AccessToken = core.GetSettingsValue("Database", "AccessToken", "")
            };

            if (string.IsNullOrEmpty(_API.AccountID))
            {
                _Core.WriteLog("Вы не указали AccountID");
                return false;
            }

            if (string.IsNullOrEmpty(_API.UserID))
            {
                _Core.WriteLog("Вы не указали UserID");
                return false;
            }

            if (string.IsNullOrEmpty(_API.AccessToken))
            {
                _Core.WriteLog("Вы не указали AccessToken");
                return false;
            }


            return true;
        }

        /* Финализация плагина, если это требуется */
        public void Finalization()
        {
            
        }
    }
}
