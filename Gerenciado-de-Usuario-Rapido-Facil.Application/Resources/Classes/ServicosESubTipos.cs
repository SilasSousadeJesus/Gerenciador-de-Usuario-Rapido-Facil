using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Resources.Classes
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
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Ronda Noturna" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Monitoramento por Câmeras" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Controle de Acesso" }
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
                },

                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Manutenção de Ar Condicionado",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Limpeza de Filtros" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Carga de Gás" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Higienização" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Reparo Técnico" }
                    }
                },

                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Sistema de Combate a Incêndio",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Recarga de Extintores" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Teste de Hidrantes" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Inspeção de Alarmes" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Plano de Emergência" }
                    }
                },

                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Manutenção de CFTV",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Instalação de Câmeras" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Manutenção de DVR/NVR" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Configuração de Acesso Remoto" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Troca de Equipamentos" }
                    }
                },

                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Controle de Acesso Eletrônico",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Instalação de Biometria" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Cartões RFID" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Manutenção de Cancelas" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Interfones" }
                    }
                },

                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Manutenção de Portões Automáticos",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Troca de Motores" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Ajuste de Sensores" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Lubrificação" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Reparo de Trilhos" }
                    }
                },

                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Manutenção Predial",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Inspeção Estrutural" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Reparos em Fachada" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Impermeabilização" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Manutenção de Telhado" }
                    }
                },

                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Limpeza de Caixa d'Água",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Higienização" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Desinfecção" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Inspeção Sanitária" }
                    }
                },

                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Manutenção de Geradores",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Teste de Carga" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Troca de Óleo" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Reparo Elétrico" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Revisão Preventiva" }
                    }
                },

                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Facilities",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Zeladoria" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Recepção" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Apoio Operacional" }
                    }
                },

                new Servico
                {
                    Id = Guid.NewGuid(),
                    Nome = "Paisagismo",
                    ServicoSubtipos = new List<ServicoSubtipo>
                    {
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Projeto de Jardim" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Reforma de Áreas Verdes" },
                        new ServicoSubtipo { Id = Guid.NewGuid(), Nome = "Jardim Vertical" }
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
