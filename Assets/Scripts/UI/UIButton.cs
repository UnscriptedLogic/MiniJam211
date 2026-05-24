using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    
    public Button Button => button;
}
