using Components;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    [SerializeField] private AttentionManager attentionManager;
    [SerializeField] private AudioSource ambient1;
    [SerializeField] private AudioSource ambient2;
    [SerializeField] private AudioSource ambient3;
    [SerializeField] private AudioSource ambient4;

    void Update()
    {
        if (attentionManager != null && attentionManager.npcDeadCount == 1)
        {
            ambient1.Stop();
            ambient1.Play();
            ambient2.Play();
        }
        if (attentionManager != null && attentionManager.npcDeadCount == 2)
        {
            ambient1.Stop();
            ambient2.Stop();
            ambient1.Play();
            ambient2.Play();
            ambient3.Play();
        }
        if (attentionManager != null && attentionManager.npcDeadCount == 3)
        {
            ambient1.Stop();
            ambient2.Stop();
            ambient3.Stop();
            ambient1.Play();
            ambient2.Play();
            ambient3.Play();
            ambient4.Play();
        }
    }
}
