using UnityEngine;

[CreateAssetMenu(fileName = "Player state", menuName = "Player state", order = 0)]
public class PlayerState : ScriptableObject
{
    public bool CanMove { get; set; }
    public bool CanJump { get; set; }
}