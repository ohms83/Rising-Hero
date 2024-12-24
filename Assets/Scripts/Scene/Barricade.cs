#define DRAW_GIZMO

using System.Collections.Generic;
using UnityEngine;

namespace Scene
{
    public class Barricade : MonoBehaviour
    {
        private Collider2D m_collider2D;

        private void Start()
        {
            m_collider2D = GetComponent<Collider2D>();
        }

#if DRAW_GIZMO
        private readonly List<Vector2> m_contactPoints = new();
        private readonly List<ColliderDistance2D> m_separationVectors = new();
#endif
        private void OnCollisionStay2D(Collision2D other)
        {
            var separationVector = other.collider.Distance(m_collider2D);
            other.transform.Translate(separationVector.normal * separationVector.distance);
            
#if DRAW_GIZMO
            m_contactPoints.Clear();
            foreach (var contact in other.contacts)
            {
                m_contactPoints.Add(contact.point);
            }
            
            m_separationVectors.Clear();
            m_separationVectors.Add(separationVector);
#endif
        }

        private void OnCollisionExit2D(Collision2D other)
        {
#if DRAW_GIZMO
            m_contactPoints.Clear();
            m_separationVectors.Clear();
#endif
        }

        private void OnDrawGizmos()
        {
#if DRAW_GIZMO
            foreach (var contact in m_contactPoints)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(contact, new Vector3(0.1f, 0.1f, 0.1f));
            }
            foreach (var separation in m_separationVectors)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(separation.pointB, separation.normal * separation.distance);
            }
#endif
        }
    }
}
