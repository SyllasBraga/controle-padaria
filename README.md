# controle-padaria
Programa desenvolvido com C# em linha de comando como projeto final da matéria de Programação II do bacharel em Sistemas de Informação.

### Arrays e variaveis essenciais para operações:
#### Arrays:
- Stock[1000] stocks
- Product[1000] products
- Sale[1000] sales
- CashRegister[1000] cashRegisters
#### Variaveis (serão utilizadas para salvar o ultimo indice gravado, e assim recuperar quando necessário):
- int countStock;
- int countProduct;
- int countSales;
- int countCashRegister;

### Structs:
- Stock: { string batch;
        string description;
        DateTime validity;
        float price };
- Product: { Product product;
        int stockAmount;
        int stockSaled;
        int stockInitial; }
- Sale: { Product product;
        DateTime saleDate;
        int quantity; }
- CashRegister: { Sale[] sales;
        float totalValue;
        string paymentType;
        DateTime date; }
### Menus:
- Estoque: 
	- Registrar estoque para um novo produto
	- Listar todos os produtos em estoque
- Produto:
	- Registrar um novo produto
	- Listar produtos cadastrados
- Venda:
	- Registrar uma nova venda
		- Buscar produto pela descrição
	- Listar todas as vendas
- Fluxo Caixa:
	- Registrar um novo fluxo de caixa
	- Listar todos os fluxos de caixa

### Observações: 
	- O vetor produtos devem ser salvos em um arquivo .bin;
	- Os demais vetores em um arquivo .txt;
	- Ordenar os produtos por ordem alfabetica de acordo com a descrição;
	- Implementar busca binária para buscar produtos no momento da venda.

