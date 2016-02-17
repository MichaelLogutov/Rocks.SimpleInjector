using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Rocks.SimpleInjector.Attributes;
using SimpleInjector;
using Xunit;

namespace Rocks.SimpleInjector.Tests
{
    public interface IServiceA
    {
    }

    public class ServiceA : IServiceA
    {
    }

    public interface IServiceB
    {
    }

    [NoAutoRegistration]
    public class ServiceB : IServiceB
    {
    }

    public class AutoRegistrationTests
    {
        [Fact]
        public void ByDefault_RegistersSelfImplementingTypes ()
        {
            // arrange
            var container = new Container { Options = { AllowOverridingRegistrations = true } };


            // act
            container.AutoRegisterSelfImplementingTypes (Assembly.GetExecutingAssembly (), Lifestyle.Transient);


            // assert
            container.GetRegistration (typeof (IServiceA)).Should ().NotBeNull ();
        }


        [Fact]
        public void NoAutoRegistrationSelfImplementingClass_DoesNotRegistersIt ()
        {
            // arrange
            var container = new Container { Options = { AllowOverridingRegistrations = true } };


            // act
            container.AutoRegisterSelfImplementingTypes (Assembly.GetExecutingAssembly (), Lifestyle.Transient);


            // assert
            container.GetRegistration (typeof (IServiceB)).Should ().BeNull ();
        }
    }
}