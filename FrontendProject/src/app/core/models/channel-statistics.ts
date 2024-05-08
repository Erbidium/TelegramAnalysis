import { Channel } from "./channel";

export interface ChannelStatistics {
    channel: Channel;
    parsedPostsCount: number;
    parsedReactionsCount: number;
    parsedCommentsCount: number;
    startDate?: Date;
    endDate?: Date;
}
