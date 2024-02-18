using Cards.Core.Models;

namespace Cards.Api.ViewModels
{
    public class CardItemViewModel
    {

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public Status Status { get; set; } = Status.ToDo;
    }
}
