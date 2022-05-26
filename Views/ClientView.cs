using Controllers;
using Entities;
using Models;

namespace Views;

public static class ClientView
{
    public static void Request()
    {
        Console.Clear();
        
        var quit = false;
        while (!quit)
        {
            Console.WriteLine("Выберите действие:\n" +
                              "1. Информация клиента\n" +
                              "2. Банки\n" +
                              "3. Депозиты\n" +
                              "4. Loan\n" +
                              "5. Выход\n");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    foreach (var item in ClientController.Info())
                        Console.WriteLine(item);
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    BankAccounts();
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    Deposit();
                    break;
                case ConsoleKey.D4:
                    Console.Clear();
                    Loan();
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

    private static void BankAccounts()
    {
        var curBankAccs = ClientController.CheckBankAccounts();
        var quit = false;
        while (!quit)
            if (curBankAccs.Count == 0)
            {
                Console.WriteLine("Нет аккаунтов\n" +
                                  "1.Добавить\n" +
                                  "2.Выход");

                var firstChoice = InputHelper.GetIntInBounds(1, 2);

                if (firstChoice == 1)
                    ClientController.AddBankAcc();

                quit = true;
            }
            else
            {
                curBankAccs = ClientController.CheckBankAccounts();
                Console.WriteLine("1. Add new account\n" +
                                  "2. Check balance on accounts\n" +
                                  "3. Replenish balance\n" +
                                  "4. Withdraw cash\n" +
                                  "5. Transfer money to another account\n" +
                                  "6. Freeze/Block account\n" +
                                  "7. Delete account\n" +
                                  "8. Exit");

                int choice;
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        ClientController.AddBankAcc();
                        curBankAccs = ClientController.CheckBankAccounts();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        curBankAccs = ClientController.CheckBankAccounts();
                        Console.WriteLine($"You have {curBankAccs.Count} bank account(s)");
                        foreach (var item in curBankAccs)
                            Console.WriteLine($"ID: \t{item.AccountNumber}\n" +
                                              $"Balance: ${item.Balance}\n" +
                                              $"State: {item.State}");
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        Console.WriteLine("Choose account you want to replenish\n" +
                                          "Tip: you cant replenish frozen account");
                        foreach (var item in curBankAccs)
                            Console.WriteLine($"ID: \t{item.AccountNumber}\n" +
                                              $"Balance: ${item.Balance}\n" +
                                              $"State: {item.State}");

                        choice = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                            curBankAccs[^1].AccountNumber);

                        while (!ClientController.CheckAccount(choice) ||
                               ClientController.CheckState(choice, "BankAccounts") is "Frozen" or "Blocked")
                        {
                            while (!ClientController.CheckAccount(choice))
                            {
                                Console.WriteLine($"You do not have such account with ID = {choice}. Попробуйте снова");
                                choice = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                                    curBankAccs[^1].AccountNumber);
                            }

                            Console.WriteLine($"Your account with ID = {choice} is frozen. Попробуйте снова");
                            choice = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                                curBankAccs[^1].AccountNumber);
                        }

                        Console.Write("Enter sum you want to replenish (max $100.000 in one operation): ");
                        var replenish = InputHelper.GetIntInBounds(0, 100000);

                        while (ClientController.CheckUserCash() < replenish)
                        {
                            Console.WriteLine(
                                $"You do not have enough cash. Try smth below ${ClientController.CheckUserCash()}");
                            replenish = InputHelper.GetIntInBounds(0, 100000);
                        }

                        ClientController.ReplenishBalance(choice, replenish);
                        curBankAccs = ClientController.CheckBankAccounts();

                        break;
                    case ConsoleKey.D4:
                        Console.Clear();
                        
                        int moneyCheck = 0;
                        foreach (var item in curBankAccs)
                            moneyCheck += item.Balance;

                        if (moneyCheck == 0)
                        {
                            Console.WriteLine("You do not have any account with balance above $0");
                            break;
                        }
                        
                        Console.WriteLine("Choose account you want to withdraw");
                        foreach (var item in curBankAccs)
                            Console.WriteLine($"ID: \t{item.AccountNumber}\n" +
                                              $"Balance: ${item.Balance}\n" +
                                              $"State: {item.State}");

                        choice = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                            curBankAccs[^1].AccountNumber);

