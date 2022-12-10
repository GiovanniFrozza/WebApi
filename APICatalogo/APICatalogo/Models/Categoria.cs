using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models;

[Table("Categorias")]
public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }


    [Key]
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(80, ErrorMessage = "O nome deve ter entre {1} e {2} caracteres", MinimumLength = 5)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300, ErrorMessage = "A imagem deve ter entre {2} e {1} caracteres", MinimumLength = 10)]
    public string? ImagemUrl{ get; set; }

    public ICollection<Produto>? Produtos { get; set; }
}
