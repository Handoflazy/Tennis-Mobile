using System.Collections.Generic;
using _Core._Scripts;
using UnityEngine;
using UnityServiceLocator;


public class VisitorSpawner : MonoBehaviour
{
    [SerializeField] VisitorFactory visitorFactory;
    [SerializeField] Transform[] rows;

    [Header("Settings")]
    [SerializeField] int numRow;

    [SerializeField]
    float space;

    [SerializeField] float randomPos;

    [SerializeField] Vector3 rotation;

    [SerializeField] float kidSize;

    [SerializeField] int visitorChanceMin;

    [SerializeField] int visitorChanceMax;

    [SerializeField] float kidOffset;
    [Header("Appearance")]
    [SerializeField] Material[] pants;

    [SerializeField] Material[] shirts;

    [SerializeField] Material[] skinTones;

    [SerializeField] Material[] shoes;

    [SerializeField] Material[] hats;


    int visitorChance;
    List<Animator> anims = new List<Animator>();

    void Awake() {
        visitorChance = (PlayerPrefs.GetInt("Tournament Match Number") * 2) + 2;
        ServiceLocator.ForSceneOf(this).Register(this);
    }

    void Start() {
        //for all rows
        for (int i = 0; i < rows.Length; i++) {
            Vector3 startPos = rows[i].position;

            int visitorCount = i % 2 == 0 ? numRow : numRow - 1;

            //spawn visitors
            for (int j = 0; j < visitorCount; j++) {
                if(Random.Range(0, visitorChance) != 0) {
                    //completely randomize all visitor settings
                    bool kid = Random.Range(0, 2) == 0;

                    Vector3 pos = kid ? startPos + Vector3.up * kidOffset : startPos;
                    pos.x -= j * space;
                    pos.x += Random.Range(-randomPos, randomPos);

                    Visitor vis = visitorFactory.Create();
                    vis.SetPositionAndRotation(pos, Quaternion.Euler(rotation));
                    Animator anim = vis.Anim;
                    anim.SetInteger("Type", Random.Range(0, 5));
                    anim.speed = Random.Range(0.75f, 1.2f);
                    anims.Add(anim);

                    vis.Eyes.speed = Random.Range(0.85f, 1.15f);

                    Material[] currentMaterials = vis.Rend.materials;

                    currentMaterials[0] = pants[Random.Range(0, pants.Length)];
                    currentMaterials[1] = shirts[Random.Range(0, shirts.Length)];
                    currentMaterials[2] = skinTones[Random.Range(0, skinTones.Length)];
                    currentMaterials[3] = shoes[Random.Range(0, shoes.Length)];

                    //assign the random materials
                    vis.Rend.materials = currentMaterials;

                    vis.Hat.GetComponent<Renderer>().material = hats[Random.Range(0, hats.Length)];
                    vis.Hat.SetActive(Random.Range(0, 3) == 0);

                    //scale visitor down if it's a kid
                    if(kid)
                        vis.transform.localScale *= kidSize;
                }
            }
        }
    }

    public void Cheer() {
        for (int i = 0; i < anims.Count; i++) {
            int random = Random.Range(0, 3);
            anims[i].SetFloat("CheeringType", (float)random / 2f);

            anims[i].SetTrigger("Cheer");
        }
    }

    //make all visitors act disappointed
    public void Disbelief() {
        for (int i = 0; i < anims.Count; i++) {
            int random = Random.Range(0, 3);
            anims[i].SetFloat("SadType", (float)random / 2f);

            anims[i].SetTrigger("Sad");
        }
    }
}