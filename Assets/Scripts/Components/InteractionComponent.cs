using System;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.FunctionLibraries;
using UnityEngine;

namespace Components
{
    //If you've played Minecraft, this is basically the WYLA/Jade mod (What You're Looking At)
    //Anything to do with gathering information about the world and the IInteractable stuff goes here
    
    public class InteractionComponent : MonoBehaviour
    {
        [SerializeField] private float interactionRange = 1f;
        [SerializeField] private List<(GameObject, IInteractable)> interactables;

        [Header("UI")]
        [SerializeField] private UIInteractWidget interactWidgetPrefab;
        [SerializeField] private Vector3 uiOffset = new Vector3(0, 1f, 0);
        
        private UIInteractWidget interactWidget;
        
        private List<(GameObject, IInteractable)> interactablesInRange = new List<(GameObject, IInteractable)>();
        public List<(GameObject, IInteractable)> InteractablesInRange => interactablesInRange;
        public bool HasInteractablesInRange => interactablesInRange.Count > 0;
        public (GameObject, IInteractable) GetFirstInteractableInRange => interactablesInRange[0];
        

        private void Start()
        {
            interactables = new List<(GameObject, IInteractable)>();
            if (interactWidgetPrefab != null)
            {
                interactWidget = Instantiate(interactWidgetPrefab, transform.position + uiOffset, Quaternion.identity, transform);
                interactWidget.SetVisible(false);
            }
            
        }

        private void FixedUpdate()
        {
            interactablesInRange = GetInteractables();
        }

        public List<(GameObject, IInteractable)> GetInteractables()
        {
            List<(GameObject, IInteractable)> circleCheck = FunctionLib.CircleCheck2DWithInterface<IInteractable>(transform.position, interactionRange, LayerMask.GetMask("Default"));
            List<(GameObject, IInteractable)> filtered = new List<(GameObject, IInteractable)>();
            
            for (int i = 0; i < circleCheck.Count; i++)
            {
                (GameObject, IInteractable) interactable = circleCheck[i];

                if (interactable.Item2.CanInteract(this))
                {
                    filtered.Add(interactable);
                }
            }

            return filtered;
        }

        public void ShowInteract(IInteractable interactable)
        {
            interactWidget.ShowInteract("E", interactable);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }

        public void HideInteract()
        {
            interactWidget.SetVisible(false);
        }
    }
}