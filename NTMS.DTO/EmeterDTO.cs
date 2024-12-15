namespace NTMS.DTO
{
    public class EmeterDTO
    {
        public int Id { get; set; }

        public string? MeterNumber { get; set; }
        public string? FlatCode { get; set; }

        public int IsActive { get; set; }

        public int? FlatId { get; set; }
       
    }
}
