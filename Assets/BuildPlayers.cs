// BuildPlayers.cs
//
//  Author:
//       oleksandr <>
//  Created:
//      8/3/2017
//
//  Copyright (c) 2017 oleksandr
using System;
using System.IO;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

public static class BuildPlayers
{
	#if UNITY_EDITOR
	private static string GetArg(string name)
	{
		var args = System.Environment.GetCommandLineArgs();
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == name && args.Length > i + 1)
			{
				return args[i + 1];
			}
		}
		return null;
	}
	// This actually builds a windows player.
	private static void DoBuildWindowsPlayer(string outputDir) {
		if (outputDir == null) {
			throw new ArgumentException("No output folder specified.");
		}
		if (!Directory.Exists (outputDir)) {
			Directory.CreateDirectory (outputDir);
		}
		Debug.Log (outputDir);
		// get the scenes from the settings dialog
		var scenes = EditorBuildSettings.scenes.Select (scene => scene.path).ToArray ();

		BuildPipeline.BuildPlayer (scenes, outputDir + "/myPlayer.exe",
			BuildTarget.StandaloneWindows,
			BuildOptions.None
		);

	}
	// This is called by the editor menu.
	[MenuItem("Automation/Build Windows Player")]
	public static void BuildWindowsPlayerInEditor() {
		// in the editor we just put the player on the desktop
		DoBuildWindowsPlayer(Environment.GetFolderPath (Environment.SpecialFolder.Desktop) +  "/windows");
	}

	// this is called by the CI server
	public static void BuildWindowsPlayerInCI() {
		// read the "-outputDir" command line argument
		var outputDir = GetArg ("-outputDir");

		// use the existing code to build the player
		DoBuildWindowsPlayer (outputDir);

		// end the application when done.
		EditorApplication.Exit (0);
	}
	#endif
}


