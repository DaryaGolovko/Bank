using Controllers;
using Entities;

namespace Views;

public static class MainView
{
    public static void Start()
    {
        while (true)
        {
            if (!BankView.ChooseBank())
            {
                Console.Write("\b \nРабота завершена!");
                return;
            }

            Console.Clear();

            var quitAuth = false;
            while (!quitAuth)
            {
                MainController.PrintCurrentBank();
                Console.WriteLine("1. Войти\n" +
                                  "2. Зарегистрироваться\n" +
                                  "3. Назад\n");
                
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        Console.Write("\b");
                        var level = AuthView.Auth();
                        switch (level)
                        {
                            case 1:
                                ClientView.Request();
                                break;
                            case 2:
                                EnterpriceView.Request();
                                break;
                            case 3:
                                OperatorView.Request();
                                break;
                            case 4:
                                ManagerView.Request();
                                break;
                            case 5:
                                AdminView.Request();
                                break;
                        }

                        break;
                    case ConsoleKey.D2:
                        Console.Write("\b");
                        RegisterView.Register();
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        quitAuth = true;
                        break;
                    default:
                        Console.Clear();
                        quitAuth = true;
                        break;
                }
            }
        }
    }
}