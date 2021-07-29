using System;
using Xunit;
using VacationApp.Serviсes;

namespace TestVacationApp.Services
{
    public class RandomEmployeeGeneratorTest
    {
        [Theory]
        [InlineData("Сидоров", "Сидорова")]
        [InlineData("Королёв", "Королёва")]
        [InlineData("Бородин", "Бородина")]
        [InlineData("Короткий", "Короткая")]
        [InlineData("Проклятый", "Проклятая")]
        [InlineData("Мостовой", "Мостовая")]
        [InlineData("Шеппард", "Шеппард")]
        public void FemaleSurnameTest(string MaleSurname, string ExpectedFemaleSurname)
        {
            string FemaleSurname = RandomEmployeeGenerator.createFemaleSurnameFromMale(MaleSurname);
            Assert.Equal(ExpectedFemaleSurname, FemaleSurname);
        }
    }
}
