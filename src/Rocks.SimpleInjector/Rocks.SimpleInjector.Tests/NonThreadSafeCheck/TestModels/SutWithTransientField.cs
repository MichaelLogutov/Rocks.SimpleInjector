namespace Rocks.SimpleInjector.Tests.NonThreadSafeCheck.TestModels
{
    internal class SutWithTransientField
    {
#pragma warning disable 169
        private readonly TransientService member;
#pragma warning restore 169
    }
}