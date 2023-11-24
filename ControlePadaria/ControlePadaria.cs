class ControlePadaria
{
    static void Main(string[] args)
    {

    }

    [Serializable]
    struct Product{
        string batch;
        string description;
        DateTime validity;
        float price;
    }

    [Serializable]
    struct Stock
    {
        Product product;
        int stockAmount;
        int stockSaled;
        int stockInitial;
    }

    [Serializable]
    struct Sale 
    { 
        Product product;
        DateTime saleDate;
        int quantity;
    }

    [Serializable]
    struct CashRegister
    {
        Sale[] sales;
        float totalValue;
        string paymentType;
        DateTime date;
    }

}