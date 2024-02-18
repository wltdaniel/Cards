using Cards.Api.EndPoints.CardItems;
using Cards.Api.Validators;
using Cards.Api.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Tests
{
    public class CardApiTests
    {


        //[Fact]
        //public async Task CreateCardItem()
        //{
        //    var testDbContextFactory = new TestContextDbFactory();
        //    var user = new ClaimsPrincipal(new ClaimsIdentity(
        //        new Claim[] { new Claim(ClaimTypes.Name, "admin@card.com") }, "admin@card.com"));
        //    var name = "Test";
        //    var cardItemInput = new CardItemViewModel() { 
        //        Color="#000000",
        //        Description="",
        //        Name="Test",
        
        //    };
        //    var cardItemOutputResult = await CardItemApi.CreateCardItemAsync(
        //        testDbContextFactory, user, cardItemInput, new  CardItemViewModelValidator(testDbContextFactory));

        //    Assert.IsType<Created<CardItemVM>>(cardItemOutputResult);
        //    var createdCardItemOutput = cardItemOutputResult as Created<CardItemVM>;
        //    Assert.Equal(201, createdCardItemOutput!.StatusCode);
        //    var actual = createdCardItemOutput!.Value!.Name;
        //    Assert.Equal(name, actual);
        //    var actualLocation = createdCardItemOutput!.Location;
        //    var expectedLocation = $"/carditems/3";
        //    Assert.Equal(expectedLocation, actualLocation);
        //}



    }
}
