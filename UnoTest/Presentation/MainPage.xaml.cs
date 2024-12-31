namespace UnoTest.Presentation;

public sealed partial class MainPage : Page
{

    public MainPage()
    {
        this.InitializeComponent();
    }

    private void GotoContacts(object sender, RoutedEventArgs e)
    {
        _ = this.Navigator()?.NavigateViewAsync<Contacts>(this);
    }
}
