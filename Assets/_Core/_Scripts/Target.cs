using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject brokenTarget;
    public Animator anim;
		
    public void Hit(){
        anim.Play("HitTarget");
        Instantiate(brokenTarget, transform.position, brokenTarget.transform.rotation);
    }
}