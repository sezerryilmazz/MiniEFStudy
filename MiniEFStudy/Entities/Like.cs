using System.ComponentModel.DataAnnotations.Schema;

namespace MiniEFStudy.Entities
{
    internal class Like : BaseEntity
    {
        public int id { get; set; }

        public int post_id { get; set; }

        [ForeignKey("post_id")]
        public virtual Post post { get; set; }

        public int user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual User user { get; set; }
    }
}
