using System;
using System.Collections.Generic;
using System.Reflection;

namespace CLR.Interop
{
	internal class CLRHost
	{
		private List<IModule> modules;

		public CLRHost()
		{
			Base.LogDebug("{0}: Starting up CLRHost", typeof(CLRHost).ToString());

			Type type = Type.GetType("Mono.Runtime");
			if (type != null) {
				MethodInfo displayName = 
					type.GetMethod("GetDisplayName",
						BindingFlags.NonPublic | BindingFlags.Static);

				if (displayName != null) {
					Base.LogDebug("Mono runtime '{0}' loaded successfully", 
						displayName.Invoke(null, null));
				}
			} else {
				Base.LogDebug(".NET runtime '{0}' loaded successfully",
					Environment.Version.ToString());
			}
		}

		private List<IModule> LoadModules()
		{
			return new List<IModule>();
		}

		public void Load()
		{
			modules = LoadModules();

			Base.LogDebug("Loading plugins");
			foreach (IModule module in modules) {
				Base.LogDebug("Attempting to load {0}", module.GetType().ToString());
				if (!module.Load()) {
					Base.LogDebug("Failed to load {0}", module.GetType().ToString());
				}
			}
		}

		public void Unload()
		{
			Base.LogDebug("Unloading plugins");
			foreach (IModule module in modules) {
				Base.LogDebug("Attempting to unload {0}", module.GetType().ToString());
				module.Unload();
			}

			modules.Clear();
		}
	}
}