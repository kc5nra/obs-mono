//
//  Source.cs
//
//  Author:
//       John R. Bradley <jrb@turrettech.com>
//
//  Copyright (c) 2014 John R. Bradley
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CLR.Interop
{
	//	sealed partial class Source
	//	{
	//		[DllImport("obs")]
	//		private static extern int blog(LogLevel logLevel, string message);
	//	}

	[Flags]
	public enum SourceFlags
	{
		Video = 1 << 0,
		Audio = 1 << 1,
		Async = 1 << 2,
		CustomDraw = 1 << 3,
		ColorMatrix = 1 << 4,
		Interaction = 1 << 5
	}

	public enum SourceType
	{
		/** Input sources */
		Input,
		/** Filter sources */
		Filter,
		/** Transition sources */
		Transition
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class SourceInfo : Attribute
	{
		/** 
		 * Unique string identifier for the source 
		 */
		private string id;

		/**
		 * Type of the source
		 */
		private SourceType sourceType;

		/**
		 * Source uses a color matrix (usually YUV sources).
		 *
		 * When this true, the VideoRender method will automatically assign a
		 * 4x4 YUV->RGB matrix to the "color_matrix" parameter of the effect, or it can
		 * be changed to a custom value.
		 */
		public bool colorMatrix;



		public SourceInfo(string id, SourceType sourceType)
		{
			this.id = id;
			this.sourceType = sourceType;

			colorMatrix = false;
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = Base.ALIGN)]
	internal unsafe struct obs_source_info_t
	{
		internal IntPtr _get_name;

		[UnmanagedFunctionPointer(Base.OBS_CALLBACK)]
		#if !DEBUG
		[SuppressUnmanagedCodeSecurity]
		#endif
		internal delegate void get_name(obs_source_info_t*self);

		private static int _sizeof;

		static obs_source_info_t()
		{
			_sizeof = Marshal.SizeOf(typeof(obs_source_info_t));
		}

		internal static obs_source_info_t *Alloc()
		{
			var ptr = (obs_source_info_t*)Marshal.AllocHGlobal(_sizeof);
			*ptr = new obs_source_info_t();
			return ptr;
		}

		internal static void Free(obs_source_info_t*ptr)
		{
			Marshal.FreeHGlobal((IntPtr)ptr);
		}
	}

	public interface ISource
	{
		string Name { get; }

		int Width { get; }

		int Height { get; }

		void Render(GS.Effect effect);

		Properties Properties { get; }


	}
}

