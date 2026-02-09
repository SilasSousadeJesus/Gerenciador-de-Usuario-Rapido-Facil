using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;
using System.ComponentModel.DataAnnotations;

namespace Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities
{
    public class TemplateHTML
    {
        public TemplateHTML() { }
        [Key]
        public Guid Id { get; set; }   
        public string Corpo { get; set; } = string.Empty;   
        public string Descricao { get; set; } = string.Empty;   
        public ETipoTemplate Tipo { get; set; }   = ETipoTemplate.NaoClassificado;
    }
}
