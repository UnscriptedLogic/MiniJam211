using System;
using Components;
using Pathfinding;
using UI;
using Unity.Cinemachine;
using UnityEngine;

namespace Components
{
    //Anything that requires attention must have this component so any logic to do with attention recieving
    //goes here
    
    public class AttentionComponent : MonoBehaviour
    {
        [SerializeField] float currentAttentionValue = 1f;
        [SerializeField] private float attentionDecay = 0.05f;
        [SerializeField] private float attentionMax = 1f;
        
        [Header("UI")]
        [SerializeField] private UIAttentionBar attentionBarPrefab;
        [SerializeField] private Vector3 uiOffset = new Vector3(0, 1f, 0);

        private UIAttentionBar attentionBar;

        [SerializeField] private CinemachineImpulseSource impulseSource;

        [SerializeField] private ParticleSystem heartParticle;

        public float CurrentAttentionValue => currentAttentionValue;
        
        public event Action<float> OnAttentionChanged;
        public event Action<AttentionComponent> OnAttentionDepleted;
        public event Action<AttentionComponent> OnAttentionGiven;

        private void Start()
        {
            if (attentionBarPrefab != null)
            {
                attentionBar = Instantiate(attentionBarPrefab, transform.position + uiOffset, Quaternion.identity, transform);
            }
            
            AttentionManager.instance?.AddAttentionComponent(this);
            
            SetAttentionValue(currentAttentionValue);
        }

        public void SetAttentionValue(float value)
        {
            currentAttentionValue = value;
            if (attentionBar != null)
            {
                attentionBar.SetSliderValue(currentAttentionValue / attentionMax);
            }
        }

        public void DecayAttention()
        {
            currentAttentionValue -= attentionDecay;
            currentAttentionValue = Mathf.Clamp(currentAttentionValue, 0, attentionMax);
            if (attentionBar != null)
            {
                attentionBar.SetSliderValue(currentAttentionValue / attentionMax);
            }

            OnAttentionChanged?.Invoke(currentAttentionValue);
            
            
            if (currentAttentionValue <= 0)
            {
                ScreenShakeManager.Instance.CameraShake(impulseSource);
                AttentionDepleted();
                
                Destroy(gameObject);
            }
        }

        private void AttentionDepleted()
        {
            OnAttentionDepleted?.Invoke(this);
        }

        public void RecieveAttention(float value, AttentionGiverComponent instigator)
        {
            currentAttentionValue += value;
            currentAttentionValue = Mathf.Clamp(currentAttentionValue, 0, attentionMax);
            
            SetAttentionValue(currentAttentionValue);

            OnAttentionGiven?.Invoke(this);
            OnAttentionChanged?.Invoke(currentAttentionValue);

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
