using Ninject;
using SysSped.Domain.Interfaces;
using SysSped.Infra.CrossCutting.Excel;
using SysSped.Infra.CrossCutting.Txt;
using SysSped.Infra.Data;
using System.Windows;

namespace SysSped.Apresentation
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel container;

        private void ConfigureContainer()
        {
            this.container = new StandardKernel();
            container.Bind<IImportacaoRepository>().To<ImportacaoRepository>();
            container.Bind<IExcelService>().To<ArquivoImportacaoService>().InTransientScope();
            container.Bind<ITxtService>().To<TxtService>().InTransientScope();
        }

        private void ComposeObjects()
        {
            Current.MainWindow = this.container.Get<MainWindow>();
            Current.MainWindow.Title = "DI with Ninject";
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Current.MainWindow.Show();
        }
    }
}
