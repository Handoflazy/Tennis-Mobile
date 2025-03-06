using UnityEngine;
using UnityEditor;

public static class OutfitGenerator 
{
    private static readonly int[] HatTypes = { 0, 1, 2,3,4,5,6 };
    private static readonly bool[] GlassesOptions = { true, false };
    private static readonly bool[] GenderOptions = { true, false };
    private static readonly int[] HairTypes = { 0, 1, 2};
    [MenuItem("Tools/Generate Outfits")]
    public static void GenerateOutfits()
    {
        for (int i = 0; i < 20; i++)
        {
            Outfit outfit = ScriptableObject.CreateInstance<Outfit>();

            outfit.HatType = HatTypes[Random.Range(0, HatTypes.Length)];
            outfit.Glasses = GlassesOptions[Random.Range(0, GlassesOptions.Length)];
            outfit.Female = GenderOptions[Random.Range(0, GenderOptions.Length)];
            outfit.HairType = HairTypes[Random.Range(0, HairTypes.Length)];

            // Assign random materials (assuming you have materials in Resources/Materials)
            outfit.Skin = GetRandomMaterial();
            outfit.HatMat = GetRandomMaterial();
            outfit.Shirt = GetRandomMaterial();
            outfit.Pants = GetRandomMaterial();
            outfit.Shoes = GetRandomMaterial();
            outfit.Racket = GetRandomMaterial();
            outfit.Hair = GetRandomMaterial();

            AssetDatabase.CreateAsset(outfit, $"Assets/_Core/Resources/Player_{i}.asset");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static Material GetRandomMaterial()
    {
        Material[] materials = Resources.LoadAll<Material>($"VisitorsMaterials");
        return materials[Random.Range(0, materials.Length)];
    }
}