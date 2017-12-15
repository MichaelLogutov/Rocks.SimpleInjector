using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Common;
using System.Linq;
using System.Reflection;
using Rocks.SimpleInjector.NotThreadSafeCheck;
using Rocks.SimpleInjector.NotThreadSafeCheck.Models;
using Rocks.SimpleInjector.Tests.Library;
using Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Services;
using Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts;
using SimpleInjector;
using Xunit;

#if NET461 || NET471
    using System.Data.Entity;
#endif
#if NETCOREAPP2_0
    using Microsoft.EntityFrameworkCore;
#endif

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck
{
    public class ThreadSafetyCheckerTests
    {
        #region Public methods

        [Fact]
        public void WithTransientField_ReturnsIt()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithTransientField));


            // assert
            ShouldHaveViolation<FieldInfo>(result, ThreadSafetyViolationType.NonSingletonRegistration);
        }


        [Fact]
        public void WithTransientFieldFromBase_ReturnsIt()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithTransientFieldFromBase));


            // assert
            ShouldHaveViolation<FieldInfo>(result, ThreadSafetyViolationType.NonSingletonRegistration);
        }


        [Fact]
        public void WithSingletonField_ReturnsNothing()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithSingletonField));


            // assert
            ShouldHaveNoViolations(result);
        }


        [Fact]
        public void WithSingletonAndTransientField_ReturnsNothing()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithSingletonAndTransientField));


            // assert
            ShouldHaveViolation<FieldInfo>(result, ThreadSafetyViolationType.NonSingletonRegistration, "transientService");
        }


        [Fact]
        public void WithEvent_ReturnsIt()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithEvent));


            // assert
            ShouldHaveViolation<EventInfo>(result, ThreadSafetyViolationType.EventFound, "Event");
        }


        [Fact]
        public void WithTransientAutoProperty_ReturnsIt()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithTransientProperty));


            // assert
            ShouldHaveViolation<PropertyInfo>(result,
                                              ThreadSafetyViolationType.NonSingletonRegistration,
                                              "TransientService",
                                              expectedCount: 1);
        }


        [Fact]
        public void WithNonReadonlyField_ReturnsIt()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithNonReadonlyField));


            // assert
            ShouldHaveViolation<FieldInfo>(result, ThreadSafetyViolationType.NonReadonlyMember);
        }


        [Fact]
        public void WithReadonlyMutableField_ReturnsIt()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithReadonlyMutableField));


            // assert
            ShouldHaveViolation<FieldInfo>(result, ThreadSafetyViolationType.MutableReadonlyMember);
        }


        [Fact]
        public void WithReadonlyValueTypeField_ReturnsNothing()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithReadonlyValueTypeField));


            // assert
            ShouldHaveNoViolations(result);
        }


        [Fact]
        public void WithPropertyWithSet_ReturnsNothing()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithPropertyWithSet));


            // assert
            ShouldHaveViolation<PropertyInfo>(result, ThreadSafetyViolationType.NonReadonlyMember, "String");
        }


        [Fact]
        public void WithValueTypePropertyWithoutSet_ReturnsNothing()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithValueTypePropertyWithoutSet));


            // assert
            ShouldHaveNoViolations(result);
        }


        [Fact]
        public void WithMutablePropertyWithoutSet_ReturnsIt()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithMutablePropertyWithoutSet));


            // assert
            ShouldHaveViolation<PropertyInfo>(result, ThreadSafetyViolationType.MutableReadonlyMember, "Object");
        }


        [Fact]
        public void WithNotMutableReferenceReadonlyFields_ReturnsNothing()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithNotMutableReferenceReadonlyFields));


            // assert
            ShouldHaveNoViolations(result);
        }


        [Fact]
        public void WithMutableMembersWithThreadSafeAttribute_ReturnsNothing()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithMutableMembersWithThreadSafeAttribute));


            // assert
            ShouldHaveNoViolations(result);
        }


        [Fact]
        public void WithThreadSafeReadonlyReferenceMembers_ReturnsNothing()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithThreadSafeReadonlyReferenceMembers));


            // assert
            ShouldHaveNoViolations(result);
        }


        [Fact]
        public void WithSelfReferenceReadonlyMembers_ReturnsNothing()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithSelfReferenceReadonlyMembers));


            // assert
            ShouldHaveNoViolations(result);
        }


        [Fact]
        public void WithCyclicReferenceReadonlyMembers_ReturnsNothing()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithCyclicReferenceReadonlyMembers));


            // assert
            ShouldHaveNoViolations(result);
        }


        [Fact]
        public void WithCyclicReferenceAndNotThreadSafeMembers_ReturnsThem()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithCyclicReferenceAndNotThreadSafeMembers));


            // assert
            ShouldHaveViolation<PropertyInfo>(result, ThreadSafetyViolationType.MutableReadonlyMember, "List");
        }


        [Fact]
        public void EntityFrameworkDbContext_ReturnsViolations()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(DbContext));


            // assert
            ShouldHaveViolations(result);
        }


        [Fact]
        public void WithBaseClassWithLinqDbContextProperty_ReturnsIt()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithBaseClassWithLinqDbContextProperty));


            // assert
            ShouldHaveViolation<PropertyInfo>(result, ThreadSafetyViolationType.MutableReadonlyMember, "DbContext");
        }


        [Fact]
        public void WithStaticReadonlyMutableField_ReturnsIt()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithStaticReadonlyMutableField));


            // assert
            ShouldHaveViolation<FieldInfo>(result, ThreadSafetyViolationType.MutableReadonlyMember, "List");
        }


        [Fact]
        public void WithStaticReadonlyMutableProperty_ReturnsIt()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithStaticReadonlyMutableProperty));


            // assert
            ShouldHaveViolation<PropertyInfo>(result, ThreadSafetyViolationType.MutableReadonlyMember, "List");
        }


        [Fact]
        public void WithCompilerGeneratedCachedAnonymousMethodDelegate_ReturnsNothing()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithCompilerGeneratedCachedAnonymousMethodDelegate));


            // assert
            ShouldHaveNoViolations(result);
        }


        [Fact]
        public void WithWithConstantField_ReturnsNothing()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithConstantField));


            // assert
            ShouldHaveNoViolations(result);
        }


        [Fact]
        public void WithBaseGenericThreadSafeClass_ReturnsNothing()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithBaseGenericThreadSafeClass));


            // assert
            ShouldHaveNoViolations(result);
        }


        [Fact]
        public void WithBaseGenericClassAndMutableReadonlyProperty_ReturnsIt()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithBaseGenericClassAndMutableReadonlyProperty));


            // assert
            ShouldHaveViolation<PropertyInfo>(result, ThreadSafetyViolationType.MutableReadonlyMember, "Property");
        }


        [Fact]
        public void WithKnownNotMutableGenericTypeButWithMutableArgument_ReturnsIt()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithKnownNotMutableGenericTypeButWithMutableArgument));


            // assert
            ShouldHaveViolation<MemberInfo>(result, ThreadSafetyViolationType.MutableReadonlyMember);
        }


        [Fact]
        public void WithKnownNotMutableGenericTypesAndComplexThreadSafeArguments_ReturnsIt()
        {
            // arrange
            var sut = CreateSut();


            // act
            var result = sut.Check(typeof(SutWithKnownNotMutableGenericTypesAndComplexThreadSafeArguments));


            // assert
            ShouldHaveNoViolations(result);
        }

        #endregion

        #region Private methods

        private static ThreadSafetyChecker CreateSut()
        {
            var container = new Container();

            container.Register<TransientService>(Lifestyle.Transient);
            container.RegisterSingleton<SingletonService>();

            var sut = new ThreadSafetyChecker(container);

            return sut;
        }


        private static void ShouldHaveNoViolations(IReadOnlyList<NotThreadSafeMemberInfo> result)
        {
            result.Should().BeEmpty(because: FormatBecause(result));
        }


        private static void ShouldHaveViolations(IReadOnlyList<NotThreadSafeMemberInfo> result)
        {
            result.Should().NotBeEmpty(because: FormatBecause(result));
        }


        private static void ShouldHaveViolation<TMember>(IReadOnlyList<NotThreadSafeMemberInfo> result,
                                                         ThreadSafetyViolationType violationType,
                                                         string memberName = "member",
                                                         int? expectedCount = null)
        {
            result.Should().NotBeNullOrEmpty();

            if (expectedCount != null)
                result.Should().HaveCount(expectedCount.Value, because: FormatBecause(result));

            result.Should().Contain(x => x.ViolationType == violationType &&
                                         x.Member.GetType().IsSameOrInherits(typeof(TMember)) &&
                                         x.Member.Name == memberName);
        }


        private static string FormatBecause(IEnumerable<NotThreadSafeMemberInfo> result)
        {
            return "because: " + Environment.NewLine +
                   Environment.NewLine +
                   string.Join(Environment.NewLine,
                               result.Select(x => "• " + x)) +
                   Environment.NewLine + Environment.NewLine;
        }

        #endregion
    }
}