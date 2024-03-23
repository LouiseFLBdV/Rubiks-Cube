using System;
using System.Linq;
using UnityEngine;
using static RubicksCubeCreator.CubeUtility;

namespace RubicksCubeCreator
{
    public class CubeCore
    {
        private Transform m_ParentTransform;
        private int m_Dimension;
        private Block m_BlockPrefab;
        private Block[,,] m_Blocks;

        public Block[] BlocksFlatten
        {
            get
            {
                return m_Blocks.Cast<Block>().ToArray();
            }
        }

        public Vector3 BlockScale
        {
            get
            {
                return m_ParentTransform.localScale;
            }
        }

        /// <summary>
        /// Constructor for drawing the cube
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="cubeSize"></param>
        /// <param name="renderParams"></param>
        /// <param name="cubeMesh"></param>
        /// <param name="parentTransform"></param>
        public CubeCore(CubePreset preset, Transform parentTransform)
        {
            m_Dimension = preset.Dimension;
            m_BlockPrefab = preset.BlockPrefab;
            m_ParentTransform = parentTransform;
            m_Blocks = new Block[m_Dimension, m_Dimension, m_Dimension];
            Vector3 centerOffset = new Vector3((m_Dimension - 1) / 2f, (m_Dimension - 1) / 2f, (m_Dimension - 1) / 2f);
            for (int i = 0; i < m_Dimension; i++)
            {
                for (int j = 0; j < m_Dimension; j++)
                {
                    for (int k = 0; k < m_Dimension; k++)
                    {
                        Vector3 settedPosition = new Vector3(i, j, k) - centerOffset;
                        GameObject cube = GameObject.Instantiate(m_BlockPrefab.gameObject);
                        Block rubicksBlock = cube.GetComponent<Block>();
                        cube.name = $"Block: {i}-{j}-{k}";
                        cube.transform.localScale = BlockScale;
                        cube.transform.SetParent(m_ParentTransform);
                        cube.transform.localPosition = settedPosition;
                        cube.transform.localRotation = Quaternion.identity;
                        rubicksBlock.Init(new Vector3Int(i, j, k), m_Dimension, preset.Materials);
                        m_Blocks[i, j, k] = rubicksBlock;
                    }
                }
            }
        }
        public void Destroy()
        {
            foreach (var item in BlocksFlatten)
            {
                GameObject.Destroy(item.gameObject);
            }
        }
        public bool IsSolved()
        {
            foreach (var item in BlocksFlatten)
            {
                if (!item.IsSolved)
                {
                    return false;
                }
            }
            return true;
        }
        public Block GetBlockAtPosition(Vector3Int position) => BlocksFlatten.Where(x => x.RealCurrentPosition == position).FirstOrDefault();
        public Block GetBlockOnThePoint(Vector3 point)
        {
            foreach (var item in BlocksFlatten)
            {
                Vector3[] bounds = new Vector3[2];
                bounds[0] = item.transform.position - item.transform.localScale / 1.8f;
                bounds[1] = item.transform.position + item.transform.localScale / 1.8f;
                if (point.x >= bounds[0].x && point.x <= bounds[1].x &&
                    point.y >= bounds[0].y && point.y <= bounds[1].y &&
                    point.z >= bounds[0].z && point.z <= bounds[1].z)
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// Get blocks on the X move
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Block[] GetRubicksBlocksX(int value)
        {
            Block[] rubicksBlocks = new Block[m_Dimension * m_Dimension];
            int index = 0;
            for (int i = 0; i < m_Dimension; i++)
            {
                for (int j = 0; j < m_Dimension; j++)
                {
                    rubicksBlocks[index] = BlocksFlatten.Where(x => x.CurrentPosition.x == value).ToArray()[index];
                    index++;
                }
            }
            return rubicksBlocks;
        }
        /// <summary>
        /// Get blocks on the Y move
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Block[] GetRubicksBlocksY(int value)
        {
            Block[] rubicksBlocks = new Block[m_Dimension * m_Dimension];
            int index = 0;
            for (int i = 0; i < m_Dimension; i++)
            {
                for (int j = 0; j < m_Dimension; j++)
                {
                    rubicksBlocks[index] = BlocksFlatten.Where(x => x.CurrentPosition.y == value).ToArray()[index];
                    index++;
                }
            }
            return rubicksBlocks;
        }
        /// <summary>
        /// Get blocks on the Z move
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Block[] GetRubicksBlocksZ(int value)
        {
            Block[] rubicksBlocks = new Block[m_Dimension * m_Dimension];
            int index = 0;
            for (int i = 0; i < m_Dimension; i++)
            {
                for (int j = 0; j < m_Dimension; j++)
                {
                    rubicksBlocks[index] = BlocksFlatten.Where(x => x.CurrentPosition.z == value).ToArray()[index];
                    index++;
                }
            }
            return rubicksBlocks;
        }
        /// <summary>
        /// Get blocks on the move
        /// </summary>
        /// <param name="dimensionIndex"></param>
        /// <param name="cubeDirectionAxis"></param>
        /// <returns></returns>
        public Block[] GetRubicksBlocks(int dimensionIndex, CubeDirectionAxis cubeDirectionAxis)
        {
            switch (cubeDirectionAxis)
            {
                case CubeDirectionAxis.X:
                    return GetRubicksBlocksX(dimensionIndex);
                case CubeDirectionAxis.Y:
                    return GetRubicksBlocksY(dimensionIndex);
                case CubeDirectionAxis.Z:
                    return GetRubicksBlocksZ(dimensionIndex);
                default:
                    return null;
            }
        }
        public Block[] GetRubicksBlocks(Sides side)
        {
            Block[] rubicksBlocks = new Block[m_Dimension * m_Dimension];
            switch(side)
            {
                case Sides.Front:
                    rubicksBlocks = GetRubicksBlocksZ(m_Dimension - 1);
                    break;
                case Sides.Back:
                    rubicksBlocks = GetRubicksBlocksZ(0);
                    break;
                case Sides.Left:
                    rubicksBlocks = GetRubicksBlocksX(0);
                    break;
                case Sides.Right:
                    rubicksBlocks = GetRubicksBlocksX(m_Dimension - 1);
                    break;
                case Sides.Top:
                    rubicksBlocks = GetRubicksBlocksY(m_Dimension - 1);
                    break;
                case Sides.Bottom:
                    rubicksBlocks = GetRubicksBlocksY(0);
                    break;
            }
            return rubicksBlocks;
        }
        public Block[] GetRubicksBlocks(CubeMove move)
        {
            return GetRubicksBlocks(move.LayerIndex, move.Direction);
        }
        public BlocksRotator GetRotatorForBlocks(Block[] rubicksBlocks, CubeMove move)
        {
            BlocksRotator blocksRotator = new BlocksRotator(m_ParentTransform, rubicksBlocks.ToList(), move);
            return blocksRotator;
        }
        public Sides GetSideWithPoint(Vector3 point)
        {
            foreach (Sides s in Enum.GetValues(typeof(Sides)))
            {
                if(s == Sides.None) continue;
                Block[] blocks = GetRubicksBlocks(s);
                Vector3[] bounds = new Vector3[2];
                bounds[0] = blocks[0].transform.position - blocks[0].transform.localScale / 1.8f;
                bounds[1] = blocks[blocks.Length - 1].transform.position + blocks[blocks.Length - 1].transform.localScale / 1.8f;
                if (point.x >= bounds[0].x && point.x <= bounds[1].x &&
                    point.y >= bounds[0].y && point.y <= bounds[1].y &&
                    point.z >= bounds[0].z && point.z <= bounds[1].z)
                {
                    return s;
                }
            }
            return Sides.None;
        }
        public Sides GetSideWithNormal(Vector3 normal)
        {
            if(Vector3.Dot(normal,m_ParentTransform.TransformDirection(Vector3.left)) == 1) return Sides.Left;
            else if(Vector3.Dot(normal,m_ParentTransform.TransformDirection(Vector3.right)) == 1) return Sides.Right;
            else if(Vector3.Dot(normal,m_ParentTransform.TransformDirection(Vector3.up)) == 1) return Sides.Top;
            else if(Vector3.Dot(normal,m_ParentTransform.TransformDirection(Vector3.down)) == 1) return Sides.Bottom;
            else if(Vector3.Dot(normal,m_ParentTransform.TransformDirection(Vector3.forward)) == 1) return Sides.Front;
            else if(Vector3.Dot(normal,m_ParentTransform.TransformDirection(Vector3.back)) == 1) return Sides.Back;
            else return Sides.None;
        }
        public Sides GetClosestSideWithNormal(Vector3 normal)
        {
            Sides side = GetSideWithNormal(normal);
            if(side != Sides.None) return side;
            else
            {
                float min = float.MaxValue;
                Sides closestSide = Sides.None;
                foreach(Sides s in Enum.GetValues(typeof(Sides)))
                {
                    if(s == Sides.None) continue;
                    float dot = Vector3.Dot(normal,m_ParentTransform.TransformDirection(GetNormalOfSide(s)));
                    if(dot < min)
                    {
                        min = dot;
                        closestSide = s;
                    }
                }
                return closestSide;
            }
        }
        public Vector3 GetNormalOfSide(Sides side)
        {
            switch(side)
            {
                case Sides.Front:
                    return m_ParentTransform.TransformDirection(Vector3.forward);
                case Sides.Back:
                    return m_ParentTransform.TransformDirection(Vector3.back);
                case Sides.Left:
                    return m_ParentTransform.TransformDirection(Vector3.left);
                case Sides.Right:
                    return m_ParentTransform.TransformDirection(Vector3.right);
                case Sides.Top:
                    return m_ParentTransform.TransformDirection(Vector3.up);
                case Sides.Bottom:
                    return m_ParentTransform.TransformDirection(Vector3.down);
                default:
                    return Vector3.zero;
            }
        }
        public Color[] GetColorsOfFace(Sides side)
        {
            Color[] colors = new Color[m_Dimension * m_Dimension];
            Block[] blocks = new Block[colors.Length];
            switch (side)
            {
                case Sides.None:
                    break;
                case Sides.Left:
                    blocks = GetRubicksBlocks(side).SortByZ().SortByY();
                    break;
                case Sides.Right:
                    blocks = GetRubicksBlocks(side).SortByZ(true).SortByY();
                    break;
                case Sides.Bottom:
                    blocks = GetRubicksBlocks(side).SortByX().SortByZ();
                    break;
                case Sides.Top:
                    blocks = GetRubicksBlocks(side).SortByX(true).SortByZ();
                    break;
                case Sides.Back:
                    blocks = GetRubicksBlocks(side).SortByX().SortByY();
                    break;
                case Sides.Front:
                    blocks = GetRubicksBlocks(side).SortByX(true).SortByY();
                    break;
                default:
                    break;
            }
            for (int i = 0; i < colors.Length; i++)
            {
                Block currentBlock = blocks[i];
                Color blockSideColor =
                    currentBlock.GetVisibleSideColor(currentBlock.RelativeSide(side));
                colors[i] = blockSideColor;
            }
            return colors;
        }
    }
}
