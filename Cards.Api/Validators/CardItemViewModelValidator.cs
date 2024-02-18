using Cards.Api.ViewModels;
using Cards.Core.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Cards.Api.Validators
{
    public class CardItemViewModelValidator: AbstractValidator<CardItemViewModel>
    {
        private readonly CardDbContext _context;
        public CardItemViewModelValidator(IDbContextFactory<CardDbContext> _contextFactory)
        {
            _context = _contextFactory.CreateDbContext();
            RuleFor( c=> c.Name).NotEmpty().MaximumLength(50).MinimumLength(3)
                .Must(IsUnique).WithMessage("Card Name should be unique.");
            RuleFor(c => c.Color!).IsValidColor();
        }

        private bool IsUnique(string name)
        {
            return !_context.CardItems.Any(t => t.Name == name);
        }
    }


}
