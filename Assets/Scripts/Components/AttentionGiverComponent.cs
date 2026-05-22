using UnityEngine;

namespace Components
{
    //I don't actually know what to really do with this class, but when I created the event,
    //it didn't feel right to pass in a gameObject or Monobehaviour, so now this class exists
    
    public class AttentionGiverComponent : MonoBehaviour
    {
        [SerializeField] private float attentionGiveValue = 1f;
        [SerializeField] private float attentionAmount = 10f;
        [SerializeField] private float attentionMax = 10f;

        public void GiveAttention(float value, AttentionComponent recipient)
        {
            recipient.RecieveAttention(value, this);
        }
    }
}