using MyTgClient;

namespace TelegramClient;

public partial class AuthorizForm : Form
{
        private Client _tg;
        public Client Client => _tg;
        
        private TextBox? _txtPhone;
        private Button? _btnStartAuth;
        private TextBox? _txtCode;
        private Button? _btnFinishAuth;
        private TextBox? _txtPassword;
        private Button? _btnPassword;

        public AuthorizForm()
        {
            InitializeComponent();
            
            _tg = new Client();
            _tg.AuthCodeNeeded += () =>
            {
                try
                {
                    MessageBox.Show("Введите код");
                }
                catch
                {
                    MessageBox.Show("<UNK> <UNK> <UNK>");
                }

            };
            _tg.PasswordNeeded += () =>
            {
                try
                {
                    Invoke(() =>
                    {
                        _txtPassword!.Enabled = true;
                        _btnPassword!.Enabled = true;
                        _txtPassword.Visible = true;
                        _btnPassword.Visible = true;
                        MessageBox.Show("Введите пароль (2FA)");
                    });
                }
                catch
                {
                    MessageBox.Show("<UNK> <UNK> <UNK>");
                }
            };
            _tg.Ready += () =>
            {
                try
                {
                    MessageBox.Show("Успешная авторизация!");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch
                {
                    MessageBox.Show("<UNK> <UNK> <UNK>");
                }
            };
            
        }

        // Кнопка подтверждения телефона
        private async void btnStartAuth_Click(object sender, EventArgs e)
        {
            string phone = _txtPhone!.Text.Trim();
            await _tg.StartAsync(phone);
        }

        // кнопка подтверждения кода подтверждения
        private async void btnFinishAuth_Click(object sender, EventArgs e)
        {
            string code = _txtCode!.Text.Trim();
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Введите код подтверждения.");
                return;
            }

            try
            {
                await _tg.SubmitCodeAsync(code);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вводе кода: {ex.Message}");
            }
        }
        
        // Кнопка подтверждения пароля
        private async void btnPassword_Click(object sender, EventArgs e)
        {
            string password = _txtPassword!.Text.Trim();
            
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пароль не может быть пустым.");
                return;
            }
            try
            {
                await _tg.SubmitPasswordAsync(password);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вводе пароля: {ex.Message}");
            }
        }
}
    