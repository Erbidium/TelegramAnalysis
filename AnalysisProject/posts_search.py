from similarity_analysis.natasha_navec import get_embedding

def find_spread_by_root(root_post, post_index, processed_post_text, posts_ordered_by_date, similar_posts, model, nlp, processed_posts, depth, channels, processed_input):
    for index, post in enumerate(posts_ordered_by_date):
        if index <= post_index:
            continue

        processed_post = processed_posts[index]
        if len(processed_post) == 0:
            continue

        # Natasha
        post_embedding = get_embedding(processed_post)
        # similarity_result = cosine_similarity(input_embedding, post_embedding)
        similarity_result = model.wv.n_similarity(processed_post_text, processed_post)

        post_id = post.id

        channel = next((channel for channel in channels if channel.id == post.channelid), None)

        # replace post in graph if similarity is higher
        post_search_result_in_graph = [p for p in similar_posts if post_id == p['post_id']]
        given_post_in_graph = post_search_result_in_graph[0] if post_search_result_in_graph else None

        if similarity_result > 0.5:
            if given_post_in_graph and given_post_in_graph.similarity >= similarity_result:
                continue
            elif given_post_in_graph and given_post_in_graph.similarity < similarity_result:
                similar_posts = [p for p in similar_posts if p['post_id'] != post_id]

            similarity_result_with_wanted = model.wv.n_similarity(processed_input, processed_post)

            similar_posts.append({
                "post_id": post.id,
                "text": post.text,
                "similarity": float(similarity_result),
                "similarity_with_wanted": float(similarity_result_with_wanted),
                "created_at": post.createdat,
                "root_id": root_post.id,
                "channel_title": channel.title
            })

            find_spread_by_root(post, index, processed_post, posts_ordered_by_date, similar_posts, model, nlp, processed_posts, depth + 1, channels, processed_input)
            break