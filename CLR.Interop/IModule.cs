using System;

namespace CLR.Interop
{
	public interface IModule
	{
		bool Load();

		void Unload();
	}
}