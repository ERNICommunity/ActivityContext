using System;
using System.Collections.Generic;
using System.Threading;

namespace ActivityContext
{
    /// <summary>
    /// TODO
    /// </summary>
    public static class ActivityContext
    {
        private const string DefaultActivityName = "Activity";
        private static readonly AsyncLocal<KeyValuePair<string, Guid>[]> Context = new AsyncLocal<KeyValuePair<string, Guid>[]>();

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public static IDisposable Activity()
        {
            return Activity(DefaultActivityName);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IDisposable Activity(string name)
        {
            return Activity(name, Guid.NewGuid());
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IDisposable Activity(string name, Guid id)
        {
            var item = new KeyValuePair<string, Guid>(name, id);
            var currentContext = Context.Value;

            if (currentContext == null)
            {
                Context.Value = new[] { new KeyValuePair<string, Guid>(name, id) };
            }
            else
            {
                var newContext = new KeyValuePair<string, Guid>[currentContext.Length + 1];
                Array.Copy(currentContext, newContext, currentContext.Length);
                newContext[newContext.Length - 1] = item;
                Context.Value = newContext;
            }

            return new EndActivity(item);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<string, Guid>[] GetCurrentActivities()
        {
            return Context.Value ?? Array.Empty<KeyValuePair<string, Guid>>();
        }

        private class EndActivity : IDisposable
        {
            private readonly KeyValuePair<string, Guid> _item;

            public EndActivity(KeyValuePair<string, Guid> item)
            {
                _item = item;
            }

            public void Dispose()
            {
                var currentContext = Context.Value;
                if (currentContext != null)
                {
                    var index = Array.IndexOf(currentContext, _item);
                    if (index != -1)
                    {
                        var newContext = new KeyValuePair<string, Guid>[currentContext.Length - 1];
                        Array.Copy(currentContext, newContext, index);
                        Array.ConstrainedCopy(currentContext, index + 1, newContext, index, currentContext.Length - index - 1);
                        Context.Value = newContext;
                    }
                }
            }
        }
    }
}
