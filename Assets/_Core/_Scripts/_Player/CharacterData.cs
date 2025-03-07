using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Player Data",menuName = "Data/PlayerData")]
public class CharacterData : DescriptionBaseSO //TODO: Make it  to be ServiceLocator
{
    [Header("Locomotion Settings")]
    [SerializeField]
    float speed;

    [SerializeField]
    float turnSpeed;

    [SerializeField]
    float moveRange;

    [Header("Ball Settings")][ SerializeField]
    float ballMoveSpeed;

   [SerializeField]
    float ballRange;

    [SerializeField]
    float force;

    [SerializeField]
    float upForce;

    [Header("Power bar Settings")]
    [SerializeField]
    float powerBarSpeedMin;

    [SerializeField]
    float powerBarSpeedMax;

    [SerializeField]
    float powerBarMaxSlowdown;
    
    [SerializeField] private float maxDragTime;
    [SerializeField] private float dragDistance;
    [SerializeField] private float swipeSensitivity;
    
    public float MaxDragTime => maxDragTime;
    public float DragDistance => dragDistance;
    public float SwipeSensitivity => swipeSensitivity;
    public float Speed => speed;
    public float TurnSpeed => turnSpeed;
    public float BallRange => ballRange;
    public float Force => force;
    public float UpForce => upForce;
    public float MoveRange => moveRange;
    public float PowerBarSpeedMin => powerBarSpeedMin;
    public float PowerBarSpeedMax => powerBarSpeedMax;
    public float PowerBarMaxSlowdown => powerBarMaxSlowdown;
    public float BallMoveSpeed => ballMoveSpeed;
}