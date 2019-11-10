using System;

public static class StringExtentions
{
    public static bool ContainsIgnoreCase(this string str, StringComparison comp, string toCheck)
    {
        try
        {
            return str?.IndexOf(toCheck, comp) >= 0;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static bool ContainsIgnoreCase(this string str, string toCheck)
    {
        try
        {
            return str?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static bool ContainsIgnoreCase(this string str, StringComparison comp, params string[] toCheck)
    {
        try
        {
            foreach (var item in toCheck)
            {
                if (str.ContainsIgnoreCase(comp, item))
                {
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static bool ContainsIgnoreCase(this string str, params string[] toCheck)
    {
        try
        {
            foreach (var item in toCheck)
            {
                if (str.ContainsIgnoreCase(StringComparison.OrdinalIgnoreCase, item))
                {
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static bool Contains(this string str, params string[] toCheck)
    {
        try
        {
            foreach (var item in toCheck)
            {
                if (str.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }


    public static bool EqualsIgnoreCase(this string str, string toCheck)
    {
        try
        {
            return str.Equals(toCheck, StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}

