using UnityEngine;

[CreateAssetMenu(fileName = "New outfit", menuName = "Character/Outfit")]
public class Outfit : ScriptableObject{
    
    //holds all the outfit values
    public int HatType;
    public bool Glasses;
    public bool Female;
    public int HairType;
	
    public Material Skin;
    public Material HatMat;
    public Material Shirt;
    public Material Pants;
    public Material Shoes;
    public Material Racket;
    public Material Hair;
}