using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doublel.UseCases.NetCore.Tests
{
    public class TestUseCase : UseCase<int, int>
    {
        public TestUseCase(int data) : base(data)
        {
        }

        public override string Id => "Test name";

        public override string Description => "Test description.";
    }
}
