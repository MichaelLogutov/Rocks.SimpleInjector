using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocks.SimpleInjector.NotThreadSafeCheck;
using Rocks.SimpleInjector.NotThreadSafeCheck.Models;
using Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels;
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
            result.Should ().BeEmpty ();
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
            result.Should ().BeEmpty ();
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
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithMutablePropertyWithoutSet_ReturnsNothing ()
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
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithMutableMembersWithNotMutableAttribute_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithMutableMembersWithNotMutableAttribute));


            // assert
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithThreadSafeReadonlyReferenceMembers_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithThreadSafeReadonlyReferenceMembers));


            // assert
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithSelfReferenceReadonlyMembers_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithSelfReferenceReadonlyMembers));


            // assert
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithCyclicReferenceReadonlyMembers_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithCyclicReferenceReadonlyMembers));


            // assert
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithCyclicReferenceAndNotThreadSafeMembers_ReturnsNothing ()
        {
            // arrange
            var sut = CreateSut ();


            // act
            var result = sut.Check (typeof (SutWithCyclicReferenceAndNotThreadSafeMembers));


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


        private static void ShouldHaveViolation<TMember> (IList<NotThreadSafeMemberInfo> result,
                                                          ThreadSafetyViolationType violationType,
                                                          string memberName = "member",
                                                          int? expectedCount = null)
        {
            result.Should ().NotBeNullOrEmpty ();

            if (expectedCount != null)
                result.Should ().HaveCount (expectedCount.Value);

            result.Should ().Contain (x => x.ViolationType == violationType &&
                                           x.Member.GetType ().IsSameOrInherits (typeof (TMember)) &&
                                           x.Member.Name == memberName);
        }

        #endregion
    }
}