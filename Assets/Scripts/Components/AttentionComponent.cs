using System;
using Components;
using UI;
using UnityEngine;

namespace Components
{
    //Anything that requires attention must have this component so any logic to do with attention recieving
    //goes here
    
    public class AttentionComponent : MonoBehaviour
    {
        [SerializeField] private float attentionValue = 1f;
        [SerializeField] private float attentionDecay = 0.05f;
        [SerializeField] private float attentionMax = 1f;
        
        [Header("UI")]
        [SerializeField] private UIAttentionBar attentionBarPrefab;
        [SerializeField] private Vector3 uiOffset = new Vector3(0, 1f, 0);

        private UIAttentionBar attentionBar;
        
        private void Start()
        {
            attentionBar = Instantiate(attentionBarPrefab, transform.position + uiOffset, Quaternion.identity, transform);
            
            AttentionManager.instance.AddAttentionComponent(this);
        }

        public void DecayAttention()
        {
            attentionValue -= attentionDecay;
            attentionValue = Mathf.Clamp(attentionValue, 0, attentionMax);
            attentionBar.SetSliderValue(attentionValue/attentionMax);

            if (attentionValue <= 0)
            {
                AttentionDepleted();
            }
        }

        private void AttentionDepleted()
        {
            AttentionManager.OnAttentionDepleted(this, this);
        }

        public void RecieveAttention(float value, AttentionGiverComponent instigator)
        {
            attentionValue += value;
            attentionValue = Mathf.Clamp(attentionValue, 0, attentionMax);
            
            AttentionManager.OnAttentionGiven(this, new AttentionGivenArgs()
            {
                value = value,
                instigator = instigator,
                recipient = this
            });
            
            attentionBar.SetSliderValue(attentionValue/attentionMax);
        }
    }

}