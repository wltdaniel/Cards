using Cards.Core.Models;

namespace Cards.Api.ViewModels
{
    public class CardItemVM(string? name, string color, string description, Status status, DateTime createdAt)
    {
        public string? Name { get; set; } = name;
        public string Color { get; set; } = color;
        public DateTime CreatedAt { get; set; } = createdAt;
        public string Description { get; set; } = description;
        public Status Status { get; set; } = status;


    }
}
