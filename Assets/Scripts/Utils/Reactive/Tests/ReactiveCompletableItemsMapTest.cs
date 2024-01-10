using HNS.domain;
using NUnit.Framework;
using UniRx;

namespace Utils.Reactive.Tests
{
    public class ReactiveCompletableItemsMapTest
    {
        [Test]
        public void TestGetItem()
        {
            var map = new ReactiveCompletableItemsMap<long, string>();
            var key = 0L;
            string futureItem = "";

            map.GetInFuture(key).Subscribe(_ => futureItem = _);

            Assert.False(map.Get(key, out var notExistentItemFlow));

            map.Set(key, "ab");

            string existentItem = "";

            Assert.True(map.Get(key, out var existentItemFlow));

            existentItemFlow.Subscribe(_ => existentItem = _);

            Assert.True(existentItem == "ab");
            Assert.True(futureItem == "ab");
        }

        [Test]
        public void TestItemFlowCompletes()
        {
            var map = new ReactiveCompletableItemsMap<long, string>();
            var key = 0L;
            string futureItem = "";
            bool completed = false;

            map.GetInFuture(key).DoOnCompleted(() => completed = true).Subscribe(_ => futureItem = _);

            Assert.False(map.Get(key, out var notExistentItemFlow));

            map.Set(key, "ab");

            string existentItem = "";

            Assert.True(map.Get(key, out var existentItemFlow));

            existentItemFlow.Subscribe(_ => existentItem = _);

            map.Clear();
            map.Set(key, "abcdef");

            Assert.True(existentItem == "ab");
            Assert.True(futureItem == "ab");
            Assert.True(completed);
        }
    }
}