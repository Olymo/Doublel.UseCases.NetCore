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

        public override int Id => 1;

        public override string Description => "Test description.";

        public override string Name => "Test name";
    }
}
