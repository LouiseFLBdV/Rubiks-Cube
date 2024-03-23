using System.Collections.Generic;
using UnityEngine;
using static RubicksCubeCreator.CubeUtility;

namespace RubicksCubeCreator
{
    [AddComponentMenu("RubicksCubeCreator/Block")]

    public class Block : MonoBehaviour
    {
        private int m_dimensionLength;
        [SerializeField] private MeshRenderer m_MeshRenderer;
        public Vector3Int StartPosition { get; private set; }
        private BlockSide m_VisibleSides = BlockSide.None;
        public Vector3Int CurrentPosition
        {
            get
            {
                Vector3 centerOffset = new Vector3((m_dimensionLength - 1) / 2f, (m_dimensionLength - 1) / 2f, (m_dimensionLength - 1) / 2f);
                Vector3 currentPosition = transform.localPosition + centerOffset;
                Vector3Int currentPositionInt = new Vector3Int(Mathf.RoundToInt(currentPosition.x), Mathf.RoundToInt(currentPosition.y), Mathf.RoundToInt(currentPosition.z));
                return currentPositionInt;
            }
        }
        public Vector3Int RealCurrentPosition
        {
            get
            {
                Vector3 currentPosition = transform.localPosition;
                Vector3Int currentPositionInt = new Vector3Int(Mathf.RoundToInt(currentPosition.x), Mathf.RoundToInt(currentPosition.y), Mathf.RoundToInt(currentPosition.z));
                return currentPositionInt;
            }
        }
        public bool IsSolved
        {
            get
            {
                return StartPosition == CurrentPosition;
            }
        }

        public void Init(Vector3Int startPosition, int dimensionLength, CubeMaterialPack materials)
        {
            StartPosition = startPosition;
            m_dimensionLength = dimensionLength;
            m_VisibleSides = CubeUtility.GetVisibleSides(StartPosition, m_dimensionLength);
            SetMaterials(materials);
        }
        public Color GetVisibleSideColor(BlockSide side)
        {
            if (!m_VisibleSides.HasFlag(side))
            {
                Debug.LogError($"Block {name} doesn't have side {side} visible");
                return Color.clear;
            }
            switch (side)
            {
                case BlockSide.Back:
                    return m_MeshRenderer.sharedMaterials[0].color;
                case BlockSide.Right:
                    return m_MeshRenderer.sharedMaterials[1].color;
                case BlockSide.Top:
                    return m_MeshRenderer.sharedMaterials[2].color;
                case BlockSide.Left:
                    return m_MeshRenderer.sharedMaterials[3].color;
                case BlockSide.Front:
                    return m_MeshRenderer.sharedMaterials[4].color;
                case BlockSide.Bottom:
                    return m_MeshRenderer.sharedMaterials[6].color;
                default:
                    return Color.black;
            }
        }
        public int GetLayerIndex(CubeDirectionAxis cubeDirectionAxis)
        {
            switch (cubeDirectionAxis)
            {
                case CubeDirectionAxis.X:
                    return CurrentPosition.x;
                case CubeDirectionAxis.Y:
                    return CurrentPosition.y;
                case CubeDirectionAxis.Z:
                    return CurrentPosition.z;
                default:
                    return 0;
            }
        }
        public BlockSide RelativeSide(Sides side)
        {
            // Calculate relative side to rotation
            return GetSideWithVector3(side.ToVector3());
        }
        public BlockSide GetSideWithVector3(Vector3 direction)
        {
            float upDot = Vector3.Dot(transform.up, transform.parent.TransformDirection(direction));
            float rightDot = Vector3.Dot(transform.right, transform.parent.TransformDirection(direction));
            float forwardDot = Vector3.Dot(transform.forward, transform.parent.TransformDirection(direction));
            if (Mathf.Approximately(Mathf.Abs(upDot), 1f))
            {
                if (upDot > 0f) //Up
                {
                    return BlockSide.Top;
                }
                else //Down
                {
                    return BlockSide.Bottom;
                }
            }
            else if (Mathf.Approximately(Mathf.Abs(rightDot), 1f))
            {
                if (rightDot > 0f) //Right
                {
                    return BlockSide.Right;
                }
                else //Left
                {
                    return BlockSide.Left;
                }
            }
            else if (Mathf.Approximately(Mathf.Abs(forwardDot), 1f))
            {
                if (forwardDot > 0f) //Forward
                {
                    return BlockSide.Front;
                }
                else //Back
                {
                    return BlockSide.Back;
                }
            }
            return BlockSide.None;
        }
        [ContextMenu("Debug me")]
        public void DebugMe()
        {
            Debug.Log($"Start position: {StartPosition} Current position: {CurrentPosition}, Real Current Position {RealCurrentPosition} ", gameObject);
            Debug.Log($"Visible Sides {m_VisibleSides}");
        }
        private void SetMaterials(CubeMaterialPack materials)
        {
            List<Material> mats = new List<Material>
            {
                m_VisibleSides.HasFlag(BlockSide.Back) ? materials.BlockMaterialBack : materials.BlockMaterialBase,
                m_VisibleSides.HasFlag(BlockSide.Right) ? materials.BlockMaterialRight : materials.BlockMaterialBase,
                m_VisibleSides.HasFlag(BlockSide.Top) ? materials.BlockMaterialTop : materials.BlockMaterialBase,
                m_VisibleSides.HasFlag(BlockSide.Left) ? materials.BlockMaterialLeft : materials.BlockMaterialBase,
                m_VisibleSides.HasFlag(BlockSide.Front) ? materials.BlockMaterialFront : materials.BlockMaterialBase,
                materials.BlockMaterialBase,
                m_VisibleSides.HasFlag(BlockSide.Bottom) ? materials.BlockMaterialBottom : materials.BlockMaterialBase
            };
            m_MeshRenderer.sharedMaterials = mats.ToArray();
        }
    }
}
