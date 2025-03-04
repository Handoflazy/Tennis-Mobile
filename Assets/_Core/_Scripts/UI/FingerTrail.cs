using System;
using System.Linq;
using UnityEngine;
using UnityUtils;

public class FingerTrail: MonoBehaviour
{
    [SerializeField] Transform[] sprites;
    
    private int index;
    

    void Start()
    {
        Disable();
        sprites = transform.Children().ToArray();
    }
    void Update()
    {
        Vector3 pos = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            if (index < sprites.Length)
                sprites[index].gameObject.SetActive(true);
            for(int i = sprites.Length - 1; i >= 0; i--){
                if(i == 0){
                    sprites[i].position = pos;
                    continue;
                }
					
                if(sprites[i].gameObject.activeSelf)
                    sprites[i].position = sprites[i - 1].position;
            }
            index++;
        }
        else if(Input.GetMouseButtonUp(0)){
            index = 0;
            Disable();
        }
    }

    void Disable() {
        transform.ForEveryChild((t) => t.gameObject.SetActive(false));
    }

}
