using UnityEngine;

public class RandomForce : MonoBehaviour
{
    public float force;
    void Start(){
        //randomize force
        force *= Random.Range(.7f, 1.4f);
		
        //get random direction
        Vector3 dir = new(GetRandom(), GetRandom(), GetRandom());
		
        //add force
        GetComponent<Rigidbody>().AddForce(dir * force);
    }
    float GetRandom(){
        return Random.Range(-1f, 1f);
    }
}