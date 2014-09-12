using System;
using System.Reflection;

using Mono.Interop.Internal;

namespace Mono.Interop
{
	public class MonoHost
	{
		public MonoHost ()
		{
			Type type = Type.GetType ("Mono.Runtime");
			if (type != null) {
				MethodInfo displayName = 
					type.GetMethod ("GetDisplayName", 
						BindingFlags.NonPublic | BindingFlags.Static);

				if (displayName != null) {
					Base.Log (Base.LogLevel.LOG_ERROR, "Mono runtime '{0}' loaded successfully.", 
						displayName.Invoke (null, null));
				}
			}

		}
	}
}

