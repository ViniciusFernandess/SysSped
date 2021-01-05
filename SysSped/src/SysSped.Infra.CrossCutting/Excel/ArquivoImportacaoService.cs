using OfficeOpenXml;
using SysSped.Domain.Core;
using SysSped.Domain.Entities.Importacao;
using SysSped.Domain.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SysSped.Infra.CrossCutting.Excel
{
    public class ArquivoImportacaoService : Validation, IExcelService
    {
        #region PROPRIEDADES
        public readonly IReadOnlyCollection<string> CamposPadroes = new List<string> {
            "codigointerno",
            "Descrição Cliente",
            "ean",
            "ncm",
            "ncm_ex",
            "cest",
            "Nome do Cliente",
            "icms_cst_entrada",
            "icms_cst_saida",
            "icms_aliquota_interna",
            "icms_aliquota_interna_saida",
            "icms_aliquota_efetiva_entrada",
            "icms_aliquota_efetiva_saida",
            "icms_aliquota_interestadual",
            "icms_aliquota_interestadual_saida",
            "icms_reducao_base_calculo",
            "icms_reducao_base_calculo_saida",
            "cfop_dentro_estado_entrada",
            "cfop_dentro_estado_saida",
            "cfop_fora_estado_entrada",
            "cfop_fora_estado_saida",
            "mva_original_atacado",
            "mva_original_industria",
            "mva_original_recalculada",
            "mva_ajustada_interestadual_4",
            "mva_ajustada_interestadual_12",
            "mva_ajustada_interestadual_recalculada",
            "desc_icms",
            "codigo",
            "descricao",
            "dt_inicio",
            "dt_fim",
            "legislacao",
            "pis_cst_entrada",
            "pis_cst_saida",
            "pis_aliquota_entrada",
            "pis_aliquota_saida",
            "pis_natureza_receita",
            "cofins_cst_entrada",
            "cofins_cst_saida",
            "cofins_aliquota_entrada",
            "cofins_aliquota_saida",
            "cofins_natureza_receita",
            "ipi_cst_entrada",
            "ipi_cst_saida",
            "ipi_aliquota",

        };
        public FileInfo Arquivo { get; private set; }

        #endregion

        public ArquivoImportacaoService()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public ArquivoImportacao ExecutaLeitura(ExcelPackage pkg)
        {
            var sheet = ObterArquivoExcel(pkg);
            if (!IsValid()) return null;

            var camposArquivo = ObterTodosCamposCabecalho(sheet);

            foreach (var campo in camposArquivo)
                VerificaSeEhCampoPadrao(campo);
            if (!IsValid()) return null;

            VerificaSeTemTodosOsCamposPadroes(camposArquivo);
            if (!IsValid()) return null;

            var arquivoMapeado = ExecutaMapeamentoCampos(sheet);

            arquivoMapeado.Rows = arquivoMapeado.Rows.DistinctBy(x => x.codigointerno).ToList();
            return arquivoMapeado;
        }

        private ArquivoImportacao ExecutaMapeamentoCampos(ExcelWorksheet sheet)
        {
            var arquivoImportacao = new ArquivoImportacao();

            var start = sheet.Dimension.Start;
            var end = sheet.Dimension.End;

            for (int row = start.Row + 1; row <= end.Row; row++)
            { // Row by row...
                var rowImportacao = new RowImportacao();

                PrencheRow(sheet, row, start, end, rowImportacao);
                arquivoImportacao.Rows.Add(rowImportacao);
            }

            return arquivoImportacao;
        }

        private void PrencheRow(ExcelWorksheet sheet, int row, ExcelCellAddress start, ExcelCellAddress end, RowImportacao rowImportacao)
        {
            rowImportacao.rowAdress = row.ToString();

            for (int col = start.Column; col <= end.Column; col++)
            { // ... Cell by cell...
                PreenchePropriedade(sheet, row, col, rowImportacao);
            }
        }

        private void PreenchePropriedade(ExcelWorksheet sheet, int row, int col, RowImportacao rowObject)
        {
            var nomeColuna = sheet.Cells[1, col].Text;
            var valorColuna = sheet.Cells[row, col].Text;

            switch (nomeColuna)
            {
                case "codigointerno":
                    rowObject.codigointerno = valorColuna;
                    break;
                case "Descrição Cliente":
                    rowObject.DescricaoCliente = valorColuna;
                    break;
                case "ean":
                    rowObject.ean = valorColuna;
                    break;
                case "ncm":
                    rowObject.ncm = valorColuna;
                    break;
                case "ncm_ex":
                    rowObject.ncm_ex = valorColuna;
                    break;
                case "cest":
                    rowObject.cest = valorColuna;
                    break;
                case "Nome do Cliente":
                    rowObject.NomedoCliente = valorColuna;
                    break;
                case "icms_cst_entrada":
                    rowObject.icms_cst_entrada = valorColuna;
                    break;
                case "icms_cst_saida":
                    rowObject.icms_cst_saida = valorColuna;
                    break;
                case "icms_aliquota_interna":
                    rowObject.icms_aliquota_interna = valorColuna;
                    break;
                case "icms_aliquota_interna_saida":
                    rowObject.icms_aliquota_interna_saida = valorColuna;
                    break;
                case "icms_aliquota_efetiva_entrada":
                    rowObject.icms_aliquota_efetiva_entrada = valorColuna;
                    break;
                case "icms_aliquota_efetiva_saida":
                    rowObject.icms_aliquota_efetiva_saida = valorColuna;
                    break;
                case "icms_aliquota_interestadual":
                    rowObject.icms_aliquota_interestadual = valorColuna;
                    break;
                case "icms_aliquota_interestadual_saida":
                    rowObject.icms_aliquota_interestadual_saida = valorColuna;
                    break;
                case "icms_reducao_base_calculo":
                    rowObject.icms_reducao_base_calculo = valorColuna;
                    break;
                case "icms_reducao_base_calculo_saida":
                    rowObject.icms_reducao_base_calculo_saida = valorColuna;
                    break;
                case "cfop_dentro_estado_entrada":
                    rowObject.cfop_dentro_estado_entrada = valorColuna;
                    break;
                case "cfop_dentro_estado_saida":
                    rowObject.cfop_dentro_estado_saida = valorColuna;
                    break;
                case "cfop_fora_estado_entrada":
                    rowObject.cfop_fora_estado_entrada = valorColuna;
                    break;
                case "cfop_fora_estado_saida":
                    rowObject.cfop_fora_estado_saida = valorColuna;
                    break;
                case "mva_original_atacado":
                    rowObject.mva_original_atacado = valorColuna;
                    break;
                case "mva_original_industria":
                    rowObject.mva_original_industria = valorColuna;
                    break;
                case "mva_original_recalculada":
                    rowObject.mva_original_recalculada = valorColuna;
                    break;
                case "mva_ajustada_interestadual_4":
                    rowObject.mva_ajustada_interestadual_4 = valorColuna;
                    break;
                case "mva_ajustada_interestadual_12":
                    rowObject.mva_ajustada_interestadual_12 = valorColuna;
                    break;
                case "mva_ajustada_interestadual_recalculada":
                    rowObject.mva_ajustada_interestadual_recalculada = valorColuna;
                    break;
                case "desc_icms":
                    rowObject.desc_icms = valorColuna;
                    break;
                case "codigo":
                    rowObject.codigo = valorColuna;
                    break;
                case "descricao":
                    rowObject.descricao = valorColuna;
                    break;
                case "dt_inicio":
                    rowObject.dt_inicio = valorColuna;
                    break;
                case "dt_fim":
                    rowObject.dt_fim = valorColuna;
                    break;
                case "legislacao":
                    rowObject.legislacao = valorColuna;
                    break;
                case "pis_cst_entrada":
                    rowObject.pis_cst_entrada = valorColuna;
                    break;
                case "pis_cst_saida":
                    rowObject.pis_cst_saida = valorColuna;
                    break;
                case "pis_aliquota_entrada":
                    rowObject.pis_aliquota_entrada = valorColuna;
                    break;
                case "pis_aliquota_saida":
                    rowObject.pis_aliquota_saida = valorColuna;
                    break;
                case "pis_natureza_receita":
                    rowObject.pis_natureza_receita = valorColuna;
                    break;
                case "cofins_cst_entrada":
                    rowObject.cofins_cst_entrada = valorColuna;
                    break;
                case "cofins_cst_saida":
                    rowObject.cofins_cst_saida = valorColuna;
                    break;
                case "cofins_aliquota_entrada":
                    rowObject.cofins_aliquota_entrada = valorColuna;
                    break;
                case "cofins_aliquota_saida":
                    rowObject.cofins_aliquota_saida = valorColuna;
                    break;
                case "cofins_natureza_receita":
                    rowObject.cofins_natureza_receita = valorColuna;
                    break;
                case "ipi_cst_entrada":
                    rowObject.ipi_cst_entrada = valorColuna;
                    break;
                case "ipi_cst_saida":
                    rowObject.ipi_cst_saida = valorColuna;
                    break;
                case "ipi_aliquota":
                    rowObject.ipi_aliquota = valorColuna;
                    break;
                default:
                    Erros.Add(new RequestResult(Resource.OBJETO_NAO_TEM_PROPRIEDADE_COM_NOME_DA_COLUNA));
                    break;
            }
        }

        private ExcelWorksheet ObterArquivoExcel(ExcelPackage package)
        {
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //var package = new ExcelPackage(Arquivo);
            var workbook = package.Workbook;

            var naoTemSheet = workbook.Worksheets.Count == 0;
            if (naoTemSheet)
            {
                Erros.Add(new RequestResult(Resource.ARQUIVO_EXCEL_NULO));
                return null;
            }

            var sheet = workbook.Worksheets[0];
            return sheet;
        }

        private bool VerificaSeEhCampoPadrao(string nomeCampo)
        {
            var ehCampoValido = CamposPadroes.Contains(nomeCampo);

            if (!ehCampoValido)
                Erros.Add(new RequestResult(Resource.CAMPO_INVALIDO_NO_ARQUIVO));

            return CamposPadroes.Contains(nomeCampo);
        }

        private List<string> ObterTodosCamposCabecalho(ExcelWorksheet sheet)
        {
            var start = sheet.Dimension.Start;
            var end = sheet.Dimension.End;

            var camposCabecalho = new List<string>();

            for (int col = start.Column; col <= end.Column; col++)
            { // ... Cell by cell...
                var cellValue = sheet.Cells[1, col].Text; // This got me the actual value I needed.

                var temNaLista = camposCabecalho.Any(c => c.ToLower() == cellValue.ToLower());

                if (!string.IsNullOrEmpty(cellValue) && !temNaLista)
                    camposCabecalho.Add(cellValue);
            }

            return camposCabecalho;
        }

        private bool VerificaSeTemTodosOsCamposPadroes(List<string> camposArquivo)
        {
            var arquivoTemTodosCamposPadrao = CamposPadroes.All(x => camposArquivo.Contains(x));

            if (!arquivoTemTodosCamposPadrao)
                Erros.Add(new RequestResult(Resource.ARQUIVO_ESTA_FALTANDO_CAMPO_PADRAO));

            return arquivoTemTodosCamposPadrao;
        }

    }
}
