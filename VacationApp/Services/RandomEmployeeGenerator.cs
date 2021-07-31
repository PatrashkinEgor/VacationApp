using System;
using System.IO;
using VacationApp.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace VacationApp.Services

{

    /// <summary>
    /// List of possible professions. Written in enum according to the terms of reference.
    /// </summary>
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
    /// <summary>
    /// The class is designed to create an employee object with random properties. 
    /// The last name, first name and patronymic are selected from those available in 
    /// the REGSettings.json file. The ending of the surname varies depending on the 
    /// gender of the employee. Age of employee depends on min hiring age and retirement 
    /// age. Retirement age depends on gender. The hiring age and both retirement ages 
    /// are placed in REGSettings.json file.
    /// </summary>

    public class RandomEmployeeGenerator : IEmployeeFactory
    {
#if DEBUG
    private const string _settingsPath = "../../../Services/REGSettings.json";
#else
    private const string _settingsPath = "Services/REGSettings.json";
#endif
        private readonly Random _rand = new();
        private readonly Settings _settings;
        private int _nextEmployeeId = 0;


        /// <summary>
        /// Constructor. Deserializes settings.
        /// </summary>
        public RandomEmployeeGenerator()
        {
            _settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(_settingsPath));
        }

        /// <summary>
        /// Сreates an employee object with random properties, except Id. Id is uniq for emploeeGenerator.
        /// </summary>
        /// <returns>New Employee object</returns>
        public Employee GetNewEmployee()
        {

            string surname = _settings.Surnames[_rand.Next(_settings.Surnames.Length)];
            string name, secondName;
            int age;

            Gender gender = (Gender)_rand.Next(Enum.GetValues(typeof(Gender)).Length);
            GenderSpecificSettings gSettings = gender == Gender.Male ? _settings.Male : _settings.Female;
            if (gender == Gender.Female) surname = СreateFemaleSurnameFromMale(surname);

            name = gSettings.Names[_rand.Next(gSettings.Names.Length)];
            secondName = gSettings.SecondNames[_rand.Next(gSettings.SecondNames.Length)];
            age = _rand.Next(_settings.MinHiringAge, gSettings.RetirementAge);
            var position = (Position)_rand.Next(Enum.GetValues(typeof(Position)).Length);

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

        /// <summary>
        /// Creates a female surname from a male surname by replacing the ending.
        /// Works only for Russian style surname on Russian language.
        /// </summary>
        /// <param name="MaleSurname"></param>
        /// <returns> string FemaleSurname</returns>
        public static string СreateFemaleSurnameFromMale(string MaleSurname)
        {
            if (Regex.IsMatch(MaleSurname, "(ов|ев|ёв|ин)$"))
            {
                return MaleSurname + "а";
            }
            return Regex.Replace(MaleSurname, "(ий|ый|ой)$", "ая");

        }


        /// <summary>
        /// DTO object. Required for deserialization of  the settings file
        /// </summary>
        private class GenderSpecificSettings
        {
            public string[] Names { get; set; }
            public string[] SecondNames { get; set; }
            public int RetirementAge { get; set; }
        }

        /// <summary>
        /// DTO object. Required for deserialization of  the settings file
        /// </summary>
        private class Settings
        {
            public string[] Surnames { get; set; }
            public int MinHiringAge { get; set; }
            public GenderSpecificSettings Male { get; set; }
            public GenderSpecificSettings Female { get; set; }
        }
    }
}
