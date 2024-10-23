using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PMES.Manual.Net6.Validations
{
    public class WeightValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (double.TryParse(value.ToString(), out var myValue))
            {
                if (myValue >= 0 && myValue <= 300)
                {
                    return new ValidationResult(true, null);
                }
                else
                {
                    return new ValidationResult(false, "重量可能超出范围!");
                }
            }
            else
            {
                return new ValidationResult(false, "输入格式错误");
            }
        }
    }
}