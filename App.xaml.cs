using EMS.Employees;
using System.Windows;

namespace EMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            EmployeesContextInitializer.Seed();
        }
    }
}
