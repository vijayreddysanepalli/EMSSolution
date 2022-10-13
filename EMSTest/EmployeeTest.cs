using EMS.Employees;
using EMS;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EMSTest
{
    [TestFixture]
    public class EmployeeTest
    {
        [Test]
        public async Task Add_Employee()
        {
            //Arrange
            Employee employee = new Employee();
            employee.id = "0";
            employee.name = "Chitti";
            employee.email = "test12@yahasasoo1sdss234587sd8.com";
            employee.gender = "male";
            employee.status = "active";

            MainViewModel mainViewModel = new MainViewModel("true");
            int OriginalCount = mainViewModel.ItemsSource.Count;

            //act
            var result = await mainViewModel.CreateOrUpdateEmployeeAsync(employee, true);


            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Convert.ToInt32(result.id) > 0);
            Assert.IsTrue(mainViewModel.ItemsSource.Count > OriginalCount);

            //Clean the data
            await mainViewModel.DeleteEmployeeAsync(employee);


        }
        [Test]
        public async Task Update_Employee()
        {
            //Arrange
            Employee employee = new Employee();
            employee.id = "0";
            employee.name = "update testing";
            employee.email = "test12@yahoohotmail1234.com";
            employee.gender = "male";
            employee.status = "active";

            MainViewModel mainViewModel = new MainViewModel("true");

            //act
            var CreateResult = await mainViewModel.CreateOrUpdateEmployeeAsync(employee, true);
            CreateResult.email = "update@test123.com";
            CreateResult.name = "Update";
            var UpdatedResult = await mainViewModel.CreateOrUpdateEmployeeAsync(CreateResult, false);


            //Assert
            Assert.IsNotNull(CreateResult);
            Assert.IsTrue(Convert.ToInt32(CreateResult.id) > 0);
            Assert.IsTrue(UpdatedResult.name == "Update");
            Assert.IsTrue(UpdatedResult.email == "update@test123.com");

            //Clean the data
            await mainViewModel.DeleteEmployeeAsync(employee);
        }
        [Test]
        public async Task Delete_Employee()
        {
            //Arrange
            Employee employee = new Employee();
            employee.id = "0";
            employee.name = "Chitti";
            employee.email = "test12@yahoosdas12345.com";
            employee.gender = "male";
            employee.status = "active";

            MainViewModel mainViewModel = new MainViewModel("true");
            int OriginalCount = mainViewModel.ItemsSource.Count;

            //act
            var CreateResult = await mainViewModel.CreateOrUpdateEmployeeAsync(employee, true);
            await mainViewModel.DeleteEmployeeAsync(employee);
            var emp = mainViewModel.GetEmployeeAsync($"https://gorest.co.in/public-api/users/{employee.id}");



            //Assert
            Assert.IsNotNull(CreateResult);
            Assert.IsTrue(Convert.ToInt32(CreateResult.id) > 0);
            Assert.IsNull(emp.Result.id);
            Assert.IsNull(emp.Result.name);
            Assert.IsNull(emp.Result.email);
            Assert.IsNull(emp.Result.gender);
            Assert.IsNull(emp.Result.status);

        }
    }
}