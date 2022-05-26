using Controllers;
using Entities;

namespace Views;

public static class AdminView
{
    public static void Request()
    {
        Console.Clear();
        var quit = false;
        while (!quit)
        {
            Console.WriteLine("\nВыберите действие:\n" +
                              "1. Информация ою администратореf\n" +
                              "2. Логи банков\n" +
                              "3. Отмена операций\n" +
                              "4. Просмотр статуса пользователей\n" +
                              "5. Skip month\n" +
                              "6. Выход\n");
            
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    foreach (var item in AdminController.Info())
                        Console.WriteLine(item);
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    
                    foreach (var item in AdminController.Logs())
                        Console.WriteLine(item);
                    
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    Console.WriteLine("Отменено");
                    break;
                case ConsoleKey.D4:
                    Console.Clear();
                    
                    foreach (var item in AdminController.ShowUsers())
                        Console.WriteLine(item);
                    
                    Console.WriteLine("Выберите пользователя");
                    List<int> usersId = AdminController.GetAllUserId();

                    if (usersId.Count == 0)
                    {
                        Console.WriteLine("Нет пользователей");
                        break;
                    }

                    int user = InputHelper.GetIntInBounds(usersId[0], usersId[^1]);

                    while (!AdminController.CheckAccIfExists("UsersAuth", user))
                    {
                        Console.WriteLine($"Нет пользователей с id {user}. Попробуйте снова");
                        user = InputHelper.GetIntInBounds(usersId[0], usersId[^1]);
                    }
                    
                    AdminController.BlockUser(user);

                    break;
                case ConsoleKey.D5:
                    Console.Clear();
                    AdminController.SkipMonth();
                    Console.WriteLine("Month skipped successful");
                    break;
                case ConsoleKey.D6:
                    Console.Clear();
                    quit = true;
                    break;
                default: 
                    Console.Write("\b");
                    break;
            }
        }
    }
}