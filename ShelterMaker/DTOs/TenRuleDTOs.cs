namespace ShelterMaker.DTOs
{
    
        public class TenRuleCreateDto
        {
            public bool? Completed { get; set; } = false;
            public bool? Confirmed { get; set; } = false;
        }

        public class TenRuleDetailDto
        {
            public int? TenRulesId { get; set; }
            public bool? Completed { get; set; }
            public bool? Confirmed { get; set; }
        }
    public class TenRuleUpdateDto
    {
        public bool? Completed { get; set; }
        public bool? Confirmed { get; set; }
    }

}
