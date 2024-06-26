export interface DistributionGraphNode {
    post_id: number;
    text: string;
    similarity: number;
    similarity_with_wanted: number;
    created_at: Date;
    root_id?: number;

    channel_title: string;
}
