using System.Data;
using Microsoft.Data.Sqlite;
using Models;

namespace Entities;

public class Database
{
    private readonly SqliteConnection _connection;

    private Database(string password)
    {
        var dbSettings = new SqliteConnectionStringBuilder(@"Data Source=../../../../encrypted.db")
        {
            Mode = SqliteOpenMode.ReadWriteCreate,
            Password = password
        }.ToString();

        _connection = new SqliteConnection(dbSettings);
    }

    public static async Task<Database> CreateAsync(string password)
    {
        var db = new Database(password);
        await db._connection.OpenAsync();

        return db;
    }
    
    public async void Create()
    {
        var stm = "insert into Banks(Name) values ('Belinvestbank')";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = "insert into Banks(Name) values ('Belarusbank')";
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = "insert into Banks(Name) values ('Aflabank')";
        await cmd.ExecuteNonQueryAsync();

        for (var i = 0; i < 10; i++)
        {
            var name = File.ReadLines(@"../../../../companies.txt").Skip(new Random().Next(i * 6, (i + 1) * 6)).First();

            cmd.CommandText = "insert into Organizations(name, pan, bic, address) " +
                              $"values ('{name}', {new Random().Next(100000, 999999)}, {new Random().Next(100000, 999999)}, '{name}')";
            await cmd.ExecuteNonQueryAsync();
        }

        for (var i = 0; i < 100; i++)
        {
            var name = File.ReadLines(@"../../../../names.txt").Skip(new Random().Next(i * 51, (i + 1) * 51)).First();
            var surname = File.ReadLines(@"../../../../surnames.txt").Skip(new Random().Next(i * 10, (i + 1) * 10)).First();
            var passport = File.ReadLines(@"../../../../passport.txt").Skip(new Random().Next(i * 10, (i + 1) * 10)).First();
            var identification = File.ReadLines(@"../../../../identification.txt").Skip(new Random().Next(i * 10, (i + 1) * 10)).First();
            var phone = $"29{new Random().Next(100000, 999999)}";
            var email = surname + "@gmail.com";
            var company = File.ReadLines(@"../../../../companies.txt").Skip(new Random().Next(75)).First();
            var bank = new Random().Next(1, 4);

            cmd.CommandText = $"insert into UsersAuth(login, password, bank) values ('{name}', '{surname}', {bank})";
            await cmd.ExecuteNonQueryAsync();

            cmd.CommandText = "insert into UsersInfo(Name, Surname, Passport, Identification, Phone, Email, AccessLevel, Bank, Salary, Cash, Work) " +
                              $"values ('{name}','{surname}','{passport}','{identification}','{phone}','{email}'," +
                              $"1, {bank}, {new Random().Next(500, 2000)}, {new Random().Next(10000, 99999)}, '{company}')";
            await cmd.ExecuteNonQueryAsync();
        }

        cmd.CommandText = "select Name from Organizations";
        await cmd.ExecuteNonQueryAsync();

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        var info = new List<string>();

        while (rdr.ReadAsync().Result)
            info.Add(rdr.GetString(0));
        await rdr.CloseAsync();

        for (var i = 0; i < 10; i++)
        {
            var name = File.ReadLines(@"../../../../names.txt").Skip(new Random().Next((i + 100) * 10, (i + 101) * 10)).First();
            var surname = File.ReadLines(@"../../../../surnames.txt").Skip(new Random().Next((i + 100) * 10, (i + 101) * 10)).First();
            var passport = File.ReadLines(@"../../../../passport.txt").Skip(new Random().Next((i + 100) * 10, (i + 101) * 10)).First();
            var identification = File.ReadLines(@"../../../../identification.txt").Skip(new Random().Next((i + 100) * 10, (i + 101) * 10)).First();
            var phone = $"29{new Random().Next(100000, 999999)}";
            var email = surname + "@gmail.com";
            var bank = new Random().Next(1, 4);

            cmd.CommandText = $"insert into UsersAuth(login, password, bank) values ('{name}', '{surname}', {bank})";
            await cmd.ExecuteNonQueryAsync();

            cmd.CommandText = "insert into UsersInfo(Name, Surname, Passport, Identification, Phone, Email, AccessLevel, Bank, Salary, Cash, Work) " +
                              $"values ('{name}','{surname}','{passport}','{identification}', '{phone}','{email}'," +
                              $"2, '{new Random().Next(1, 4)}', {new Random().Next(500, 2000)}, {new Random().Next(10000, 99999)}, '{info[i]}')";
            await cmd.ExecuteNonQueryAsync();

            cmd.CommandText =
                $"insert into BankAccounts(Owner, Balance, Bank) values ('{passport}', {new Random().Next(700000, 900000)}, {bank})";
            await cmd.ExecuteNonQueryAsync();

            cmd.CommandText =
                $"update Organizations set Specialist = '{passport}', Account = '{i + 1}' where ID = {i + 1}";
            await cmd.ExecuteNonQueryAsync();
        }

        cmd.CommandText = "insert into UsersAuth(Login, Password, Bank) VALUES ('blessed', '12345', 1)";
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = "insert into UsersAuth(Login, Password, Bank) VALUES ('stolen', '12345', 1)";
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = "insert into UsersAuth(Login, Password, Bank) VALUES ('vankaralev', '12345', 1)";
        await cmd.ExecuteNonQueryAsync();

        for ( int i = 1; i < 4; i++)
        {
            cmd.CommandText =
                $"insert into UsersInfo(Name, Passport, Identification, Phone, Email, AccessLevel, Bank) values ('Egor', 'Undef1', 'Undef1', '299072507', 'krupenich.egor@mail.ru', 5, {i})";
            await cmd.ExecuteNonQueryAsync();

            cmd.CommandText =
                $"insert into UsersInfo(Name, Passport, Identification, Phone, Email, AccessLevel, Bank) values ('Igor', 'Undef2', 'Undef2', '331234567', 'stolenforall@gmail.com', 4, {i})";
            await cmd.ExecuteNonQueryAsync();

            cmd.CommandText =
                $"insert into UsersInfo(Name, Passport, Identification, Phone, Email, AccessLevel, Bank) values ('Vanya', 'Undef3', 'Undef3', '295347514', 'vankaralev@mail.ru', 3, {i})";
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task<List<string>> CheckRegInfo()
    {
        var stm = $"select ID, Login, Password from DeclaredUsers";
        
        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
        
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        List<string> info = new List<string>();

        while (rdr.ReadAsync().Result && rdr.HasRows)
            info.Add($"ID: {rdr.GetString(0)}\n" +
                     $"Login: {rdr.GetString(1)}\n" +
                     $"Password: {rdr.GetString(2)}");

        return info;
    }
    
    public async void ConfirmRegistration(int id)
    {
        var stm = $"select Login, Password, Name, Surname, Passport, Identification, Phone, Email, Bank, Salary, Cash, Work from DeclaredUsers where ID = {id}";
        
        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
        
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        await rdr.ReadAsync();
        if (!rdr.HasRows) return;
        
        List<string> info = new List<string>
        {
            rdr.GetString(0),
            rdr.GetString(1),
            rdr.GetString(2),
            rdr.GetString(3),
            rdr.GetString(4),
            rdr.GetString(5),
            rdr.GetString(6),
            rdr.GetString(7),
            rdr.GetInt32(8).ToString(),
            rdr.GetInt32(9).ToString(),
            rdr.GetInt32(10).ToString(),
            rdr.GetString(11)
        };
        
        await rdr.CloseAsync();
        
        cmd.CommandText = $"delete from DeclaredUsers where ID = {id}";
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"insert into UsersAuth(login, password, bank) values ('{info[0]}', '{info[1]}', {info[8]})";
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = "insert into UsersInfo(Name, Surname, Passport, Identification, Phone, Email, AccessLevel, Bank, Salary, Cash, Work) " +
                          $"values ('{info[2]}','{info[3]}','{info[4]}','{info[5]}','{info[6]}','{info[7]}'," +
                          $"1, {info[8]}, {info[9]}, {info[10]}, '{info[11]}')";
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Confirm registration', 'Confirmed registration for {info[0]}')";
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<string>> ChooseBank()
    {
        var stm = "select Name from Banks";

        await using var cmd = new SqliteCommand(stm, _connection);
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        var info = new List<string>();

        while (rdr.ReadAsync().Result && rdr.HasRows)
            info.Add(rdr.GetString(0));

        return info;
    }

    private int ChooseIntBank(string bankName)
    {
        var banks = ChooseBank().Result;
        if (banks.Count == 0)
            return 0;

        if (bankName == banks[0])
            return 1;
        if (bankName == banks[1])
            return 2;
        return 3;
    }

    public async void Register(List<string> info, string bankName)
    {
        var bank = ChooseIntBank(bankName);
        var orgs = GetOrganizations().Result;

        var stm = "insert into DeclaredUsers(login, password, name, surname, passport, identification, Phone, email, Bank, Salary, Cash, Work) " +
                  $"VALUES ('{info[0]}', '{info[1]}', '{info[2]}','{info[3]}', '{info[4]}', '{info[5]}'," +
                  $"'{info[6]}', '{info[7]}', {bank}, {new Random().Next(500, 2000)}, {new Random().Next(10000, 100000)}, '{orgs[new Random().Next(11)]}')";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Registration', 'Registration request {info[0]}')";
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<string>> Authorize(string login, string password, string bank)
    {
        string stm;
        var banks = ChooseBank().Result;

        if (bank == banks[0])
            stm =
                $"select Name, Surname, Passport, Identification, Phone, Email, AccessLevel, Cash, Work from UsersInfo, UsersAuth WHERE UsersInfo.Bank = 1 and UsersAuth.ID = UsersInfo.ID and UsersAuth.Login= '{login}' and UsersAuth.Password = '{password}'";
        else if (bank == banks[1])
            stm =
                $"select Name, Surname, Passport, Identification, Phone, Email, AccessLevel, Cash, Work from UsersInfo, UsersAuth WHERE UsersInfo.Bank = 2 and UsersAuth.ID = UsersInfo.ID and UsersAuth.Login= '{login}' and UsersAuth.Password = '{password}'";
        else
            stm =
                $"select Name, Surname, Passport, Identification, Phone, Email, AccessLevel, Cash, Work from UsersInfo, UsersAuth WHERE UsersInfo.Bank = 3 and UsersAuth.ID = UsersInfo.ID and UsersAuth.Login= '{login}' and UsersAuth.Password = '{password}'";


        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        await using var rdr = cmd.ExecuteReaderAsync().Result;
        await rdr.ReadAsync();

        List<string> info = new List<string>();
        
        if (!rdr.HasRows) return info;

        info = new List<string>
        {
            rdr.GetString(0),
            rdr.GetString(1),
            rdr.GetString(2),
            rdr.GetString(3),
            rdr.GetString(4),
            rdr.GetString(5),
            rdr.GetInt32(6).ToString(),
            rdr.GetInt32(7).ToString(),
            rdr.GetString(8)
        };

        await rdr.CloseAsync();
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Authorization', 'User {login} logged-in {bank}')";
        await cmd.ExecuteNonQueryAsync();

        return info;
    }

    public async Task<bool> CheckLoginInDb(string login, string bankName)
    {
        var stm = "select Login, Bank, Status from UsersAuth";
        await using var cmd = new SqliteCommand(stm, _connection);

        var bank = ChooseIntBank(bankName);

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        while (rdr.ReadAsync().Result && rdr.HasRows)
            if (rdr.GetString(0) == login)
                if(rdr.GetInt32(1) == bank)
                    if(rdr.GetString(2) == "Active")
                        return true;

        return false;
    }

    public async void BlockClient(int id)
    {
        var stm = $"select Status from UsersAuth where ID = {id}";
        
        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
        
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        await rdr.ReadAsync();

        if (rdr.HasRows && rdr.GetString(0) == "Active")
        {
            await rdr.CloseAsync();
            cmd.CommandText = $"update UsersAuth set Status = 'Blocked' where ID = '{id}'";
            await cmd.ExecuteNonQueryAsync();
            
            cmd.CommandText = $"insert into Logs(type, affect) values ('User', 'User {id} was blocked')";
            await cmd.ExecuteNonQueryAsync();
        }
        else if (rdr.HasRows)
        {
            await rdr.CloseAsync();
            cmd.CommandText = $"update UsersAuth set Status = 'Active' where ID = '{id}'";
            await cmd.ExecuteNonQueryAsync();
            
            cmd.CommandText = $"insert into Logs(type, affect) values ('User', 'User {id} was unblocked')";
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task<bool> IsLoginFree(string login, string bankName)
    {
        var bank = ChooseIntBank(bankName);

        var stm =
            $"select Login from UsersAuth where Bank = {bank} union select Login from DeclaredUsers where Bank = {bank};";
        await using var cmd = new SqliteCommand(stm, _connection);

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        while (rdr.ReadAsync().Result && rdr.HasRows)
            if (rdr.GetString(0) == login)
                return false;

        return true;
    }

    public async Task<string> CheckUserInDb(string login, string password)
    {
        var stm = "select Login, Password from UsersAuth";
        await using var cmd = new SqliteCommand(stm, _connection);

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        var ans = string.Empty;
        while (rdr.ReadAsync().Result && rdr.HasRows)
        {
            if (rdr.GetString(0) == login)
                if (rdr.GetString(1) == password)
                    ans = "Good";
                else
                    ans = "WrongPass";
        }

        return ans;
    }

    private async Task<List<string>> GetOrganizations()
    {
        var stm = "select Name from Organizations";
        await using var cmd = new SqliteCommand(stm, _connection);

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        var ans = new List<string>();
        while (rdr.ReadAsync().Result && rdr.HasRows)
            ans.Add(rdr.GetString(0));

        return ans;
    }
    
    public async Task<List<string>> Enterprice(string passport)
    {
        var stm = $"select Type, Name, PAN, BIC, Address, Account from Organizations where Specialist = '{passport}'";
        await using var cmd = new SqliteCommand(stm, _connection);

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        var ans = new List<string>();
        await rdr.ReadAsync();
        
        if (rdr.HasRows)
        {
            ans = new List<string>
            {
                rdr.GetString(0),
                rdr.GetString(1),
                rdr.GetString(2),
                rdr.GetString(3),
                rdr.GetString(4),
                rdr.GetString(5)
            };
        }

        return ans;
    }
    
    public async Task<List<string>> ShowUsers()
    {
        var stm = $"select ID, Login, Status from UsersAuth";
        await using var cmd = new SqliteCommand(stm, _connection);

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        var ans = new List<string>();
        await rdr.ReadAsync();
        
        while (rdr.HasRows)
            ans.Add($"ID: {rdr.GetInt32(0)}\n" +
                    $"Login: {rdr.GetString(1)}\n" +
                    $"Status: {rdr.GetString(2)}");

        return ans;
    }

    public async Task<List<int>> SalaryPlan(string organization, string passport)
    {
        var stm = $"select Salary from UsersInfo where Work = '{organization}'";
        await using var cmd = new SqliteCommand(stm, _connection);
        
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        int ans = 0, workCount = 0;
        while (rdr.ReadAsync().Result && rdr.HasRows)
        {
            ans += rdr.GetInt32(0);
            workCount++;
        }

        await rdr.CloseAsync();
        
        cmd.CommandText = $"insert into SalaryPlan(Organization, Money, Specialist) values ('{organization}', {ans}, '{passport}')";
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Salary plan', 'Salary plan request for {organization}')";
        await cmd.ExecuteNonQueryAsync();

        return new List<int> { ans, workCount };
    }

    public async Task<List<string>> CheckSalaryPlans()
    {
        var stm = "select * from SalaryPlan";
        await using var cmd = new SqliteCommand(stm, _connection);
        
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        List<string> info = new List<string>();
        while (rdr.ReadAsync().Result && rdr.HasRows)
            info.Add($"ID: {rdr.GetInt32(0).ToString()}\n" +
                     $"Organization: {rdr.GetString(1)}\n" +
                     $"Total salary: {rdr.GetInt32(2).ToString()}");

        return info;
    }

    public async Task<List<string>> CheckTransfers()
    {
        List<string> info = new List<string>();
        
        var stm = "select * from Transfer where Type != 'Cancelled'";
        await using var cmd = new SqliteCommand(stm, _connection);
        
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        while (rdr.ReadAsync().Result && rdr.HasRows)
            info.Add($"ID: {rdr.GetInt32(0)}\n" +
                     $"Sender: {rdr.GetInt32(1)}\n" +
                     $"Receiver: {rdr.GetInt32(2)}\n" +
                     $"Money: {rdr.GetInt32(3)}\n" +
                     $"Type: {rdr.GetString(4)}\n");
        
        return info;
    }
    
    public async Task<List<string>> CheckTransfersForOperator()
    {
        List<string> info = new List<string>();
        
        var stm = "select * from Transfer where Type != 'Enterprice' and Type != 'Cancelled'";
        await using var cmd = new SqliteCommand(stm, _connection);
        
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        while (rdr.ReadAsync().Result && rdr.HasRows)
            info.Add($"ID: {rdr.GetInt32(0)}\n" +
                     $"Sender: {rdr.GetInt32(1)}\n" +
                     $"Receiver: {rdr.GetInt32(2)}\n" +
                     $"Money: {rdr.GetInt32(3)}\n" +
                     $"Type: {rdr.GetString(4)}\n");
        
        return info;
    }
    
    public async Task<List<string>> CheckLoans(string bankName, string status = "All")
    {
        List<string> info = new List<string>();
        
        int bank = ChooseIntBank(bankName);

        string stm;
        if (status == "All")
            stm = $"select ID, Owner, Sum, Percent, Status, Payment, Remains from Loans where Bank = {bank}";
        else
            stm = $"select ID, Owner, Sum, Percent, Status, Payment, Remains from Loans where Bank = {bank} and Status = '{status}'";

        
        await using var cmd = new SqliteCommand(stm, _connection);
        
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        while (rdr.ReadAsync().Result && rdr.HasRows)
            info.Add($"ID: {rdr.GetInt32(0)}\n" +
                     $"Owner: {rdr.GetString(1)}\n" +
                     $"Sum: {rdr.GetInt32(2)}\n" +
                     $"Percent: {rdr.GetInt32(3)}\n" +
                     $"Status: {rdr.GetString(4)}\n" +
                     $"Payment: {rdr.GetInt32(5)}\n" +
                     $"Remains: {rdr.GetInt32(6)}");
        
        return info;
    }

    public async void CancelTransfer(int id)
    {
        var stm = $"select FirstAccount, SecondAccount, Money, Type from Transfer where ID = {id}";
        await using var cmd = new SqliteCommand(stm, _connection);
        
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        await rdr.ReadAsync();
        if (rdr.HasRows && rdr.GetString(3) != "Replenish" && rdr.GetString(3) != "Cancelled")
        {
            TransferModel transfer = new TransferModel(rdr.GetInt32(1), rdr.GetInt32(0), rdr.GetInt32(2));

            TransferMoney(transfer, rdr.GetString(3));
            
            await rdr.CloseAsync();
            
            cmd.CommandText = $"insert into Logs(type, affect) values ('Cancelled transfer', 'Cancelled transfer with id: {id}')";
            await cmd.ExecuteNonQueryAsync();
        }
        else if (rdr.HasRows && rdr.GetString(3) != "Cancelled")
        {
            int money = rdr.GetInt32(2);
            int bankId = rdr.GetInt32(1);
            await rdr.CloseAsync();

            cmd.CommandText = $"update BankAccounts set Balance = Balance - {money} where ID = {bankId}";
            await cmd.ExecuteNonQueryAsync();
            
            cmd.CommandText = $"update Transfer set Type = 'Cancelled' where ID = {id}";
            await cmd.ExecuteNonQueryAsync();
            
            cmd.CommandText = $"insert into Logs(type, affect) values ('Cancelled replenishment', 'Cancelled replenishment with id: {id}')";
            await cmd.ExecuteNonQueryAsync();
        }

        
        
    }

    public async void ConfirmSalaryPlan(int id)
    {
        var stm = $"select Money, Organization, Specialist from SalaryPlan where ID = {id}";
        await using var cmd = new SqliteCommand(stm, _connection);
        
        await cmd.ExecuteNonQueryAsync();
        
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        int total = 0;
        string bankAcc = String.Empty;
        string organization = String.Empty;
        
        await rdr.ReadAsync();
        if (rdr.HasRows)
        {
            total = rdr.GetInt32(0);
            organization = rdr.GetString(1);
            bankAcc = rdr.GetString(2);
        }
        await rdr.CloseAsync();
        
        cmd.CommandText = $"delete from SalaryPlan where ID = {id}";
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = $"update BankAccounts set Balance = Balance - {total} where Owner = '{bankAcc}'";
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"update UsersInfo set Cash = Cash + Salary where Work = '{organization}'";
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Confirm Salary plan', 'Confirmed salary plan with id: {id}')";
        await cmd.ExecuteNonQueryAsync();
    }
    
    public async void RejectSalaryPlan(int id)
    {
        var stm = $"delete from SalaryPlan where ID = {id}";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Deleted Salary plan', 'Deleted salary plan with id: {id}')";
        await cmd.ExecuteNonQueryAsync();
    }

    public async void ConfirmLoan(int id)
    {
        var stm = $"update Loans set Status = 'Active' where ID = {id}";
        
        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = $"insert into Logs(type, affect) values ('Confirm Loan', 'Confirmed loan with id: {id}')";
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> CheckInfoInUsers(string dest, string info, string bankName)
    {
        var bank = ChooseIntBank(bankName);
        var stm =
            $"select {dest} from UsersInfo where Bank = {bank} union select {dest} from DeclaredUsers where Bank = {bank}";

        await using var cmd = new SqliteCommand(stm, _connection);

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        var check = new List<string>();
        while (rdr.ReadAsync().Result && rdr.HasRows)
            check.Add(rdr.GetString(0));

        return check.Any(item => item == info);
    }

    public async Task<List<BankAccModel>> CheckUserBankAccounts(string owner, string bankName)
    {
        var bank = ChooseIntBank(bankName);

        var stm = $"select ID, Balance, Status from BankAccounts where Owner = '{owner}' and Bank = {bank}";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        List<BankAccModel> list = new();
        while (rdr.ReadAsync().Result && rdr.HasRows)
            list.Add(new BankAccModel(rdr.GetInt32(0), rdr.GetInt32(1), bank, rdr.GetString(2)));

        return list;
    }

    public async void CreateNewBankAccount(string owner, string bankName)
    {
        var bank = ChooseIntBank(bankName);

        var stm = $"insert into BankAccounts(owner, balance, bank) values ('{owner}', 0, {bank})";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Account', 'New bank account for {owner} in {bankName}')";
        await cmd.ExecuteNonQueryAsync();
    }

    public async void Delete(string passport, int id, string table)
    {
        string stm;
        stm = table == "BankAccounts" ? $"select Balance from BankAccounts where ID = {id}" : $"select Sum, Profit from Deposit where ID = {id}";
        
        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
        
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        await rdr.ReadAsync();
        
        int money = 0;
        if (rdr.HasRows)
            money = table == "BankAccounts" ? rdr.GetInt32(0) : money = rdr.GetInt32(0) + rdr.GetInt32(1);

        await rdr.CloseAsync();

        cmd.CommandText = $"update UsersInfo set Cash = Cash + {money} where Passport = '{passport}'";
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = $"delete from {table} where ID = {id}";
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText =
            $"insert into Logs(type, affect) values ('Account/Deposit', 'Deleted account/deposit with {id} for {passport}')";
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> CheckAccount(string passport, int account)
    {
        var stm = $"select ID from BankAccounts where Owner = '{passport}'";

        await using var cmd = new SqliteCommand(stm, _connection);

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        while (rdr.ReadAsync().Result && rdr.HasRows)
            if (rdr.GetInt32(0) == account)
                return true;

        return false;
    }
    
    public async Task<bool> CheckDeposit(string passport, int account)
    {
        var stm = $"select ID from Deposit where Owner = '{passport}'";

        await using var cmd = new SqliteCommand(stm, _connection);

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        while (rdr.ReadAsync().Result && rdr.HasRows)
            if (rdr.GetInt32(0) == account)
                return true;

        return false;
    }

    public async Task<int> CheckAccountMoney(int acc)
    {
        var stm = $"select Balance from BankAccounts where ID = {acc}";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        await rdr.ReadAsync();
        return rdr.HasRows ? rdr.GetInt32(0) : 0;
    }
    
    public async Task<int> CheckDepositMoney(int deposit)
    {
        var stm = $"select Sum from Deposit where ID = {deposit}";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        await rdr.ReadAsync();
        if (rdr.HasRows)
            return rdr.GetInt32(0);
        return 0;
    }

    public async Task<int> CheckUserCash(string owner)
    {
        var stm = $"select Cash from UsersInfo where Passport = '{owner}'";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        await rdr.ReadAsync();
        if (rdr.HasRows)
            return rdr.GetInt32(0);
        return -1;
    }

    public async void ReplenishAccount(string owner, int acc, int money)
    {
        var curBalance = CheckAccountMoney(acc).Result;
        var stm = $"update BankAccounts set Balance = {curBalance + money} where ID = {acc}";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        var curCash = CheckUserCash(owner).Result;
        cmd.CommandText = $"update UsersInfo set Cash = {curCash - money} where Passport = '{owner}'";
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = "insert into Transfer(FirstAccount, SecondAccount, Money, Type) " +
            $"values ({-1}, {acc}, {money}, 'Replenish')";
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Account', 'Account {acc} was has been replenished for {money} by {owner}')";
        await cmd.ExecuteNonQueryAsync();
    }

    public async void WithdrawCash(string owner, int acc, int money)
    {
        var stm = $"update BankAccounts set Balance = Balance - {money} where ID = {acc}";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = $"update UsersInfo set Cash = Cash + {money} where Passport = '{owner}'";
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Account', '{owner} withdrew {money} from account {acc}')";
        await cmd.ExecuteNonQueryAsync();
    }
    
    public async void WithdrawDeposit(string owner, int deposit, int money)
    {
        var stm = $"update Deposit set Sum = Sum - {money} where ID = {deposit}";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = $"update UsersInfo set Cash = Cash + {money} where Passport = '{owner}'";
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Deposit', '{owner} withdrew {money} from account {deposit}')";
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<int>> GetAllTableId(string table)
    {
        var stm = $"select ID from {table}";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        List<int> info = new();
        while (rdr.ReadAsync().Result && rdr.HasRows)
            info.Add(rdr.GetInt32(0));

        return info;
    }

    public bool CheckAccountIfExists(string table, int id)
    {
        List<int> info = GetAllTableId(table).Result;

        return info.Any(item => item == id);
    }
    public async void TransferMoney(TransferModel transfer, string type)
    {
        var stm = "insert into Transfer(FirstAccount, SecondAccount, Money, Type) " +
                  $"values ({transfer.FirstAccount}, {transfer.SecondAccount}, {transfer.Money}, '{type}')";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        if (type == "Replenish")
        {
            cmd.CommandText = transfer.SecondAccount == -1 ? $"update BankAccounts set Balance = Balance - {transfer.Money} where ID = {transfer.FirstAccount}" : $"update BankAccounts set Balance = Balance - {transfer.Money} where ID = {transfer.FirstAccount}";
            await cmd.ExecuteNonQueryAsync();
        }
        else if (type is "BankAccounts" or "Enterprice")
        {
            cmd.CommandText =
                $"update BankAccounts set Balance = Balance - {transfer.Money} where ID = {transfer.FirstAccount}";
            await cmd.ExecuteNonQueryAsync();
        
            cmd.CommandText =
                $"update BankAccounts set Balance = Balance + {transfer.Money} where ID = {transfer.SecondAccount}";
            await cmd.ExecuteNonQueryAsync();
        }
        else
        {
            cmd.CommandText =
                $"update Deposit set Sum = Sum - {transfer.Money} where ID = {transfer.FirstAccount}";
            await cmd.ExecuteNonQueryAsync();
        
            cmd.CommandText =
                $"update Deposit set Sum = Sum + {transfer.Money} where ID = {transfer.SecondAccount}";
            await cmd.ExecuteNonQueryAsync();
        }
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Transfer', '${transfer.Money} was sent from {transfer.FirstAccount} to {transfer.SecondAccount}')";
        await cmd.ExecuteNonQueryAsync();
    }

    public async void Freeze(int acc, string table)
    {
        var stm = $"select Status from {table} where ID = {acc}";
        
        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
        
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        await rdr.ReadAsync();

        if (rdr.HasRows && rdr.GetString(0) is "Active" or "Blocked")
        {
            await rdr.CloseAsync();
            cmd.CommandText = $"update {table} set Status = 'Frozen' where ID = '{acc}'";
            await cmd.ExecuteNonQueryAsync();
        }
        else
        {
            await rdr.CloseAsync();
            cmd.CommandText = $"update {table} set Status = 'Active' where ID = '{acc}'";
            await cmd.ExecuteNonQueryAsync();
        }
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Account/Deposit', 'Account/deposit {acc} was frozen/unfrozen')";
        await cmd.ExecuteNonQueryAsync();
    }
    
    public async void Block(int acc, string table)
    {
        var stm = $"select Status from {table} where ID = {acc}";
        
        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
        
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        await rdr.ReadAsync();

        if (rdr.HasRows && rdr.GetString(0) is "Active" or "Frozen")
        {
            await rdr.CloseAsync();
            cmd.CommandText = $"update {table} set Status = 'Blocked' where ID = '{acc}'";
            await cmd.ExecuteNonQueryAsync();
        }
        else
        {
            await rdr.CloseAsync();
            cmd.CommandText = $"update {table} set Status = 'Active' where ID = '{acc}'";
            await cmd.ExecuteNonQueryAsync();
        }
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Account/Deposit', 'Account/deposit {acc} was blocked/unblocked')";
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<string> CheckState(int acc, string table)
    {
        var stm = $"select Status from {table} where ID = '{acc}'";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        await rdr.ReadAsync();
        
        if (rdr.HasRows && rdr.GetString(0) == "Active")
            return "Active";
        
        if (rdr.HasRows && rdr.GetString(0) == "Frozen")
            return "Frozen";

        return "Empty";
    }

    public async Task<int> GetSalary(string passport)
    {
        var stm = $"select Salary from UsersInfo where Passport = '{passport}'";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        await using var rdr = cmd.ExecuteReaderAsync().Result;

        await rdr.ReadAsync();
        if (rdr.HasRows)
            return rdr.GetInt32(0);
        return 0;
    }

    public async void TakeCredit(string passport, string bankName, CreditModel credit)
    {
        var bank = ChooseIntBank(bankName);
        var stm = "insert into Loans(Owner, Sum, Bank, Percent, Date, Payment, Remains) " +
                  $"values ('{passport}', {credit.Money}, {bank}, {credit.Percent}, '{credit.Date}', {credit.Payment}, {credit.Remains})";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Credit', 'Credit was taken by {passport} in {bankName}')";
        await cmd.ExecuteNonQueryAsync();
    }

    public async void TakeInstallment(string passport, string bankName, InstallmentModel installment)
    {
        var bank = ChooseIntBank(bankName);
        var stm = "insert into Loans(Owner, Sum, Bank, Date, Payment, Remains) " +
                  $"values ('{passport}', {installment.Money}, {bank}, '{installment.Date}', {installment.Payment}, {installment.Remains})";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Installment', 'Installment was taken by {passport} in {bankName}')";
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<string>> LoanInfo(string passport)
    {
        string stm = $"select ID, Status, Remains, Percent from Loans where Owner = '{passport}'";
        
        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
    
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        List<string> loans = new List<string>();

        while (rdr.ReadAsync().Result && rdr.HasRows)
            loans.Add($"ID: {rdr.GetInt32(0)}\n" +
                      $"Status: {rdr.GetString(1)}" +
                      $"Remains: {rdr.GetInt32(2)} (Percent: {rdr.GetInt32(3)}%)");
        
        return loans;
    }
    
    public async Task<List<int>> DepositInfo(string passport)
    {
        string stm = $"select ID, Sum, Percent, Profit from Deposit where Owner = '{passport}'";
        
        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
    
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        List<int> deposits = new List<int>();

        while (rdr.ReadAsync().Result && rdr.HasRows)
        {
            deposits.Add(rdr.GetInt32(0));
            deposits.Add(rdr.GetInt32(1));
            deposits.Add(rdr.GetInt32(2));
            deposits.Add(rdr.GetInt32(3));
        }

        return deposits;
    }

    public async void TakeDeposit(string passport, string bankName, DepositModel deposit)
    {
        var bank = ChooseIntBank(bankName);
        var stm = "insert into Deposit(Owner, Sum, Bank, Percent, StartDate, Profit) " +
                  $"values ('{passport}', {deposit.Money}, {bank}, {deposit.Percent}, '{deposit.Date}', {deposit.Profit})";

        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
        
        cmd.CommandText = $"insert into Logs(type, affect) values ('Deposit', 'Deposit was taken by {passport} in {bankName}')";
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<string>> CheckLogs()
    {
        string stm = "select Type, Affect from Logs";
        
        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
    
        await using var rdr = cmd.ExecuteReaderAsync().Result;

        List<string> loans = new List<string>();

        while (rdr.ReadAsync().Result && rdr.HasRows)
            loans.Add($"Type: {rdr.GetString(0)}, action: {rdr.GetString(1)}\n");
        
        return loans;
    }

    public async void SkipMonth()
    {
        var stm = $"update Deposit set Sum = Sum + Sum * Percent / 100 where Status = 'Active'";
        
        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = $"update Deposit set Profit = Sum * Percent / 100 where Status = 'Active'";
        await cmd.ExecuteNonQueryAsync();
    }
    
    public async void CancelRegistration(int id)
    {
        var stm = $"delete from DeclaredUsers where ID = {id}";
        
        await using var cmd = new SqliteCommand(stm, _connection);
        await cmd.ExecuteNonQueryAsync();
    }
}