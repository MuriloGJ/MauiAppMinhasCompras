using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao;
        string _categoria;
        double _quantidade;
        double _preco;
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao
        {
            get => _descricao;
            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, preencha a Descrição");
                }
                _descricao = value;
            }
        }
        public string Categoria
        {
            get => _categoria;
            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, preencha a Categoria");
                }
                _categoria = value;
            }
        }

        public double Quantidade
        {
            get => _quantidade;
            set
            {
                if (value < 0)
                {
                    throw new Exception("Quantidade não pode ser negativa");
                }
                _quantidade = value;
            }
        }

        public double Preco
        {
            get => _preco;
            set
            {
                if (value < 0)
                {
                    throw new Exception("Preço não pode ser negativo");
                }
                _preco = value;
            }
        }

        public double Total { get => Quantidade * Preco; }

    }
    public class TotalCategoria
    {
        public string Categoria { get; set; }
        public double Totalc { get; set; }
    }
}
