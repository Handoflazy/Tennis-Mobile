using UnityEngine;

public class ModifyOutfit : MonoBehaviour
{
    [SerializeField] Outfit outfit;
    
    [SerializeField] Renderer character;
    [SerializeField] Renderer racket;
    [SerializeField] Renderer[] hatRenderers;
    [SerializeField] GameObject glasses;
    [SerializeField] GameObject[] hats;
    [SerializeField] GameObject[] hair;
    [SerializeField] GameObject skirt;
    
    [SerializeField] int rendererPantsIndex = 0;
    [SerializeField] int rendererShirtIndex = 1;
    [SerializeField] int rendererSkinIndex = 2;
    [SerializeField] int rendererShoesIndex = 3;
	
    [SerializeField] bool player;
    [SerializeField] bool dontUpdateOnAwake;
    
    
    public void SetOutfit(Outfit outfit)
    {
        this.outfit = outfit;
        SetOutfit(false);
    }
    
    void OnValidate(){
        if(outfit == null)
            return;
		
        SetOutfit(true);
    }
    void Awake(){
        if(dontUpdateOnAwake)
            return;
		
        if(!player){
            int index = PlayerPrefs.GetInt(SaveConst.MATCH);
            outfit = Resources.Load<Outfit>("Outfit_" + index);
        }
        else{
            int index = PlayerPrefs.GetInt("Player",0);
            outfit = Resources.Load<Outfit>("Player_" + index);
        }
		
        SetOutfit(false);
    }
    public void Initialize(int index){
        outfit = Resources.Load<Outfit>("Outfit_" + index);
        SetOutfit(false);
    }
    public void SetOutfit(bool editor){
        if(outfit == null)
            return;
		
        Material[] currentMaterials = editor ? character.sharedMaterials : character.materials;
					
        currentMaterials[rendererPantsIndex] = outfit.Pants;
        currentMaterials[rendererShirtIndex] = outfit.Shirt;
        currentMaterials[rendererSkinIndex] = outfit.Skin;
        currentMaterials[rendererShoesIndex] = outfit.Shoes;
		
        if(editor){
            character.sharedMaterials = currentMaterials;
            racket.sharedMaterial = outfit.Racket;
			
            for(int i = 0; i < hair.Length; i++){
                hair[i].GetComponent<Renderer>().sharedMaterial = outfit.Hair;
            }
        }
        else{
            character.materials = currentMaterials;
            racket.material = outfit.Racket;
			
            for(int i = 0; i < hair.Length; i++){
                hair[i].GetComponent<Renderer>().material = outfit.Hair;
            }
        }
		
        for(int i = 0; i < hair.Length; i++){
            hair[i].SetActive(outfit.HairType == i);
        }
		
        skirt.SetActive(outfit.Female);
		
        for(int i = 0; i < hatRenderers.Length; i++){
            if(i == outfit.HatType){
                if(editor){
                    hatRenderers[i].sharedMaterial = outfit.HatMat;
                }
                else{
                    hatRenderers[i].material = outfit.HatMat;
                }
				
                hats[i].SetActive(true);
				
                if(outfit.Female && !editor)
                    hats[i].transform.Translate(Vector3.forward * 0.06f);
            }
            else{
                hats[i].SetActive(false);
            }
        }
		
        glasses.SetActive(outfit.Glasses);
    }
}