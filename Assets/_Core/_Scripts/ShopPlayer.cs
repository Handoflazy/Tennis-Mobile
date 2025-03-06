using System;
using Eflatun.SceneReference;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Character{
    public string name;
}
public class ShopPlayer : MonoBehaviour
{
	[Header("Elements")] 
	[SerializeField] private SceneReference gameScene;
	[SerializeField,Required] GameObject playerPrefab;
	[SerializeField] Character[] characters;
	[SerializeField] RuntimeAnimatorController idle;
	[SerializeField] TextMeshProUGUI nameLabel;
	[SerializeField] Transform cameraHolder;
	[Header("Settings")]
	[SerializeField] float dist;
	[SerializeField] float maxDragTime;
	[SerializeField] float dragDistance;
	[SerializeField] float transitionSpeed;
	[Header("Button")]
	[SerializeField] GameObject rightButton;
	[SerializeField] GameObject leftButton;
	[SerializeField] GameObject unlockButton;
	
    float startPos;
    float startTime;
	
    bool canSwitch;
	
    int current;
	
    Vector3 camTarget;
	
    int mannequinCount;

    private void Start() {
	    bool doneLoading = false;
	    Vector3 pos = Vector3.zero;
	    while(!doneLoading){
		    Outfit next = Resources.Load<Outfit>("Player_" + mannequinCount);
			
		    if(next != null){
			    GameObject newMannequin = Instantiate(playerPrefab, pos, playerPrefab.transform.rotation);
				
			    newMannequin.GetComponent<Animator>().runtimeAnimatorController = idle;
			    newMannequin.GetComponent<Player>().enabled = false;
				
			    newMannequin.GetComponentInChildren<ParticleSystem>().Stop();
				
			    newMannequin.GetComponent<ModifyOutfit>().SetOutfit(next);
			    newMannequin.GetComponent<ModifyOutfit>().SetOutfit(false);
				
			    mannequinCount++;
		    }
		    else{
			    doneLoading = true;
		    }
			
		    pos += Vector3.right * dist;
	    }
	    current = PlayerPrefs.GetInt("Player");
	    UpdateCamera();
	    cameraHolder.position = Vector3.right * (dist * current);
	    
    }
    void Update(){
	    //move camera to currently selected character
	    cameraHolder.position = Vector3.MoveTowards(cameraHolder.position, camTarget, Time.deltaTime * transitionSpeed);
		
	    float currentPos = Input.mousePosition.x;
		
	    //check for swipe motion to move the camera left and right
	    if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
		    startPos = currentPos;
		    startTime = Time.time;
			
		    canSwitch = true;
	    }
	    else if(Input.GetMouseButton(0) && canSwitch){
		    if(Time.time - startTime > maxDragTime){
			    canSwitch = false;
		    }
		    else if(Mathf.Abs(startPos - currentPos) > dragDistance){
			    ChangeCharacter(currentPos < startPos);
				
			    canSwitch = false;
		    }
	    }
    }
    public void ChangeCharacter(bool right){
	    if((current == 0 && !right) || (current == mannequinCount - 1 && right))
		    return;
		
	    current += right ? 1 : -1;
	    UpdateCamera();
    }
    private void UpdateCamera() {
	    camTarget = Vector3.right * (dist * current);
		
	    if(current < characters.Length)
		    nameLabel.text = characters[current].name;
		
	    bool unlocked = PlayerPrefs.GetInt("Unlocked" + current) == 1 || current < 4;
		
	    unlockButton.SetActive(!unlocked);
	    leftButton.SetActive(current > 0);
	    rightButton.SetActive(current < mannequinCount - 1);
    }
    
    public void Select(){
	    Debug.Log(1);
	    PlayerPrefs.SetInt("Player", current);
	    SceneManager.LoadScene(gameScene.Path);
    }
    
    public void Unlock(){
	    
	    //TODO: PLAY ADs
	    PlayerPrefs.SetInt("Unlocked" + current, 1);
	    PlayerPrefs.SetInt("Player", current);
		
	    unlockButton.SetActive(false);
	    
    }
}