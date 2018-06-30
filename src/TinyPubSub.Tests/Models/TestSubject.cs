using System.Threading.Tasks;
using TinyPubSubLib;

namespace TinyPubSub.Tests.Models
{
    public class TestSubject
    {
        public bool IsSuccessful
        {
            get;
            set;
        }

        [TinySubscribe("test")]
        public void DoEpicStuff()
        {
            IsSuccessful = true;
        }

        [TinySubscribe("test-async")]
        public async Task DoEpicAsyncStuff()
        {
            await Task.Delay(1);
            IsSuccessful = true;
        }

        /// <summary>
        /// This method will only be called if the Publish passes
        /// TestType as argument.
        /// </summary>
        /// <param name="data">Data.</param>
        [TinySubscribe("test-with-arguments")]
        public void DoEpicStuffWithArgument(TestType data)
        {
            if (data.DuckLength == 42)
            {
                IsSuccessful = true;
            }
        }

        /// <summary>
        /// This method will only be called if the Publish passes
        /// TestType as argument.
        /// </summary>
        /// <param name="data">Data.</param>
        [TinySubscribe("test-with-bad-arguments")]
        public void DoEpicStuffWithBadArgument(TestType data)
        {
            if (data.DuckLength == 42)
            {
                IsSuccessful = true;
            }
        }
    }
}
