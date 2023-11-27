using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.Diagnostics.CodeAnalysis;

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
        setup();
        saveStock();
    }

    static void saveStock()
    {
        Product[] sortedProducts = sortProducts();
        Console.WriteLine("--- CADASTRO DE ESTOQUE --- \n");
        showProducts();
        Console.Write("Digite o ID do produto que você deseja cadastrar estoque: ");
        int productId = int.Parse(Console.ReadLine());
        Product product = binarySearchProductById(sortedProducts, productId);
        Console.WriteLine("Digite a quantidade total de produtos em estoque: ");
        int stockInitial = int.Parse(Console.ReadLine());

    }

    static Product binarySearchProductById(Product[] productsSorted, int productId)
    {
        int left = 0;
        int right = productsSorted.Length-1;
        Product product = new Product();
        while (left <= right)
        {
            int middle = (left + right) / 2;
            if (productsSorted[middle].id == productId)
            {
                return productsSorted[middle];
            }
            else if (middle < productsSorted.Length)
            {
                right = middle - 1;
            }
            else
            {
                left = middle + 1;
            }
        }
        return product;
    }

    static void saveProducts()
    {
        Console.WriteLine("Quantos produtos deseja cadastrar? " + "\n Limite de " + (products.Length - countProduct));
        int amountProducts = int.Parse(Console.ReadLine());
        int oldCountProduct = countProduct;
        countProduct += amountProducts;
        Console.Clear();
        for (int i = oldCountProduct; i < countProduct; i++)
        {
            products[i].id = i+1;
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
            Console.WriteLine("Digite o preço do produto (R$): ");
            products[i].price = float.Parse(Console.ReadLine());
            Console.Clear();
        }
        saveProductsBinFile(products, countProduct);
    }

    static void saveProductsBinFile(Product[] productsArray, int total)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream wr = new FileStream("products.bin", FileMode.Create, FileAccess.Write);
        int i;
        for (i = 0; i < total; i++)
        {
            formatter.Serialize(wr, productsArray[i]); 
        }
        saveCountProductsBinFile(ref i);
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

    static Product[] sortProducts()
    {
        Product[] sortProdutcs = new Product[countProduct];
        for(int i=0; i<countProduct; i++)
        {
            sortProdutcs[i] = products[i];
        }
        Array.Sort(sortProdutcs, (p1, p2) => p1.description.CompareTo(p2.description));
        return sortProdutcs;
    }

    static void saveCountProductsBinFile(ref int total)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream wr = new FileStream("countProducts.bin", FileMode.Create, FileAccess.Write);
        formatter.Serialize(wr, total);
    }
    static void readBinAmountProducts()
    {
        IFormatter formatter = new BinaryFormatter();
        if (File.Exists("countProducts.bin"))
        {
            Stream rd = new FileStream("countProducts.bin", FileMode.Open, FileAccess.Read);
            countProduct = (int)formatter.Deserialize(rd);
            rd.Close();
        }
    }
    static void showProducts()
    {
        Product[] productsForShow = sortProducts();
        Array.Clear(products, 0, products.Length);
        readBinProducts();
        Console.WriteLine("--- PRODUTOS CADASTRADOS NO SISTEMA ---\n");
        for (int i = 0; i< productsForShow.Length; i++)
        {
            Console.WriteLine(productsForShow[i].ToString());
        }
        Console.WriteLine("\n Tecle 'enter' para continuar.\n");
        Console.ReadLine();
    }
    static void setup()
    {
        readBinProducts();
        readBinAmountProducts();
    }

    [Serializable]
    struct Product{
        public string batch;
        public string description;
        public DateTime validity;
        public float price;
        public int id;
        public override string ToString() =>
            $"ID: {id} - Lote: {batch} - Descrição: {description} - Validade: {validity} - Preço: R${price}";
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