using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Thread-safe implementation of <see cref="IDiagnosticStore"/> using ConcurrentDictionary.
	/// </summary>
	internal class MemoryDiagnosticStore : IDiagnosticStore, IDisposable
	{
		private readonly ConcurrentDictionary<string, DiagnosticInfo> _diagnostics = new();
		private Timer _cleanupTimer;

		public MemoryDiagnosticStore()
			: this(TimeSpan.Zero)
		{ }

		public MemoryDiagnosticStore(TimeSpan cleanUpInterval)
		{
			if (cleanUpInterval > TimeSpan.Zero)
			{
				_cleanupTimer = new Timer(Cleanup, null, TimeSpan.Zero, cleanUpInterval);
			}
		}

		/// <inheritdoc/>
		public bool TryAdd(string diagnosticId, DiagnosticInfo diagnosticInfo) => _diagnostics.TryAdd(diagnosticId, diagnosticInfo);

		/// <inheritdoc/>
		public bool TryGetValue(string diagnosticId, out DiagnosticInfo diagnosticInfo) => _diagnostics.TryGetValue(diagnosticId, out diagnosticInfo);

		/// <inheritdoc/>
		public void AddOrUpdate(string diagnosticId, DiagnosticInfo diagnosticInfo) => _diagnostics.AddOrUpdate(diagnosticId, diagnosticInfo, (key, oldValue) => diagnosticInfo);

		/// <inheritdoc/>
		public bool TryRemove(string diagnosticId, out DiagnosticInfo diagnosticInfo) => _diagnostics.TryRemove(diagnosticId, out diagnosticInfo);

		/// <inheritdoc/>
		public bool ContainsKey(string diagnosticId) => _diagnostics.ContainsKey(diagnosticId);

		/// <inheritdoc/>
		public ICollection<string> GetAllKeys() => _diagnostics.Keys;

		/// <inheritdoc/>
		public int Count => _diagnostics.Count;

		/// <summary>
		/// Cleans up the diagnostic store by removing entries for requests that have been garbage collected.
		/// </summary>
		public void Cleanup(object state)
		{
			try
			{
				// Remove diagnostic information for requests that have been garbage collected
				foreach (string key in GetAllKeys())
				{
					if (TryGetValue(key, out DiagnosticInfo diagnosticInfo))
					{
						if (!diagnosticInfo.RequestReference.TryGetTarget(out HttpRequestMessage request))
						{
							TryRemove(key, out _);
						}
					}
				}
			}
			catch
			{
				// Intentionally left empty
			}
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			// Call 'Dispose' to release resources
			Dispose(true);

			// Tell the GC that we have done the cleanup and there is nothing left for the Finalizer to do
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ReleaseManagedResources();
			}
			else
			{
				// The object went out of scope and the Finalizer has been called.
				// The GC will take care of releasing managed resources, therefore there is nothing to do here.
			}

			ReleaseUnmanagedResources();
		}

		private void ReleaseManagedResources()
		{
			if (_cleanupTimer != null)
			{
				_cleanupTimer.Dispose();
				_cleanupTimer = null;
			}
		}

		private void ReleaseUnmanagedResources()
		{
			// We do not hold references to unmanaged resources
		}
	}
}
