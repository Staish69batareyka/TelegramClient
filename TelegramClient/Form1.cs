using MyTgClient;


namespace TelegramClient
{
    public partial class Form1 : Form
    {
        private TextBox txtPhone;
        private Button btnStartAuth;
        private TextBox txtCode;
        private Button btnFinishAuth;
        private ListBox lstChats;
        private TextBox txtMessage;
        private Button btnSend;
        private TextBox txtHistory;

        public Form1()
        {
            InitializeComponent();
            
        }

       

        private void btnStartAuth_Click(object sender, EventArgs e)
        {
            string phone = txtPhone.Text.Trim();
            MessageBox.Show($"Начинаем авторизацию для: {phone}");
            // TODO: передать номер в TDLib
        }

        private void btnFinishAuth_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text.Trim();
            MessageBox.Show($"Код подтверждения: {code}");
            // TODO: передать код в TDLib
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                txtHistory.AppendText("Вы: " + message + Environment.NewLine);
                txtMessage.Clear();
                // TODO: отправить сообщение в выбранный чат
            }
        }
    }
}