                        while (!ClientController.CheckAccount(choice) ||
                               ClientController.CheckState(choice, "BankAccounts") is "Frozen" or "Blocked")
                        {
                            while (!ClientController.CheckAccount(choice))
                            {
                                Console.WriteLine($"You do not have such account with ID = {choice}. Попробуйте снова");
                                choice = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                                    curBankAccs[^1].AccountNumber);
                            }

                            Console.WriteLine($"Your account with ID = {choice} is frozen. Попробуйте снова");
                            choice = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                                curBankAccs[^1].AccountNumber);
                        }

                        Console.WriteLine("Enter sum you want to withdraw (max $100.000 in one operation): ");
                        var withdraw = InputHelper.GetIntInBounds(0, 100000);

                        while (ClientController.CheckAccountMoney(choice) < withdraw)
                        {
                            Console.WriteLine($"You do not have enough money on account with ID = {choice}. Попробуйте снова");
                            choice = InputHelper.GetIntInBounds(0, 100000);
                        }

                        ClientController.WithdrawCash(choice, withdraw);
                        curBankAccs = ClientController.CheckBankAccounts();

                        break;
                    case ConsoleKey.D5:
                        Console.Clear();

                        moneyCheck = 0;
                        foreach (var item in curBankAccs)
                            moneyCheck += item.Balance;

                        if (moneyCheck == 0)
                        {
                            Console.WriteLine("You do not have any account with balance above $0");
                            break;
                        }

                        Console.WriteLine("Choose account you want to transfer from");
                        foreach (var item in curBankAccs)
                            Console.WriteLine($"ID: \t{item.AccountNumber}\n" +
                                              $"Balance: ${item.Balance}\n" +
                                              $"State: {item.State}");
                        var firstAcc = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                            curBankAccs[^1].AccountNumber);

                        while (!ClientController.CheckAccount(firstAcc) ||
                               ClientController.CheckState(firstAcc, "BankAccounts") is "Frozen" or "Blocked")
                        {
                            while (!ClientController.CheckAccount(firstAcc))
                            {
                                Console.WriteLine($"You do not have such account with ID = {firstAcc}. Попробуйте снова");
                                firstAcc = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                                    curBankAccs[^1].AccountNumber);
                            }

                            Console.WriteLine($"Your account with ID = {firstAcc} is frozen. Попробуйте снова");
                            firstAcc = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                                curBankAccs[^1].AccountNumber);
                        }

                        Console.WriteLine("Enter account account to which you want to transfer money");

                        List<int> allBankAccs = ClientController.GetAllBankAccId();
                        
                        var secondAcc = InputHelper.GetIntInBounds(allBankAccs[0], allBankAccs[^1]);

                        while (ClientController.CheckState(secondAcc, "BankAccounts") is "Empty" or "Frozen" or "Blocked" || 
                               !ClientController.CheckAccIfExists("BankAccounts", secondAcc))
                        {
                            Console.WriteLine($"There is no suitable account with ID = {secondAcc}. Попробуйте снова");
                            secondAcc = InputHelper.GetIntInBounds(allBankAccs[0], allBankAccs[^1]);
                        }

                        Console.WriteLine("Enter how much money you want to transfer (max $100.000 in one operation):");
                        var money = InputHelper.GetIntInBounds(0, 100000);

                        while (ClientController.CheckAccountMoney(firstAcc) < money)
                        {
                            Console.WriteLine("You do not have enough money. Попробуйте снова");
                            money = InputHelper.GetIntInBounds(0, 100000);
                        }

                        ClientController.TransferMoney(firstAcc, secondAcc, money, "BankAccounts");
                        curBankAccs = ClientController.CheckBankAccounts();
                        break;
                    case ConsoleKey.D6:
                        Console.Clear();
                        Console.WriteLine("Choose account you want to freeze/block(unfreeze/unblock):");
                        foreach (var item in curBankAccs)
                            Console.WriteLine($"ID: \t{item.AccountNumber}\n" +
                                              $"Balance: ${item.Balance}\n" +
                                              $"State: {item.State}");

                        choice = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                            curBankAccs[^1].AccountNumber);
                        
                        while (!ClientController.CheckAccount(choice) || !ClientController.CheckAccIfExists("BankAccounts", choice))
                        {
                            Console.WriteLine($"You do not have such account with ID = {choice}. Попробуйте снова");
                            choice = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                                curBankAccs[^1].AccountNumber);
                        }
                        
                        Console.WriteLine("If you want to freeze/unfreeze this account press F, to block/unblock press B");
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.F:
                                Console.Clear();
                                ClientController.Freeze(choice, "BankAccounts");
                                Console.WriteLine($"Account {choice} frozen/unfrozen successfully");
                                break;
                            case ConsoleKey.B:
                                Console.Clear();
                                ClientController.Block(choice, "BankAccounts");
                                Console.WriteLine($"Account {choice} blocked/unblocked successfully");
                                break;
                            default:
                                Console.Write("\b");
                                break;
                        }

                        curBankAccs = ClientController.CheckBankAccounts();
                        
                        break;
                    case ConsoleKey.D7:
                        Console.Clear();
                        Console.WriteLine("Choose account you want to delete:");
                        foreach (var item in curBankAccs)
                            Console.WriteLine($"ID: \t{item.AccountNumber}\n" +
                                              $"Balance: ${item.Balance}\n" +
                                              $"State: {item.State}");

                        choice = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                            curBankAccs[^1].AccountNumber);

                        while (!ClientController.CheckAccIfExists("BankAccounts", choice) || 
                               !ClientController.CheckAccount(choice))
                        {
                            Console.WriteLine($"You do not have such account with ID = {choice}. Попробуйте снова");
                            choice = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                                curBankAccs[^1].AccountNumber);
                        }

                        ClientController.Delete(choice, "BankAccounts");
                        curBankAccs = ClientController.CheckBankAccounts();
                        Console.WriteLine("Deleted successfully");
                        break;
                    case ConsoleKey.D8:
                        Console.Clear();
                        quit = true;
                        break;
                    default:
                        Console.Clear();
                        quit = true;
                        break;
                }
            }
    }

    private static void Loan()
    {
        List<string> loans = ClientController.GetLoans();
        
        bool quit = false;

        while (!quit)
        {
            Console.WriteLine($"You have {loans.Count} loans\n" +
                              "1. Check all loans\n" +
                              "2. Take new credit\n" +
                              "3. Take new installment\n" +
                              "4. Exit");
            
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.Write("\b");
                    
                    foreach(var item in loans)
                        Console.WriteLine(item);
                    
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    TakeACredit();
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    TakeAnInstallment();
                    break;
                case ConsoleKey.D4:
                    Console.Clear();
                    quit = true;
                    break;
            }
        }

    }
    private static void Deposit()
    {
        List<BankAccModel> curBankAccs = ClientController.CheckBankAccounts();
        List<int> allBankAccs = ClientController.GetAllBankAccId();
        
        if (curBankAccs.Count == 0)
        {
            Console.WriteLine("You do not have any account to make a deposit\n" +
                              "Firstly you should create a bank account\n");
            return;
        }
        
        int moneyCheck = 0;
        foreach (var item in curBankAccs)
            moneyCheck += item.Balance;

        if (moneyCheck == 0)
        {
            Console.WriteLine("You do not have any account with balance above $0\n" +
                              "Firstly replenish at least one of your bank accounts");
            return;
        }

        bool quit = false;
        List<int> depositIds = ClientController.GetAllDepositId();

        while (!quit)
        {
            Console.WriteLine("1. Open new deposit\n" +
                              "2. Check info about all deposits\n" +
                              "3. Withdraw money\n" +
                              "4. Transfer money to another deposit\n" +
                              "5. Freeze/block deposit\n" +
                              "6. Delete deposit\n" +
                              "7. Exit\n");
            int money, deposit;
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    Console.WriteLine("Choose bank account from which you want to open a deposit");
                    Console.WriteLine($"You have {curBankAccs.Count} bank account(s)");
                    foreach (var item in curBankAccs)
                        Console.WriteLine($"ID: \t{item.AccountNumber}\n" +
                                          $"Balance: ${item.Balance}\n" +
                                          $"State: {item.State}");

                    int choice = InputHelper.GetIntInBounds(allBankAccs[0], allBankAccs[^1]);
                    
                    while (!ClientController.CheckAccount(choice) ||
                           ClientController.CheckState(choice, "BankAccounts") is "Frozen" or "Blocked")
                    {
                        while (!ClientController.CheckAccount(choice))
                        {
                            Console.WriteLine($"You do not have such account with ID = {choice}. Попробуйте снова");
                            choice = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                                curBankAccs[^1].AccountNumber);
                        }

                        Console.WriteLine($"Your account with ID = {choice} is frozen. Try another one");
                        choice = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber,
                            curBankAccs[^1].AccountNumber);
                    }
                    
                    Console.WriteLine("How much money do you want to deposit");
                    money = InputHelper.GetIntInBounds(1, ClientController.CheckAccountMoney(choice));
                    
                    ClientController.TakeDeposit(new DepositModel(DateTime.Now.ToString("dd-MM-yyyy"), 5, money));
                    curBankAccs = ClientController.CheckBankAccounts();
                    Console.WriteLine("Deposit successfully opened :)");
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    ShowAllDeposits();
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    Console.WriteLine("Choose deposit to withdraw");
                    ShowAllDeposits();
                    
                    deposit = InputHelper.GetIntInBounds(depositIds[0], depositIds[^1]);

                    while (!ClientController.CheckDeposit(deposit) ||
                           ClientController.CheckState(deposit, "Deposit") is "Frozen" or "Blocked")
                    {
                        while (!ClientController.CheckDeposit(deposit))
                        {
                            Console.WriteLine($"You do not have such deposit with ID = {deposit}. Попробуйте снова");
                            deposit = InputHelper.GetIntInBounds(depositIds[0], depositIds[^1]);
                        }

                        Console.WriteLine($"Your deposit with ID = {deposit} is frozen or blocked. Попробуйте снова");
                        deposit = InputHelper.GetIntInBounds(depositIds[0], depositIds[^1]);
                    }
                    
                    Console.WriteLine("How much money do you want to withdraw");
                    money = InputHelper.GetIntInBounds(1, ClientController.CheckDepositMoney(deposit));
                    
                    while (ClientController.CheckDepositMoney(deposit) < money)
                    {
                        Console.WriteLine("You do not have enough money on this deposit");
                        money = InputHelper.GetIntInBounds(1, ClientController.CheckDepositMoney(deposit));
                    }
                    
                    ClientController.WithdrawDeposit(deposit, money);
                    break;
                case ConsoleKey.D4:
                    Console.Clear();
                    
                    List<int> deposits = ClientController.GetDeposits();
                    
                    moneyCheck = 0;
                    for (int i = 0; i < deposits.Count; i += 4)
                        moneyCheck += deposits[i + 1];

                    if (moneyCheck == 0)
                    {
                        Console.WriteLine("You do not have any deposit with balance above $0");
                        break;
                    }
                    
                    Console.WriteLine("Choose deposit to transfer from");
                    ShowAllDeposits();
                    var sender = InputHelper.GetIntInBounds(depositIds[0], depositIds[^1]);
                    
                    while (!ClientController.CheckDeposit(sender) ||
                           ClientController.CheckState(sender, "Deposit") is "Frozen" or "Blocked")
                    {
                        while (!ClientController.CheckDeposit(sender))
                        {
                            Console.WriteLine($"You do not have such deposit with ID = {sender}. Попробуйте снова");
                            sender = InputHelper.GetIntInBounds(depositIds[0], depositIds[^1]);
                        }

                        Console.WriteLine($"Your deposit with ID = {sender} is frozen or blocked. Попробуйте снова");
                        sender = InputHelper.GetIntInBounds(depositIds[0], depositIds[^1]);
                    }
                    
                    Console.WriteLine("Enter deposit to which you want to transfer money");
                    
                    var receiver = InputHelper.GetIntInBounds(depositIds[0], depositIds[^1]);
                    
                    while (!ClientController.CheckAccIfExists("Deposit", receiver))
                    {
                        Console.WriteLine($"There is no suitable deposit with ID = {receiver}. Попробуйте снова");
                        receiver = InputHelper.GetIntInBounds(depositIds[0], depositIds[^1]);
                    }

                    Console.WriteLine("Какую сумму вы хотите перевести?:");
                    money = InputHelper.GetIntInBounds(0, 100000);

                    while (moneyCheck < money)
                    {
                        Console.WriteLine("Недостаточно средств. Попробуйте снова");
                        money = InputHelper.GetIntInBounds(0, 100000);
                    }
                    
                    ClientController.TransferMoney(sender, receiver, money, "Deposit");
                    break;
                case ConsoleKey.D5:
                    Console.Clear();
                    Console.WriteLine("Выберите депозит");
                    ShowAllDeposits();
                    deposit = InputHelper.GetIntInBounds(depositIds[0], depositIds[^1]);

                    while (!ClientController.CheckDeposit(deposit))
                    {
                        Console.WriteLine($"У вас нет депозита с ID = {deposit}. Попробуйте снова");
                        deposit = InputHelper.GetIntInBounds(depositIds[0], depositIds[^1]);
                    }
                    
                    Console.WriteLine("Заморозка - F, блокировка - B");
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.F:
                            ClientController.Freeze(deposit, "Deposit");
                            Console.WriteLine("Успешно.");
                            break;
                        case ConsoleKey.B:
                            ClientController.Block(deposit, "Deposit");
                            Console.WriteLine("Успешно.");
                            break;
                        default:
                            Console.Write("\b");
                            break;
                    }
                    
                    ClientController.Freeze(deposit, "Deposit");
                    break;
                case ConsoleKey.D6:
                    Console.Clear();
                    Console.WriteLine("Выберите, какой депозит удалить");
                    ShowAllDeposits();
                    deposit = InputHelper.GetIntInBounds(depositIds[0], depositIds[^1]);

                    while (!ClientController.CheckDeposit(deposit))
                    {
                        Console.WriteLine($"Нет депозита с ID = {deposit}. Попробуйте снова");
                        deposit = InputHelper.GetIntInBounds(depositIds[0], depositIds[^1]);
                    }
                    
                    ClientController.Delete(deposit, "Deposit");
                    depositIds = ClientController.GetAllDepositId();
                    break;
                case ConsoleKey.D7:
                    Console.Write("\b");
                    quit = true;
                    break;
                default:
                    Console.Write("\b");
                    break;
            }
        }
    }

    private static void ShowAllDeposits()
    {
        List<int> deposits = ClientController.GetDeposits();

        for (int i = 0; i < deposits.Count; i += 4)
            Console.WriteLine($"ID: {deposits[i]}\n" +
                              $"Баланс: {deposits[i + 1]}\n" +
                              $"Процент: {deposits[i + 2]}\n" +
                              $"Месячный взнос: {deposits[i + 3]}");
        
    }
    private static void TakeACredit()
    {
        List<BankAccModel> curBankAccs = ClientController.CheckBankAccounts();
        
        if (curBankAccs.Count == 0)
        {
            Console.WriteLine("Аккаунт не найден");
            return;
        }
        
        
        foreach (var item in curBankAccs)
            Console.WriteLine($"ID: {item.AccountNumber}\n" +
                              $"Баланс: ${item.Balance}\n");
        
        Console.Write("Выберите аккаунт: ");
        var acc = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber, curBankAccs[^1].AccountNumber);

        while (!ClientController.CheckAccount(acc))
        {
            Console.Write($"Нет аккаунта с ID = {acc}. Попробуйте снова: ");
            acc = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber, curBankAccs[^1].AccountNumber);
        }

        var salary = ClientController.GetSalary();

        Console.Write($"1. 3 месяца - выплата {3 * salary / 2} - 5%\n" +
                          $"2. 6 месяца - выплата {6 * salary / 2} - 7%\n" +
                          $"3. 12 месяца - выплата {12 * salary / 2} - 10%\n" +
                          $"4. 24 месяца - выплата {24 * salary / 2} - 15%\n" +
                          $"5. 36 месяца - выплата {36 * salary / 2} - 20%\n" +
                          $"Выберите период: ");
        
        var period = InputHelper.GetIntInBounds(1, 5);
        var date = DateTime.Now;
        Console.Write("Какую сумму вы хотите снять?: ");
        int money;

        switch (period)
        {
            case 1:
                money = InputHelper.GetIntInBounds(1, 3 * salary / 2);
                ClientController.TakeCredit(new CreditModel(date.ToString("dd-MM-yyyy"), 3, 5, money));
                break;
            case 2:
                money = InputHelper.GetIntInBounds(1, 6 * salary / 2);
                ClientController.TakeCredit(new CreditModel(date.ToString("dd-MM-yyyy"), 6, 7, money));
                break;
            case 3:
                money = InputHelper.GetIntInBounds(1, 12 * salary / 2);
                ClientController.TakeCredit(new CreditModel(date.ToString("dd-MM-yyyy"), 12, 10, money));
                break;
            case 4:
                money = InputHelper.GetIntInBounds(1, 24 * salary / 2);
                ClientController.TakeCredit(new CreditModel(date.ToString("dd-MM-yyyy"), 24, 15, money));
                break;
            case 5:
                money = InputHelper.GetIntInBounds(1, 36 * salary / 2);
                ClientController.TakeCredit(new CreditModel(date.ToString("dd-MM-yyyy"), 36, 20, money));
                break;
        }
    }

    private static void TakeAnInstallment()
    {
        List<BankAccModel> curBankAccs = ClientController.CheckBankAccounts();
        
        if (curBankAccs.Count == 0)
        {
            Console.WriteLine("Аккаунт не найден");
            return;
        }
        
        
        foreach (var item in curBankAccs)
            Console.WriteLine($"ID: {item.AccountNumber}\n" +
                              $"Баланс: ${item.Balance}\n");

        Console.Write("Выберите аккаунт для перевода: ");
        var acc = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber, curBankAccs[^1].AccountNumber);

        while (!ClientController.CheckAccount(acc))
        {
            Console.Write($"Нет аккаунта с ID = {acc}. Попробуйте снова: ");
            acc = InputHelper.GetIntInBounds(curBankAccs[0].AccountNumber, curBankAccs[^1].AccountNumber);
        }

        var salary = ClientController.GetSalary();

        Console.WriteLine($"1. 3 месяца - выплата {3 * salary / 2}\n" +
                          $"2. 6 месяца - выплата {6 * salary / 2}\n" +
                          $"3. 12 месяца - выплата {12 * salary / 2}\n" +
                          $"4. 24 месяца - выплата {24 * salary / 2}\n" +
                          $"5. 36 месяца - выплата {36 * salary / 2}\n" +
                          "Выберите период: ");
        
        var period = InputHelper.GetIntInBounds(1, 5);
        var date = DateTime.Now;
        Console.Write("Введите сумму: ");
        int money;

        switch (period)
        {
            case 1:
                money = InputHelper.GetIntInBounds(1, 3 * salary / 2);
                ClientController.TakeInstallment(new InstallmentModel(date.ToString("dd-MM-yyyy"), 3, money));
                break;
            case 2:
                money = InputHelper.GetIntInBounds(1, 6 * salary / 2);
                ClientController.TakeInstallment(new InstallmentModel(date.ToString("dd-MM-yyyy"), 6, money));
                break;
            case 3:
                money = InputHelper.GetIntInBounds(1, 12 * salary / 2);
                ClientController.TakeInstallment(new InstallmentModel(date.ToString("dd-MM-yyyy"), 12, money));
                break;
            case 4:
                money = InputHelper.GetIntInBounds(1, 24 * salary / 2);
                ClientController.TakeInstallment(new InstallmentModel(date.ToString("dd-MM-yyyy"), 24, money));
                break;
            case 5:
                money = InputHelper.GetIntInBounds(1, 36 * salary / 2);
                ClientController.TakeInstallment(new InstallmentModel(date.ToString("dd-MM-yyyy"), 36, money));
                break;
        }
    }
}