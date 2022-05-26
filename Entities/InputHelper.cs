using System.Text.RegularExpressions;

namespace Entities;

public static class InputHelper
{
    public static int GetInt()
    {
        int ans;

        while (!int.TryParse(Console.ReadLine(), out ans))
            Console.WriteLine("Entered data is not an integer");

        return ans;
    }

    public static int GetIntInBounds(int left = 0, int right = 100)
    {
        var ans = GetInt();

        while (ans < left || ans > right)
        {
            Console.WriteLine($"Your number is not in bounds [{left}, {right}]");
            ans = GetInt();
        }

        return ans;
    }

    private static string GetNotEmptyString(int left = 1, int right = 50)
    {
        var ans = Console.ReadLine();

        while (ans?.Length <= left || ans?.Length >= right)
        {
            Console.WriteLine($"String is not in bounds [{left}; {right}], try again");
            ans = Console.ReadLine();
        }

        return ans ?? "ThisStringIsEmpty";
    }

    public static string GetStrWithLettersAndNumbers(int left = 1, int right = 50)
    {
        var ans = GetNotEmptyString();

        while (ans.Length < left || ans.Length > right || !ans.All(x => char.IsLetter(x) || char.IsNumber(x)))
        {
            Console.WriteLine($"Entered data is incorrect string. Enter only [{left}; {right}] letters and numbers");
            ans = GetNotEmptyString();
        }

        return ans;
    }

    public static string GetStrWithLetters(int left = 1, int right = 50)
    {
        var ans = GetNotEmptyString();

        while (ans.Length < left || ans.Length > right || !ans.All(char.IsLetter))
        {
            Console.WriteLine($"Entered data is incorrect string. Enter only [{left}; {right}] letters");
            ans = GetNotEmptyString();
        }

        return ans;
    }

    public static string GetCorrectPassword()
    {
        var password = GetNotEmptyString();

        while (!Regex.IsMatch(password, @"^([a-zA-Z0-9!#$%&'()*+,-\./:;<=>?@[\]^_`{|}]{4,30})?$"))
        {
            Console.WriteLine("Your password is incorrect. Try again:");
            password = GetCorrectPassword();
        }

        return password;
    }

    public static string GetCorrectEmail()
    {
        var email = GetNotEmptyString();

        while (!Regex.IsMatch(email, @"^(([a-z0-9-_]+.)*[a-z0-9-_]+@[a-z0-9]+(.[a-z0-9]+)*.[a-z]{2,6})?$"))
        {
            Console.WriteLine("Entered email is incorrect. Try again: ");
            email = GetNotEmptyString();
        }

        return email;
    }

    public static string GetCorrectPassport()
    {
        var passport = GetNotEmptyString();

        while (!Regex.IsMatch(passport,
                   @"^((([A|H|K]{1}[B]{1})|([B]{1}[M]{1})|([K]{1}[H]{1})|([M|P|S|D]{1}[P]{1})|([M]{1}[C]{1}))[0-9]{7})?$"))
        {
            Console.WriteLine($"Entered passport \"{passport}\" is incorrect. Try again: ");
            passport = GetNotEmptyString();
        }

        return passport;
    }

    public static string GetCorrectIdentification()
    {
        var identification = GetNotEmptyString();

        while (!Regex.IsMatch(identification,
                   @"^([0-9]{7}[A|B|C|K|E|M|H]{1}[0-9]{3}(([G]{1}[B]{1})|([P]{1}[B]{1})|([B]{1}[A]{1})|([B]{1}[I]{1}))[0-9]{1})?$"))
        {
            Console.WriteLine($"Entered identification \"{identification}\" is incorrect. Try again: ");
            identification = GetNotEmptyString();
        }

        return identification;
    }

    public static string GetCorrectPhone()
    {
        var phone = GetNotEmptyString();

        while (!Regex.IsMatch(phone, @"^((([2]{1}[5|9]{1})|([3]{2})|([4]{2}))[0-9]{7})?$"))
        {
            Console.WriteLine($"Entered phone \"{phone}\" is incorrect. Try again: ");
            phone = GetNotEmptyString();
        }

        return phone;
    }
}