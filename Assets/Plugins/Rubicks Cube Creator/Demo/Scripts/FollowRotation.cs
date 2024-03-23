using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubicksCubeCreator.Demo
{
    public class FollowRotation : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        private void LateUpdate()
        {
            transform.rotation = m_Target.rotation;
        }
    }
}
