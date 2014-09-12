using System;

namespace Mono.Interop
{
	public interface IModule
	{
		bool Load ();

		void Unload ();
	}
}