using System.IO;
using UnityEditor;
using UnityEngine;


public static class Setup
{
	[MenuItem("Tools/Setup/Create Default Folders")]
	public static void CreateDefaultFolders()
	{
		Folders.CreateDefault ("Assets","_Core", "3rdParty", "Editor", "Plugin", "Resources", "Sandbox", "Settings","Standard Assets","StreamingAssets");
		Folders.CreateDefault("_Core", "_Prefabs", "_Scripts", "_Scenes", "_Art", "_Materials", "_Audio", "_Animations", "_Fonts", "_Shaders", "_UI", "_Textures", "_Models", "_Physics","_ScriptableObject");
		AssetDatabase.Refresh();
	}
}

static class Folders
{
	public static void CreateDefault(string root, params string[] folders)
	{
		
		var fullPath = Path.Combine(Application.dataPath, root);
		foreach (var folder in folders)
		{
			var path = Path.Combine(fullPath, folder);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}
	}
}