using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

using Framework.Core;
using Framework.Core.Serialization;
using Framework.ExpressionParsers;

using Anon.RQ_Calc.DataBase;
using Anon.RQ_Calc.DataBase.EntityFramework;
using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;
using Anon.RQ_Calc.WPF;
using Framework.DataBase;

namespace Rq_Calc_WPF
{
    public partial class App
    {
        private IApplicationContext _context;

        
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception is ClientException)
            {
                MessageBox.Show("ClientException: " + e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning); 
            }
            else
            {
                MessageBox.Show("UnhandledException: " + e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            e.Handled = true;  
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var loadWindow = new LoadWindow();

            var startupCode = e.Args.Length >= 2 && e.Args[0] == "Code" ? e.Args[1] : null;

            var versionInfo = FileVersionInfo.GetVersionInfo(typeof(App).Assembly.Location);

            var version = new System.Version(versionInfo.FileVersion);

            loadWindow.SetVersion(version);

            ThreadPool.QueueUserWorkItem(_ =>
            {
                loadWindow.Invoke(w => w.Show());

                var getDataSourceFunc = FuncHelper.Create(this.GetDataSource)
                                                      .WithTryFinally(() => loadWindow.Invoke(w => w.SetStatus("Загрузка базы данных")));

                var dataSource = getDataSourceFunc();

                var getContextFunc = FuncHelper.Create(() => ApplicationContext.Create(dataSource))
                                               .WithTryFinally(() => loadWindow.Invoke(w => w.SetStatus("Создание контекста")));

                this._context = getContextFunc();
                
                loadWindow.Invoke(() =>
                {
                    var mainWindow = new MainWindow(this._context, version, Rq_Calc_WPF.Properties.Settings.Default, startupCode);
                    
                    mainWindow.Show();

                    loadWindow.Close();
                });
            });
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            this._context.Maybe(c => c.DataSource.Dispose());
        }


        private IDataSource<IPersistentDomainObjectBase> GetDataSource()
        {
            var connection = new SQLiteConnection(Rq_Calc_WPF.Properties.Settings.Default.ConnectionString);

            return new Anon.RQ_Calc.DataBase.EntityFramework.RQDBContext(connection, true, ImplementTypeResolver.Default.WithCache());//.WithCache();
        }
    }
}