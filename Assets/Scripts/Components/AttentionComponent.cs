using System;
using Components;
using UI;
using Unity.Cinemachine;
using UnityEngine;

namespace Components
{
    //Anything that requires attention must have this component so any logic to do with attention recieving
    //goes here
    
    public class AttentionComponent : MonoBehaviour
    {
        public float attentionValue = 1f;
        [SerializeField] private float attentionDecay = 0.05f;
        [SerializeField] private float attentionMax = 1f;
        
        [Header("UI")]
        [SerializeField] private UIAttentionBar attentionBarPrefab;
        [SerializeField] private Vector3 uiOffset = new Vector3(0, 1f, 0);

        private UIAttentionBar attentionBar;

        [SerializeField] private CinemachineImpulseSource impulseSource;

        [SerializeField] private ParticleSystem heartParticle;

        private void Start()
        {
            if (attentionBarPrefab != null)
            {
                attentionBar = Instantiate(attentionBarPrefab, transform.position + uiOffset, Quaternion.identity, transform);
            }
            
            AttentionManager.instance?.AddAttentionComponent(this);
            
            SetAttentionValue(attentionValue);
        }

        public void SetAttentionValue(float value)
        {
            attentionValue = value;
            if (attentionBar != null)
            {
                attentionBar.SetSliderValue(attentionValue / attentionMax);
            }
        }

        public void DecayAttention()
        {
            attentionValue -= attentionDecay;
            attentionValue = Mathf.Clamp(attentionValue, 0, attentionMax);
            if (attentionBar != null)
            {
                attentionBar.SetSliderValue(attentionValue / attentionMax);
            }

            if (attentionValue <= 0)
            {
                ScreenShakeManager.Instance.CameraShake(impulseSource);
                Debug.Log("Attention Depleted!");
                AttentionDepleted();
            }
        }

        private void AttentionDepleted()
        {
            AttentionManager.instance?.OnAttentionDepleted?.Invoke(this, this);
        }

        public void RecieveAttention(float value, AttentionGiverComponent instigator)
        {
            attentionValue += value;
            attentionValue = Mathf.Clamp(attentionValue, 0, attentionMax);
            
            SetAttentionValue(attentionValue);

            AttentionManager.instance?.OnAttentionGiven?.Invoke(this, new AttentionGivenArgs()
            {
                value = value,
                instigator = instigator,
                recipient = this
            });

            SpawnHeartParticles();
        }

        private void SpawnHeartParticles()
        {
            if (heartParticle != null)
            {
                ParticleSystem particles = Instantiate(heartParticle, transform.position, Quaternion.identity);
            }
        }
    }

}
