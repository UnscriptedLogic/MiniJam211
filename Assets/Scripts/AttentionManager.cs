using System;
using Components;
using UnityEngine;

public class AttentionGivenArgs : EventArgs
{
    public float value;
    public AttentionGiverComponent instigator;
    public AttentionComponent recipient;
}

public class AttentionManager : MonoBehaviour
{
    
    
    public static EventHandler<AttentionGivenArgs> OnAttentionGiven;
}
