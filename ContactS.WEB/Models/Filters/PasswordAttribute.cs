using System.ComponentModel.DataAnnotations;
using System;


namespace ContactS.WEB.Models.Filters
{
    public class PasswordAttribute : ValidationAttribute
    {
        public PasswordAttribute()
        {

        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                bool Up = false, Low = false, Dig = false;
                string strval = value.ToString();
                string Lowval = strval.ToLower();
                string Upval = strval.ToUpper();
                int lenght = strval.Length;
                for(int i = 0; i < lenght; ++i)
                {
                    if (Char.IsDigit(strval[i])) Dig = true;
                    if (Lowval[i] == strval[i]) Low = true;
                    if (Upval[i] == strval[i]) Up = true;
                }
                return Dig&&Up&&Low;
            }
            return false;
        }
    }
}