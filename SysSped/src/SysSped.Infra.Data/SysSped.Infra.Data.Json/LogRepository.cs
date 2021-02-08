using Newtonsoft.Json;
using SysSped.Domain.Entities.CorrecaoSped;
using SysSped.Domain.Entities.Relatorio;
using SysSped.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysSped.Infra.Data.Json
{
    public class LogRepository : BaseRepository, ILogRepository
    {
        private readonly string basePath;
        private List<AlteracoesSped> lstAlteracoes = new List<AlteracoesSped>();

        public LogRepository()
        {
            basePath = $@"{path}\Bloco0000";
        }

        public void FinalizaTransacaoAtualizar()
        {
            var json = JsonConvert.SerializeObject(lstAlteracoes);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            var nomeArquivo = $@"{basePath}\{DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss")}.json";

            File.WriteAllText(nomeArquivo, json);
        }

        public List<AlteracoesSped> ObterAlteracoesPorIdBloco0000(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bloco0000> ObterBloco0000Ativos()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bloco0000> ObterBloco0000Ativos(int idBloco0000)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bloco0000> ObterBloco0000Ativos(IEnumerable<int> idBsloco0000)
        {
            throw new NotImplementedException();
        }

        public void RegistrarLog(Bloco0000 bloco0000, Domain.Core.EnumTipoSped tipoBloco, int indiceCampo, string nomeCampo, int indiceLinha, string codigoInterno, string ean, string campoAntigo, string campoNovo)
        {
            var obj = new AlteracoesSped {
                IdBloco0000 = bloco0000.Id,
                NroLinha = (indiceLinha + 1).ToString(),
                IndiceCampo = indiceCampo.ToString(),
                NomeCampo = nomeCampo,
                CodigoInterno = codigoInterno,
                Ean = ean,
                TipoSped = tipoBloco,
                ValorAntigo = campoAntigo,
                ValorAtual = campoNovo,
                DataCadastro = DateTime.Now.ToString()
            };

            lstAlteracoes.Add(obj);
        }

        public Guid RegistrarLogBloco0000(Bloco0000 bloco0000)
        {
            bloco0000.Id = Guid.NewGuid();
            string json = JsonConvert.SerializeObject(bloco0000);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            var nomeArquivo = $@"{basePath}\{DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss")}.json";

            System.IO.File.WriteAllText(nomeArquivo, json);

            return bloco0000.Id;
        }
    }
}
