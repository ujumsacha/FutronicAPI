using System.ComponentModel.Design;

namespace Application_Enrollement
{
    internal static class Program
    {
        private static IServiceProvider serviceProvider { get; set; }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        public static IServiceContainer ConfigureServices()
        {
            IServiceContainer services = new ServiceContainer();
            services.AddService(typeof(Form1), new Form1());


            return services;
        }
    }
}