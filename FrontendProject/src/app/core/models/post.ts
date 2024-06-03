export interface Post {
    id: number;
    channelId: number;
    telegramId: number;
    text: string;
    hash: string;
    viewsCount: number;
    createdAt: Date;
    editedAt: Date;
    parsedAt: Date;
}
