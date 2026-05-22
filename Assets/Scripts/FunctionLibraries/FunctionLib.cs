using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.FunctionLibraries
{
    //Some (hopefully) useful shit. Call them anywhere. Their static anyway.
    
    public static class FunctionLib
    {
        public static List<GameObject> CircleCheck2D(Vector2 center, float radius, LayerMask layerMask)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, layerMask);
            List<GameObject> results = new List<GameObject>(hits.Length);

            foreach (Collider2D hit in hits)
            {
                if (hit == null)
                {
                    continue;
                }

                GameObject go = hit.gameObject;
                if (!results.Contains(go))
                {
                    results.Add(go);
                }
            }

            return results;
        }

        public static List<GameObject> FilterGameObjectsWithInterface<TInterface>(IEnumerable<GameObject> gameObjects)
            where TInterface : class
        {
            List<GameObject> filtered = new List<GameObject>();

            foreach (GameObject go in gameObjects)
            {
                if (go == null)
                {
                    continue;
                }

                TInterface component = go.GetComponent(typeof(TInterface)) as TInterface;
                if (component != null)
                {
                    filtered.Add(go);
                }
            }

            return filtered;
        }

        public static List<GameObject> CircleCheck2DWithInterface<TInterface>(Vector2 center, float radius, LayerMask layerMask)
            where TInterface : class
        {
            List<GameObject> hits = CircleCheck2D(center, radius, layerMask);
            return FilterGameObjectsWithInterface<TInterface>(hits);
        }

        public static bool Raycast2D(Vector2 origin, Vector2 direction, float distance, LayerMask layerMask, out RaycastHit2D hit)
        {
            Vector2 normalizedDirection = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.zero;
            hit = Physics2D.Raycast(origin, normalizedDirection, distance, layerMask);
            return hit.collider != null;
        }

        public static RaycastHit2D[] RaycastAll2DSorted(Vector2 origin, Vector2 direction, float distance, LayerMask layerMask)
        {
            Vector2 normalizedDirection = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.zero;
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, normalizedDirection, distance, layerMask);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            return hits;
        }

        public static bool TryGetClosestRaycastHit2D(
            Vector2 origin,
            Vector2 direction,
            float distance,
            LayerMask layerMask,
            out RaycastHit2D closestHit)
        {
            RaycastHit2D[] hits = RaycastAll2DSorted(origin, direction, distance, layerMask);
            if (hits.Length > 0 && hits[0].collider != null)
            {
                closestHit = hits[0];
                return true;
            }

            closestHit = default;
            return false;
        }

        public static bool HasLineOfSight2D(Vector2 from, Vector2 to, LayerMask blockingMask)
        {
            Vector2 delta = to - from;
            float distance = delta.magnitude;
            if (distance <= 0.001f)
            {
                return true;
            }

            RaycastHit2D hit = Physics2D.Raycast(from, delta.normalized, distance, blockingMask);
            return hit.collider == null;
        }

        public static bool IsGroundedByRay2D(
            Vector2 origin,
            float checkDistance,
            LayerMask groundMask,
            out RaycastHit2D groundHit)
        {
            groundHit = Physics2D.Raycast(origin, Vector2.down, checkDistance, groundMask);
            return groundHit.collider != null;
        }

        public static bool IsHittingWallByRay2D(
            Vector2 origin,
            float checkDistance,
            float facingDirectionX,
            LayerMask wallMask,
            out RaycastHit2D wallHit)
        {
            Vector2 direction = facingDirectionX >= 0f ? Vector2.right : Vector2.left;
            wallHit = Physics2D.Raycast(origin, direction, checkDistance, wallMask);
            return wallHit.collider != null;
        }

        public static List<GameObject> RaycastAll2DWithInterface<TInterface>(
            Vector2 origin,
            Vector2 direction,
            float distance,
            LayerMask layerMask)
            where TInterface : class
        {
            RaycastHit2D[] hits = RaycastAll2DSorted(origin, direction, distance, layerMask);
            List<GameObject> hitObjects = new List<GameObject>(hits.Length);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null)
                {
                    continue;
                }

                GameObject go = hit.collider.gameObject;
                if (!hitObjects.Contains(go))
                {
                    hitObjects.Add(go);
                }
            }

            return FilterGameObjectsWithInterface<TInterface>(hitObjects);
        }
    }
}
