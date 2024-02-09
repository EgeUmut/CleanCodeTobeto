

class Program
{
    static void Main(string[] args)
    {

        IProductService productService = new ProductManager(new CentralBankServiceAdapter());
        productService.Sell(new Product() { Id = 1, Name = "Laptop", Price = 1000 },
            new Officer() { Id = 1, Name = "Engin" });

    }
}

class Customer : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsStudent { get; set; }

}

interface IEntity
{
}

class Product : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

}


class ProductManager : IProductService
{
    private IBankService bankService;


    public ProductManager(IBankService _bankService)
    {

        bankService = _bankService;
    }

    public void Sell(Product product, ICustomer customer)
    {
        decimal price = product.Price;
        price = customer.GetPrice(price);
        price = bankService.ConvertRate(new CurrencyRate() { Currency = 1, Price = price });
        Console.WriteLine(price);
        Console.ReadLine();
    }
}

interface IProductService
{
    void Sell(Product product, ICustomer customer);
}


class FakeBankService : IBankService
{
    //paramatre olarak obje kullanılması encasulation
    public decimal ConvertRate(CurrencyRate currencyRate)
    {
        //gliştirme ortamında service bağımlılığı olmamalı
        return currencyRate.Price / (decimal)30.5;
    }



}

interface IBankService
{
    decimal ConvertRate(CurrencyRate currencyRate);
}

class CurrencyRate
{
    public decimal Price { get; set; }
    public int Currency { get; set; }

}

//Adapter Design Patter
class CentralBankServiceAdapter : IBankService
{
    public decimal ConvertRate(CurrencyRate currencyRate)
    {
        CentralBankService centralBankService = new CentralBankService();
        return centralBankService.ConvertCurrency(currencyRate);
    }
}

//Merkez bankası kodu gibi
class CentralBankService
{
    public decimal ConvertCurrency(CurrencyRate currencyRate)
    {
        return currencyRate.Price / (decimal)32.5;
    }
}

interface ICustomer
{
    int Id { get; set; }
    string Name { get; set; }
    decimal GetPrice(decimal price);

}

class Officer : ICustomer
{
    public int Id { get; set; }
    public string Name { get; set; }

    public decimal GetPrice(decimal price)
    {
        return price * (decimal)0.80;
    }
}

class Student : ICustomer
{
    public int Id { get; set; }
    public string Name { get; set; }

    public decimal GetPrice(decimal price)
    {
        return price * (decimal)0.90;
    }
}
