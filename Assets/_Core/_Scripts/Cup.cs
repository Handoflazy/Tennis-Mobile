using UnityEngine;

public class Cup : MonoBehaviour
{
    //cup objects
    [SerializeField] GameObject[] cups;
    
    void Awake(){
        //disable all cups and activate the one that belongs to this tournament
        foreach(GameObject cup in cups){
            cup.SetActive(false);
        }
		
        int cupIndex = PlayerPrefs.GetInt("Tournament") % cups.Length;
		
        cups[cupIndex].SetActive(true);
    }
}