namespace SonicShare
{
    public partial class App : Application
    {
        public App(WebAppHost webAppHost)
        {
            InitializeComponent();

            UserAppTheme = AppTheme.Dark;

            MainPage = new MainPage();

            // Start web app server.
            _ = Task.Run(async() => 
            { 
                await webAppHost.StartAsync();
            });
        }
    }
}
