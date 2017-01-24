using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grains
{
    [Table("UserInfo", Schema = "user")]
    public class UserInfoState
    {
        [Required, Key]
        public long UserId { get; set; }
        public string Name { get; set; } = "name";
    }
}
