using System.ComponentModel.DataAnnotations.Schema;

namespace MiniEFStudy.Entities
{
    internal class Post : BaseEntity
    {
        public int id { get; set; }
        public string description { get; set; }

        [ForeignKey("user_id")]
        public virtual User user { get; set; }
        public string image { get; set; }

        public virtual ICollection<Like> likes { get; set; }
    }
}
