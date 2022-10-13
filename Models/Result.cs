using System.Collections.Generic;

namespace EMS.Employees
{
    public class Result
    {
        public string code { get; set; }
        public Meta meta { get; set; }
        public IEnumerable<Employee> data { get; set; }
    }

    public class SingleResult
    {
        public string code { get; set; }
        public Meta meta { get; set; }
        public Employee data { get; set; }
    }
}
