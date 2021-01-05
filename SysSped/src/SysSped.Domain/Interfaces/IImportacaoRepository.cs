using SysSped.Domain.Entities.Importacao;
using System.Collections.Generic;

namespace SysSped.Domain.Interfaces
{
    public interface IImportacaoRepository
    {
        void Importar(ArquivoImportacao model);
        void InativarImportacaoAtual();
        bool VerificarSeTemImportacaoAtiva();

        string ObterCST_PisProdutoPorEan(string ean);
        string ObterCST_CofinsProdutoPorEan(string ean);
        string ObterAliquotaPisProdutoPorEan(string ean);
        string ObterAliquotaCofinsProdutoPorEan(string ean);

        string ObterCST_PisProdutoPorCod_Item(string codInternoItem);
        string ObterCST_CofinsProdutoPorCod_Item(string codInternoItem);
        string ObterAliquotaPisProdutoPorCod_Item(string codInternoItem);
        string ObterAliquotaCofinsProdutoPorCod_Item(string codInternoItem);

        List<RowImportacao> ObterImportacaoAtiva();
        RowImportacao ObterPorCodItem(string codInternoItem);
        void InativarRow(RowImportacao rowBD);
        void InserirRowAtualizada(RowImportacao rowPlanilha);
    }
}
