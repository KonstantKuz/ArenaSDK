using System.Text;
using System.Text.RegularExpressions;

public static class Util
{
    public static bool IsNullOrEmpty(this string value)
    {
        return value == null || value.Equals(string.Empty);
    }
        
    public static bool IsCharactersValid(this string value, string parameterName, out string invalidMessage)
    {
        invalidMessage = string.Empty;
        if (!Regex.IsMatch(value, @"^[a-zA-Z0-9]+$"))
        {
            invalidMessage = $"{parameterName} must contain only a-z, A-Z, 0-9 characters.";
            return false;
        }

        return true;
    }

    public static bool IsNumber(this string value, string parameterName, out string description, out int num)
    {
        num = 0;
        description = string.Empty;
        if (int.TryParse(value, out num)) return true;
        description = $"{parameterName} must be a number.";
        return false;
    }
        
    public static bool IsInRange(this int number, string parameterName, out string invalidMessage, int minIncluded = 0, int maxIncluded = 0)
    {
        invalidMessage = string.Empty;
        var isLess = minIncluded > 0 && number <= minIncluded;
        var isMore = maxIncluded > 0 && number >= maxIncluded;
        if (isLess || isMore)
        {
            invalidMessage = GetInvalidNumberMessage(parameterName, minIncluded, minIncluded);
            return false;
        }

        return true;
    }
        
    private static string GetInvalidNumberMessage(string parameterName, int minValueIncluded, int maxValueIncluded)
    {
        var builder = new StringBuilder();
        builder.Append($"Invalid value : {parameterName} must be");
        var isLess = parameterName.Length <= minValueIncluded;
        var isMore = parameterName.Length >= maxValueIncluded;
        if (isLess)
        {
            builder.Append($"greater than {minValueIncluded}");
        }

        if (isLess && isMore) builder.Append(" and ");
            
        if (isMore)
        {
            builder.Append($"less than {maxValueIncluded}");
        }

        builder.Append(".");
        return builder.ToString();
    }
}