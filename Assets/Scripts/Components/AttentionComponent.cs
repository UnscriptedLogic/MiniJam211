using System;
using System.Diagnostics;
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

        [SerializeField] private Entity entity;
        [SerializeField] private AttentionManager attentionManager;

        [Header("Sound")]
        [SerializeField] private AudioSource deathAudio;
        [SerializeField] private AudioSource attentionDepletionAudio;
        [SerializeField] private AudioSource attentionGainAudio;
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
            if (currentAttentionValue <= .8f && currentAttentionValue > .6f)
            {
                entity.AnimateSprite(1);
            }

            if (currentAttentionValue <= .6f && currentAttentionValue > .4f)
            {
                entity.AnimateSprite(2);
            }

            if (currentAttentionValue <= .4f && currentAttentionValue > .2f)
            {
                entity.AnimateSprite(3);
            }

            if (currentAttentionValue <= .2f && currentAttentionValue > 0)
            {
                entity.AnimateSprite(4);
            }

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
                bool isDead = true;
                if (isDead)
                {
                    AttentionDepleted();
                    deathAudio.Play();
                    Destroy(gameObject);
                    isDead = false;
                }
                
            }

        }

        private void AttentionDepleted()
        {
            OnAttentionDepleted?.Invoke(this);
            attentionManager.HandleAttentionDepleted();
        }

        public void RecieveAttention(float value, AttentionGiverComponent instigator)
        {
            if (currentAttentionValue >= .8f && currentAttentionValue < 1f)
            {
                entity.AnimateSprite(0);
            }

            if (currentAttentionValue >= .6f && currentAttentionValue < .8f)
            {
                entity.AnimateSprite(1);
            }

            if (currentAttentionValue >= .4f && currentAttentionValue < .6f)
            {
                entity.AnimateSprite(2);
            }

            if (currentAttentionValue >= .2f && currentAttentionValue < .4f)
            {
                entity.AnimateSprite(3);
            }

            currentAttentionValue += value;
            currentAttentionValue = Mathf.Clamp(currentAttentionValue, 0, attentionMax);
            
            SetAttentionValue(currentAttentionValue);

            OnAttentionGiven?.Invoke(this);
            OnAttentionChanged?.Invoke(currentAttentionValue);

            SpawnHeartParticles();

            attentionGainAudio.Play();
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
