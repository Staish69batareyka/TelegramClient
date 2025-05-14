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
        private TextBox txtPassword;
        private Button btnPassword;
        
        private ListBox lstChats;
        private TextBox txtMessage;
        private Button btnSend;
        private TextBox txtHistory;

        private Task InvokeAsync(Func<Task> func)
        {
            return InvokeRequired
                ? Invoke(func)
                : func();
        }
        public Form1()
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
                _chats = await _tg.GetChatsAsync();
                lstChats.Items.Clear();
                foreach (var chat in _chats)
                {
                    lstChats.Items.Add($"{chat.Title}");
                }
                
            };
            lstChats.SelectedIndexChanged += lstChats_SelectedIndexChanged;
            _tg.NewMessageReceived += async message =>
            {
                // Если это сообщение текущего чата — обновим историю
                if (_selectedChat != null && message.ChatId == _selectedChat.Id)
                {
                    await InvokeAsync(async () =>
                    {
                        await LoadChatHistoryAsync(_selectedChat.Id);
                    });
                }
            };
            
        }
        
        // Функция прогрузки истории
        
        private async Task LoadChatHistoryAsync(long chatId, long fromMessageId = 0, int limit = 50)
        {
            var history = await _tg.GetChatHistoryAsync(chatId, fromMessageId, limit);

            if (history == null) return;

            txtHistory.Clear();
            

            // Сообщения идут в обратном порядке, от старых к новым
            foreach (var msg in history.Reverse())
            {
                if (msg.Content is TdApi.MessageContent.MessageText text)
                {
                    string sender = msg.SenderId is TdApi.MessageSender.MessageSenderUser user
                        ? $"User {user.UserId}"
                        : "System";

                    txtHistory.AppendText($"{sender}: {text.Text.Text}\r\n");
                }
            }
        }

       
        // Кнопки
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
        
        private async void btnPassword_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text.Trim();
            await _tg.SubmitPasswordAsync(password);
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text.Trim();
            if (_selectedChat == null || string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Выберите чат и введите сообщение.");
                return;
            }

            await _tg.SendMessageAsync(_selectedChat.Id, message);
            txtMessage.Clear();
        }
        
        
        
        
        // Выбор чата из списка
        private async void lstChats_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lstChats.SelectedIndex;
            if (index >= 0 && index < _chats.Count)
            {
                _selectedChat = _chats[index];
                txtHistory.Clear();
                _tg.SetCurrentChatId(_selectedChat.Id); 
                await LoadChatHistoryAsync(_selectedChat.Id);
            }
        }

    }
}
