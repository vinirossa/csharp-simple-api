using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConfimApi.Models
{
    public class Categoria
    {
        [Column("ID")]
        [Key]
        public int Id { get; set; }

        [Column("DESCRICAO")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        public string Descricao { get; set; }
    }
}