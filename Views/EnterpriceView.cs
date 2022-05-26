using Controllers;
using Entities;

namespace Views;

public static class EnterpriceView
{
    public static void Request()
    {
        Console.Clear();
        
        var quit = false;
        while (!quit)
        {
            Console.WriteLine("Выбор действия:\n" +
                              "1. Информация о компании\n" +
                              "2. ЗП проект\n" +
                              "4. Перевод\n" +
                              "5. Выход\n");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    foreach(var item in EnterpriceController.EnterpriceInfo())
                        Console.WriteLine(item);
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    foreach(var item in EnterpriceController.SpecialistInfo())
                        Console.WriteLine(item);
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    List<int> info = EnterpriceController.Salary();
                    Console.WriteLine($"Количество работников: {info[1]}\n" +
                                      $"Сумма выплаты: ${info[0]}\n" +
                                      $"Запрос отправлен");
                    
                    break;
                case ConsoleKey.D4:
                    Console.Clear();
                    TransferMoney();
                    break;
                case ConsoleKey.D5:
                    Console.Clear();
                    quit = true;
                    break;
                default:
                    Console.Write("\b");
                    break;
            }
        }
    }

    public static void TransferMoney()
    {
        Console.WriteLine("Количество денег на перевод?");
        int money = InputHelper.GetIntInBounds(1, EnterpriceController.CheckMoney());
        
        Console.WriteLine("Выберите предприятие, на которое оформить переводить\n" +
                          "Введите число от 1 до 10");
        int account = InputHelper.GetIntInBounds(1, 10);
        
        EnterpriceController.TransferMoney(account, money);
    }
}