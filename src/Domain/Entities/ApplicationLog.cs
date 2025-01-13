using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("LOGS")]
    public class ApplicationLog
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }

        // Aditional Properties
        public string Action { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
    }
}