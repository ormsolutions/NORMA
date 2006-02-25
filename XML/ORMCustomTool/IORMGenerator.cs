using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Microsoft.Build.BuildEngine;

namespace Neumont.Tools.ORM.ORMCustomTool
{
	public interface IORMGenerator
	{
		/// <summary>
		/// The non-localized official name of this <see cref="IORMGenerator"/>.
		/// </summary>
		/// <remarks>
		/// This name must match the name of the <see cref="Microsoft.Win32.RegistryKey"/> in which this
		/// <see cref="IORMGenerator"/> is registered.
		/// </remarks>
		string OfficialName
		{
			get;
		}
		/// <summary>
		/// The localized name of this <see cref="IORMGenerator"/> for display in the user interface.
		/// </summary>
		[Localizable(true)]
		string DisplayName
		{
			get;
		}
		/// <summary>
		/// The localized description of this <see cref="IORMGenerator"/> for display in the user interface.
		/// </summary>
		[Localizable(true)]
		string DisplayDescription
		{
			get;
		}

		/// <summary>
		/// The official name of the output format provided by this <see cref="IORMGenerator"/>.
		/// </summary>
		/// <remarks>
		/// The official names for several predefined output formats are accessible through <see cref="ORMOutputFormat"/>.
		/// </remarks>
		string ProvidesOutputFormat
		{
			get;
		}
		
		/// <summary>
		/// The official name(s) of the output format(s) required by this <see cref="IORMGenerator"/> as input.
		/// </summary>
		/// <remarks>
		/// The official names for several predefined output formats are accessible through <see cref="ORMOutputFormat"/>.
		/// </remarks>
		ReadOnlyCollection<string> RequiresInputFormats
		{
			get;
		}

		/// <summary>
		/// Returns the default name of the file generated for a specific source file name.
		/// </summary>
		/// <param name="sourceFileName">A <see cref="String"/> containing the name (without file extension) of the source ORM file.</param>
		string GetOutputFileDefaultName(string sourceFileName);

		/// <summary>
		/// Adds a <see cref="BuildItem"/> for the generated file to <paramref name="buildItemGroup"/>.
		/// </summary>
		/// <param name="buildItemGroup">The <see cref="BuildItemGroup"/> to which the <see cref="BuildItem"/> for the generated file should be added.</param>
		/// <param name="sourceFileName">The name of the source ORM file. This will usually be used as the value for the &lt;DependentUpon&gt; item metadata.</param>
		/// <param name="outputFileName">The name of the generated file. This will usually be used as the value for the Include attribute of the <see cref="BuildItem"/> for the generated file.</param>
		/// <returns>The <see cref="BuildItem"/> for the generated file.</returns>
		/// <remarks>
		/// If this <see cref="IORMGenerator"/> generates compilable output that is useful at design time, the &lt;DesignTime&gt;
		/// item metadata should be set to the value "True" (without the quotes).
		/// </remarks>
		BuildItem AddGeneratedFileBuildItem(BuildItemGroup buildItemGroup, string sourceFileName, string outputFileName);

		/// <summary>
		/// Generates the output for <paramref name="buildItem"/> to <paramref name="outputStream"/>, using the read-only <see cref="Stream"/>s
		/// contained in <paramref name="inputFormatSteams"/> as input.
		/// </summary>
		/// <param name="buildItem">The <see cref="BuildItem"/> for which output is to be generated.</param>
		/// <param name="outputStream">The <see cref="Stream"/> to which output is to be generated.</param>
		/// <param name="inputFormatStreams">A read-only <see cref="IDictionary&lt;String,Stream&gt;"/> containing pairs of official output format names and read-only <see cref="Stream"/>s containing the output in that format.</param>
		/// <remarks>
		/// <para><paramref name="inputFormatStreams"/> is only guarenteed to contain the output <see cref="Streams"/>s for
		/// the formats "ORM", "OIAL", and any formats returned by this <see cref="IORMGenerator"/>'s implementation of
		/// <see cref="IORMGenerator.RequiresInputFormats"/>.</para>
		/// <para>Implementations of this method are responsible for resetting the <see cref="Stream.Position"/> of any
		/// <see cref="Stream"/> obtained from <paramref name="inputFormatStreams"/> to the beginning of that <see cref="Stream"/>
		/// if they directly or indirectly alter that <see cref="Stream.Position"/>. This does not apply to <paramref name="outputStream"/>.</para>
		/// </remarks>
		void GenerateOutput(BuildItem buildItem, Stream outputStream, IDictionary<string, Stream> inputFormatStreams);
	}
}
