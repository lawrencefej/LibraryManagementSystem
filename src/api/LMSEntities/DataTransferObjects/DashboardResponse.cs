using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class DashboardResponse
    {
        [Required]
        public int TotalCards { get; set; }

        [Required]
        public int TotalItems { get; set; }

        [Required]
        public int TotalAuthors { get; set; }

        [Required]
        public int TotalCheckouts { get; set; }

        [Required]
        public ChartDto CategoryDistribution { get; set; }

        [Required]
        public ChartDto TypeDistribution { get; set; }

        [Required]
        public ChartDto ReturnsByMonth { get; set; }

        [Required]
        public ChartDto ReturnsByDay { get; set; }

        [Required]
        public ChartDto CheckoutsByMonth { get; set; }

        [Required]
        public ChartDto CheckoutsByDay { get; set; }

    }
}
