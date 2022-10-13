using DevExpress.Mvvm;
using EMS.Employees;
using DevExpress.Mvvm.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using DevExpress.Mvvm.Xpf;
using EMS.Services;
using System.Threading.Tasks;

namespace EMS
{
    public class MainViewModel : ViewModelBase
    {
        EmployeesContext _Context;
        IList<Employee> _ItemsSource;
        private readonly string _IsTest = null;
        public MainViewModel()
        {

        }
        public MainViewModel(string IsTest)
        {
            _IsTest = IsTest;
        }

        public  IList<Employee> ItemsSource
        {
            get
            {
                _Context = new EmployeesContext();
                if (_ItemsSource == null && !IsInDesignMode)
                {
                    if (string.IsNullOrEmpty(_IsTest))
                    {
                        while (!(EmployeesContext.Employees != null))
                        {
                            //do nothing
                        }
                    }
                    

                    _ItemsSource = EmployeesContext.Employees == null ? new List<Employee>() { } : EmployeesContext.Employees.ToList();
                }
                else
                {
                    _ItemsSource = EmployeesContext.Employees.ToList();
                }
                return _ItemsSource;
            }
        }
        [Command]
        public async void ValidateRow(RowValidationArgs args)
        {
            var item = (Employee)args.Item;
            await this.CreateOrUpdateEmployeeAsync(item, args.IsNewItem);

        }
        public async Task<Employee> CreateOrUpdateEmployeeAsync(Employee employee, bool IsNewItem)
        {
            if (employee == null)
                return null;
            else
            {
                if (IsNewItem)
                {
                    employee.id = "0";
                    var result = await EmployeeService.CreateEmployeeAsync(employee);
                    employee.id = result.LocalPath.Split('/').Last();
                    if (EmployeesContext.Employees == null)
                    {
                        var employees = new List<Employee>();
                        employees.Add(employee);
                        EmployeesContext.Employees = employees;
                    }
                    else
                    {
                        EmployeesContext.Employees.ToList().Add(employee);
                    }
                    


                }
                else
                {
                    var UpdateReulst = await EmployeeService.UpdateEmployeeAsync(employee);
                    return UpdateReulst;
                }

            }

            return employee;
        }
        public async Task<Employee> GetEmployeeAsync(string path)
        {
            return await EmployeeService.GetEmployeeAsync(path);
        }
        [Command]
        public async void ValidateRowDeletion(ValidateRowDeletionArgs args)
        {
            var item = (Employee)args.Items.Single();
            await DeleteEmployeeAsync(item);
            
        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            EmployeesContext.Employees.ToList().Remove(employee);
            await EmployeeService.DeleteEmployeeAsync(employee.id);
        }

        [Command]
        public void DataSourceRefresh(DataSourceRefreshArgs args)
        {
            _ItemsSource = null;
            RaisePropertyChanged(nameof(ItemsSource));
        }
    }
}