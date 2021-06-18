using SysSped.Domain.Core;
using SysSped.Domain.Entities.CorrecaoSped;
using System;
using System.Collections.Generic;
using System.Text;

namespace SysSped.Domain.Interfaces
{
    public interface ICorrecaoSpedService
    {

        bool TratarSped(Sped sped, string[] linhasArquivoOriginal);
        
        void AplicaRegraProdNaoEncontrado(C170 bloco170Arquivo, EnumTipoNota tipoNota);

        void CorrigirFormatacaoC170Sped(Sped sped, string[] linhasArquivoOriginal);

        void RecalcularBlocoC100(C100 bloco100);

        void AplicaRegraDosCfop(C170 bloco170);

        bool VerificaSeCfopEhDaRegra(string cfop);

        bool CorrigirArquivoSped(Sped sped, string[] linhasArquivoOriginal);

        List<string> CorrigirCampos(C170 c170, List<string> campos);

        List<string> CorrigirFormatacaoCampos(List<string> campos);

        List<string> CorrigirCampos(C100 c100, List<string> campos);

        void CorrigirCampoRegistrarLog(Bloco0000 bloco0000, EnumTipoSped tipoBloco, List<string> campos, int indiceArquivo, int indiceCampoOriginal, string codigoInterno, string ean, string campoTratado, string nomeCampo);

        List<string> ObterCampoDaLinha(int indiceArquivo, string[] linhasArquivoOriginal);

        void AplicaCSTCofinsTratado(C170 bloco170Arquivo);

        string ObtercstCofinsTratadoProduto(C170 bloco170Arquivo);

        string ObterAliquotaCofinsProdutoPlanilha(C170 bloco170Arquivo);

        void AplicaCSTPisTratado(C170 bloco170Arquivo);

        string ObterAliquotaPisProdutoPlanilha(C170 bloco170Arquivo);

        string ObtercstPisTratadoProduto(C170 bloco170Arquivo);

        string CalcularVL_PIS_COFINS(string _VL_BC_PISCofins, string _AliquotaPisCofinsProdutoPlanilha);

        string CalcularBC(string _QTD, string _VL_ITEM);

    }
}
