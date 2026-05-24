using System;
using System.Collections.Generic;
using Components;
using JetBrains.Annotations;
using Unity.VisualScripting;
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
    private int npcCount;   
    public int npcDeadCount;

    public static AttentionManager instance;
    
    private List<AttentionComponent> attentionComponents;

    public List<AttentionComponent> AttentionComponents => attentionComponents;

    public void AddAttentionComponent(AttentionComponent attentionComponent)
    {
        if (attentionComponent != null && !attentionComponents.Contains(attentionComponent))
        {
            attentionComponents.Add(attentionComponent);
            //attentionComponent.OnAttentionDepleted += HandleAttentionDepleted;
        }
    }
    
    public void RemoveAttentionComponent(AttentionComponent attentionComponent)
    {
        if (attentionComponent != null && attentionComponents.Contains(attentionComponent))
        {
            attentionComponents.Remove(attentionComponent);
            //attentionComponent.OnAttentionDepleted -= HandleAttentionDepleted;
        }
    }
    
    /*public HUDScript HUDScript;
    private void Awake()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("Seeker") && obj.activeInHierarchy)
            {
                npcCount++;
            }
        }
        HUDScript.UpdateNPCCountUI(npcCount);
        npcDeadCount = 0;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        attentionComponents = new List<AttentionComponent>();
    }

    public void HandleAttentionDepleted(AttentionComponent depletedNPC)
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("Seeker") && obj.activeInHierarchy)
            {
                npcCount++;
            }
        }
        HUDScript.UpdateNPCCountUI(npcCount - npcDeadCount);
        NPCStatusCheck();
    }

    public void NPCStatusCheck()
    {
        foreach (var attention in attentionComponents)
        {
            if (attention == null) continue;

            if (attention.attentionValue <= 0)
            {
                npcDeadCount++;
                return;
            }
            if (npcDeadCount == npcCount)
            {
                Debug.Log("All NPCs are dead. Game over.");
            }
        }
    }   */
    
}
