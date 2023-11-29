using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.Diagnostics.CodeAnalysis;

class ControlePadaria
{
    static Stock[] productsStock = new Stock[1000];
    static Product[] products = new Product[1000];
    static CashRegister[] cashRegister = new CashRegister[1000];

    static int countStock = 0;
    static int countProduct = 0;
    static int countCashRegister = 0;

    static string nameFileStock = "Stock";
    static string nameFileProduct = "Product";
    static string nameFileCashRegister = "CashResgister";

    static void Main(string[] args)
    {
        setup();
        saveCashResgiter();
    }
    static void showProducts()
    {
        Product[] productsForShow = sortProductsByDescription(products);
        Array.Clear(products, 0, products.Length);
        readBinProduct();
        Console.WriteLine("--- PRODUTOS CADASTRADOS NO SISTEMA ---\n");
        for (int i = 0; i < productsForShow.Length; i++)
        {
            Console.WriteLine(productsForShow[i].ToString());
        }
        Console.WriteLine("\n Tecle 'enter' para continuar.\n");
        Console.ReadLine();
    }

    static void showStock()
    {
        readBinStock();
        Console.WriteLine("--- ESTOQUE CADASTRADO NO SISTEMA ---\n");
        for (int i = 0; i < countStock; i++)
        {
            Console.WriteLine(productsStock[i].ToString());
        }
        Console.WriteLine("\n Tecle 'enter' para continuar.\n");
        Console.ReadLine();
    }
    static void saveStock()
    {
        Product[] sortedProducts = sortProductsByDescription(products);
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
            int stock = int.Parse(Console.ReadLine());
            productsStock[oldCountStock].stockInitial = stock;
            productsStock[oldCountStock].stockAmount = stock;
        }

