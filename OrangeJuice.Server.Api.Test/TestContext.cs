using System;

namespace OrangeJuice.Server.Api.Test
{
    internal class TestContext : IDisposable
    {
        private readonly Action _disposeAction;

        public TestContext(Action testAction)
            : this(testAction, () => { })
        {
        }

        public TestContext(Action testAction, Action disposeAction = null)
        {
            if (testAction == null)
                throw new ArgumentNullException("testAction");
            testAction();

            if (disposeAction == null)
                disposeAction = () => { };

            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            _disposeAction();
        }
    }
}