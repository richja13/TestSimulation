public class MarcoPoloSystem
{
    public static string Calculate(int i)
    {
        if (i % 3 == 0 && i % 5 == 0)
            return "Marco-Polo";
        else if (i % 5 == 0)
            return "Polo";
        else if (i % 3 == 0)
             return "Marco";
        else
            return "No match";
    }
}
