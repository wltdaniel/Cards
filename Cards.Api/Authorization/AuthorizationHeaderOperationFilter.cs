using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cards.Api.Authorization
{
    public class AuthorizationHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var endPointMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            var isCadUserAuthorized = endPointMetadata.Any(metaItem => metaItem is AuthorizeAttribute);
            var allowAnonymous = endPointMetadata.Any(metaItem => metaItem is AllowAnonymousAttribute);

            if (!isCadUserAuthorized || allowAnonymous)
            {
                return;
            }
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Security = new List<OpenApiSecurityRequirement>
            {

             new() {
             {
                 new OpenApiSecurityScheme {
                     Reference = new OpenApiReference {
                         Id = "Bearer",
                         Type = ReferenceType.SecurityScheme
                     }
                 },
                 new List<string>()
             }
         }
            };
        }
    }
}
