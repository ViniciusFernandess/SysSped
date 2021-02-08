using Newtonsoft.Json;
using SysSped.Domain.Entities.Importacao;
using SysSped.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SysSped.Infra.Data.Json
{
    public class ImportacaoRepository : BaseRepository, IImportacaoRepository
    {
        private List<RowImportacao> rowsImportacaoAtivos;
        private readonly string basePath;
        public ImportacaoRepository()
        {
            basePath = $@"{path}\ArquivoImportacao";
            ObterRowsAtivas();
        }


        private void ObterRowsAtivas()
        {
            var files = Directory.EnumerateFiles(basePath).ToList();

            var listaDatas = new Dictionary<DateTime, string>();

            foreach (var file in files)
            {
                var nomeArquivo = file.Split('\\').LastOrDefault();

                var campos = nomeArquivo.Replace(" ", @"-").Replace(".json", "").Split('-');

                var dia = campos[0];
                var mes = campos[1];
                var ano = campos[2];
                var hora = campos[3];
                var min = campos[4];
                var seg = campos[5];

                var strData = $@"{ano}/{mes}/{dia} {hora}:{min}:{seg}";

                var sucess = DateTime.TryParse(strData, out var data);
                if (sucess)
                    listaDatas.Add(data, file);
            }

            var primeiro = listaDatas.OrderBy(x => x.Key).FirstOrDefault().Value;

            if (File.Exists(primeiro))
            {
                var file = File.ReadAllText(primeiro);
                rowsImportacaoAtivos = JsonConvert.DeserializeObject<List<RowImportacao>>(file);
            }
        }



        public void FinalizaTransacaoAtualizar()
        {
            string json = JsonConvert.SerializeObject(rowsImportacaoAtivos);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            var nomeArquivo = $@"{basePath}\{DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss")}.json";

            System.IO.File.WriteAllText(nomeArquivo, json);
        }

        public void Importar(ArquivoImportacao model)
        {
            foreach (var row in model.Rows)
            {
                row.id = Guid.NewGuid();
            }

            string json = JsonConvert.SerializeObject(model.Rows);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            var nomeArquivo = $@"{basePath}\{DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss")}.json";

            System.IO.File.WriteAllText(nomeArquivo, json);
        }

        public void InativarImportacaoAtual()
        {
            //throw new NotImplementedException();
        }

        public void InativarRow(RowImportacao rowBD)
        {
            //throw new NotImplementedException();
        }

        public void InserirRowAtualizada(RowImportacao rowPlanilha)
        {
            var index = rowsImportacaoAtivos.FindIndex(x => x.codigointerno == rowPlanilha.codigointerno);
            rowsImportacaoAtivos.RemoveAt(index);

            rowPlanilha.id = Guid.NewGuid();
            rowsImportacaoAtivos.Insert(index, rowPlanilha);
        }

        public string ObterAliquotaCofinsProdutoPorCod_Item(string codInternoItem)
        {
            var obj = rowsImportacaoAtivos.FirstOrDefault(x => x.codigointerno == codInternoItem);
            return obj?.cofins_aliquota_entrada ?? "";
        }

        public string ObterAliquotaCofinsProdutoPorEan(string ean)
        {
            var obj = rowsImportacaoAtivos.FirstOrDefault(x => x.ean == ean);
            return obj?.cofins_aliquota_entrada ?? "";
        }

        public string ObterAliquotaPisProdutoPorCod_Item(string codInternoItem)
        {
            var obj = rowsImportacaoAtivos.FirstOrDefault(x => x.codigointerno == codInternoItem);
            return obj?.pis_aliquota_entrada ?? "";
        }

        public string ObterAliquotaPisProdutoPorEan(string ean)
        {
            var obj = rowsImportacaoAtivos.FirstOrDefault(x => x.ean == ean);
            return obj?.pis_aliquota_entrada ?? "";
        }

        public string ObterCST_CofinsProdutoPorCod_Item(string codInternoItem)
        {
            var obj = rowsImportacaoAtivos.FirstOrDefault(x => x.codigointerno == codInternoItem);
            return obj?.cofins_cst_entrada ?? "";
        }

        public string ObterCST_CofinsProdutoPorEan(string ean)
        {
            var obj = rowsImportacaoAtivos.FirstOrDefault(x => x.ean == ean);
            return obj?.cofins_cst_entrada ?? "";
        }

        public string ObterCST_PisProdutoPorCod_Item(string codInternoItem)
        {
            var obj = rowsImportacaoAtivos.FirstOrDefault(x => x.codigointerno == codInternoItem);
            return obj?.pis_cst_entrada ?? "";
        }

        public string ObterCST_PisProdutoPorEan(string ean)
        {
            var obj = rowsImportacaoAtivos.FirstOrDefault(x => x.ean == ean);
            return obj?.pis_cst_entrada ?? "";
        }

        public List<RowImportacao> ObterImportacaoAtiva()
        {
            throw new NotImplementedException();
        }

        public RowImportacao ObterPorCodItem(string codInternoItem)
        {
            var objObtido = rowsImportacaoAtivos.FirstOrDefault(x => x.codigointerno == codInternoItem);
            return objObtido;
        }

        public bool VerificarSeTemImportacaoAtiva()
        {
            return rowsImportacaoAtivos != null && rowsImportacaoAtivos.Any();
        }
    }
}
