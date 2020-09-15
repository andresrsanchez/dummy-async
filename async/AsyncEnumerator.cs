using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace async
{
    class AsyncEnumerator
    {
        IEnumerable<string> SomeSource(int x) => new GeneratedEnumerable(x);
        async IAsyncEnumerable<string> SomeSourceAsync(int x, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            for (int i = 0; i < 5; i++)
            {
                await Task.Delay(100, cancellationToken); // simulate async something
                yield return $"result from SomeSource, x={x}, result {i}";
            }
        }

        public void demo_sync()
        {
            using (var iter = SomeSource(42).GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    var item = iter.Current;
                    Console.WriteLine(item);
                }
            }
        }

        public async ValueTask demo_async(
            CancellationToken cancellationToken = default, 
            CancellationToken tokenB = default, 
            CancellationToken tokenA = default)
        {
            await using (var iter = SomeSourceAsync(42).GetAsyncEnumerator(cancellationToken))
            {
                while (await iter.MoveNextAsync())
                {
                    var item = iter.Current;
                    Console.WriteLine(item);
                }
            }

            await foreach (var item in SomeSourceAsync(42)) { }

            await foreach (var item in SomeSourceAsync(42).WithCancellation(cancellationToken)) { }
            
            await foreach (var item in SomeSourceAsync(42, cancellationToken)) { }

            await foreach (var item in SomeSourceAsync(42, cancellationToken).WithCancellation(cancellationToken)) { }

            await foreach (var item in SomeSourceAsync(42, tokenA).WithCancellation(tokenB)) { }
        }


        class GeneratedEnumerable : IEnumerable<string>
        {
            private int x;
            public GeneratedEnumerable(int x)
                => this.x = x;

            public IEnumerator<string> GetEnumerator()
                => new GeneratedEnumerator(x);

            // non-generic fallback
            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }

        class GeneratedEnumerator : IEnumerator<string>
        {
            private int x, i;
            public GeneratedEnumerator(int x)
                => this.x = x;

            public string Current { get; private set; }

            // non-generic fallback
            object IEnumerator.Current => Current;

            // if we had "finally" code, it would go here
            public void Dispose() { }

            // our "advance" logic
            public bool MoveNext()
            {
                if (i < 5)
                {
                    Current = $"result from SomeSource, x={x}, result {i}";
                    i++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // this API is essentially deprecated and never used
            void IEnumerator.Reset() => throw new NotSupportedException();
        }
    }
}
