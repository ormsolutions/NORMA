#region zlib/libpng Copyright Notice
/**************************************************************************\
* Neumont Build System                                                     *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* This software is provided 'as-is', without any express or implied        *
* warranty. In no event will the authors be held liable for any damages    *
* arising from the use of this software.                                   *
*                                                                          *
* Permission is granted to anyone to use this software for any purpose,    *
* including commercial applications, and to alter it and redistribute it   *
* freely, subject to the following restrictions:                           *
*                                                                          *
* 1. The origin of this software must not be misrepresented; you must not  *
*    claim that you wrote the original software. If you use this software  *
*    in a product, an acknowledgment in the product documentation would be *
*    appreciated but is not required.                                      *
*                                                                          *
* 2. Altered source versions must be plainly marked as such, and must not  *
*    be misrepresented as being the original software.                     *
*                                                                          *
* 3. This notice may not be removed or altered from any source             *
*    distribution.                                                         *
\**************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Build.Tasks;

namespace Neumont.Build.Tasks
{
	/// <summary>
	/// Merges one or more resources into a manifest resource (<c>.resources</c>) file.
	/// </summary>
	public class ResourceMerger : Task
	{
		#region Execute method
		/// <summary>See <see cref="Task.Execute"/>.</summary>
		public override bool Execute()
		{
			TaskLoggingHelper log = base.Log;
			ITaskItem targetManifestResource = this._targetManifestResource;
			ITaskItem[] mergeResources = this._mergeResources;
			this._outputResource = null;

			if (mergeResources.Length <= 0)
			{
				// If we don't have any resources to merge, then we have already succeeded at (not) merging them.
				return true;
			}

			FileInfo targetManifestResourceFileInfo = new FileInfo(targetManifestResource.GetMetadata("FullPath"));

			if (!targetManifestResourceFileInfo.Exists)
			{
				log.LogError("The specified manifest resource file (\"{0}\") does not exist.", targetManifestResource.ItemSpec);
				return false;
			}

			// UNDONE: In all of the IO in this method, we aren't doing any handling of situations where the file changes between when we initially
			// look at its size and when we actually read the content.

			// Get all of the new resources and their values.
			Dictionary<string, byte[]> mergeResourcesValues = new Dictionary<string, byte[]>(mergeResources.Length, StringComparer.Ordinal);
			foreach (ITaskItem mergeResource in mergeResources)
			{
				System.Diagnostics.Debug.Assert(string.Equals(mergeResource.GetMetadata("MergeTarget"), targetManifestResource.ItemSpec, StringComparison.OrdinalIgnoreCase),
					"Trying to emit a resource into a different manifest resource than the one specified for MergeTarget.");

				FileInfo mergeResourceFileInfo = new FileInfo(mergeResource.GetMetadata("FullPath"));
				if (!mergeResourceFileInfo.Exists)
				{
					log.LogError("The specified resource file to merge (\"{0}\") does not exist.", mergeResource.ItemSpec);
					return false;
				}

				byte[] mergeResourceBytes = new byte[mergeResourceFileInfo.Length];
				using (FileStream mergeResourceFileStream = new FileStream(mergeResourceFileInfo.FullName, FileMode.Open,
					FileAccess.Read, FileShare.Read, mergeResourceBytes.Length, FileOptions.SequentialScan))
				{
					mergeResourceFileStream.Read(mergeResourceBytes, 0, mergeResourceBytes.Length);
				}

				string resourceName = mergeResource.GetMetadata("ResourceName");
				if (string.IsNullOrEmpty(resourceName))
				{
					log.LogError("The specified resource file to merge (\"{0}\") is missing a ResourceName metadata value.", mergeResource.ItemSpec);
					return false;
				}

				if (mergeResourcesValues.ContainsKey(resourceName))
				{
					log.LogError("The specified resource file to merge (\"{0}\") has a duplicate ResourceName metadata value (\"{2}\").", mergeResource.ItemSpec, resourceName);
					return false;
				}
				mergeResourcesValues.Add(resourceName, mergeResourceBytes);
			}

			// Read the existing .resources file into a byte array.
			byte[] originalResourcesBytes = new byte[targetManifestResourceFileInfo.Length];
			using (FileStream originalResourcesFileStream = new FileStream(targetManifestResourceFileInfo.FullName, FileMode.Open,
				FileAccess.Read, FileShare.Read, originalResourcesBytes.Length, FileOptions.SequentialScan))
			{
				originalResourcesFileStream.Read(originalResourcesBytes, 0, originalResourcesBytes.Length);
			}

			// The FileMode.Truncate on the next line is to make the .resources file zero-length so that we don't have to worry about any excess being left behind.
			using (ResourceWriter resourceWriter = new ResourceWriter(new FileStream(targetManifestResourceFileInfo.FullName, FileMode.Truncate,
			FileAccess.ReadWrite, FileShare.None, originalResourcesBytes.Length + (mergeResources.Length * 1024), FileOptions.SequentialScan)))
			{
				// Copy the resources from the original .resources file (now stored in the byte array) into the new .resources file.
				using (ResourceReader resourceReader = new ResourceReader(new MemoryStream(originalResourcesBytes, 0, originalResourcesBytes.Length, false, false)))
				{
					foreach (System.Collections.DictionaryEntry entry in resourceReader)
					{
						string resourceName = (string)entry.Key;
						string resourceType;
						byte[] resourceData;
						resourceReader.GetResourceData(resourceName, out resourceType, out resourceData);

						if (mergeResourcesValues.ContainsKey(resourceName))
						{
							log.LogMessage(MessageImportance.Normal, "Skipping copying resource \"{0}\" of type \"{1}\" to new manifest resource file \"{2}\". A new resource with this name will be merged.",
								resourceName, resourceType, targetManifestResource.ItemSpec);
						}
						else
						{
							resourceWriter.AddResourceData(resourceName, resourceType, resourceData);
							log.LogMessage(MessageImportance.Low, "Copied resource \"{0}\" of type \"{1}\" to new manifest resource file \"{2}\".", resourceName, resourceType, targetManifestResource.ItemSpec);
						}
					}
				}

				// Add each of the new resources into the new .resources file.
				foreach (KeyValuePair<string, byte[]> mergeResourceValue in mergeResourcesValues)
				{
					resourceWriter.AddResource(mergeResourceValue.Key, mergeResourceValue.Value);
					log.LogMessage(MessageImportance.Low, "Added new resource \"{0}\" to new manifest resource file \"{1}\".", mergeResourceValue.Key, targetManifestResource.ItemSpec);
				}
			}

			this._outputResource = targetManifestResource;
			return true;
		}
		#endregion // Execute method

		#region Properties
		private ITaskItem[] _mergeResources;
		/// <summary>
		/// The resources to be merged into <see cref="TargetManifestResource"/>.
		/// </summary>
		[Required]
		public ITaskItem[] MergeResources
		{
			get
			{
				return this._mergeResources;
			}
			set
			{
				this._mergeResources = value;
			}
		}

		private ITaskItem _targetManifestResource;
		/// <summary>
		/// The manifest resource (<c>.resources</c>) file into which the <see cref="MergeResources"/> should be merged.
		/// </summary>
		[Required]
		public ITaskItem TargetManifestResource
		{
			get
			{
				return this._targetManifestResource;
			}
			set
			{
				this._targetManifestResource = value;
			}
		}

		private ITaskItem _outputResource;
		/// <summary>
		/// The manifest resource (<c>.resources</c>) file into which the <see cref="MergeResources"/> have been merged.
		/// </summary>
		[Output]
		public ITaskItem OutputResource
		{
			get
			{
				return this._outputResource;
			}
		}
		#endregion // Properties
	}
}
