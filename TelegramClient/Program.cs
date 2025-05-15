namespace TelegramClient;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        using (var loginForm = new AuthorizForm())
        {
            var result = loginForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                Application.Run(new Form1(loginForm.Client)); // передаём авторизованный Client
            }
        }
    }
}