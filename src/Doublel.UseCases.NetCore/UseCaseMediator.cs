using System;
using Microsoft.Extensions.DependencyInjection;

namespace Doublel.UseCases.NetCore
{
    public class UseCaseMediator
    {
        private readonly IServiceProvider _provider;

        public UseCaseMediator(IServiceProvider provider)
        {
            _provider = provider;
        }

        public TResult Execute<TUseCase, TData, TResult>(TUseCase useCase)
            where TUseCase : UseCase<TData, TResult>
        {
            var executor = _provider.GetService<UseCaseExecutor<TUseCase, TData, TResult>>();
            var handler = _provider.GetService<IUseCaseHandler<TUseCase, TData, TResult>>();
            return executor.ExecuteUseCase(useCase, handler);
        }
    }
}
