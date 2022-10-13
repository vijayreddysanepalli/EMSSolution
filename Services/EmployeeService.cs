using EMS.Employees;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Formatting;

namespace EMS.Services
{
    public class EmployeeService
    {
        #region snippet_HttpClient
        static HttpClient client = new HttpClient();
        #endregion

        private static HttpClient GenerateHeaders()
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56");
            return client;
        }


        #region snippet_CreateEmployeeAsync
        public static async Task<Uri> CreateEmployeeAsync(Employee employee)
        {
            var httpClient = GenerateHeaders();
            
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "https://gorest.co.in/public-api/users", employee);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }
        #endregion

        #region snippet_GetEmployeeAsync
        public static async Task<Employee> GetEmployeeAsync(string path)
        {
            var formatters = new List<MediaTypeFormatter>() {
                        new JsonMediaTypeFormatter()
                    };
            var httpClient = GenerateHeaders();

            Employee employee = null;
            HttpResponseMessage response = httpClient.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<SingleResult>(formatters);
                employee = result.data;
            }
            return employee;
        }
        #endregion
        #region snippet_GetEmployeesAsync
        public static async Task<IEnumerable<Employee>> GetEmployeesAsync(bool isExport = false)
        {
            try
            {
                var formatters = new List<MediaTypeFormatter>() {
                        new JsonMediaTypeFormatter()
                    };
                var httpClient = GenerateHeaders();
                List<Employee> employees = null;
                HttpResponseMessage response = await httpClient.GetAsync("https://gorest.co.in/public-api/users/");
                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsAsync<Result>(formatters);
                    var numberOfPages = Convert.ToInt32(results.meta.pagination.pages);
                    var numberOfRecords = Convert.ToInt32(results.meta.pagination.total);
                    employees = results.data.ToList();
                    //This is not a good approach, however because of time constraints
                    //I am pulling the first 40 pages and displaying
                    //two scenarios here, one is if we load all the data on page load it take a while to open the application and that's not good
                    //second approch is just to load all the pages on page load and on page click we can get the data on demand
                    //If needed, we can remove the loop check to load all the data on page load 

                    for (int i = 2; i < numberOfPages; i++)
                    {
                        var result = await GetEmployeesByPageAsync(i);
                        employees.AddRange(result);
                        if (i > 30)
                            break;
                    }
                }
                return employees;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }
        #endregion

        #region snippet_UpdateEmployeeAsync
        public static async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            var httpClient = GenerateHeaders();
            HttpResponseMessage response = await httpClient.PutAsJsonAsync(
                $"https://gorest.co.in/public-api/users/{employee.id}", employee);
            response.EnsureSuccessStatusCode();

            return await GetEmployeeAsync($"https://gorest.co.in/public-api/users/{employee.id}");

        }
        #endregion
        #region snippet_GetEmployeesByPageAsync
        public static async Task<IEnumerable<Employee>> GetEmployeesByPageAsync(int page)
        {
            var formatters = new List<MediaTypeFormatter>() {
                        new JsonMediaTypeFormatter()
                    };
            var httpClient = GenerateHeaders();
            IEnumerable<Employee> employees = null;
            HttpResponseMessage response = await httpClient.GetAsync($"https://gorest.co.in/public-api/users?page={page}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var results = await response.Content.ReadAsAsync<Result>(formatters);
                    employees = results.data.ToList();
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    employees = await GetEmployeesByPageAsync(page);
                }

            }
            return employees;

        }
        #endregion

        #region snippet_DeleteEmployeeAsync
        public static async Task<HttpStatusCode> DeleteEmployeeAsync(string id)
        {
            var httpClient = GenerateHeaders();
            HttpResponseMessage response = await httpClient.DeleteAsync(
                $"https://gorest.co.in/public-api/users/{id}");
            return response.StatusCode;
        }
        #endregion

    }
}