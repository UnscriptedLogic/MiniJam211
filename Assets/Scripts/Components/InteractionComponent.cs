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

        public List<(GameObject, IInteractable)> Interactables => interactables;
        public bool HasInteractables => interactables.Count > 0;
        
        private void FixedUpdate()
        {
            interactables.Clear();
            
            interactables = GetInteractables();
        }

        public List<(GameObject, IInteractable)> GetInteractables()
        {
            List<(GameObject, IInteractable)> circleCheck = FunctionLib.CircleCheck2DWithInterface<IInteractable>(transform.position, 0.2f, LayerMask.GetMask("Default"));
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
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}