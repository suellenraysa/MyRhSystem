namespace MyRhSystem.APP
{
    using MauiApp = Microsoft.Maui.Controls.Application;
    public partial class App : MauiApp
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}
