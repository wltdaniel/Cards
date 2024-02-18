using Cards.Api.ViewModels;
using Cards.Core.Infrastructure;
using Cards.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Linq.Dynamic.Core;
using Azure;

namespace Cards.Api.EndPoints.CardItems
{
    public static class CardItemApi
    {
        public static RouteGroupBuilder MapCardItemApiEndpoints(this RouteGroupBuilder cardGroup)
        {
            cardGroup.MapPost("/", CreateCardItemAsync).Accepts<CardItemViewModel>("application/json").Produces(201).ProducesProblem(401).ProducesProblem(400);
            cardGroup.MapGet("/", GetAllCardItemsAsync).Produces(200, typeof(PagedResultsViewModel<CardItemVM>)).ProducesProblem(401);
            cardGroup.MapGet("/{id}", GetCardItemByIdAsync).Produces(200, typeof(CardItemVM)).ProducesProblem(401);
            cardGroup.MapPut("/{id}", UpdateCardItemAsync).Accepts<CardItemViewModel>("application/json").Produces(201).ProducesProblem(404).ProducesProblem(401);
            cardGroup.MapDelete("/{id}", DeleteCardItemAsync).Produces(204).ProducesProblem(404).ProducesProblem(401);

            return cardGroup;
        }


        public static async Task<IResult> CreateCardItemAsync(IDbContextFactory<CardDbContext> _contextFactory, ClaimsPrincipal principal, CardItemViewModel vm, IValidator<CardItemViewModel> vmValidator)
        {
            var validationResult = vmValidator.Validate(vm);
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            using var context = _contextFactory.CreateDbContext();
            var cardItem = new CardItem
            {
                Name = vm.Name,
                Color = vm.Color,
                Description = vm.Description,
                Status = vm.Status,

            };
            var email = principal.FindFirst(ClaimTypes.Name)!.Value;
            var currentUser = await context.CardUsers.FirstOrDefaultAsync(t => t.Email == email);

            if (currentUser is null)
                return TypedResults.Problem("Card User not found!");
            cardItem.User = currentUser!;
            cardItem.CardOwnerId = currentUser!.Id;
            cardItem.CreatedAt = DateTime.UtcNow;
            context.CardItems.Add(cardItem);
            await context.SaveChangesAsync();
            return TypedResults.Created($"/carditems/{cardItem.Id}", new CardItemVM(cardItem.Name, cardItem.Color!, cardItem.Description!, cardItem.Status, cardItem.CreatedAt));
        }


