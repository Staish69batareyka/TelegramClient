using MyTgClient;
using TdLib;


namespace TelegramClient
{
    
    public partial class Form1 : Form
    {
        private readonly Client _tg;
        private List<TdApi.Chat> _chats = new();
        private TdApi.Chat? _selectedChat;


        
        private ListView lstChats;
        private TextBox txtMessage;
        private Button btnSend;
        private TextBox txtHistory;
        private NotifyIcon notifyIcon;
        private ProgressBar progressBar;
        
        public Form1(Client client)
        {
            InitializeComponent();
            _tg = client;   
            
            lstChats.SelectedIndexChanged += lstChats_SelectedIndexChanged;
            
            // Реализация подписки на историю и уведомления
            _tg.NewMessageReceived += async message =>
            {
                try
                {
                    await SafeInvokeAsync(async () =>
                    {
                        if (_selectedChat != null && message.ChatId == _selectedChat.Id)
                        {
                            await LoadChatHistoryAsync(_selectedChat.Id);
                        }

                        // Показываем уведомление, если окно не в фокусе или свёрнуто
                        if (!this.Focused || this.WindowState == FormWindowState.Minimized)
                        {
                            string sender = message.SenderId is TdApi.MessageSender.MessageSenderUser user
                                ? $"User {user.UserId}"
                                : "System";

                            if (message.Content is TdApi.MessageContent.MessageText text)
                            {
                                ShowTrayNotification($"{sender}: {text.Text.Text}");
                            }
                            else
                            {
                                ShowTrayNotification($"{sender}: новое сообщение");
                            }
                        }
                    }); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка уведомления: " + ex.Message);
                }
                
            };

            LoadChatsAsync();

        }
        
        
        // Функция для показа уведомления
        void ShowTrayNotification(string message)
        {
            if (notifyIcon == null) return;
            
            notifyIcon.BalloonTipTitle = "Новое сообщение";
            notifyIcon.BalloonTipText = message + " ";
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.BalloonTipTitle = "Тест уведомления";
            notifyIcon.BalloonTipText = "Это уведомление должно появиться";
            notifyIcon.ShowBalloonTip(5000);
        }
        
        // Функция обеспечения безопасности потоков
        private Task SafeInvokeAsync(Func<Task> action)
        {
            if (InvokeRequired)
            {
                var tcs = new TaskCompletionSource<object?>();

                BeginInvoke(async () =>
                {
                    try
                    {
                        await action();
                        tcs.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                });

                return tcs.Task;
            }
            else
            {
                return action();
            }
        }

        
        // Подгрузка чатов
        private async Task LoadChatsAsync()
        {
            var chats = await _tg.GetChatsAsync();
            if (chats == null) return;

            _chats = chats;
            lstChats.Items.Clear();

            foreach (var chat in _chats)
            {
                lstChats.Items.Add(chat.Title);
            }
        }

        // Очистка NotifyIcon при закрытии формы
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            notifyIcon.Dispose();
            base.OnFormClosed(e);
        }
        
        
        // Проверка формата
        private string FormatMessage(TdApi.Message message)
        {

            return message.Content switch
            {
                TdApi.MessageContent.MessageText text =>
                    $"{text.Text.Text}",

                TdApi.MessageContent.MessageDocument doc =>
                    $"📄 Документ: {doc.Document.FileName} ({doc.Document.Document_.ExpectedSize})",

                TdApi.MessageContent.MessagePhoto photo =>
                    $"📷 Фото ({photo.Photo.Sizes.Last().Photo.Id})",

                TdApi.MessageContent.MessageVideo video =>
                    $"🎥 Видео ({video.Video.Duration} сек)",

                _ => $" [неподдерживаемый тип сообщения]"
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
                string displayText = FormatMessage(msg);
                string sender = msg.SenderId is TdApi.MessageSender.MessageSenderUser user
                    ? $"User {user.UserId}"
                    : "System";

                txtHistory.AppendText($"{sender}: {displayText}\r\n");
            }

        }


        // Кнопка отправления сообщения
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
            int index = lstChats.SelectedIndices[0];
            
            if (index >= 0 && index < _chats.Count)
            {
                _selectedChat = _chats[index];
                txtHistory.Clear();
                _tg.SetCurrentChatId(_selectedChat.Id); 
                await LoadChatHistoryAsync(_selectedChat.Id);
           }
            
        }
        
        
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void ShowUploadProgress()
        {
            progressBar.Visible = true;
        }
        private void HideUploadProgress()
        {
            progressBar.Visible = false;
        }
        private async void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null)
                return;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                string filePath = files[0]; // можно расширить до нескольких
                ShowUploadProgress(); //  отображаем прогресс
                
                int index = lstChats.SelectedIndices[0];
                if (index >= 0 && index < _chats.Count)
                {
                    _selectedChat = _chats[index];
                    await _tg.SendFileAsync(_selectedChat.Id, filePath, "📎 Файл");
                }

                HideUploadProgress(); // убираем прогресс
            }
        }
    }
}
