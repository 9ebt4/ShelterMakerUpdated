using System.ComponentModel.DataAnnotations;

namespace ShelterMaker.DTOs
{
    public class ItemCreateDto
    {
        // Assuming Content is required to create an Item
        [Required]
        public string Content { get; set; }

        // This property is assumed to be required since every item must belong to a checklist
        [Required]
        public int CheckListId { get; set; }

        // IsChecked can be optional and default to false if not provided
        public bool IsChecked { get; set; } = false;
    }

    public class ItemUpdateDto
    {
        public string? Content { get; set; }
        public bool? IsChecked { get; set; }
    }
}
