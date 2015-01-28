namespace Rocks.SimpleInjector.Tests.NotThreadSafeCheck.TestModels
{
    internal class SutWithTransientField
    {
#pragma warning disable 169
        private readonly TransientService member;
#pragma warning restore 169
    }
}