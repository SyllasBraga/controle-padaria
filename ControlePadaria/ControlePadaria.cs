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

    static string nameFileStock = "Stock";
    static string nameFileProduct = "Product";
    static string nameFileSales = "Sales";
    static string nameFileCashRegister = "CashResgister";

    static void Main(string[] args)
    {
        setup();
        saveStock();
        showStock();
    }

    static void saveStock()
    {
        Product[] sortedProducts = sortProducts();
        Console.WriteLine("--- CADASTRO DE ESTOQUE --- \n");
        showProducts();
        Console.Write("Digite o ID do produto que você deseja cadastrar estoque: ");
        int oldCountStock = countStock;
        countStock += 1;
        for (int i=oldCountStock; i<countStock; i++)
        {
            int productId = int.Parse(Console.ReadLine());
            productsStock[oldCountStock].product = binarySearchProductById(sortedProducts, productId);
            Console.WriteLine("Digite a quantidade total de produtos em estoque: ");
            productsStock[oldCountStock].stockInitial = int.Parse(Console.ReadLine());
        }

        saveBinFile(productsStock, countStock, nameFileStock);
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
        saveBinFile(products, countProduct, nameFileProduct);
    }

    static void saveBinFile<T>(T[] array, int total, string nameFile)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream wr = new FileStream($"array{nameFile}.bin", FileMode.Create, FileAccess.Write);
        int i;
        for (i = 0; i < total; i++)
        {
            formatter.Serialize(wr, array[i]); 
        }
        wr.Close();
        saveCountBinFile(ref i, nameFile);
    }
    static void readBin(string nameFile)
    {
        IFormatter formatter = new BinaryFormatter();
        if (File.Exists($"array{nameFile}.bin"))
        {
            Stream rd = new FileStream($"array{nameFile}.bin", FileMode.Open, FileAccess.Read);
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

    static void saveCountBinFile(ref int total, string nameFile)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream wr = new FileStream($"count{nameFile}.bin", FileMode.Create, FileAccess.Write);
        formatter.Serialize(wr, total);
    }
    static void readBinCount(string nameFile)
    {
        IFormatter formatter = new BinaryFormatter();
        if (File.Exists($"count{nameFile}.bin"))
        {
            Stream rd = new FileStream($"count{nameFile}.bin", FileMode.Open, FileAccess.Read);
            countProduct = (int)formatter.Deserialize(rd);
            rd.Close();
        }
    }

    static void showProducts()
    {
        Product[] productsForShow = sortProducts();
        Array.Clear(products, 0, products.Length);
        readBin(nameFileProduct);
        Console.WriteLine("--- PRODUTOS CADASTRADOS NO SISTEMA ---\n");
        for (int i = 0; i< productsForShow.Length; i++)
        {
            Console.WriteLine(productsForShow[i].ToString());
        }
        Console.WriteLine("\n Tecle 'enter' para continuar.\n");
        Console.ReadLine();
    }
    static void showStock()
    {
        readBin(nameFileStock);
        Console.WriteLine("--- ESTOQUE CADASTRADO NO SISTEMA ---\n");
        for (int i = 0; i < countStock; i++)
        {
            Console.WriteLine(productsStock[i].ToString());
        }
        Console.WriteLine("\n Tecle 'enter' para continuar.\n");
        Console.ReadLine();
    }
    static void setup()
    {
        readBinCount(nameFileStock);
        readBinCount(nameFileProduct);
        readBin(nameFileProduct);
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