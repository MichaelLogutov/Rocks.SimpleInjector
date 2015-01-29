using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocks.SimpleInjector.NotThreadSafeCheck;
using Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels;
using SimpleInjector;

namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck
{
    [TestClass]
    public class GetNotThreadSafeMembersTests
    {
        #region Public methods

        [TestMethod]
        public void WithTransientField_ReturnsIt ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithTransientField));


            // assert
            ShouldHaveViolation<FieldInfo> (result, ThreadSafetyViolationType.NonSingletonRegistration);
        }


        [TestMethod]
        public void WithTransientFieldFromBase_ReturnsIt ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithTransientFieldFromBase));


            // assert
            ShouldHaveViolation<FieldInfo> (result, ThreadSafetyViolationType.NonSingletonRegistration);
        }


        [TestMethod]
        public void WithSingletonField_ReturnsNothing ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithSingletonField));


            // assert
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithSingletonAndTransientField_ReturnsNothing ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithSingletonAndTransientField));


            // assert
            ShouldHaveViolation<FieldInfo> (result, ThreadSafetyViolationType.NonSingletonRegistration, "transientService");
        }


        [TestMethod]
        public void WithEvent_ReturnsIt ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithEvent));


            // assert
            ShouldHaveViolation<EventInfo> (result, ThreadSafetyViolationType.EventFound, "Event");
        }


        [TestMethod]
        public void WithTransientAutoProperty_ReturnsIt ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithTransientProperty));


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
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithNonReadonlyField));


            // assert
            ShouldHaveViolation<FieldInfo> (result, ThreadSafetyViolationType.NonReadonlyMember);
        }


        [TestMethod]
        public void WithReadonlyMutableField_ReturnsIt ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithReadonlyMutableField));


            // assert
            ShouldHaveViolation<FieldInfo> (result, ThreadSafetyViolationType.MutableReadonlyMember);
        }


        [TestMethod]
        public void WithReadonlyValueTypeField_ReturnsNothing ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithReadonlyValueTypeField));


            // assert
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithPropertyWithSet_ReturnsNothing ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithPropertyWithSet));


            // assert
            ShouldHaveViolation<PropertyInfo> (result, ThreadSafetyViolationType.NonReadonlyMember, "String");
        }


        [TestMethod]
        public void WithValueTypePropertyWithoutSet_ReturnsNothing ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithValueTypePropertyWithoutSet));


            // assert
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithMutablePropertyWithoutSet_ReturnsNothing ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithMutablePropertyWithoutSet));


            // assert
            ShouldHaveViolation<PropertyInfo> (result, ThreadSafetyViolationType.MutableReadonlyMember, "Object");
        }


        [TestMethod]
        public void WithNotMutableReferenceReadonlyFields_ReturnsNothing ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithNotMutableReferenceReadonlyFields));


            // assert
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithMutableMembersWithNotMutableAttribute_ReturnsNothing ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithMutableMembersWithNotMutableAttribute));


            // assert
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithThreadSafeReadonlyReferenceMembers_ReturnsNothing ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithThreadSafeReadonlyReferenceMembers));


            // assert
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithSelfReferenceReadonlyMembers_ReturnsNothing ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithSelfReferenceReadonlyMembers));


            // assert
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithCyclicReferenceReadonlyMembers_ReturnsNothing ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithCyclicReferenceReadonlyMembers));


            // assert
            result.Should ().BeEmpty ();
        }


        [TestMethod]
        public void WithCyclicReferenceAndNotThreadSafeMembers_ReturnsNothing ()
        {
            // arrange
            var container = CreateContainer ();


            // act
            var result = container.GetNotThreadSafeMembers (typeof (SutWithCyclicReferenceAndNotThreadSafeMembers));


            // assert
            ShouldHaveViolation<PropertyInfo> (result, ThreadSafetyViolationType.MutableReadonlyMember, "List");
        }

        #endregion

        #region Private methods

        private static Container CreateContainer ()
        {
            var container = new Container ();

            container.Register<TransientService> (Lifestyle.Transient);
            container.RegisterSingle<SingletonService> ();

            return container;
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