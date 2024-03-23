using System.Collections.Generic;
using UnityEngine;

namespace RubicksCubeCreator
{
    public class BlocksRotator
    {
        private Transform m_Transform;
        private Transform m_MainParent;
        private List<Block> rubicksBlocks = new List<Block>();
        private CubeMove m_Move;
        public Transform Transform { get => m_Transform; }
        public List<Block> RubicksBlocks { get => rubicksBlocks; }
        public CubeMove Move { get => m_Move; }

        public BlocksRotator(Transform parent, List<Block> rubicksBlocks, CubeMove move)
        {
            GameObject rotator = new GameObject("Rotator");
            m_MainParent = parent;
            m_Transform = rotator.transform;
            m_Transform.SetParent(m_MainParent);
            m_Transform.localPosition = Vector3.zero;
            m_Transform.localEulerAngles = Vector3.zero;
            this.rubicksBlocks = rubicksBlocks;
            m_Move = move;
            rubicksBlocks.ForEach(x => x.transform.SetParent(Transform));
        }
        public void Kill()
        {
            rubicksBlocks.ForEach(x => x.transform.SetParent(m_MainParent));
            GameObject.Destroy(Transform.gameObject);
        }
        public void CompleteAndKill(float completeAngle)
        {
            Rotate(completeAngle);
            Kill();
        }
        public void Rotate(float angle)
        {
            Vector3 rotatorAngles = Transform.localEulerAngles;
            switch (m_Move.Direction)
            {
                case CubeDirectionAxis.None:
                    break;
                case CubeDirectionAxis.X:
                    rotatorAngles.x = angle;
                    break;
                case CubeDirectionAxis.Y:
                    rotatorAngles.y = angle;
                    break;
                case CubeDirectionAxis.Z:
                    rotatorAngles.z = angle;
                    break;
                default:
                    break;
            }
            Transform.localEulerAngles = rotatorAngles;
        }
    }
}
