using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mancoba.Sompisi.Utils.Helpers
{

	internal sealed class Timer : CancellationTokenSource
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        /// <param name="millisecondsDueTime">The milliseconds due time.</param>
        /// <param name="millisecondsPeriod">The milliseconds period.</param>
        /// <param name="waitForCallbackBeforeNextPeriod">if set to <c>true</c> [wait for callback before next period].</param>
        internal Timer(Action<object> callback, object state, int millisecondsDueTime, int millisecondsPeriod, bool waitForCallbackBeforeNextPeriod = true)
		{
			try
			{
				Task.Delay(millisecondsDueTime, Token).ContinueWith(async (t, s) =>
				{
					var tuple = (Tuple<Action<object>, object>)s;
					while (!IsCancellationRequested)
					{
						try
						{
							if (waitForCallbackBeforeNextPeriod)
								tuple.Item1(tuple.Item2);
							else
								await Task.Run(() => tuple.Item1(tuple.Item2));

							try
							{
								await Task.Delay(millisecondsPeriod, Token).ConfigureAwait(false);
							}
							catch (TaskCanceledException) { }
						}
						catch (Exception exception)
						{
							ErrorHandler.HandleError("Time failed to fire!", exception);
						}
					}
				}, Tuple.Create(callback, state), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
			}
			catch (Exception exception)
			{
				ErrorHandler.HandleError("Failed to start timer!", exception);
			}
		}

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
		{
			if (disposing)
				Cancel();

			base.Dispose(disposing);
		}
	}

	public delegate void TimerCallback(object state);

	public sealed class StopWatch : CancellationTokenSource, IDisposable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="StopWatch"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        public StopWatch(TimerCallback callback, object state)
		{
			Task.Delay(0, Token).ContinueWith(async (t, s) =>
				{
					var tuple = (Tuple<TimerCallback, object>) s;

					while (true)
					{
						if (IsCancellationRequested)
							break;
						await Task.Run(() => tuple.Item1(tuple.Item2));
						await Task.Delay(1);
						Count = Count + 1;
					}

				}, Tuple.Create(callback, state), CancellationToken.None,
				TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
				TaskScheduler.Default);
		}

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public new void Dispose() { base.Cancel(); }

		public int Count{ get; set; } 
	}

}

