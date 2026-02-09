using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.Resources.Classes
{
    public class ServicosESubTipos
    {
        public List<Servico> ObterServicos()
        {
            var listaServico = new List<Servico>
            {
                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Manutenção de Elevadores",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Inspeção Periódica" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Reparo de Cabos" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Substituição de Peças" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Emergência 24h" }
                    }
                },
                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Limpeza de Áreas Comuns",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Limpeza Diária" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Limpeza Semanal" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Limpeza Pós-Evento" }
                    }
                },
                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Segurança Patrimonial",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo {Id = Guid.NewGuid(), Nome = "Ronda Noturna" },
                        new ServicoSubtipo {Id = Guid.NewGuid(), Nome = "Monitoramento por Câmeras" },
                        new ServicoSubtipo {Id = Guid.NewGuid(), Nome = "Controle de Acesso" }
                    }
                },
                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Manutenção de Jardim",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Poda de Árvores" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Corte de Grama" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Adubação" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Irrigação" }
                    }
                },
                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Gestão de Resíduos",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Coleta Seletiva" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Descarte de Resíduos Perigosos" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Reciclagem" }
                    }
                },
                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Controle de Pragas",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Dedetização" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Desratização" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Controle de Insetos" }
                    }
                },
                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Manutenção de Piscina",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Limpeza da Piscina" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Tratamento de Água" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Reparos Estruturais" }
                    }
                },
                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Administração de Condomínio",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Gestão Financeira" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Assembleias de Condomínio" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Gestão de Contratos" }
                    }
                },
                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Serviços de Portaria",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Controle de Entrada e Saída" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Recebimento de Encomendas" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Atendimento a Moradores" }
                    }
                },
                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Manutenção Elétrica",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Troca de Lâmpadas" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Reparos em Fiação" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Instalação de Equipamentos" }
                    }
                },
                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Manutenção Hidráulica",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Desentupimentos" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Reparo de Vazamentos" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Instalação de Torneiras" }
                    }
                },
                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Reparos Gerais",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Pequenos Reparos" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Pintura" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Troca de Vidros" }
                    }
                }
            };

            foreach (var servico in listaServico)
            {
                if (!servico.ServicoSubtipos.Any()) continue;

                foreach (var subServico in servico.ServicoSubtipos)
                {
                    subServico.ServicoId = servico.Id;
                }
            }

            return listaServico;
        }
    }
}
