using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConfimApi.Models
{
    public class Subcategoria
    {
        [Column("ID")]
        [Key]
        public int Id { get; set; }

        [Column("DESCRICAO")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        public string Descricao { get; set; }

        [Column("ID_CATEGORIA")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Categoria inválida")]
        public int IdCategoria { get; set; }
    }
}