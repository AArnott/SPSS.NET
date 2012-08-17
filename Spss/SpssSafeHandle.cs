//-----------------------------------------------------------------------
// <copyright file="SpssSafeHandle.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
//     Copyright (c) Brigham Young University
// </copyright>
//-----------------------------------------------------------------------

namespace Spss {
	using System;
	using Microsoft.Win32.SafeHandles;

	public class SpssSafeHandle : SafeHandleMinusOneIsInvalid {
		private SpssFileAccess accessMode;

		/// <summary>
		/// Initializes a new instance of the <see cref="SpssSafeHandle"/> class.
		/// </summary>
		/// <param name="handle">The SPSS handle.</param>
		/// <param name="accessMode">The access mode the handle was opened with.</param>
		public SpssSafeHandle(int handle, SpssFileAccess accessMode) : base(true) {
			this.accessMode = accessMode;
			this.SetHandle(new IntPtr(handle));
		}

		public static implicit operator int(SpssSafeHandle handle) {
			if (handle.IsInvalid) {
				throw new InvalidOperationException("Invalid SPSS handle.");
			}

			if (handle.IsClosed) {
				throw new InvalidOperationException("SPSS handle has been closed.");
			}

			return handle.handle.ToInt32();
		}

		/// <summary>
		/// When overridden in a derived class, executes the code required to free the handle.
		/// </summary>
		/// <returns>
		/// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a releaseHandleFailed MDA Managed Debugging Assistant.
		/// </returns>
		protected override bool ReleaseHandle() {
			switch (this.accessMode) {
				case SpssFileAccess.Read:
					ReturnCode result = SpssSafeWrapper.spssCloseReadImpl(handle.ToInt32());
					return result == ReturnCode.SPSS_OK;
				case SpssFileAccess.Append:
					result = SpssSafeWrapper.spssCloseAppendImpl(handle.ToInt32());
					return result == ReturnCode.SPSS_OK;
				case SpssFileAccess.Create:
					result = SpssSafeWrapper.spssCloseWriteImpl(handle.ToInt32());
					return result == ReturnCode.SPSS_OK || result == ReturnCode.SPSS_DICT_NOTCOMMIT;
				default:
					return false;
			}
		}
	}
}
