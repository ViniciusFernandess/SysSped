using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using OfficeOpenXml;
using SysSped.Domain.Interfaces;
using SysSped.Domain.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SysSped.Apresentation
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly IImportacaoRepository _repoImportacao;
        private readonly ILogRepository _logRepository;
        private readonly IExcelService _servExcel;
        private readonly ITxtService _servSped;
        private string modo;
        public ProgressDialogController Progresso { get; set; }

        public MainWindow(IImportacaoRepository repoImportacao, ILogRepository logRepository, IExcelService servExcel, ITxtService servSped)
        {
            _repoImportacao = repoImportacao;
            _logRepository = logRepository;
            _servExcel = servExcel;
            _servSped = servSped;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            InitializeComponent();
        }

        private void AplicarModoImportarPlanilha(object sender, RoutedEventArgs e)
        {
            DesabilitarTodosBotoes();

            modo = "Planilha";
            btnAtualizar.Visibility = Visibility.Visible;
            btnReImportar.Visibility = Visibility.Visible;
        }

        private void AplicarModoCorrigirSped(object sender, RoutedEventArgs e)
        {
            DesabilitarTodosBotoes();

            modo = "Sped";
            btnCorrigirSped.Visibility = Visibility.Visible;
        }

        private void DesabilitarTodosBotoes()
        {
            btnAtualizar.Visibility = Visibility.Hidden;
            btnReImportar.Visibility = Visibility.Hidden;
            btnCorrigirSped.Visibility = Visibility.Hidden;
            txtFile.Text = "...";
        }

        private void btnEscolherArquivo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (modo == "Sped")
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.txt)|*.txt";
            else
                openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.xlsx)|*.xlsx";

            if (openFileDialog.ShowDialog() == true)
                txtFile.Text = openFileDialog.FileName;

        }

        private void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            //var controller = await this.ShowProgressAsync("Por favor, aguarde.", "Processando...");

            //await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            //{
            var serv = new ImportacaoService(_repoImportacao, _servExcel);

            var pkg = new ExcelPackage(new FileInfo(txtFile.Text));
            serv.AtualizarBase(pkg);

            if (serv.IsValid())
                txtEditor.Text = "Base atualizada com sucesso.";
            else
                txtEditor.Text = string.Join(@"\m", serv.Erros.Select(x => x.Mensagem));
            //}));

            //await controller.CloseAsync();
        }

        private void btnReImportar_Click(object sender, RoutedEventArgs e)
        {
            //var controller = await this.ShowProgressAsync("Por favor, aguarde.", "Processando...");

            //await Task.Factory.StartNew(() =>
            //{
            var serv = new ImportacaoService(_repoImportacao, _servExcel);

            var pkg = new ExcelPackage(new FileInfo(txtFile.Text));
            serv.RenovarBase(pkg);

            if (serv.IsValid())
                txtEditor.Text = "Importado com sucesso.";
            else
                txtEditor.Text = string.Join(@"\m", serv.Erros.Select(x => x.Mensagem));
            //});

            //await controller.CloseAsync();
        }

        private async void btnCorrigirSped_Click(object sender, RoutedEventArgs e)
        {
            //var controller = await this.ShowProgressAsync("Por favor, aguarde.", "Processando...");

            Progresso = await this.ShowProgressAsync("Progresso", "Registrando marcação de ponto do usuário. Aguarde...");
            Progresso.SetIndeterminate();

            await Task.Factory.StartNew(() =>
            {
                var arquivo = new FileInfo(txtFile.Text);

                if (!arquivo.Exists || arquivo.Extension != ".txt")
                    return;

                var serv = new CorrecaoSpedService(_repoImportacao, _logRepository);

                var txtArquivo = ObtemTextoLidoArquivo(txtFile.Text);

                var sped = _servSped.ExecutaLeitura(txtArquivo);
                serv.TratarSped(sped, txtArquivo);

                var arquivoIrginal = new FileInfo(txtFile.Text);
                var caminhoDest = arquivoIrginal.FullName.Replace(arquivoIrginal.Extension, $@"_Corrigido_{DateTime.Now.ToString("dd-MM-yyyy HHmmss")}{arquivoIrginal.Extension}");

                File.WriteAllLines(caminhoDest, txtArquivo);
            });

            await Progresso?.CloseAsync();
            //await this.ShowMessageAsync("Correção de Sped", "Finalizado com sucesso!");

        }


        private string[] ObtemTextoLidoArquivo(string caminhoArquivo)
        {
            return File.ReadAllLines(caminhoArquivo);
        }
    }
}