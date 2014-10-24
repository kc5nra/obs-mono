#define LOG_METHOD_NAMES 

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CLR.Interop
{

	public enum LogLevel
	{
		/**
		 * Use if there's a problem that can potentially affect the program,
		 * but isn't enough to require termination of the program.
		 *
		 * Use in creation functions and core subsystem functions.  Places that
		 * should definitely not fail.
		 */
		LOG_ERROR = 100,

		/**
		 * Use if a problem occurs that doesn't affect the program and is
		 * recoverable.
		 *
		 * Use in places where where failure isn't entirely unexpected, and can
		 * be handled safely.
		 */
		LOG_WARNING = 200,

		/**
		 * Informative essage to be displayed in the log.
		 */
		LOG_INFO = 300,

		/**
		 * Debug message to be used mostly by developers.
		 */
		LOG_DEBUG = 400
	}

	sealed partial class Base
	{
		[DllImport("obs")]
		private static extern int blog(LogLevel logLevel, string message);
	}

	sealed partial class Base
	{

		internal const int ALIGN = 0;
		internal const CallingConvention OBS_CALL = CallingConvention.Cdecl;
		internal const CallingConvention OBS_CALLBACK = CallingConvention.Cdecl;

		public static void Log(LogLevel logLevel, string format, 
			params object[] args)
		{
			blog(logLevel, String.Format("CLR: " + format, args));
		}

		public static void LogError(string format, params object[] args)
		{
			Log(LogLevel.LOG_ERROR, format, args);
		}

		public static void LogWarning(string format, params object[] args)
		{
			Log(LogLevel.LOG_WARNING, format, args);
		}

		public static void LogDebug(string format, params object[] args)
		{
			Log(LogLevel.LOG_DEBUG, format, args);
		}

		public static void LogInfo(string format, params object[] args)
		{
			Log(LogLevel.LOG_INFO, format, args);
		}
	}

	internal abstract unsafe class OBSStruct<C, S>
	{
		private static Dictionary<IntPtr, C> _roots = new Dictionary<IntPtr, C>();
		private S* _self;

		protected object SyncRoot { get { return this; } }

		internal static C FromNativeOrNull(S* ptr)
		{
			C value = null;
			bool found;
			lock (_roots) {
				found = _roots.TryGetValue((IntPtr)ptr, out value);
			}
			return found ? value : null;
		}

		internal static C FromNative(S* ptr)
		{
			var value = FromNativeOrNull(ptr);
			if (value == null)
				throw new ArgumentException("object not found");
			return value;
		}

		internal S* ToNative()
		{
			return _self;
		}
	}
}