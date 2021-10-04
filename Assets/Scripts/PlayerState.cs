using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player state", menuName = "Player state", order = 0)]
public class PlayerState : ScriptableObject
{
    [field: NonSerialized]
    public bool CanMove { get; set; }
    

    [field: NonSerialized]
    public bool CanJump { get; set; }
}
