using System;
using System.Collections.Generic;
using Components;
using UnityEngine;


public class AttentionGivenArgs : EventArgs
{
    public float value;
    public AttentionGiverComponent instigator;
    public AttentionComponent recipient;
}

//Centralized stuff for attention. We can potentially pull everyone's current attention
//and do something with it. Probably display it in the UI or stuff.

public class AttentionManager : MonoBehaviour
{
    
    public static AttentionManager instance;
    
    private List<AttentionComponent> attentionComponents;

    public List<AttentionComponent> AttentionComponents => attentionComponents;
    
    public EventHandler<AttentionGivenArgs> OnAttentionGiven;
    public EventHandler<AttentionComponent> OnAttentionDepleted;

    public void AddAttentionComponent(AttentionComponent attentionComponent)
    {
        if (attentionComponent != null && !attentionComponents.Contains(attentionComponent))
        {
            attentionComponents.Add(attentionComponent);
        }
    }
    
    public void RemoveAttentionComponent(AttentionComponent attentionComponent)
    {
        if (attentionComponent != null && attentionComponents.Contains(attentionComponent))
        {
            attentionComponents.Remove(attentionComponent);
        }
    }
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        attentionComponents = new List<AttentionComponent>();
    }
}
