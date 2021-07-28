using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VacationApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VacationApp.Serviсes
{
    class RundomEmployeeGenerator : IEmployeeFactory
    {
        Random _rand = new();
        public RundomEmployeeGenerator()
        {
            JObject jObject = JObject.Parse(File.ReadAllText("../../../Serviсes/REGSettings.json"));
        }
        Employee IEmployeeFactory.GetNewEmployee()
        {
            return new Employee();
        }
    }
}
