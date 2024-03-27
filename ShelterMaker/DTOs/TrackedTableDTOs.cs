namespace ShelterMaker.DTOs
{
    public class TrackedTableCreateDto
    {
        public string TableName { get; set; }
    }

    public class TrackedTableDetailDto
    {
        public int TrackedTableId { get; set; }
        public string TableName { get; set; }
    }
    public class TrackedTableUpdateDto
    {
        public string? TableName { get; set; }
    }

}