        saveBinFile(productsStock, countStock, ref nameFileStock);
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
            products[i].id = i + 1;
            Console.WriteLine("Qual é o lote do produto?");
            products[i].batch = Console.ReadLine();
            Console.WriteLine("Insira uma descrição para o produto: ");
            products[i].description = Console.ReadLine();
            Console.WriteLine("Qual é a data de validade do produto?\nDigite no formato AA/MM/YYY.");
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
        saveBinFile(products, countProduct, ref nameFileProduct);
    }

    static void saveCashResgiter()
    {
        Console.WriteLine("--- Rgeistro de Caixa ---\n");
        int oldCountCashRegister = countCashRegister;
        countCashRegister += 1;
        for(int i = oldCountCashRegister; i < countCashRegister; i++)
        {
            float totalValue = 0;
            Product[] sortedProducts = sortProductsById(products);
            Console.Write("Digite o tipo de pagamento: ");
            cashRegister[i].paymentType = Console.ReadLine();
            cashRegister[i].date = DateTime.Now;
            Console.WriteLine("Digite o número de produtos que foram vendidos: ");
            int numberProducts = int.Parse(Console.ReadLine());
     
            Sale[] salesProduct = new Sale[numberProducts];
            for (int j=0; j<salesProduct.Length; j++)
            {
                Console.Clear();
                showProducts();
                Console.Write("Digite o ID do produto que você deseja cadastrar estoque: ");
                int productId = int.Parse(Console.ReadLine());
                salesProduct[j].product = binarySearchProductById(sortedProducts, productId);
                Console.WriteLine("Digite a quantidade que a ser vendida: ");
                salesProduct[j].quantity = int.Parse(Console.ReadLine());
                Console.WriteLine(salesProduct[j].product.ToString());
                totalValue += salesProduct[j].product.price * salesProduct[j].quantity;
            }
            cashRegister[i].totalValue = totalValue;
            Console.WriteLine(cashRegister[i].totalValue);
            cashRegister[i].sales = salesProduct;
            Console.WriteLine(cashRegister[i].ToString());
            saveTxtFile(nameFileCashRegister, cashRegister[i].ToString());
        }
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
            else if (productsSorted[middle].id > productId)
            {
                right = middle - 1;
            }
            else
            {
                left = middle + 1;
            }
        }
        product.description = "Not found";
        return product;
    }

    static Product[] sortProductsByDescription(Product[] productsSorted)
    {
        Product[] sortProdutcs = new Product[countProduct];
        for (int i = 0; i < countProduct; i++)
        {
            sortProdutcs[i] = productsSorted[i];
        }
        Array.Sort(sortProdutcs, (p1, p2) => p1.description.CompareTo(p2.description));
        return sortProdutcs;
    }

    static Product[] sortProductsById(Product[] productsSorted)
    {
        Product[] sortProdutcs = new Product[countProduct];
        for (int i = 0; i < countProduct; i++)
        {
            sortProdutcs[i] = productsSorted[i];
        }
        Array.Sort(sortProdutcs, (p1, p2) => p1.id.CompareTo(p2.id));
        return sortProdutcs;
    }

    static void saveCountBinFile(ref int total, ref string nameFile)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream wr = new FileStream($"count{nameFile}.bin", FileMode.Create, FileAccess.Write);
        formatter.Serialize(wr, total);
    }

    static void saveBinFile<T>(T[] array, int total, ref string nameFile)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream wr = new FileStream($"array{nameFile}.bin", FileMode.Create, FileAccess.Write);
        int i;
        for (i = 0; i < total; i++)
        {
            formatter.Serialize(wr, array[i]); 
        }
        wr.Close();
        saveCountBinFile(ref i, ref nameFile);
    }
    static void readBinProduct()
    {
        IFormatter formatter = new BinaryFormatter();
        if (File.Exists($"array{nameFileProduct}.bin"))
        {
            Stream rd = new FileStream($"array{nameFileProduct}.bin", FileMode.Open, FileAccess.Read);
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

    static void readBinStock()
    {
        IFormatter formatter = new BinaryFormatter();
        if (File.Exists($"array{nameFileStock}.bin"))
        {
            Stream rd = new FileStream($"array{nameFileStock}.bin", FileMode.Open, FileAccess.Read);
            int i = 0;
            while (rd.Position < rd.Length)
            {
                Stock stock = (Stock)formatter.Deserialize(rd);
                productsStock[i] = stock;
                i++;
            }
            rd.Close();
        }
    }

    static int readBinCount(string nameFile)
    {
        IFormatter formatter = new BinaryFormatter();
        if (File.Exists($"count{nameFile}.bin"))
        {
            Stream rd = new FileStream($"count{nameFile}.bin", FileMode.Open, FileAccess.Read);
            int count = (int)formatter.Deserialize(rd);
            rd.Close();
            return count;
        }
        return 0;
    }

    static void saveTxtFile(string nameFile, string content)
    {
        StreamWriter wr = new StreamWriter($"{nameFile}.txt", true);
        wr.WriteLine(content);
        wr.Close();
    }

    static void setup()
    {
        countProduct = readBinCount(nameFileProduct);
        countStock = readBinCount(nameFileStock);
        readBinProduct();
        readBinStock();
    }

    [Serializable]
    struct Product : IComparable<Product>
    {
        public string batch;
        public string description;
        public DateTime validity;
        public float price;
        public int id;
        public int CompareTo(Product other)
        {
            return id.CompareTo(other.id);
        }
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
        public override string ToString() =>
            $"Produto: {product.description} - Estoque atual: {stockAmount} - Estoque vendido: {stockSaled} - Estoque inicial: {stockInitial}";
    }

    [Serializable]
    struct Sale 
    {
        public Product product;
        public int quantity;
        public override string ToString() =>
            $"\n  - Produto: {product.description} - Quantidade: {quantity}";
    }

    [Serializable]
    struct CashRegister
    {
        public Sale[] sales;
        public float totalValue;
        public string paymentType;
        public DateTime date;
        public override string ToString()
        {
            string saleString = "";
            for(int i = 0; i < sales.Length; i++)
            {
                saleString+= sales[i].ToString();
            }
            return $"Data da venda: {date} - Tipo de pagamento: {paymentType} - Valor total: R${totalValue} \nLista de produtos: {saleString}";
        }
    }
}