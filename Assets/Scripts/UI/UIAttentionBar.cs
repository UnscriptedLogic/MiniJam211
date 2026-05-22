using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIAttentionBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        
        public void SetSliderValue(float value)
        {
            slider.value = value;
        }
    }
}