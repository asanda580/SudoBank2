// 1. Explain the line below.
public interface IRepository
{
    void Create(BankAccount account);
    BankAccount Read(string accountNumber);
    void Update(BankAccount account);
    void Delete(string accountNumber);
}


public abstract class AccountBase
{
    public string AccountNumber { get; set; }
    public string AccountHolder { get; set; }
    public double Balance { get; set; }
    // 2. CalculateInterest is abstract, what does that mean?
    public abstract double CalculateInterest();

    public virtual void GenerateStatement()
    {
        Console.WriteLine("Generating generic account statement...");
    }
    // 3. What does virtual mean in this context of GenerateStatement
    public virtual void GenerateStatement(DateTime startDate)
    {
        Console.WriteLine($"Generating statement starting from {startDate.ToShortDateString()}");
    }
}
// 4. Describe this class below. How do we call the method below.
public static class BankValidator
{
    public static void ValidateString(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new FormatException();
        }
    }
}

// 5. Describe the inheritance happening below and what it means.
public partial class BankManager : IRepository
{
    private BankAccount[] _accounts = new BankAccount[100];
    private int _accountCount = 0;

    public void Create(BankAccount account)
    {
        if (account == null || _accountCount >= _accounts.Length)
        {
            // 6. What does this line below do in this context.
            throw new Exception();
        }
        _accounts[_accountCount] = account;
        _accountCount++;
    }
}

public partial class BankManager
{
    public BankAccount Read(string accountNumber)
    {
        for (int i = 0; i < _accountCount; i++)
        {
            if (_accounts[i].AccountNumber == accountNumber)
            {
                return _accounts[i];
            }
        }
        throw new Exception();
    }

    public void Update(BankAccount account)
    {
        BankAccount existing = Read(account.AccountNumber);
        existing.AccountHolder = account.AccountHolder;
        existing.Balance = account.Balance;
    }

    public void Delete(string accountNumber)
    {
        int indexToRemove = -1;
        for (int i = 0; i < _accountCount; i++)
        {
            if (_accounts[i].AccountNumber == accountNumber)
            {
                indexToRemove = i;
                break;
            }
        }

        if (indexToRemove == -1) throw new Exception();

        for (int i = indexToRemove; i < _accountCount - 1; i++)
        {
            _accounts[i] = _accounts[i + 1];
        }

        _accounts[_accountCount - 1] = null;
        _accountCount--;
    }
}

// 7. Describe the inheritance below and the significance of AccountBase
public class BankAccount : AccountBase
{
    public double InterestRate { get; set; }

    public override double CalculateInterest()
    {
        return Balance * (InterestRate / 100);
    }

    public override void GenerateStatement()
    {
        Console.WriteLine($"Account: {AccountNumber} | Holder: {AccountHolder} | Balance: R{Balance}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        BankManager manager = new BankManager();

        try
        {
            Console.WriteLine("--- Banking Management System ---");

            Console.Write("Enter Account Number: ");
            string accNum = Console.ReadLine();
            BankValidator.ValidateString(accNum);

            Console.Write("Enter Account Holder: ");
            string holder = Console.ReadLine();
            BankValidator.ValidateString(holder);

            BankAccount newAccount = new BankAccount
            {
                AccountNumber = accNum,
                AccountHolder = holder,
                Balance = 7500.50,
                InterestRate = 5.25
            };

            manager.Create(newAccount);

            double interest = newAccount.CalculateInterest();
            Console.WriteLine($"\nRecord Stored. Projected Interest: R{interest}");

            newAccount.GenerateStatement();
            newAccount.GenerateStatement(DateTime.Now);

            Console.WriteLine("\nTesting Delete Operation...");
            manager.Delete(accNum);
            Console.WriteLine("Account removed successfully.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: Format not recognized.");
        }
        catch (Exception)
        {
            Console.WriteLine("Error: Operation failed.");
        }
    }
}