using System.Text;
using TL;

namespace ParsingProject.BLL.Services;

public class DataParser
{
    private static string Config(string what)
    {
        switch (what)
        {
            case "api_id": return "";
            case "api_hash": return "";
            case "phone_number": return "";
            case "verification_code": Console.Write("Code: "); return "";
            default: return null;                  // let WTelegramClient decide the default config
        }
    }

    public async Task ParseData()
    {
        using var client = new WTelegram.Client(Config);
        var myself = await client.LoginUserIfNeeded();
        Console.WriteLine($"We are logged-in as {myself} (id {myself.id})");

        var chats = await client.Messages_GetAllChats();

        foreach (var (id, chat) in chats.chats)
        {
            if (chat.IsActive)
                Console.WriteLine($"{id,10}: {chat}");
            
            var allMessages = await client.Messages_GetHistory(chat);

            Console.OutputEncoding = Encoding.UTF8;
            
            Console.WriteLine($"Messages count: {allMessages.Messages.Length}");

            foreach (var m in allMessages.Messages)
            {
                Console.WriteLine(m);
                
                Console.WriteLine($"Views: {(m as dynamic).views}");

                var reactions = await client.Messages_GetMessagesReactions(chat, m.ID);
                var allReactions = (reactions.UpdateList[0] as UpdateMessageReactions)!.reactions;
                
                foreach (var reactionCount in allReactions.results)
                {
                    Console.WriteLine($"{(reactionCount.reaction as dynamic).emoticon}: {reactionCount.count}");
                }

                var result = await client.Messages_GetDiscussionMessage(chat, m.ID);

                var replies = await client.Messages_GetReplies(chat, m.ID);

                foreach (var reply in replies.Messages)
                {
                    Console.WriteLine(reply);
                }
            }
        }
    }
}