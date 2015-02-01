using System;
using System.Collections.Generic;
using System.Data.Linq;
using FluentAssertions;
using FluentAssertions.Common;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocks.SimpleInjector.NotThreadSafeCheck;
using Rocks.SimpleInjector.NotThreadSafeCheck.Models;
using Rocks.SimpleInjector.Tests.Library;
using Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Services;
using Rocks.SimpleInjector.Tests.NotThreadSafeCheck.Suts;
using SimpleInjector;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck
{
    [TestClass]
    public class ThreadSafetyCheckerTests
    {
        #region Public methods

        [TestMethod]
        public void WithTransientField_ReturnsIt ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithTransientField));


            // assert
            ShouldHaveViolation<FieldInfo> (result, ThreadSafetyViolationType.NonSingletonRegistration);
        }


        [TestMethod]
        public void WithTransientFieldFromBase_ReturnsIt ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithTransientFieldFromBase));


            // assert
            ShouldHaveViolation<FieldInfo> (result, ThreadSafetyViolationType.NonSingletonRegistration);
        }


        [TestMethod]
        public void WithSingletonField_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithSingletonField));


            // assert
            ShouldHaveNoViolations (result);
        }


        [TestMethod]
        public void WithSingletonAndTransientField_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithSingletonAndTransientField));


            // assert
            ShouldHaveViolation<FieldInfo> (result, ThreadSafetyViolationType.NonSingletonRegistration, "transientService");
        }


        [TestMethod]
        public void WithEvent_ReturnsIt ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithEvent));


            // assert
            ShouldHaveViolation<EventInfo> (result, ThreadSafetyViolationType.EventFound, "Event");
        }


        [TestMethod]
        public void WithTransientAutoProperty_ReturnsIt ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithTransientProperty));


            // assert
            ShouldHaveViolation<PropertyInfo> (result,
                                               ThreadSafetyViolationType.NonSingletonRegistration,
                                               "TransientService",
                                               expectedCount: 1);
        }


        [TestMethod]
        public void WithNonReadonlyField_ReturnsIt ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithNonReadonlyField));


            // assert
            ShouldHaveViolation<FieldInfo> (result, ThreadSafetyViolationType.NonReadonlyMember);
        }


        [TestMethod]
        public void WithReadonlyMutableField_ReturnsIt ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithReadonlyMutableField));


            // assert
            ShouldHaveViolation<FieldInfo> (result, ThreadSafetyViolationType.MutableReadonlyMember);
        }


        [TestMethod]
        public void WithReadonlyValueTypeField_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithReadonlyValueTypeField));


            // assert
            ShouldHaveNoViolations (result);
        }


        [TestMethod]
        public void WithPropertyWithSet_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithPropertyWithSet));


            // assert
            ShouldHaveViolation<PropertyInfo> (result, ThreadSafetyViolationType.NonReadonlyMember, "String");
        }


        [TestMethod]
        public void WithValueTypePropertyWithoutSet_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithValueTypePropertyWithoutSet));


            // assert
            ShouldHaveNoViolations (result);
        }


        [TestMethod]
        public void WithMutablePropertyWithoutSet_ReturnsIt ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithMutablePropertyWithoutSet));


            // assert
            ShouldHaveViolation<PropertyInfo> (result, ThreadSafetyViolationType.MutableReadonlyMember, "Object");
        }


        [TestMethod]
        public void WithNotMutableReferenceReadonlyFields_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithNotMutableReferenceReadonlyFields));


            // assert
            ShouldHaveNoViolations (result);
        }


        [TestMethod]
        public void WithMutableMembersWithNotMutableAttribute_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithMutableMembersWithNotMutableAttribute));


            // assert
            ShouldHaveNoViolations (result);
        }


        [TestMethod]
        public void WithThreadSafeReadonlyReferenceMembers_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithThreadSafeReadonlyReferenceMembers));


            // assert
            ShouldHaveNoViolations (result);
        }


        [TestMethod]
        public void WithSelfReferenceReadonlyMembers_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithSelfReferenceReadonlyMembers));


            // assert
            ShouldHaveNoViolations (result);
        }


        [TestMethod]
        public void WithCyclicReferenceReadonlyMembers_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithCyclicReferenceReadonlyMembers));


            // assert
            ShouldHaveNoViolations (result);
        }


        [TestMethod]
        public void WithCyclicReferenceAndNotThreadSafeMembers_ReturnsThem ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithCyclicReferenceAndNotThreadSafeMembers));


            // assert
            ShouldHaveViolation<PropertyInfo> (result, ThreadSafetyViolationType.MutableReadonlyMember, "List");
        }


        [TestMethod]
        public void LinqDataContext_ReturnsViolations ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (DataContext));


            // assert
            ShouldHaveViolations (result);
        }


        [TestMethod]
        public void WithBaseClassWithLinqDataContextProperty_ReturnsIt ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithBaseClassWithLinqDataContextProperty));


            // assert
            ShouldHaveViolation<PropertyInfo> (result, ThreadSafetyViolationType.MutableReadonlyMember, "DataContext");
        }


        [TestMethod]
        public void WithStaticReadonlyMutableField_ReturnsIt ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithStaticReadonlyMutableField));


            // assert
            ShouldHaveViolation<FieldInfo> (result, ThreadSafetyViolationType.MutableReadonlyMember, "List");
        }


        [TestMethod]
        public void WithStaticReadonlyMutableProperty_ReturnsIt ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithStaticReadonlyMutableProperty));


            // assert
            ShouldHaveViolation<PropertyInfo> (result, ThreadSafetyViolationType.MutableReadonlyMember, "List");
        }

        #endregion

        #region Private methods

        private static ThreadSafetyChecker CreateSut ()
        {
            var container = new Container ();

            container.Register<TransientService> (Lifestyle.Transient);
            container.RegisterSingle<SingletonService> ();

            var sut = new ThreadSafetyChecker (container);

            return sut;
        }


        private static void ShouldHaveNoViolations (IReadOnlyList<NotThreadSafeMemberInfo> result)
        {
            result.Should ().BeEmpty (because: FormatBecause (result));
        }


        private static void ShouldHaveViolations (IReadOnlyList<NotThreadSafeMemberInfo> result)
        {
            result.Should ().NotBeEmpty (because: FormatBecause (result));
        }


        private static void ShouldHaveViolation<TMember> (IReadOnlyList<NotThreadSafeMemberInfo> result,
                                                          ThreadSafetyViolationType violationType,
                                                          string memberName = "member",
                                                          int? expectedCount = null)
        {
            result.Should ().NotBeNullOrEmpty ();

            if (expectedCount != null)
                result.Should ().HaveCount (expectedCount.Value, because: FormatBecause (result));

            result.Should ().Contain (x => x.ViolationType == violationType &&
                                           x.Member.GetType ().IsSameOrInherits (typeof (TMember)) &&
                                           x.Member.Name == memberName);
        }


        private static string FormatBecause (IEnumerable<NotThreadSafeMemberInfo> result)
        {
            return "because: " + Environment.NewLine +
                   Environment.NewLine +
                   string.Join (Environment.NewLine,
                                result.Select (x => "• " + x)) +
                   Environment.NewLine + Environment.NewLine;
        }

        #endregion
    }
}