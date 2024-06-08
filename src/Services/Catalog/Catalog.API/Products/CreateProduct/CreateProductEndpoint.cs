﻿
namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    public record CreateProductRespnose(Guid Id);

    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products",
                async (CreateProductRequest request, ISender sender) => 
                {
                    var command = request.Adapt<CreateProductCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<CreateProductRespnose>();

                    return Results.Created($"/product/{response.Id}", response);
                })
                .WithName("CreateProduct")
                .Produces<CreateProductRespnose>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Product")
                .WithDescription("Create Product");
        }
    }
}
