using TdLib;
using TdLib.Bindings;

namespace MyTgClient;

public class Client
{
    private TdClient _client = new();
    private TdApi.AuthorizationState _authState;
    private long _currentChatId;
    private bool _authorized = false;
    private string? _phoneNumber;

    public event Action? AuthCodeNeeded;
    public event Action? PasswordNeeded;
    public event Action? Ready;
    public event Action<TdApi.Message>? NewMessageReceived;

    public Client()
    {
        _client.UpdateReceived += async (_, update) => await OnUpdate(update);
    }

    public async Task StartAsync(string phone)
    {
        _phoneNumber = phone;

        await Task.Run(async () =>
        {
            await _client.ExecuteAsync(new TdApi.SetTdlibParameters
            {
                ApiId = 23613057,
                ApiHash = "a0fc7ea7c76b14a6af35f854bf85ac8a",
                SystemLanguageCode = "en", 
                DeviceModel = "PC", 
                ApplicationVersion = "1.0",
                UseMessageDatabase = true,
                UseSecretChats = false,
                FilesDirectory = "tdlib_files",
                DatabaseDirectory = "tdlib_db"
            });
            await _client.ExecuteAsync(new TdApi.SetDatabaseEncryptionKey
            {
                NewEncryptionKey = new byte[] { } 
            });
        });
    }

    private async Task OnUpdate(TdApi.Update update)
    {
        switch (update)
        {
            case TdApi.Update.UpdateAuthorizationState state:
                _authState = state.AuthorizationState;

                switch (_authState)
                {
                    case TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber:
                        if (!string.IsNullOrEmpty(_phoneNumber))
                        {
                            await _client.ExecuteAsync(new TdApi.SetAuthenticationPhoneNumber
                            {
                                PhoneNumber = _phoneNumber
                            });
                        }
                        break;

                    case TdApi.AuthorizationState.AuthorizationStateWaitCode:
                        AuthCodeNeeded?.Invoke();
                        break;

                    case TdApi.AuthorizationState.AuthorizationStateWaitPassword:
                        PasswordNeeded?.Invoke();
                        break;

                    case TdApi.AuthorizationState.AuthorizationStateReady:
                        Ready?.Invoke();
                        _authorized = true;
                        break;
                }

                break;

            case TdApi.Update.UpdateNewMessage newMessage:
                if (_authorized && newMessage.Message.ChatId == _currentChatId)
                {
                    NewMessageReceived?.Invoke(newMessage.Message);
                }
                break;
        }
    }

    public void SetCurrentChatId(long chatId)
    {
        _currentChatId = chatId;
    }

    public async Task SubmitCodeAsync(string code)
    {
        await _client.ExecuteAsync(new TdApi.CheckAuthenticationCode
        {
            Code = code
        });
    }

    public async Task SubmitPasswordAsync(string password)
    {
        await _client.ExecuteAsync(new TdApi.CheckAuthenticationPassword
        {
            Password = password
        });
    }

    public async Task<List<TdApi.Chat>> GetChatsAsync(int limit = 20)
    {
        var result = new List<TdApi.Chat>();

        var chats = await _client.ExecuteAsync(new TdApi.GetChats
        {
            Limit = limit
        });

        foreach (var id in chats.ChatIds)
        {
            var chat = await _client.ExecuteAsync(new TdApi.GetChat
            {
                ChatId = id
            });

            result.Add(chat);
        }

        return result;
    }

    public async Task SendMessageAsync(long chatId, string text)
    {
        await _client.ExecuteAsync(new TdApi.SendMessage
        {
            ChatId = chatId,
            InputMessageContent = new TdApi.InputMessageContent.InputMessageText
            {
                Text = new TdApi.FormattedText { Text = text }
            }
        });
    }
   
    public async Task SendFileAsync(long chatId, string filePath, string caption = "")
    {
        // Загрузка файла
        var inputFile = new TdApi.InputFile.InputFileLocal
        {
            Path = filePath
        };

        var documentContent = new TdApi.InputMessageContent.InputMessageDocument
        {
            Document = inputFile,
            Caption = new TdApi.FormattedText
            {
                Text = caption
            }
        };

        await _client.ExecuteAsync(new TdApi.SendMessage
        {
            ChatId = chatId,
            InputMessageContent = documentContent
        });
    }



    

    public async Task<TdApi.Message[]> GetChatHistoryAsync(long chatId, long fromMessageId = 0, int limit = 50)
    {
        var response = await _client.ExecuteAsync(new TdApi.GetChatHistory
        {
            ChatId = chatId,
            FromMessageId = fromMessageId,
            Limit = limit,
            Offset = 0,
            OnlyLocal = false
        });

        return (response as TdApi.Messages)?.Messages_ ?? Array.Empty<TdApi.Message>();
    }
}
