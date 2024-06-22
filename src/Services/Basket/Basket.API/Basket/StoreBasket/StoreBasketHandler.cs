
using static Discount.Grpc.DiscountProtoService;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    public class StoreBasketCommandHandler 
        (IBasketRepository repository, DiscountProtoServiceClient discountProto)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await deductDiscount(command.Cart, cancellationToken).ConfigureAwait(false);

            await repository.StoreBasket(command.Cart, cancellationToken);

            return new StoreBasketResult(command.Cart.UserName);
        }

        private async Task deductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            //TODO: communicate with Discount.Grpc and calculate latest prices of product into shopping cart
            foreach (var item in cart.Items)
            {
                var coupon = await discountProto.GetDiscountAsync(
                    new Discount.Grpc.GetDiscountRequest { ProductName = item.ProductName },
                    cancellationToken: cancellationToken);

                item.Price -= coupon.Amount;
            }
        }
    }
}
