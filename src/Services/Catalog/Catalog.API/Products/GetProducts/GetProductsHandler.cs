
namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResponse>;
    public record GetProductsResponse(IEnumerable<Product> Products);
    internal class GetProductsQueryHandler
        (IDocumentSession session)
        : IQueryHandler<GetProductsQuery, GetProductsResponse>
    {
        public async Task<GetProductsResponse> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>()
                .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 2, cancellationToken);

            return new GetProductsResponse(products);
        }
    }
}
