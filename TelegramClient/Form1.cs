using MyTgClient;
using TdLib;


namespace TelegramClient;

public partial class Form1 : Form
{
    private readonly Client _tg;
    private List<TdApi.Chat> _chats = new();
    private TdApi.Chat? _selectedChat;


    private ListView? _lstChats;
    private TextBox? _txtMessage;
    private Button? _btnSend;
    private TextBox? _txtHistory;
    private NotifyIcon? _notifyIcon;
    private ProgressBar? _progressBar;
    private Button _btnSendFile;

    public Form1(Client client)
    {
        InitializeComponent();
        _tg = client;
        _lstChats!.SelectedIndexChanged += lstChats_SelectedIndexChanged;
        
        // Обработка ошибок получения чата
        try
        {
            LoadChatsAsync().ConfigureAwait(false); // Запуск без блокировки UI
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось загрузить чаты: {ex.Message}");
        }

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

        _ = LoadChatsAsync();
    }


    // Функция для показа уведомления
    void ShowTrayNotification(string message)
    {
        if (_notifyIcon == null) return;

        _notifyIcon.BalloonTipTitle = "Новое сообщение";
        _notifyIcon.BalloonTipText = message + " ";
        _notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
        _notifyIcon.BalloonTipTitle = "Тест уведомления";
        _notifyIcon.BalloonTipText = "Это уведомление должно появиться";
        _notifyIcon.ShowBalloonTip(5000);
    }

    // Функция обеспечения безопасности потоков
    private Task SafeInvokeAsync(Func<Task> action)
    {
        if (InvokeRequired)
        {
            var tcs = new TaskCompletionSource<object?>();

            BeginInvoke(async void () =>
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
        try
        {
            var chats = await _tg.GetChatsAsync();
            if (chats.Count == 0)
            {
                MessageBox.Show("Чаты не найдены или не загружены.");
                return;
            }

            _chats = chats;
            _lstChats!.Items.Clear();
            foreach (var chat in _chats)
            {
                _lstChats.Items.Add(chat.Title);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки чатов: {ex.Message}");
        }
    }

    // Очистка NotifyIcon при закрытии формы
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _notifyIcon!.Dispose();
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
    private async Task LoadChatHistoryAsync(long chatId)
    {
        var history = await _tg.GetChatHistoryAsync(chatId);
        
        _txtHistory!.Clear();

        // Сообщения идут в обратном порядке, от старых к новым
        foreach (var msg in history.AsEnumerable().Reverse())
        {
            string displayText = FormatMessage(msg);
            string sender = msg.SenderId is TdApi.MessageSender.MessageSenderUser user
                ? $"User {user.UserId}"
                : "System";

            _txtHistory.AppendText($"{sender}: {displayText}\r\n");
        }
    }


    // Кнопка отправления сообщения
    private async void btnSend_Click(object sender, EventArgs e)
    {
        string message = _txtMessage!.Text.Trim();
        if (_selectedChat == null || string.IsNullOrEmpty(message))
        {
            MessageBox.Show("Выберите чат и введите сообщение.");
            return;
        }

        await _tg.SendMessageAsync(_selectedChat.Id, message);
        _txtMessage.Clear();
    }
    
    private async void btnSendFile_Click(object sender, EventArgs e)
    {
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Title = "Выберите файл для отправки";
            openFileDialog.Filter = "Все файлы (*.*)|*.*";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    int index = _lstChats!.SelectedIndices[0];
                    if (index >= 0 && index < _chats.Count)
                    {
                        _ = _tg.SendFileAsync(_chats[index].Id,filePath);
                        MessageBox.Show("Файл отправлен!");
                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при отправке файла: {ex.Message}");
                }
            }
        }
    }



    // Выбор чата из списка
    private async void lstChats_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_lstChats!.SelectedIndices.Count == 0) return;
        
        int index = _lstChats!.SelectedIndices[0];

        if (index >= 0 && index < _chats.Count)
        {
            _selectedChat = _chats[index];
            _txtHistory!.Clear();
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
        _progressBar!.Visible = true;
    }

    private void HideUploadProgress()
    {
        _progressBar!.Visible = false;
    }

    private async void Form1_DragDrop(object sender, DragEventArgs e)
    {
        if (e.Data == null)
            return;

        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop)!;
        if (files.Length > 0)
        {
            string filePath = files[0]; // можно расширить до нескольких
            ShowUploadProgress(); //  отображаем прогресс

            int index = _lstChats!.SelectedIndices[0];
            if (index >= 0 && index < _chats.Count)
            {
                _selectedChat = _chats[index];
                await _tg.SendFileAsync(_selectedChat.Id, filePath, "📎 Файл");
            }

            HideUploadProgress(); // убираем прогресс
        }
    }
}