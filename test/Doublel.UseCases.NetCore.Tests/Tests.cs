using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Doublel.UseCases.NetCore.Tests
{
    public class Tests : IClassFixture<UseCaseFixture>
    {

        private readonly UseCaseFixture _fixture;

        public Tests(UseCaseFixture fixture) => _fixture = fixture;

        [Fact]
        public void UseCaseRegistrationWorks()
        {
            _fixture.ServiceProvider.Should().NotBeNull();
            _fixture.ServiceProvider.GetService<UseCaseMediator>().Should().NotBeNull();
        }

        [Fact]
        public void CanExecuteUseCase()
        {
            var mediator = _fixture.ServiceProvider.GetService<UseCaseMediator>();
            var result = mediator.Execute<TestUseCase, int, int>(new TestUseCase(5));
            
            result.Should().Be(10);
        }

        [Fact]
        public void CustomUseCaseExecutorAndExecutesCorrectly()
        {
            var services = new ServiceCollection();
            services.AddSingleton(new Mock<IUseCaseLogRepository>().Object);
            var actor = new Mock<IApplicationActor>();

            actor.SetupGet(x => x.AllowedUseCases).Returns(new List<string> { "Test name", "Test name 1" });
            actor.SetupGet(x => x.Identifier).Returns(1);
            actor.SetupGet(x => x.Identity).Returns("test");

            services.AddUseCases(new Assembly[] { GetType().Assembly }, typeof(TestUseCaseExecutor<,,>));
            services.AddSingleton(actor.Object);

            var serviceProvider = services.BuildServiceProvider();

            var mediator = serviceProvider.GetService<UseCaseMediator>();
            var result = mediator.Execute<TestUseCase, int, int>(new TestUseCase(5));

            result.Should().Be(10);
            TestUseCaseExecutorInicialized.Should().BeTrue(); 
        }

        public static bool TestUseCaseExecutorInicialized = false;
    }
}
