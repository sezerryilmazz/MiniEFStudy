using Microsoft.EntityFrameworkCore;
using MiniEFStudy;
using MiniEFStudy.Entities;

namespace ConsoleToWeb
{
    class Program
    {
        private static EFStudyContext dbContext;
        static void Main(string[] args)
        {
            using (dbContext = new EFStudyContext())
            {
                var q1Resp = get_posts(1, new List<int> { 1, 2, 3, 4 }); 
                var q2Resp = merge_posts(create_dummy_merge_posts_input());
            }
            Console.ReadLine();
        }
        private static List<PostModel> get_posts(int user_id, List<int> post_ids)
        {
            var postList = new List<PostModel>();
            // return dbContext.Set<Post>().Where(x => post_ids.Contains(x.id)).Include(x => x.user).ToList();

            // inner join olmadığı koşulda:
            // SELECT * FROM [Posts] AS [p] WHERE [p].[id] IN(1, 2, 3, 4)
            var posts =  dbContext.Set<Post>().Where(x => post_ids.Contains(x.id)).ToList();
            foreach (var item in posts)
            {
                //SELECT * FROM [Users] AS[u] WHERE[u].[id] = @__p_0
                dbContext.Entry(item).Reference(x => x.user).Load();
            }
            // SELECT [f].[following_id] FROM [Follows] AS[f]
            // WHERE [f].[follower_id] = @__user_id_0 AND[f].[following_id] IN(1, 2, 3, 4)
            var follows = dbContext.Set<Follow>().Where(x => x.follower_id == user_id 
                                                       && posts.Select(y => y.user).Select(y => y.id).Distinct()
                                                               .Contains(x.following_id)).Select(x => x.following_id).ToList();

            //SELECT [l].[post_id] FROM [Likes] AS[l] WHERE [l].[user_id] = @__user_id_0 AND[l].[post_id] IN(1, 2, 3, 4)
            var likes = dbContext.Set<Like>().Where(x => x.user_id == user_id 
                                                         && posts.Select(x => x.id).Contains(x.post_id))
                                             .Select(x => x.post_id).ToList();

            foreach (var item in post_ids)
            {
                postList.Add(posts.Select(x => new PostModel
                {
                    id  = x.id,
                    description = x.description,
                    image = x.image,
                    created_at = x.created_at, 
                    liked = likes.Exists(y => y == x.id),
                    owner = new UserModel()
                    {
                        id = x.user.id,
                        username = x.user.username,
                        full_name = x.user.full_name,
                        profile_picture = x.user.profile_picture,
                        followed = follows.Exists(y => y == x.user.id) || user_id == x.user.id,
                    }
                }).FirstOrDefault(x => x.id == item));
            }
            return postList;
        }
        private static List<PostModelForMerge> merge_posts(List<List<PostModelForMerge>> list_of_posts)
        {
            var result = new List<PostModelForMerge>();
            foreach (var posts in list_of_posts)
            {
                result.AddRange(posts);
            }
            return result.DistinctBy(x => x.id).Reverse().ToList();
        }

        private static List<List<PostModelForMerge>> create_dummy_merge_posts_input()
        {
            var result = new List<List<PostModelForMerge>>();
            var subList = new List<PostModelForMerge>
            {
                new PostModelForMerge
                {
                    id = 1,
                    created_at = 15
                },
                new PostModelForMerge
                {
                    id = 2,
                    created_at = 17
                },
                new PostModelForMerge
                {
                    id = 3,
                    created_at = 19
                }
            };
            result.Add(subList);
            subList = new List<PostModelForMerge>
            {
                new PostModelForMerge
                {
                    id = 4,
                    created_at = 74
                },
                new PostModelForMerge
                {
                    id = 2,
                    created_at = 45
                },
                new PostModelForMerge
                {
                    id = 5,
                    created_at = 85
                }
            };
            result.Add(subList);
            result.Add(subList);
            return result;
        }
    }
    
    struct UserModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public string profile_picture { get; set; }
        public bool followed { get; set; }
    }
    struct PostModel
    {
        public int id { get; set; }
        public string description { get; set; }
        public UserModel owner { get; set; }
        public string image { get; set; }
        public int created_at { get; set; }
        public bool liked { get; set; }
    }
    struct PostModelForMerge
    {
        public int id { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public int created_at { get; set; }
    }
}