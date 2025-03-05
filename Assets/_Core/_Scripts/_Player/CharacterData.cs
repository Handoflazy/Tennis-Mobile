using System;
using System.ComponentModel.Design;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Player Data",menuName = "Data/PlayerData")]
public class CharacterData : DescriptionBaseSO //TODO: Make it  to be ServiceLocator
{
    [BoxGroup("Locomotion Settings"),SerializeField] float speed;
    [BoxGroup("Locomotion Settings"),SerializeField] float turnSpeed;
    [BoxGroup("Locomotion Settings"),SerializeField] float moveRange;
    
    [BoxGroup("Ball Settings"),SerializeField] float ballMoveSpeed;
    [BoxGroup("Ball Settings"),SerializeField] float ballRange;
    [BoxGroup("Ball Settings"),SerializeField] float force;
    [BoxGroup("Ball Settings"),SerializeField] float upForce;
    
    [FormerlySerializedAs("powerbarSpeedMin")] [BoxGroup("Power bar Settings"),SerializeField] float powerBarSpeedMin;
    [FormerlySerializedAs("powerbarSpeedMax")] [BoxGroup("Power bar Settings"),SerializeField] float powerBarSpeedMax;
    [BoxGroup("Power bar Settings"),SerializeField] float powerBarMaxSlowdown;
    
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