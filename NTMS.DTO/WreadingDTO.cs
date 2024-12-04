namespace NTMS.DTO
{
    public class WreadingDTO
    {
        public int Id { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int PreviousReading { get; set; }

        public int CurrentReading { get; set; }

        public int? WmeterId { get; set; }
        public string? WmeterNumber { get; set; }
    }
}
