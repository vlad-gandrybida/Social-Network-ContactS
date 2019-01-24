using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactS.DAL.Entities
{
    public class Message
    {
        [Required]
        public string Content { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Time { get; set; }

        [Required]
        public virtual ClientProfile Sender { get; set; }


        [Required]
        public virtual Dialog Dialog { get; set; }

        //		public bool Seen { get; set; }

        public int Id { get; set; }
    }
}
