using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VacationApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace VacationApp.Serviсes
{
    public enum Position
    {
        Менеджер,
        Аудитор,
        Мастер,
        Инженер,
        Лаборант,
        Диспетчер,
        Бухгалтер,
        Переводчик,
        Кассир,
        Редактор
    }

    public class RandomEmployeeGenerator : IEmployeeFactory
    {
        private readonly Random _rand = new();
        private readonly Settings _settings;
        private int _nextEmployeeId = 0;

        public static string createFemaleSurnameFromMale(string MaleSurname)
        {
            if (Regex.IsMatch(MaleSurname, "(ов|ев|ёв|ин)$")){
                return MaleSurname + "а";
            }
            return Regex.Replace(MaleSurname, "(ий|ый|ой)$", "ая");

        }
        public RandomEmployeeGenerator()
        {
            _settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("../../../REGSettings.json"));
        }

        public Employee GetNewEmployee()
        {

            string surname = _settings.Surnames[_rand.Next(_settings.Surnames.Length)];
            string name, secondName;
            int age;

            Gender gender = (Gender)_rand.Next(Enum.GetValues(typeof(Gender)).Length);
            if (gender == Gender.Female) surname = createFemaleSurnameFromMale(surname);
            GenderSpecificSettings gSettings = gender == Gender.Male ? _settings.Male : _settings.Female;
            name = gSettings.Names[_rand.Next(gSettings.Names.Length)];
            secondName = gSettings.SecondNames[_rand.Next(gSettings.SecondNames.Length)];
            age = _rand.Next(_settings.MinHiringAge, gSettings.RetirementAge);
            Position position = (Position)_rand.Next(Enum.GetValues(typeof(Position)).Length);

            return new Employee()
            {
                Id = _nextEmployeeId++,
                Age = age,
                Name = name,
                SecondName = secondName,
                Surname = surname,
                Gender = gender,
                Position = position.ToString()

            };
        }

        public class GenderSpecificSettings
        {
            public string[] Names { get; set; }
            public string[] SecondNames { get; set; }
            public int RetirementAge { get; set; }
        }

        public class Settings
        {
            public string[] Surnames { get; set; }
            public int MinHiringAge { get; set; }
            public GenderSpecificSettings Male { get; set; }
            public GenderSpecificSettings Female { get; set; }
        }
    }
}
