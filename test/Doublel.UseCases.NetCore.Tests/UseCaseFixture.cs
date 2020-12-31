using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Doublel.UseCases.NetCore.Tests
{
    public class UseCaseFixture
    {
        public IServiceProvider ServiceProvider { get; }
        public UseCaseFixture()
        {
            var services = new ServiceCollection();
            services.AddSingleton(new Mock<IUseCaseLogRepository>().Object);
            var actor = new Mock<IApplicationActor>();

            actor.SetupGet(x => x.AllowedUseCaseIds).Returns(new List<int> { 1, 2, 3, 4, 5 });
            actor.SetupGet(x => x.Identifier).Returns(1);
            actor.SetupGet(x => x.Identity).Returns("test");

            services.AddUseCases(new Assembly[] { GetType().Assembly });
            services.AddSingleton(actor.Object);

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
