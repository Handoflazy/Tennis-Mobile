using System.IO;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;


public static class Setup
{
	[MenuItem("Tools/Setup/Create Default Folders")]
	public static void CreateDefaultFolders()
	{
		Folders.Create ("Assets","_Core", "3rdParty", "Editor", "Plugin", "Resources", "Sandbox", "Settings","Standard Assets","StreamingAssets");
		Folders.Create("Assets", "_Core/_Prefabs", "_Core/_Scripts", "_Core/_Scenes",
			"_Core/_Art", "_Core/_Materials", "_Core/_Audio", "_Core/_Animations", "_Core/_Fonts", "_Core/_Shaders", "_Core/_UI", "_Core/_Textures", "_Core/_Models", "_Core/_Physics","_Core/_ScriptableObject");
		AssetDatabase.Refresh();
		// Folders.Move("_Core", "Scenes");
		// Folders.Move("_Core", "Settings");
		Folders.Delete("TutorialInfo");AssetDatabase.Refresh();
		
		
	}
}

static class Folders
{
	public static void Delete(string folderName) {
		string path = $"Assets/{folderName}";
		if(AssetDatabase.IsValidFolder(path)) {
			AssetDatabase.DeleteAsset(path);
		}
	}
	public static void Move(string newParent, string folderName) {
		string sourcePath = $"Assets/{folderName}";
		if(AssetDatabase.IsValidFolder(sourcePath)) {
			string destinationPath = $"Assets/{newParent}/{folderName}";
			string error = AssetDatabase.MoveAsset(sourcePath, destinationPath);

			if(!string.IsNullOrEmpty(error)) {
				Debug.Log($"Failed to move {folderName}: {error}");
			}
		}
	}
	
	static void CreateSubFolders(string rootPath, string folderHierarchy) {
		var folders = folderHierarchy.Split('/');
		var currentPath = rootPath;
		foreach (var folder in folders) {
			currentPath = Path.Combine(currentPath, folder);
			if (!Directory.Exists(currentPath))
			{
				Directory.CreateDirectory(currentPath);
			}
		}
	}
	public static void Create(string root, params string[] folders)
	{
		
		var fullPath = Path.Combine(Application.dataPath, root);
		if(!Directory.Exists(fullPath))
		{
			Directory.CreateDirectory(fullPath);
		}
		foreach (var folder in folders)
		{
			CreateSubFolders(fullPath,folder);
		
		}
	}
}