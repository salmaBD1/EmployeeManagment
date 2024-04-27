using System.ComponentModel.DataAnnotations;

namespace employeeManagemement.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public int NumeroMembre { get; set; }
        public string CustomerName { get; set; }
        public DateTime? BookingFrom { get; set; }
        public DateTime ?BookingTo { get; set; }
        public int SalleId { get; set; }

        // Navigation property
        public Salle? Salle { get; set; }
    }
}
