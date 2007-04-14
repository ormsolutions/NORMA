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
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Build.Tasks;

namespace Neumont.Build.Tasks
{
	/// <summary>
	/// Resolves the <c>MergeTarget</c> metadata value for the items specified for <see cref="ResourceMergeTargetResolver.MergeResources"/>.
	/// </summary>
	public class ResourceMergeTargetResolver : Task
	{
		#region Execute method
		/// <summary>See <see cref="Task.Execute"/>.</summary>
		public override bool Execute()
		{
			TaskLoggingHelper log = base.Log;
			ITaskItem[] manifestResources = this._manifestResources;
			ITaskItem[] mergeResources = this._mergeResources;
			List<ITaskItem> mergeResourcesWithResolvedTargets = new List<ITaskItem>(mergeResources.Length);

			foreach (ITaskItem mergeResource in mergeResources)
			{
				string mergeTarget = mergeResource.GetMetadata("MergeTarget");
				if (string.IsNullOrEmpty(mergeTarget))
				{
					log.LogError("MergeResource item \"{0}\" does not have a MergeTarget metadata value specified.", mergeResource.ItemSpec);
					break;
				}

				if (string.Equals(Path.GetExtension(mergeTarget), ".resources", StringComparison.OrdinalIgnoreCase))
				{
					log.LogMessage(MessageImportance.Low, "MergeResource item \"{0}\" has MergeTarget metadata value \"{1}\" that does not need to be resolved.", mergeResource.ItemSpec, mergeTarget);
					// If a .resources file is already being targetted, we don't need to do anything.
					mergeResourcesWithResolvedTargets.Add(mergeResource);
				}
				else
				{
					// If a .resources file is NOT being targetted, we need to find which .resources file should be used.
					string resolvedItemSpec = null;
					foreach (ITaskItem manifestResource in manifestResources)
					{
						if (string.Equals(manifestResource.GetMetadata("OriginalItemSpec"), mergeTarget, StringComparison.OrdinalIgnoreCase))
						{
							resolvedItemSpec = manifestResource.ItemSpec;
							break;
						}
					}
					if (string.IsNullOrEmpty(resolvedItemSpec))
					{
						log.LogError("MergeResource item \"{0}\" has MergeTarget metadata value \"{1}\" that could not be resolved to a .resources file.", mergeResource.ItemSpec, mergeTarget);
						break;
					}
					log.LogMessage(MessageImportance.Low, "MergeResource item \"{0}\" has MergeTarget metadata value \"{1}\" that was resolved to \"{2}\".", mergeResource.ItemSpec, mergeTarget, resolvedItemSpec);
					// We found the final target, so set up a new ITaskItem with this value for the MergeTarget.
					ITaskItem mergeResourceWithResolvedTarget = new TaskItem(mergeResource);
					mergeResourceWithResolvedTarget.SetMetadata("MergeTarget", resolvedItemSpec);
					mergeResourcesWithResolvedTargets.Add(mergeResourceWithResolvedTarget);
				}
			}

			bool hasLoggedErrors = log.HasLoggedErrors;
			this._mergeResourcesWithResolvedTargets = (hasLoggedErrors ? new ITaskItem[0] : mergeResourcesWithResolvedTargets.ToArray());
			return !hasLoggedErrors;
		}
		#endregion // Execute method

		#region Properties
		private ITaskItem[] _mergeResources;
		/// <summary>
		/// The resources to be merged for which the <c>MergeTarget</c> metadata value should be resolved.
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

		private ITaskItem[] _manifestResources;
		/// <summary>
		/// The manifest resource (.resources) files.
		/// </summary>
		[Required]
		public ITaskItem[] ManifestResources
		{
			get
			{
				return this._manifestResources;
			}
			set
			{
				this._manifestResources = value;
			}
		}

		private ITaskItem[] _mergeResourcesWithResolvedTargets;
		/// <summary>
		/// The resources to be merged for which the <c>MergeTarget</c> metadata value has been resolved.
		/// </summary>
		[Output]
		public ITaskItem[] MergeResourcesWithResolvedTargets
		{
			get
			{
				return this._mergeResourcesWithResolvedTargets;
			}
		}
		#endregion // Properties
	}
}
