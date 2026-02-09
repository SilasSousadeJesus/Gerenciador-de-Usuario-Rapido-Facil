using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities
{
    public class MensagemChat
    {
        public MensagemChat()
        {
        }

        public Guid Id { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public DateTime DataEnvio { get; set; } = DateTime.Now;

        public Guid UsuarioId { get; set; }

        [ForeignKey("ChatId")]
        public Guid ChatId { get; set; }
        public virtual Chat Chat { get; set; }
    }
}


//CREATE TABLE Leituras (
//    LeituraId INT PRIMARY KEY IDENTITY,
//    MensagemId INT NOT NULL,
//    UsuarioId INT NOT NULL,
//    DataLeitura DATETIME NOT NULL DEFAULT GETDATE(),
//    FOREIGN KEY (MensagemId) REFERENCES Mensagens(MensagemId),
//    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId),
//    CONSTRAINT UQ_MensagemLeitura UNIQUE (MensagemId, UsuarioId)
//);