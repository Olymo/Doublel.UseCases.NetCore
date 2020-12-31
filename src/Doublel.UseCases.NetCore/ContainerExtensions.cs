using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Doublel.UseCases.NetCore
{
    public static class ContainerExtensions
    {
        public static void AddUseCases(this IServiceCollection services, Assembly[] assemblies, Type useCaseExecutorType = null)
        {
            if(useCaseExecutorType == null)
            {
                services.AddTransient(typeof(UseCaseExecutor<,,>), typeof(DefaultUseCaseExecutor<,,>));
            }
            else
            {
                services.AddTransient(typeof(UseCaseExecutor<,,>), useCaseExecutorType);
            }
            services.AddTransient<UseCaseMediator>();
            foreach (var assmebly in assemblies)
            {
                services.AddClassesAsImplementedInterface(assmebly, typeof(IValidator<>), ServiceLifetime.Transient);
                services.AddClassesAsImplementedInterface(assmebly, typeof(IUseCaseSubscriber<,,>), ServiceLifetime.Transient);
                services.AddClassesAsImplementedInterface(assmebly, typeof(IUseCaseHandler<,,>), ServiceLifetime.Transient);
                services.AddClassesAsImplementedInterface(assmebly, typeof(IUseCaseSubscriber<,,>), ServiceLifetime.Transient);
            }
        }

        private static void AddClassesAsImplementedInterface(
                this IServiceCollection services,
                Assembly assembly,
                Type compareType,
                ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            foreach (var type in assembly.GetTypesAssignableTo(compareType))
            {
                if (type.Name.Contains("Generic"))
                {
                    continue;
                }
                foreach (var implementedInterface in type.ImplementedInterfaces)
                {
                    switch (lifetime)
                    {
                        case ServiceLifetime.Scoped:
                            services.AddScoped(implementedInterface, type);
                            break;

                        case ServiceLifetime.Singleton:
                            services.AddSingleton(implementedInterface, type);
                            break;

                        case ServiceLifetime.Transient:
                            services.AddTransient(implementedInterface, type);
                            break;
                    }
                }
            }
        }

        private static List<TypeInfo> GetTypesAssignableTo(this Assembly assembly, Type compareType)
        {
            var typeInfoList = assembly.DefinedTypes.Where(x => x.IsClass
                                && !x.IsAbstract
                                && x != compareType
                                && x.GetInterfaces()
                                        .Any(i => i.IsGenericType
                                                && i.GetGenericTypeDefinition() == compareType))?.ToList();

            return typeInfoList;
        }

    }
}
