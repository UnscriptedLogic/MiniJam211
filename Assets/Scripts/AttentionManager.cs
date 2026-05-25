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
    [SerializeField] private LevelManagerScript levelManager;

    private int npcCount;   
    public int npcDeadCount;

    public static AttentionManager instance;
    
    private List<AttentionComponent> attentionComponents;

    public List<AttentionComponent> AttentionComponents => attentionComponents;

    public LostScreenManager lostScreen;

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
    
    private void Awake()
    {
        npcDeadCount = 0;
        GameObject[] allObjects = FindObjectsByType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("Seeker") && obj.activeInHierarchy)
            {
                npcCount++;
            }
        }
        npcDeadCount = 0;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        attentionComponents = new List<AttentionComponent>();
    }

    public void HandleAttentionDepleted()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        if (npcs.Length == 0)
        {
            lostScreen.levelDied = levelManager.levelIndex + 1;
            levelManager.LevelLost();
        }

    }

    
}
