﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BoundingChecker
{
    internal class NumberRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public NumberRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int number = 0;

            try
            {
                if (((string)value).Length > 0)
                    number = Int32.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            if ((number < Min) || (number > Max))
            {
                return new ValidationResult(false,
                  $"Please enter an number in the range: {Min}-{Max}.");
            }
            return ValidationResult.ValidResult;
        }
    }
}