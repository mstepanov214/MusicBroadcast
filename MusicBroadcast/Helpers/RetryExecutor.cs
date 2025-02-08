namespace MusicBroadcast.Helpers;

public class RetryExecutor
{
    private readonly int _maxRetryCount;
    private Action<Exception>? _onRetry;
    private readonly TimeSpan _initialDelay;

    public RetryExecutor(int maxRetryCount, TimeSpan? initialDelay = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxRetryCount);
        _maxRetryCount = maxRetryCount;
        _initialDelay = initialDelay ?? TimeSpan.Zero;
    }

    public RetryExecutor OnRetry(Action<Exception> handler)
    {
        if (_onRetry is not null)
        {
            throw new InvalidOperationException($"{nameof(OnRetry)} handler is already set.");
        }
        _onRetry = handler;
        return this;
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        Exception? exception = null;

        for (int attempt = 1; attempt <= _maxRetryCount; attempt++)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                exception = ex;

                if (attempt < _maxRetryCount)
                {
                    _onRetry?.Invoke(ex);
                    await Task.Delay(GetDelay(attempt));
                }
            }
        }
        throw exception!;
    }

    public async Task ExecuteAsync(Func<Task> action)
    {
        await ExecuteAsync(async () =>
        {
            await action();
            return true;
        });
    }

    private TimeSpan GetDelay(int attempt)
    {
        return _initialDelay * Math.Pow(2, attempt - 1);
    }
}
