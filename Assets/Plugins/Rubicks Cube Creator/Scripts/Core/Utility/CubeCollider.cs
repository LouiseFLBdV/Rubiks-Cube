using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubicksCubeCreator
{
    [RequireComponent(typeof(Cube),typeof(BoxCollider))]
    [AddComponentMenu("RubicksCubeCreator/Cube Collider")]
    public class CubeCollider : MonoBehaviour
    {
        private BoxCollider m_BoxCollider;
        private Cube m_RubicksCube;
        private void Awake()
        {
            m_RubicksCube = GetComponent<Cube>();
            m_BoxCollider = GetComponent<BoxCollider>();
        }
        private void Update()
        {
            m_BoxCollider.size = m_RubicksCube.transform.localScale * m_RubicksCube.Preset.Dimension;
            m_BoxCollider.center = Vector3.zero;
        }   
#if UNITY_EDITOR
        private BoxCollider m_BoxTarget => GetComponent<BoxCollider>();
        private Cube m_CubeTarget => GetComponent<Cube>();
        private CubePreset m_Preset => m_CubeTarget.Preset;
        private void OnDrawGizmosSeleted()
        {
            if (m_CubeTarget == null || m_Preset == null)
            {
                return;
            }
            Gizmos.color = Color.green;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(m_CubeTarget.transform.position, m_CubeTarget.transform.localScale * m_CubeTarget.Preset.Dimension);
        }
        private void OnValidate()
        {
            if (m_BoxTarget == null || m_CubeTarget == null || m_Preset == null)
            {
                return;
            }
            m_BoxTarget.size = m_CubeTarget.transform.localScale * m_CubeTarget.Preset.Dimension;
            m_BoxTarget.center = Vector3.zero;
            m_BoxTarget.hideFlags = HideFlags.NotEditable;
        }
#endif
    }
}
