using ParsingProject.DAL.Context;
using ParsingProject.DAL.Entities;

namespace ParsingProject;

public class DataParser
{
    public async Task ParseData(WTelegramService wt, ParsingProjectContext dbContext)
    {
        var client = wt.Client;
        
        var myself = await client.LoginUserIfNeeded();
        Console.WriteLine($"We are logged-in as {myself} (id {myself.id})");

        var chats = await client.Messages_GetAllChats();

        foreach (var (id, chat) in chats.chats)
        {
            if (chat.IsActive)
                Console.WriteLine($"{id,10}: {chat}");
            
            if (!chat.IsChannel)
                continue;

            var channelDbModel = new Channel
            {
                MainUsername = chat.MainUsername,
                Title = chat.Title,
                TelegramId = chat.ID,
                
            };

            dbContext.Channels.Add(channelDbModel);
            await dbContext.SaveChangesAsync();

            /*var allMessages = await client.Messages_GetHistory(chat);

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
            }*/
        }
    }
}