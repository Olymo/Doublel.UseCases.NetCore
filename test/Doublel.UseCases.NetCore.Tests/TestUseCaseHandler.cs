using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doublel.UseCases.NetCore.Tests
{
    public class TestUseCaseHandler : IUseCaseHandler<TestUseCase, int, int> 
    {
        public int Handle(TestUseCase useCase) => useCase.Data * 2;
    }
}
