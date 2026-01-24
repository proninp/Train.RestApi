using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movies.Api.Swagger;

/// <summary>
/// Represents the OpenAPI/Swashbuckle operation filter used to document information provided, but not used.
/// </summary>
/// <remarks>This <see cref="IOperationFilter"/> is only required due to bugs in the <see cref="SwaggerGenerator"/>.
/// Once they are fixed and published, this class can be removed.</remarks>
public class SwaggerDefaultValues : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        operation.Deprecated |= apiDescription.IsDeprecated();
        
        foreach (var responseType in apiDescription.SupportedResponseTypes)
        {
            var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
            if (!operation.Responses!.TryGetValue(responseKey, out var response))
                continue;

            foreach (var contentType in response.Content!.Keys.ToArray())
            {
                if (responseType.ApiResponseFormats.All(x => x.MediaType != contentType))
                {
                    response.Content.Remove(contentType);
                }
            }
        }

        if (operation.Parameters is null || operation.Parameters.Count == 0)
            return;

        for (var i = 0; i < operation.Parameters.Count; i++)
        {
            if (operation.Parameters[i] is not OpenApiParameter parameter)
            {
                var p = operation.Parameters[i];
                parameter = new OpenApiParameter
                {
                    Name = p.Name,
                    In = p.In,
                    Description = p.Description,
                    Required = p.Required,
                    Deprecated = p.Deprecated,
                    AllowEmptyValue = p.AllowEmptyValue,
                    AllowReserved = p.AllowReserved,
                    Style = p.Style,
                    Explode = p.Explode,
                    Schema = p.Schema as OpenApiSchema,
                    Content = p.Content,
                    Extensions = p.Extensions
                };

                operation.Parameters[i] = parameter;
            }

            var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

            // Description
            parameter.Description ??= description.ModelMetadata?.Description;

            // Default value -> Schema.Default (JsonNode)
            if (description.DefaultValue is not null &&
                description.DefaultValue is not DBNull &&
                parameter.Schema is OpenApiSchema schema &&
                schema.Default is null &&
                description.ModelMetadata is not null)
            {
                var json = JsonSerializer.Serialize(description.DefaultValue, description.ModelMetadata.ModelType);
                schema.Default = JsonNode.Parse(json);
            }

            parameter.Required |= description.IsRequired;
        }
    }
}