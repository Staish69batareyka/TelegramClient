using MyTgClient;
using TdLib;


namespace TelegramClient
{
    
    public partial class Form1 : Form
    {
        private Client _tg;
        private List<TdApi.Chat> _chats = new();
        private TdApi.Chat? _selectedChat;
        
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
            _tg = new Client();
            _tg.AuthCodeNeeded += () => MessageBox.Show("Введите код");
            _tg.PasswordNeeded += () => MessageBox.Show("Требуется пароль");
            _tg.Ready += async () =>
            {
                MessageBox.Show("Успешная авторизация!");
                _chats = await _tg.GetChatsAsync();

                lstChats.Items.Clear();
                foreach (var chat in _chats)
                {
                    lstChats.Items.Add($"{chat.Id}: {chat.Title}");
                }
                
            };
            
            
        }

       

        private async void btnStartAuth_Click(object sender, EventArgs e)
        {
            string phone = txtPhone.Text.Trim();
            await _tg.StartAsync(phone);
        }

        private async void btnFinishAuth_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text.Trim();
            await _tg.SubmitCodeAsync(code);
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text.Trim();
            if (_selectedChat == null || string.IsNullOrEmpty(message)) return;

            await _tg.SendMessageAsync(_selectedChat.Id, message);
            txtHistory.AppendText("Вы: " + message + Environment.NewLine);
            txtMessage.Clear();
        }
        
        private void lstChats_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lstChats.SelectedIndex;
            if (index >= 0 && index < _chats.Count)
            {
                _selectedChat = _chats[index];
                txtHistory.AppendText($"Выбран чат: {_selectedChat.Title}\n");
            }
        }

    }
}
