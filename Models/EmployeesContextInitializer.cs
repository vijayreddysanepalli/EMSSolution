using EMS.Services;

namespace EMS.Employees
{
    public static class EmployeesContextInitializer
    {
        public static async void Seed()
        {
            var employees = await EmployeeService.GetEmployeesAsync();
            EmployeesContext.Employees = employees;
        }
    }
}
