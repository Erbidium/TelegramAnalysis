import { Post } from "@core/models/post";

export interface Channel {
    id: number;
    telegramId: number;
    participantsCount: number;
    mainUsername?: string;
    title: string;
    posts: Post[];

    showPosts: boolean;
}
