using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AS2_Tools.Converters;

public class MoreFolderItemPositionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int position) 
            return Activator.CreateInstance(targetType);


        if (targetType == typeof(decimal?))
        {
            return new decimal(position);
        }

        if (position < 0)
            return "Last";
        return value.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType == typeof(int) && value is decimal d) 
            return System.Convert.ToInt32(d);

        if (value is string s && s.ToLower() == "last")
            return -1;
        var couldInt = int.TryParse(value?.ToString(), out var result);
        return couldInt ? result : default;
    }
}