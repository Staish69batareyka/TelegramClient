using MyTgClient;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Запускаем Telegram-клиент...");
        
        var client = new Client();
        await client.RunAsync();

        Console.WriteLine("Работа завершена.");
    }
}