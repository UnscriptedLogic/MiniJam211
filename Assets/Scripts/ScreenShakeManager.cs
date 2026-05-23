using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    public static ScreenShakeManager Instance;

    [SerializeField] private float shakeForce = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(shakeForce);
    }
}
