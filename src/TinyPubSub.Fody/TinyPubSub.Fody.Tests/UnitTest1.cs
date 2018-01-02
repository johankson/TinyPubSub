using System;
using Fody;
using Xunit;

#pragma warning disable 618

namespace TinyPubSub.Fody.Tests
{
    public class WeaverTests
    {
        TestResult testResult;

        public WeaverTests()
        {
            var weavingTask = new ModuleWeaver();
            testResult = weavingTask.ExecuteTestRun("AssemblyToTest.dll");
        }

        [Fact]
        public void ValidateHelloWorldIsInjected()
        {
        //    var type = testResult.Assembly.GetType("TheNamespace.Hello");
        //    var instance = (dynamic)Activator.CreateInstance(type);

        //    Assert.Equal("Hello World", instance.World());
        }
    }
}
