using Controllers;
using Entities;

namespace Views;

public static class ManagerView
{
    public static void Request()
    {
        Console.Clear();
        var quit = false;
        while (!quit)
        {
            Console.WriteLine("\nCВыберите действие:\n" +
                              "1. Информация о пользователе\n" +
                              "2. Переводы\n" +
                              "3. Заявки на ЗП\n" +
                              "4. Подтверждение заявок на кредиты\n" +
                              "5. Заявки пользователей\n" +
                              "6. Выход\n");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    foreach (var item in ManagerController.Info())
                        Console.WriteLine(item);
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    
                    foreach (var item in ManagerController.CheckTransfers())
                        Console.WriteLine(item);

                    if (ManagerController.CheckTransfers().Count != 0)
                    {
                        Console.WriteLine("Отменить перевод? (Y - Да, N - Нет)");

                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Y:
                                Console.Write("\b");
                                Console.WriteLine("Проверьте ID перевода");
                    
                                int transfer = InputHelper.GetIntInBounds(OperatorController.GetAllTransferId()[0],
                                    OperatorController.GetAllTransferId()[^1]);

                                while (!OperatorController.CheckAccIfExists("Transfer", transfer))
                                {
                                    Console.WriteLine($"Нет перевода с id {transfer}. Попробуйте снова");
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
                        Console.WriteLine("Нет переводов");

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
                            Console.WriteLine($"Нет ЗП проектов id {salaryPlan}. Попробйте снова");
                            salaryPlan = InputHelper.GetIntInBounds(OperatorController.GetAllSalaryPlanId()[0],
                                OperatorController.GetAllSalaryPlanId()[^1]);
                        }
                        
                        Console.WriteLine("Одобрить ЗП прокект? (Y - Да, N - Нет)");

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
                        Console.WriteLine("Нет ЗП проктов");

                    break;
                case ConsoleKey.D4:
                    Console.Clear();
                    
                    foreach (var item in ManagerController.CheckLoans())
                        Console.WriteLine(item);

                    if (ManagerController.CheckLoans("Frozen").Count != 0)
                    {
                        Console.WriteLine("Выберите кредит");
                        int loan = InputHelper.GetIntInBounds(ManagerController.GetAllLoanId()[0], ManagerController.GetAllLoanId()[^1]);
                        
                        while (!ManagerController.CheckAccIfExists("Loans", loan))
                        {
                            Console.WriteLine($"Нет кредитов с id {loan}. Попробуйте снова");
                            loan = InputHelper.GetIntInBounds(ManagerController.GetAllLoanId()[0], ManagerController.GetAllLoanId()[^1]);
                        }
                        
                        ManagerController.ConfirmLoan(loan);
                    }
                    
                    Console.WriteLine("Нет кредитов на подтверждение");
                    
                    break;
                case ConsoleKey.D5:
                    Console.Clear();
                    
                    foreach (var item in ManagerController.CheckRegInfo())
                        Console.WriteLine(item);
                    
                    if (ManagerController.CheckRegInfo().Count != 0)
                    {
                        Console.Write("Выберите пользователя: ");
                        int user = InputHelper.GetIntInBounds(ManagerController.GetAllRegId()[0], ManagerController.GetAllRegId()[^1]);
                        
                        while (!ManagerController.CheckAccIfExists("DeclaredUsers", user))
                        {
                            Console.WriteLine($"Нет пользователя с id {user}. Попробуйте снова");
                            user = InputHelper.GetIntInBounds(ManagerController.GetAllRegId()[0], ManagerController.GetAllRegId()[^1]);
                        }
                        
                        Console.WriteLine("Подствердить регистрацию? (Y - Да, N - Нет)");

                        bool quitReg = false;
                        while (!quitReg)
                        {
                            switch (Console.ReadKey().Key)
                            {
                                case ConsoleKey.Y:
                                    Console.Write("\b");
                                
                                    ManagerController.ConfirmRegistration(user);
                                    Console.WriteLine($"Клиент {user} одобрен");
                                    quitReg = true;
                                
                                    break;
                                case ConsoleKey.N:
                                    Console.Write("\b");

                                    ManagerController.CancelRegistration(user);
                                    Console.WriteLine($"Клиент {user} отклонен");
                                    quitReg = true;
                                
                                    break;
                                default:
                                    Console.Write("\b");
                                    break;
                            }
                        }
                    }
                    else
                        Console.WriteLine("Нет клиентов");
                    
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