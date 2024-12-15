namespace NTMS.DTO
{
    public class TenantDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Occupation { get; set; }

        public string? Paddress { get; set; }

        public string? Telephone { get; set; }
        public string? SecurityDeposit { get; set; }

        public string? StartDate { get; set; }

        public int IsActive { get; set; }

        public int? FlatId { get; set; }
        public string? FlatCode { get; set; }
    }
}
