using LinqToExcel;
using LinqToExcel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelParser
{
    public static class ParseExcel
    {

        public static List<string> GetUserNames(string FileName)
        {
            var excel = new ExcelQueryFactory(FileName);

            var temp = (from p in excel.Worksheet()
                        select p).ToList();

            temp.ToList();

            return (from p in excel.Worksheet<ExcelUserEmail>()
                    where p.Email != null
                    select p.Email).ToList();
        }

    }

    class ExcelUserEmail
    {
        [ExcelColumn("Email")]
        public string Email { get; set; }
    }
}
