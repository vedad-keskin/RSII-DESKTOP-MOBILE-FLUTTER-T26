using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Services.Database
{
    public class ChatIB180079
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // USER 1
        public int User1Id { get; set; }

        [ForeignKey("User1Id")]
        public User User1 { get; set; } = null!;

        // USER 2
        public int User2Id { get; set; }

        [ForeignKey("User2Id")]
        public User User2 { get; set; } = null!;

        //public ICollection<Category> ChildCategories { get; set; } = new List<Category>();


    }
} 