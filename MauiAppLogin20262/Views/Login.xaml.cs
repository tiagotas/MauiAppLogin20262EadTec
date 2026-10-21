using MauiAppLogin20262.Models;

namespace MauiAppLogin20262.Views;

public partial class Login : ContentPage
{
	List<Usuario> lista_usuario = new()
	{
		new Usuario { Email = "teste@teste.com", Senha = "123" },
        new Usuario { Email = "aluno@ead", Senha = "456" },
    };

	public Login()
	{
		InitializeComponent();

        string? usuario_logado = null;

        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            usuario_logado = await SecureStorage.Default.GetAsync("usuario_logado");

            if (usuario_logado != null)
            {
                /**
                 * https://stackoverflow.com/questions/53346910/why-displayalert-does-not-work-in-constructor
                 * https://stackoverflow.com/questions/73816335/device-starttimer-is-deprecated-in-maui-what-is-the-alternative
                 */
                Application.Current.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(0), () =>
                {
                    App.Current.MainPage = new Protegida();

                   return false; // True = Repeat again, False = Stop the timer
                });
            }
        });
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
		try
		{
            string email_digitado = txt_email.Text;
            string senha_digitada = txt_senha.Text;

            bool encontrado = lista_usuario.Any(i => i.Email == email_digitado && i.Senha == senha_digitada);

            if (encontrado)
            {
                SecureStorage.SetAsync("usuario_logado", email_digitado);

                App.Current.MainPage = new Protegida();
            }
            else
                throw new Exception("Usuário não encontrado");
        }
        catch (Exception ex) {
            DisplayAlertAsync("Ops", ex.Message, "OK");
		}
    }
}