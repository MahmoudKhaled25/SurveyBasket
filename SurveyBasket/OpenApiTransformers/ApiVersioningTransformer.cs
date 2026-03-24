using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace SurveyBasket.OpenApiTransformers;

public sealed class ApiVersioningTransformer(ApiVersionDescription apiVersionDescription) : IOpenApiDocumentTransformer
{
    private readonly ApiVersionDescription Description = apiVersionDescription;

    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Info = new()
        {
            Title = "Survey Basket Api",
            Version = Description.ApiVersion.ToString(),
            Description = document.Info.Description,
        };
       return Task.CompletedTask;
    }
}
