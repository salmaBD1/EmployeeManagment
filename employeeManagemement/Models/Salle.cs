using System.ComponentModel.DataAnnotations;

namespace employeeManagemement.Models
{

    public class Salle
    {
        [Key]
        public int Id { get; set; }
        public int Numero { get; set; }
        public string Status { get; set; }
        public int Capacite { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}
