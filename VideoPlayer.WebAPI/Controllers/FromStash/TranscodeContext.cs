using System.Collections.Concurrent;

public sealed class TranscodeContext : IDisposable
{
    private readonly ConcurrentDictionary<int, Process> _processes;
    private readonly ILogger<TranscodeContext> _logger;
    private bool _disposed;
    private readonly object _lock = new();

    public TranscodeContext(ILogger<TranscodeContext> logger)
    {
        _logger = logger;
        _processes = new ConcurrentDictionary<int, Process>();
    }

    public void AttachProcess(Process process)
    {
        ArgumentNullException.ThrowIfNull(process);
        ThrowIfDisposed();

        if (!_processes.TryAdd(process.Id, process))
        {
            _logger.LogWarning("Process {ProcessId} already attached", process.Id);
            return;
        }

        // Monitor process completion
        process.Exited += (s, e) =>
        {
            if (_processes.TryRemove(process.Id, out var p))
            {
                p.Dispose();
            }
        };
    }

    public void Dispose()
    {
        lock (_lock)
        {
            if (!_disposed)
            {
                foreach (var process in _processes.Values)
                {
                    try
                    {
                        if (!process.HasExited)
                        {
                            process.Kill(entireProcessTree: true);
                        }
                        process.Dispose();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error disposing process {ProcessId}", process.Id);
                    }
                }

                _processes.Clear();
                _disposed = true;
            }
        }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(TranscodeContext));
        }
    }
}