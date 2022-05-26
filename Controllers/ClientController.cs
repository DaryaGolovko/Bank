using Entities;
using Models;

namespace Controllers;

public static class ClientController
{
    private static ClientModel _client = new();

    public static void Active(List<string> info, int money = 0)
    {
        _client = new ClientModel(info[0], info[1], info[2], info[3],
            info[4], info[5], Convert.ToInt32(info[7]), info[8]);
    }

    public static List<string> Info()
    {
        return new List<string>
        {
            "Name: " + _client.Name,
            "Surname: " + _client.Surname,
            "Phone: 80" + _client.Phone,
            "Email: " + _client.Email,
            "Cash: " + _client.Cash,
            "Work: " + _client.Work,
            "Passport: " + _client.Passport,
            "Identification: " + _client.Identification
        };
    }

    public static void AddBankAcc() =>
        MainController.Data.CreateNewBankAccount(_client.Passport, MainController.CurrentBank());

    public static List<BankAccModel> CheckBankAccounts() => 
        MainController.Data.CheckUserBankAccounts(_client.Passport, MainController.CurrentBank()).Result;
    

    public static void Delete(int id, string type)
    {
        if (type == "BankAccounts")
            _client.Cash += MainController.Data.CheckAccountMoney(id).Result;
        else
            _client.Cash += MainController.Data.CheckDepositMoney(id).Result;
        
        MainController.Data.Delete(_client.Passport,id, type);   
    }

    public static bool CheckAccount(int acc) => 
        MainController.Data.CheckAccount(_client.Passport, acc).Result;
    
    public static bool CheckDeposit(int acc) => 
        MainController.Data.CheckDeposit(_client.Passport, acc).Result;
    

    public static int CheckAccountMoney(int acc) => 
        MainController.Data.CheckAccountMoney(acc).Result;

    public static int CheckDepositMoney(int acc) => 
        MainController.Data.CheckDepositMoney(acc).Result;
    
    public static int CheckUserCash() =>
        MainController.Data.CheckUserCash(_client.Passport).Result;

    public static void ReplenishBalance(int acc, int money)
    {
        MainController.Data.ReplenishAccount(_client.Passport, acc, money);
        _client.Cash -= money;
    }

    public static void WithdrawCash(int acc, int money)
    {
        MainController.Data.WithdrawCash(_client.Passport, acc, money);
        _client.Cash += money;
    }

    public static void Freeze(int acc, string type)
    {
        MainController.Data.Freeze(acc, type);
        if (type != "BankAccounts") return;
        foreach (var item in _client.BankAccsList.Where(item => item.AccountNumber == acc)
                     .Where(item => item.State is "Active" or "Blocked"))
            item.State = "Frozen";
    }
    
    public static void Block(int acc, string type)
    {
        MainController.Data.Block(acc, type);
        if (type != "BankAccounts") return;
        foreach (var item in _client.BankAccsList.Where(item => item.AccountNumber == acc)
                     .Where(item => item.State is "Active" or "Frozen"))
            item.State = "Blocked";
    }

    public static string CheckState(int acc, string type) => 
        MainController.Data.CheckState(acc, type).Result;

    public static List<int> GetAllBankAccId() =>
        MainController.Data.GetAllTableId("BankAccounts").Result;
    
    public static List<int> GetAllDepositId() =>
        MainController.Data.GetAllTableId("Deposit").Result;

    public static void TransferMoney(int firstAcc, int secondAcc, int money, string type) =>
        MainController.Data.TransferMoney(new TransferModel(firstAcc, secondAcc, money), type);

    public static int GetSalary() =>
        MainController.Data.GetSalary(_client.Passport).Result;

    public static void TakeCredit(CreditModel credit) =>
        MainController.Data.TakeCredit(_client.Passport, MainController.CurrentBank(), credit);
    

    public static void TakeInstallment(InstallmentModel installment) =>
        MainController.Data.TakeInstallment(_client.Passport, MainController.CurrentBank(), installment);

    public static List<string> GetLoans() =>
        MainController.Data.LoanInfo(_client.Passport).Result;

    public static void TakeDeposit(DepositModel deposit) =>
        MainController.Data.TakeDeposit(_client.Passport, MainController.CurrentBank(), deposit);

    public static List<int> GetDeposits() =>
        MainController.Data.DepositInfo(_client.Passport).Result;

    public static void WithdrawDeposit(int deposit, int money)
    {
        MainController.Data.WithdrawDeposit(_client.Passport, deposit, money);
        _client.Cash += money;
    }

    public static bool CheckAccIfExists(string table, int id) =>
        MainController.Data.CheckAccountIfExists(table, id);
}