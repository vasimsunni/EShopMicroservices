using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface ICommandHandler<in TCommand>: ICommandHandler<TCommand,Unit>
        where TCommand : ICommand<Unit>
    {

    }

    public interface ICommandHandler<in ITCommand, TRespose> : IRequestHandler<ITCommand, TRespose>
        where ITCommand : ICommand<TRespose>
        where TRespose:notnull
    {
    }
}
