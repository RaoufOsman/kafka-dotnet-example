using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace testapp.db
{
    [Table("messages")]
    public class Message
    {
        [Column("id")]
        public Guid Id { get; set; }
        
        [Column("messageText")]
        public string? MessageText { get; set; }

        [Column("createdDate")]
        public DateTime CreatedDate { get; set; }
    }
}
