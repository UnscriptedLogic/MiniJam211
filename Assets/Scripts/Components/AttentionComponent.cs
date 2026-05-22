using Components;
using UnityEngine;

namespace Components
{
    //Anything that requires attention must have this component so any logic to do with attention recieving
    //goes here
    
    public class AttentionComponent : MonoBehaviour
    {
        public void GiveAttention(float value, AttentionGiverComponent instigator)
        {
            AttentionManager.OnAttentionGiven(this, new AttentionGivenArgs()
            {
                value = value,
                instigator = instigator,
                recipient = this
            });
        }
    }

}