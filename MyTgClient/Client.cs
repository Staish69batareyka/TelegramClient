using TdLib;
using TdLib.Bindings;

namespace MyTgClient;

public class Client
{
     private TdClient _client = new();
    private bool _isReady;
    private TdApi.AuthorizationState _authState;

    public event Action? AuthCodeNeeded;
    public event Action? PasswordNeeded;
    public event Action? Ready;

    public Client()
    {
        _client.UpdateReceived += async (_, update) => await OnUpdate(update);
    }

    public async Task StartAsync(string phone)
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

        await _client.ExecuteAsync(new TdApi.SetAuthenticationPhoneNumber
        {
            PhoneNumber = phone
        });
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

    private async Task OnUpdate(TdApi.Update update)
    {
        switch (update)
        {
            case TdApi.Update.UpdateAuthorizationState state:
                _authState = state.AuthorizationState;

                switch (_authState)
                {
                    case TdApi.AuthorizationState.AuthorizationStateWaitCode:
                        AuthCodeNeeded?.Invoke();
                        break;

                    case TdApi.AuthorizationState.AuthorizationStateWaitPassword:
                        PasswordNeeded?.Invoke();
                        break;

                    case TdApi.AuthorizationState.AuthorizationStateReady:
                        _isReady = true;
                        Ready?.Invoke();
                        break;
                }

                break;
        }
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

    public async Task RunAsync()
    {
        await InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        throw new NotImplementedException();
    }
}