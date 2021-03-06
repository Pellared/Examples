﻿using Autofac;
using System.Reflection;
using Xunit;

namespace Pellared.Owned.Tests
{
    public class RegisterAutofacOwnedTests : RegisterOwnedTests
    {

        protected override void RegisterOwned(ContainerBuilder builder)
        {
            builder.RegisterAutofacOwned();
        }
    }

    public class RegisterCustomOwnedTests : RegisterOwnedTests
    {

        protected override void RegisterOwned(ContainerBuilder builder)
        {
            builder.RegisterCustomOwned();
        }
    }

    public abstract class RegisterOwnedTests
    {
        private readonly IContainer container;

        public RegisterOwnedTests()
        {
            var builder = new ContainerBuilder();
            RegisterOwned(builder);

            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsSelf();

            container = builder.Build();
        }

        protected abstract void RegisterOwned(ContainerBuilder containerBuilder);

        [Fact]
        public void Should_resolve_Owned()
        {
            var instance = container.Resolve<ClassWithOwned>();

            using (IOwned<Normal> result = instance.Owned)
            {
                Assert.IsType<Normal>(result.Value);
            }
        }

        private class ClassWithOwned
        {
            public IOwned<Normal> Owned { get; private set; }

            public ClassWithOwned(IOwned<Normal> owned)
            {
                Owned = owned;
            }
        }

        [Fact]
        public void Should_resolve_Factory_Normal()
        {
            var instance = container.Resolve<ClassWithFactory>();

            using (IOwned<Normal> result = instance.Factory.Create())
            {
                Assert.IsType<Normal>(result.Value);
            }
        }

        private class ClassWithFactory
        {
            public IFactory<Normal> Factory { get; private set; }

            public ClassWithFactory(IFactory<Normal> factory)
            {
                Factory = factory;
            }
        }

        [Fact]
        public void Should_resolve_Factory_WithArg()
        {
            var instance = container.Resolve<ClassWithFactoryWithArg>();
            var argument = "asd";

            using (IOwned<WithArg> result = instance.Factory.Create(argument))
            {
                Assert.IsType<WithArg>(result.Value);
                Assert.Equal(argument, result.Value.Argument);
            }
        }

        private class ClassWithFactoryWithArg
        {
            public IFactory<string, WithArg> Factory { get; private set; }

            public ClassWithFactoryWithArg(IFactory<string, WithArg> factory)
            {
                Factory = factory;
            }
        }

        [Fact]
        public void Should_resolve_Factory_WithArgOnly()
        {
            var instance = container.Resolve<ClassWithFactoryWithArgOnly>();

            using (IOwned<WithArg> result = instance.Factory.Create())
            {
                Assert.IsType<WithArg>(result.Value);
                Assert.Null(result.Value.Argument);
            }
        }

        private class ClassWithFactoryWithArgOnly
        {
            public IFactory<WithArg> Factory { get; private set; }

            public ClassWithFactoryWithArgOnly(IFactory<WithArg> factory)
            {
                Factory = factory;
            }
        }

        private class Normal
        {
        }

        private class WithArg
        {
            public string Argument { get; private set; }

            public WithArg()
            {
            }

            public WithArg(string argument)
            {
                Argument = argument;
            }
        }
    }
}
