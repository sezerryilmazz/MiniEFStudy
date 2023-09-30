using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniEFStudy.Entities
{
    [Keyless]
    internal class Follow : BaseEntity
    {
        public int follower_id { get; set; }
        [ForeignKey("follower_id")]
        public virtual User follower_user { get; set; }

        public int following_id { get; set; }
        [ForeignKey("following_id")]
        public virtual User following_user { get; set; }
    }
}
