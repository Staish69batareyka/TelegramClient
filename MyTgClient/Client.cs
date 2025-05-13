using TdLib;
using TdLib.Bindings;

namespace MyTgClient;

public class Client
{
    private const int ApiId = 23613057; // Получить на https://my.telegram.org/apps
    private const string ApiHash = "a0fc7ea7c76b14a6af35f854bf85ac8a"; // Получить на https://my.telegram.org/apps
    private const string PhoneNumber = "+79092415655"; // Свой номер
    private const string ApplicationVersion = "1.0.0";

    private static TdClient _client = null!;
    private static readonly ManualResetEventSlim ReadyToAuthenticate = new();

    private static bool _authNeeded; // Необходимость аутентификации
    private static bool _passwordNeeded; // Необходимость пароля

    public static async Task<Client> CreateAsync()
    {
        var instance = new Client();
        await instance.InitializeAsync();
        return instance;
    }

    private async Task InitializeAsync()
    {
        _client = new TdClient();
        _client.Bindings.SetLogVerbosityLevel(TdLogLevel.Fatal);
        _client.UpdateReceived += async (_, update) => { await ProcessUpdates(update); };

        ReadyToAuthenticate.Wait();

        if (_authNeeded)
        {
            await HandleAuthentication();
        }

        Console.WriteLine("Готовимся получить текущего пользователя");
        var currentUser = await GetCurrentUser();

        var fullUserName = $"{currentUser.FirstName} {currentUser.LastName}".Trim();
        Console.WriteLine(
            $"Текущий пользователь [{currentUser.Id}] / [@{currentUser.Usernames?.ActiveUsernames[0]}] / [{fullUserName}]");

        const int channelLimit = 10;
        var channels = GetChannels(channelLimit);

        Console.WriteLine($"Первые {channelLimit}-ть каналов:");
        await foreach (var channel in channels)
        {
            string lastMessageText = "";

            if (channel.LastMessage?.Content is TdApi.MessageContent.MessageText textContent)
            {
                lastMessageText = textContent.Text.Text;
            }
            else
            {
                lastMessageText = "(Не текстовое сообщение)";
            }

            Console.WriteLine($"[{channel.Id}] -> [{channel.Title}] ({channel.UnreadCount} сообщений не прочитано)\n" +
                              $"Последнее сообщение: {lastMessageText}\n");
            Console.WriteLine("------------------------------------------------------------------");
        }

        Console.WriteLine("Нажмите ENTER чтобы всё завершить");
        Console.ReadLine();
    }

    private static async Task HandleAuthentication()
    {
        // Устанавливаем номер телефона
        await _client.ExecuteAsync(new TdApi.SetAuthenticationPhoneNumber
        {
            PhoneNumber = PhoneNumber
        });

        // Телеграм отправил нам код и нам надо его ввести
        Console.Write("Введите код из Telegram: ");
        var code = Console.ReadLine();

        await _client.ExecuteAsync(new TdApi.CheckAuthenticationCode
        {
            Code = code
        });

        if (!_passwordNeeded)
        {
            return;
        }

        // Возможно, включена функция 2FA. В этом случае требуется облачный пароль.
        Console.Write("Введите пароль: ");
        var password = Console.ReadLine();

        await _client.ExecuteAsync(new TdApi.CheckAuthenticationPassword
        {
            Password = password
        });
    }

    private static async Task ProcessUpdates(TdApi.Update update)
    {
        // Поскольку Stdlib был создан для использования в приложении с графическим интерфейсом, нам нужно немного повозиться и отловить необходимые события, чтобы определить наше состояние.
        // Ниже вы можете найти пример простой проверки подлинности.
        // Пожалуйста, обратите внимание, что режим ожидания подтверждения состояния авторизации другим устройством не реализован.

        switch (update)
        {
            case TdApi.Update.UpdateAuthorizationState
            {
                AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters
            }:
                // Stdlib создает базу данных в текущем каталоге.
                // Поэтому создайте отдельный каталог и переключитесь на этот каталог.
                var filesLocation = Path.Combine(AppContext.BaseDirectory, "db");
                await _client.ExecuteAsync(new TdApi.SetTdlibParameters
                {
                    ApiId = ApiId,
                    ApiHash = ApiHash,
                    DeviceModel = "PC",
                    SystemLanguageCode = "en",
                    ApplicationVersion = ApplicationVersion,
                    DatabaseDirectory = filesLocation,
                    FilesDirectory = filesLocation,
                    // Доступно больше параметров!
                });
                break;

            case TdApi.Update.UpdateAuthorizationState
            {
                AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber
            }:
            case TdApi.Update.UpdateAuthorizationState
            {
                AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitCode
            }:
                _authNeeded = true;
                ReadyToAuthenticate.Set();
                break;

            case TdApi.Update.UpdateAuthorizationState
            {
                AuthorizationState: TdApi.AuthorizationState.AuthorizationStateWaitPassword
            }:
                _authNeeded = true;
                _passwordNeeded = true;
                ReadyToAuthenticate.Set();
                break;

            case TdApi.Update.UpdateUser:
                ReadyToAuthenticate.Set();
                break;

            case TdApi.Update.UpdateConnectionState { State: TdApi.ConnectionState.ConnectionStateReady }:
                // Вы можете инициировать дополнительное событие при изменении состояния соединения
                break;

            default:
                // ReSharper отключает один раз пустую инструкцию
                ;
                // Добавьте сюда точку останова, чтобы увидеть другие события
                break;
        }
    }

    private static async Task<TdApi.User> GetCurrentUser()
    {
        return await _client.ExecuteAsync(new TdApi.GetMe());
    }

    public static async IAsyncEnumerable<TdApi.Chat> GetChannels(int limit)
    {
        var chats = await _client.ExecuteAsync(new TdApi.GetChats
        {
            Limit = limit
        });

        foreach (var chatId in chats.ChatIds)
        {
            var chat = await _client.ExecuteAsync(new TdApi.GetChat
            {
                ChatId = chatId
            });

            if (chat.Type is TdApi.ChatType.ChatTypeSupergroup or TdApi.ChatType.ChatTypeBasicGroup
                or TdApi.ChatType.ChatTypePrivate)
            {
                yield return chat;
            }
        }
    }
}