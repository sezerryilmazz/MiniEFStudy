
namespace MiniEFStudy.Entities
{
    internal class User : BaseEntity
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string full_name { get; set; }
        public string profile_picture { get; set; }
        public string bio { get; set; }
        public virtual ICollection<Post> posts { get; set; }
        public virtual ICollection<Like> likes { get; set; }
    }
}
