namespace Gerenciado_de_Usuario_Papido_Facil.Domain.Entities
{
    public class Estado
    {
        public int Id { get; set; }
        public string Sigla { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        //public Regiao Regiao { get; set; }
    }

    public class Regiao
    {
        public int Id { get; set; }
        public string Sigla { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
    }

    public class UF
    {
        public int Id { get; set; }
        public string Sigla { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public Regiao Regiao { get; set; }
    }

    public class Mesorregiao
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public UF UF { get; set; }
    }

    public class Microrregiao
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public Mesorregiao Mesorregiao { get; set; }
    }


    public class Municipio
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        //public Microrregiao Microrregiao { get; set; }
    }

}
