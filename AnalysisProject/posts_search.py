from sentence_transformers import util

def find_spread_by_root(root_post, post_index, root_post_embedding, posts_ordered_by_date, similar_posts, model, post_embeddings, channels, cosine_scores_input):
    cosine_scores_root = util.pytorch_cos_sim(root_post_embedding, post_embeddings)[0]

    for index, post in enumerate(posts_ordered_by_date):
        if index <= post_index or len(post.text) == 0:
            continue

        similarity_result = cosine_scores_root[index]

        post_id = post.id

        channel = next((channel for channel in channels if channel.id == post.channelid), None)

        # replace post in graph if similarity is higher
        post_search_result_in_graph = [p for p in similar_posts if post_id == p['post_id']]
        given_post_in_graph = post_search_result_in_graph[0] if post_search_result_in_graph else None

        if similarity_result > 0.75:
            if given_post_in_graph and given_post_in_graph.similarity >= similarity_result:
                continue
            elif given_post_in_graph and given_post_in_graph.similarity < similarity_result:
                similar_posts = [p for p in similar_posts if p['post_id'] != post_id]

            similarity_result_with_wanted = cosine_scores_input[index]

            if similarity_result_with_wanted < 0.7:
                continue

            similar_posts.append({
                "post_id": post.id,
                "text": post.text,
                "similarity": float(similarity_result),
                "similarity_with_wanted": float(similarity_result_with_wanted),
                "created_at": post.createdat,
                "root_id": root_post.id,
                "channel_title": channel.title
            })

            find_spread_by_root(post, index, post_embeddings[index], posts_ordered_by_date, similar_posts, model, post_embeddings, channels, cosine_scores_input)
            break