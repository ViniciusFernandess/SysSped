using SysSped.Domain.Core;
using System;

namespace SysSped.Domain.Entities.Relatorio
{
    public class AlteracoesSped
    {
        public Guid IdBloco0000 { get; set; }
        public string NroLinha { get; set; }
        public string Bloco { get; set; }
        public string CodigoInterno { get; set; }
        public string Ean { get; set; }
        public string IndiceCampo { get; set; }
        public string NomeCampo { get; set; }
        public string ValorAntigo { get; set; }
        public string ValorAtual { get; set; }
        public string DataCadastro { get; set; }
        public EnumTipoSped TipoSped { get; set; }
    }
}
