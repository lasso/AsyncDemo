/*
This class demonstrates how to generate values in a background thread using the IAsyncEnumerable interface.
*/
class DemoValueFactory
{
    private int _minValue;
    private int _maxValue;

    private CancellationTokenSource? _cancellationTokenSource;

    public DemoValueFactory(int minValue, int maxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
    }

    public async Task StartProducing(TimeSpan? delay = null)
    {
        // Stop old thread producing values
        StopProducing();

        delay ??= TimeSpan.FromSeconds(10);
        _cancellationTokenSource = new CancellationTokenSource();
        await foreach (var current in Values(delay.Value).WithCancellation(_cancellationTokenSource.Token))
        {
            Console.Out.WriteLine($"Value: {current.Value}, GeneratedAt: {current.GeneratedAt}");
        }
    }

    public void StopProducing()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }

    private async IAsyncEnumerable<DemoValue> Values(TimeSpan delay)
    {
        if (_cancellationTokenSource == null)
        {
            yield break;
        }

        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            yield return ProduceValue();
            await Task.Delay(delay, _cancellationTokenSource.Token);
        }
    }

    private DemoValue ProduceValue() => new DemoValue(DateTimeOffset.Now, new Random().Next(_minValue, _maxValue + 1));
}