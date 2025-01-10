using System.Diagnostics;

namespace VideoPlayer.Streaming.Services;

public static class ProcessExtensions
{
  public static async Task<(int ExitCode, string Output, string Error)> RunAsync(
      this Process process,
      CancellationToken cancellationToken = default)
  {
    var outputWaitHandle = new TaskCompletionSource<string>();
    var errorWaitHandle = new TaskCompletionSource<string>();

    process.OutputDataReceived += (s, e) =>
    {
      if (e.Data == null)
        outputWaitHandle.TrySetResult(string.Empty);
      else
        outputWaitHandle.TrySetResult(e.Data);
    };

    process.ErrorDataReceived += (s, e) =>
    {
      if (e.Data == null)
        errorWaitHandle.TrySetResult(string.Empty);
      else
        errorWaitHandle.TrySetResult(e.Data);
    };

    bool started = process.Start();
    if (!started)
    {
      throw new InvalidOperationException("Could not start process");
    }

    process.BeginOutputReadLine();
    process.BeginErrorReadLine();

    await using var registration = cancellationToken.Register(() =>
    {
      try
      {
        if (!process.HasExited) process.Kill(entireProcessTree: true);
      }
      catch { /* ignore */ }
    });

    await process.WaitForExitAsync(cancellationToken);

    var output = await outputWaitHandle.Task;
    var error = await errorWaitHandle.Task;

    return (process.ExitCode, output, error);
  }
}