using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data",menuName = "Data/PlayerData")]
public class PlayerData : DescriptionBaseSO
{
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;
    [SerializeField] float ballRange;
    [SerializeField] float force;
    [SerializeField] float upForce;
    [SerializeField] float moveRange;
    [SerializeField] float powerbarSpeedMin;
    [SerializeField] float powerbarSpeedMax;
    [SerializeField] float powerbarMaxSlowdown;
    [SerializeField] float ballMoveSpeed;
    
    public float Speed => speed;
    public float TurnSpeed => turnSpeed;
    public float BallRange => ballRange;
    public float Force => force;
    public float UpForce => upForce;
    public float MoveRange => moveRange;
    public float PowerbarSpeedMin => powerbarSpeedMin;
    public float PowerbarSpeedMax => powerbarSpeedMax;
    public float PowerbarMaxSlowdown => powerbarMaxSlowdown;
    public float BallMoveSpeed => ballMoveSpeed;
}