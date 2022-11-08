using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ConestogaVirtualGameStore.CustomValidation
{
    public class LuhnValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int inputLength = value.ToString().Length;
            if (inputLength != 16)
            {
                return new ValidationResult("Credit Card Number must have 16 digits");
            }
            char[] cardNumberString =  value.ToString().ToCharArray();
            Array.Reverse(cardNumberString);
            int sum = 0;
            for (int x = 0; x <=15; x++)
            {
                if(x%2 != 0)
                {
                    int temp = (int.Parse(cardNumberString[x].ToString()))*2;
                    char[] tempChar = temp.ToString().ToCharArray();
                    if (tempChar.Length < 2)
                    {
                        sum += int.Parse(tempChar[0].ToString());
                    }
                    else
                    {
                        sum += int.Parse(tempChar[0].ToString()) + int.Parse(tempChar[1].ToString());
                    }
                }
                else
                {
                    sum += int.Parse(cardNumberString[x].ToString());
                }
            }
            if(sum%10 != 0)
            {
                return new ValidationResult("The Credit card number is invalid: Validated by the Luhn Algorithm");
            }
            return ValidationResult.Success;
        }
    }
}
