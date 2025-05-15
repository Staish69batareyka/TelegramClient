using MyTgClient;
using TdLib;

namespace TelegramClient;

public partial class AuthorizForm : Form
{
        private Client _tg;
        public Client Client => _tg;
        
        private TextBox txtPhone;
        private Button btnStartAuth;
        private TextBox txtCode;
        private Button btnFinishAuth;
        private TextBox txtPassword;
        private Button btnPassword;

        public AuthorizForm()
        {
            InitializeComponent();
            
            _tg = new Client();
            _tg.AuthCodeNeeded += () => MessageBox.Show("Введите код");
            _tg.PasswordNeeded += () =>
            {
                txtPassword.Enabled = true;
                btnPassword.Enabled = true;
                txtPassword.Visible = true;
                btnPassword.Visible = true;
                MessageBox.Show("Введите пароль (2FA)");
            };
            _tg.Ready += async () =>
            {
                MessageBox.Show("Успешная авторизация!");
                DialogResult = DialogResult.OK;
                Close();
            };
            
        }

        // Кнопка подтверждения телефона
        private async void btnStartAuth_Click(object sender, EventArgs e)
        {
            string phone = txtPhone.Text.Trim();
            await _tg.StartAsync(phone);
        }

        // кнопка подтверждения кода подтверждения
        private async void btnFinishAuth_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text.Trim();
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
            string password = txtPassword.Text.Trim();
            
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
    