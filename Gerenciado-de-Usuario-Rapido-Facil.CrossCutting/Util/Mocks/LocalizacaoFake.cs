namespace Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Mocks
{
    public static class LocalizacoesMock
    {
        public static readonly List<LocalizacaoFake> LocalizacoesFake = new()
        {
            new() { Cidade = "Salvador", Estado = "BA", Cep = "40000-000" },
            new() { Cidade = "Feira de Santana", Estado = "BA", Cep = "44000-000" },
            new() { Cidade = "São Paulo", Estado = "SP", Cep = "01000-000" },
            new() { Cidade = "Campinas", Estado = "SP", Cep = "13000-000" },
            new() { Cidade = "Rio de Janeiro", Estado = "RJ", Cep = "20000-000" },
            new() { Cidade = "Niterói", Estado = "RJ", Cep = "24000-000" },
            new() { Cidade = "Belo Horizonte", Estado = "MG", Cep = "30000-000" },
            new() { Cidade = "Uberlândia", Estado = "MG", Cep = "38400-000" },
            new() { Cidade = "Curitiba", Estado = "PR", Cep = "80000-000" },
            new() { Cidade = "Londrina", Estado = "PR", Cep = "86000-000" },
            new() { Cidade = "Porto Alegre", Estado = "RS", Cep = "90000-000" },
            new() { Cidade = "Florianópolis", Estado = "SC", Cep = "88000-000" },
            new() { Cidade = "Recife", Estado = "PE", Cep = "50000-000" },
            new() { Cidade = "Fortaleza", Estado = "CE", Cep = "60000-000" },
            new() { Cidade = "Goiânia", Estado = "GO", Cep = "74000-000" },
            new() { Cidade = "Brasília", Estado = "DF", Cep = "70000-000" }
        };
    }

    public class LocalizacaoFake
    {
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
    }
}