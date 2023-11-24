using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

class ControlePadaria
{
    static Stock[] productsStock = new Stock[1000];
    static Product[] products = new Product[1000];
    static Sale[] sales = new Sale[1000];
    static CashRegister[] cashRegister = new CashRegister[1000];

    static int countStock = 0;
    static int countProduct = 0;
    static int countSales = 0;
    static int countCashRegister = 0;

    static void Main(string[] args)
    {
        showProducts();
        saveProducts();
        showProducts();
    }

    static void saveProducts()
    {
        Console.WriteLine("Quantos produtos deseja cadastrar? " + "\n Limite de " + (products.Length - countProduct));
        int amountProducts = int.Parse(Console.ReadLine());
        int total = amountProducts + countProduct;
        Console.Clear();
        for (int i = countProduct; i < amountProducts + countProduct; i++)
        {
            Console.WriteLine("Qual é o lote do produto?");
            products[i].batch = Console.ReadLine();
            Console.WriteLine("Insira uma descrição para o produto: ");
            products[i].description = Console.ReadLine();
            Console.WriteLine("Qual é a data de validade do produto? \nDigite no formato AA/MM/YYY.");
            Console.Write("Dia: ");
            int day = int.Parse(Console.ReadLine());
            Console.Write("Mês: ");
            int month = int.Parse(Console.ReadLine());
            Console.Write("Ano: ");
            int year = int.Parse(Console.ReadLine());
            products[i].validity = new DateTime(year, month, day);
            Console.WriteLine("Digite o preço do produto: ");
            products[i].price = float.Parse(Console.ReadLine());
            Console.Clear();
        }
        saveProductsBinFile(products, total);
    }

    static void saveProductsBinFile(Product[] productsArray, int total)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream wr = new FileStream("products.bin", FileMode.Create, FileAccess.Write);

        for (int i = 0; i < total; i++)
        {
            formatter.Serialize(wr, productsArray[i]); 
        }
        wr.Close();
    }
    static void readBinProducts()
    {
        IFormatter formatter = new BinaryFormatter();
        if (File.Exists("products.bin"))
        {
            Stream rd = new FileStream("products.bin", FileMode.Open, FileAccess.Read);
            int i = 0;
            while (rd.Position < rd.Length)
            {
                Product product = (Product)formatter.Deserialize(rd);
                products[i] = product;
                i++;
            }
            rd.Close();
        }
    }
    static void showProducts()
    {
        Array.Clear(products, 0, products.Length);
        readBinProducts();
        for (int i = 0; i<products.Length; i++)
        {
            Console.WriteLine(products[i].ToString());
        }
    }

    [Serializable]
    struct Product{
        public string batch;
        public string description;
        public DateTime validity;
        public float price;
        public override string ToString() =>
            $"Lote: {batch}; Descrição: {description}; Validade: {validity}; Preço: {price}";
    }

    [Serializable]
    struct Stock
    {
        public Product product;
        public int stockAmount;
        public int stockSaled;
        public int stockInitial;
    }

    [Serializable]
    struct Sale 
    {
        public Product product;
        public DateTime saleDate;
        public int quantity;
    }

    [Serializable]
    struct CashRegister
    {
        public Sale[] sales;
        public float totalValue;
        public string paymentType;
        public DateTime date;
    }

}