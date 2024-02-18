using FluentValidation;
using System.Text.RegularExpressions;

namespace Cards.Api.Validators
{
    public static class CustomColorValidators
    {
       
            public static IRuleBuilderOptions<T, string> IsValidColor<T>(this IRuleBuilder<T, string> ruleBuilder)
            {
                return ruleBuilder.Must(IsValidColor).WithMessage("Color should conform to a 6 alphanumeric characters prefixed with a # format");
            }
        private static bool IsValidColor(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            bool hasHexColor = value.StartsWith("#");
            if(hasHexColor)
            {
                var hasValidLength = (value.Skip(1).ToArray()).Length == 6;
                return hasValidLength;
            }
            return false;        
        }
        
    }
}