        public static async Task<IResult> UpdateCardItemAsync(IDbContextFactory<CardDbContext> _contextFactory, ClaimsPrincipal claimPrincipal, int id, CardItemViewModel vm)
        {
            using var _context = _contextFactory.CreateDbContext();
            var email = claimPrincipal.FindFirst(ClaimTypes.Name)!.Value;
            bool IsAdmin = claimPrincipal.IsInRole(Membership.Admin);
            if (await _context.CardItems.FirstOrDefaultAsync(t => t.User.Email == email && t.Id == id || IsAdmin) is CardItem cardItem)
            {
                cardItem.Description = vm.Description;
                cardItem.Status = vm.Status;
                cardItem.Name = vm.Name;
                cardItem.Color = vm.Color;
                await _context.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
        public static async Task<IResult> GetCardItemByIdAsync(IDbContextFactory<CardDbContext> _contextFactory, ClaimsPrincipal claimPrincipal, int id)
        {
            using var _context = _contextFactory.CreateDbContext();
            var email = claimPrincipal.FindFirst(ClaimTypes.Name)!.Value;
            return await _context.CardItems.FirstOrDefaultAsync(t => t.User.Email == email && t.Id == id) is CardItem cardItem ? TypedResults.Ok(new CardItemVM(cardItem.Name, cardItem.Color!, cardItem.Description!, cardItem.Status, cardItem.CreatedAt)) : TypedResults.NotFound();
        }
        public static async Task<IResult> GetAllCardItemsAsync(IDbContextFactory<CardDbContext> _contextFactory, ClaimsPrincipal claimsPrincipal,

            [FromQuery(Name = "searchTerm")] string? searchTerm = "",
            [FromQuery(Name = "Status")] Status  status  = Status.ToDo,
            [FromQuery(Name = "sortTerms")] string? sortTerms = "Name",
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 10,
            [FromQuery(Name = "searchByDate")] string? searchByDate="" )
        {
            using var _context = _contextFactory.CreateDbContext();
            var email = claimsPrincipal.FindFirst(ClaimTypes.Name)!.Value;

      

            bool IsAdmin = claimsPrincipal.IsInRole(Membership.Admin);//can be moved to AuthenticationFilterHandler

            var cardUser = await _context.CardUsers.FirstAsync(a => a.Email == email);
            var skipSize = pageSize * (page - 1);
            //we can lazy load with Include 
            //var test1 = await _context.CardItems.Include(a => a.User).ToListAsync();
            //var tests=await _context.CardItems.Include(a=>a.User).FirstOrDefaultAsync(a=>a.User.Email == email);

            IQueryable<CardItem> cardItemQuerable;

            if (string.IsNullOrWhiteSpace(searchTerm))
                cardItemQuerable = _context.CardItems
                    .Where(t => t.CardOwnerId == cardUser.Id || IsAdmin)
                    .Where(a=>a.Status==status);

            else
            {
                searchTerm = searchTerm.Trim().ToLower();
                cardItemQuerable = _context.CardItems
                    .Where(t => t.CardOwnerId == cardUser.Id || IsAdmin)
                    .Where(a => a.Status == status)
                    .Where(s => s.Name!.ToLower()!.Contains(searchTerm)
                     || s.Color!.Contains(searchTerm)
                     ).AsQueryable();
            }


            // card sorting
            // sort= name, color, date of creation //to do later
            // (arrange order name ascending and date descending
            if (!string.IsNullOrWhiteSpace(sortTerms))
            {
                var sortFields = sortTerms.Split(','); // ['name','_createdAt']
                StringBuilder orderQueryBuilder = new StringBuilder();
                // using reflection to get properties of book
                // propertyInfo= [Id,Name,CreatedAt] 
                PropertyInfo[] propertyInfo = typeof(CardItem).GetProperties();


                foreach (var field in sortFields)
                {
                    string sortOrder = "ascending";
                    
                    var sortField = field.Trim();
                    if (sortField.StartsWith("-"))
                    {
                        sortField = sortField.TrimStart('-');
                        sortOrder = "descending";
                    }
                    // property = 'Name'
                    // property = 'CreatedAt'
                    var property = propertyInfo.FirstOrDefault(a => a.Name.Equals(sortField, StringComparison.OrdinalIgnoreCase));
                    if (property == null)
                        continue;
                    // orderQueryBuilder= "Name ascending,CreatedAt descending, "
                    // it have trailing , and whitespace
                    orderQueryBuilder.Append($"{property.Name.ToString()} {sortOrder}, ");
                }
                // remove trailing , and whitespace here
                // orderQuery = ""Title ascending,Year descending"
                string orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
                if (!string.IsNullOrWhiteSpace(orderQuery))
                    // use System.Linq.Dynamic.Core namespace for this
                    cardItemQuerable = cardItemQuerable.OrderBy(orderQuery);
                else
                    cardItemQuerable = cardItemQuerable.OrderBy(a => a.Id);
            }


            //switch (sortOrder)
            //{
            //    case "Name":
            //        cardItemQuerable = cardItemQuerable.OrderByDescending(s => s.Name);
            //        break;
            //    case "Date":
            //        cardItemQuerable = cardItemQuerable.OrderBy(s => s.CreatedAt);
            //        break;
            //    case "date_desc":
            //        cardItemQuerable = cardItemQuerable.OrderByDescending(s => s.CreatedAt);
            //        break;
            //    default:
            //        cardItemQuerable = cardItemQuerable.OrderBy(s => s.Name);
            //        break;
            //}





            var totalCount = await cardItemQuerable.CountAsync();                                                                     
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var pagedCardItems = await cardItemQuerable
                .Skip((page - 1) * pageSize)
                .Select(c => new CardItemVM(c.Name, c.Color!, c.Description!, c.Status, c.CreatedAt))
                .Take(pageSize).ToListAsync();

            //you can use Carter to add to Add pagination headers to the response
          

            return TypedResults.Ok(new PagedResultsViewModel<CardItemVM>()
            {
                PageNumber = page,
                PageSize = pageSize,
                Results = pagedCardItems,
                TotalCount = totalCount,
                TotalPages = totalPages,

            });
        }

        public static async Task<IResult> DeleteCardItemAsync(IDbContextFactory<CardDbContext> _contextFactory, ClaimsPrincipal claimPrincipal, int id)
        {
            using var _context = _contextFactory.CreateDbContext();
            var email = claimPrincipal.FindFirst(ClaimTypes.Name)!.Value;
            bool IsAdmin = claimPrincipal.IsInRole(Membership.Admin);
            var cardUser = await _context.CardUsers.FirstAsync(a => a.Email == email);
            if (await _context.CardItems.FirstOrDefaultAsync(t => (t.CardOwnerId == cardUser.Id && t.Id == id) || IsAdmin) is CardItem cardItem)
            {
                _context.CardItems.Remove(cardItem);
                await _context.SaveChangesAsync();
                return TypedResults.NoContent();
            }
            return TypedResults.NotFound();
        }
    }
}
