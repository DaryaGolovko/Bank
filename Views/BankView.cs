using Controllers;

namespace Views;

public static class BankView
{
    public static bool ChooseBank()
    {
        Console.WriteLine("Выберите банк:");

        var info = BankController.GetAllBanks();

        var i = 0;
        foreach (var item in info)
            Console.WriteLine($"{++i}. {item}");
        Console.WriteLine($"{++i}. Exit\n");
        
        bool quit = false;

        while (!quit)
        {
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    
                    BankController.ChooseBank(info[0]);
                    quit = true;
                    break;
                case ConsoleKey.D2:
                    Console.Write("\b");
                    BankController.ChooseBank(info[1]);
                    quit = true;
                    break;
                case ConsoleKey.D3:
                    Console.Write("\b");
                    BankController.ChooseBank(info[2]);
                    quit = true;
                    break;
                case ConsoleKey.D4:
                    Console.Write("\b");
                    return false;
                default:
                    Console.Write("\b");
                    break;
            }
        }
        
        return true;
    }
}