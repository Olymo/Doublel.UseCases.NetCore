using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doublel.UseCases.NetCore.Tests
{
    public class TestUseCaseExecutor<TUseCase, TData, TResult> : UseCaseExecutor<TUseCase, TData, TResult> where TUseCase : UseCase<TData, TResult>
    {
        private readonly IApplicationActor _actor;
        private readonly IUseCaseLogRepository _repo;

        public TestUseCaseExecutor(IApplicationActor actor, IUseCaseLogRepository repo)
        {
            _actor = actor;
            _repo = repo;
        }

        protected override IUseCaseHandler<TUseCase, TData, TResult> MakeUseCaseHandlingDecoration(IUseCaseHandler<TUseCase, TData, TResult> handler)
        {
            Tests.TestUseCaseExecutorInicialized = true;
            var authorizationDecorator = new LoggingUseCaseDecorator<TUseCase, TData, TResult>(handler, _actor, _repo);
            return authorizationDecorator;
        }
    }
}
