using Dapper;
using Dapper.Contrib.Extensions;
using SysSped.Domain.Entities.Importacao;
using SysSped.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SysSped.Infra.Data
{
    public class ImportacaoRepository : BaseRepository, IImportacaoRepository
    {
        public ImportacaoRepository() : base()
        {

        }

        public void Importar(ArquivoImportacao model)
        {
            foreach (var row in model.Rows)
            {
                _conn.Insert(row);
            }
        }

        public void InativarImportacaoAtual()
        {
            var sql = @"UPDATE `sysspeddb`.`rowimportacao` SET ativo = 0 WHERE ativo = 1";

            var retorno = _conn.Query(sql).ToList();
        }

        public string ObterAliquotaCofinsProdutoPorCod_Item(string cod_item)
        {
            var sql = $@"SELECT cofins_aliquota_entrada FROM `sysspeddb`.`rowimportacao` WHERE codigoInterno = {cod_item}";

            var retorno = _conn.Query<string>(sql).FirstOrDefault();

            return retorno;
        }

        public string ObterAliquotaPisProdutoPorCod_Item(string cod_item)
        {
            var sql = $@"SELECT pis_aliquota_entrada FROM `sysspeddb`.`rowimportacao` WHERE codigoInterno = {cod_item}";

            var retorno = _conn.Query<string>(sql).FirstOrDefault();

            return retorno;
        }

        public string ObterCST_CofinsProdutoPorCod_Item(string cod_item)
        {
            var sql = $@"SELECT cofins_cst_entrada FROM `sysspeddb`.`rowimportacao` WHERE codigoInterno = {cod_item} AND Ativo = 1";

            var retorno = _conn.Query<string>(sql).FirstOrDefault();

            return retorno;
        }

        public string ObterCST_PisProdutoPorCod_Item(string cod_item)
        {
            var sql = $@"SELECT pis_cst_entrada FROM `sysspeddb`.`rowimportacao` WHERE codigoInterno = {cod_item} AND Ativo = 1";

            var retorno = _conn.Query<string>(sql).FirstOrDefault();

            return retorno;
        }

        public bool VerificarSeTemImportacaoAtiva()
        {
            var sql = @"SELECT * FROM `sysspeddb`.`rowimportacao` WHERE ativo = 1";

            var retorno = _conn.Query<RowImportacao>(sql).ToList();

            return retorno.Any();
        }

        public List<RowImportacao> ObterImportacaoAtiva()
        {
            var sql = @"SELECT * FROM `sysspeddb`.`rowimportacao` WHERE ativo = 1";

            var retorno = _conn.Query<RowImportacao>(sql).ToList();

            return retorno;
        }

        public RowImportacao ObterPorCodItem(string codInternoItem)
        {
            var sql = $@"SELECT * FROM `sysspeddb`.`rowimportacao` WHERE ativo = 1 AND codigointerno = {codInternoItem}";

            var retorno = _conn.Query<RowImportacao>(sql).FirstOrDefault();

            return retorno;
        }

        public void InativarRow(RowImportacao rowBD)
        {
            rowBD.ativo = "0";
            _conn.Update(rowBD);
        }

        public void InserirRowAtualizada(RowImportacao rowPlanilha)
        {
            _conn.Insert(rowPlanilha);
        }

        public string ObterCST_PisProdutoPorEan(string ean)
        {
            var sql = $@"SELECT pis_cst_entrada FROM `sysspeddb`.`rowimportacao` WHERE ean = {ean} AND Ativo = 1";

            var retorno = _conn.Query<string>(sql).FirstOrDefault();

            return retorno;
        }

        public string ObterCST_CofinsProdutoPorEan(string ean)
        {
            var sql = $@"SELECT cofins_cst_entrada FROM `sysspeddb`.`rowimportacao` WHERE ean = {ean} AND Ativo = 1";

            var retorno = _conn.Query<string>(sql).FirstOrDefault();

            return retorno;
        }

        public string ObterAliquotaPisProdutoPorEan(string ean)
        {
            var sql = $@"SELECT pis_aliquota_entrada FROM `sysspeddb`.`rowimportacao` WHERE ean = {ean}";

            var retorno = _conn.Query<string>(sql).FirstOrDefault();

            return retorno;
        }

        public string ObterAliquotaCofinsProdutoPorEan(string ean)
        {
            var sql = $@"SELECT cofins_aliquota_entrada FROM `sysspeddb`.`rowimportacao` WHERE ean = {ean}";

            var retorno = _conn.Query<string>(sql).FirstOrDefault();

            return retorno;
        }

        public void FinalizaTransacaoAtualizar()
        {
            throw new System.NotImplementedException();
        }
    }
}
