using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace База_данных_фирмы
{
    internal static class Checker
    {
        public static bool CheckString(string Request)
        {
            if (string.IsNullOrWhiteSpace(Request))
                return false;
            string trimmedInput = Request.Replace(" ", "");
            return Regex.IsMatch(trimmedInput, @"^\d+(,\d+)*$");//прикол какой-то
        }

        public static string UnionLists(List<string> Values, List<string> Names)
        {
            string ToReturn = "";
            for (int i = 0; i < Names.Count; i++)
            {
                if (Values[i] != "-")
                {
                    ToReturn = ToReturn + $" {Names[i]} = '{Values[i]}' ";
                    if (i < Names.Count - 1)
                        ToReturn += ", ";
                }
            }
            return ToReturn;
        }

        public static string QueueLists(List<string> Values, List<string> Names)
        {
            string ToReturn = " ( ";
            for (int i = 0; i < Names.Count; i++)
            {
                ToReturn += $" {Names[i]} ";
                if (i < Names.Count-1)
                    ToReturn += ", ";
            }
            ToReturn += " ) values ( ";
            for (int i = 0; i < Values.Count; i++) 
            {
                ToReturn += $"'{Values[i]}'";
                if (i< Values.Count -1)
                    ToReturn += ", ";
            }
            ToReturn += ");";
            return ToReturn;
        }
    }
}
