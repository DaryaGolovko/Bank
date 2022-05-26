using Controllers;
using Entities;

namespace Views;

public static class OperatorView
{
    public static void Request()
    {
        Console.Clear();
        var quit = false;
        while (!quit)
        {
            Console.WriteLine("\nВыберите действие:\n" +
                              "1. Информация пользователя\n" +
                              "2. Переводы\n" +
                              "3. ЗП проекты\n" +
                              "4. Выход\n");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    foreach (var item in OperatorController.Info())
                        Console.WriteLine(item);
                    break;
                case ConsoleKey.D2:
                    Console.Clear();

                    foreach (var item in OperatorController.CheckTransfersForOperator())
                        Console.WriteLine(item);
                    
                    if (OperatorController.CheckTransfersForOperator().Count != 0)
                    {
                        Console.WriteLine("Хотите отменить перевод? (Y - Да, N - Нет)");

                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Y:
                                Console.Write("\b");
                                Console.WriteLine("Проверьте id перевода для подтверждения");
                    
                                int transfer = InputHelper.GetIntInBounds(OperatorController.GetAllTransferId()[0],
                                    OperatorController.GetAllTransferId()[^1]);

                                while (!OperatorController.CheckAccIfExists("Transfer", transfer))
                                {
                                    Console.WriteLine($"Нет переводов с таким id {transfer}. Попробуйте ще раз");
                                    transfer = InputHelper.GetIntInBounds(OperatorController.GetAllTransferId()[0],
                                        OperatorController.GetAllTransferId()[^1]);
                                }
                                
                                OperatorController.CancelTransfer(transfer);
                                
                                break;
                            case ConsoleKey.N:
                                Console.Write("\b");
                                break;
                            default:
                                Console.Write("\b");
                                break;
                        }
                    }
                    else
                        Console.WriteLine("Переводы отсутствуют");

                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    
                    foreach(var item in OperatorController.CheckSalaryPlans())
                        Console.WriteLine(item);
                    
                    if (OperatorController.CheckSalaryPlans().Count != 0)
                    {
                        Console.WriteLine("Выберите ЗП проект");
                        int salaryPlan = InputHelper.GetIntInBounds(OperatorController.GetAllSalaryPlanId()[0],
                            OperatorController.GetAllSalaryPlanId()[^1]);

                        while (!OperatorController.CheckAccIfExists("SalaryPlan", salaryPlan))
                        {
                            Console.WriteLine($"Нет ЗП проекта с таким id {salaryPlan}. Попробуйте еще раз");
                            salaryPlan = InputHelper.GetIntInBounds(OperatorController.GetAllSalaryPlanId()[0],
                                OperatorController.GetAllSalaryPlanId()[^1]);
                        }
                        
                        Console.WriteLine("Хотите отменить ЗП проект? (Y - Да, N - Нет)");

                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Y:
                                Console.Write("\b");
                                
                                OperatorController.ConfirmSalaryPlan(salaryPlan);
                                
                                break;
                            case ConsoleKey.N:
                                Console.Write("\b");
                                
                                OperatorController.RejectSalaryPlan(salaryPlan);

                                break;
                            default:
                                Console.Write("\b");
                                break;
                        }
                    }
                    else
                        Console.WriteLine("Нет заявок на ЗП");


                    break;
                case ConsoleKey.D4:
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