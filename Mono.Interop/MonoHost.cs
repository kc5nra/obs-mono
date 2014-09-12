using System;
using System.Reflection;

using Mono.Interop;

namespace Mono.Interop
{
	internal class MonoHost
	{
		public MonoHost()
		{
			Base.LogDebug("{0}: Starting up MonoHost", typeof(MonoHost).ToString());

			Type type = Type.GetType("Mono.Runtime");
			if (type != null) {
				MethodInfo displayName = 
					type.GetMethod("GetDisplayName",
						BindingFlags.NonPublic | BindingFlags.Static);

				if (displayName != null) {
					Base.LogDebug("Mono runtime '{0}' loaded successfully.", 
						displayName.Invoke(null, null));
				}
			}
		}
	}
}