using System.Collections.Generic;
using UnityEngine;

namespace RubicksCubeCreator
{
    public static class CubeUtility
    {
        [System.Flags]
        public enum BlockSide
        {
            None = 0,
            Left = 1 << 0,   // 1 Red = 3
            Right = 1 << 1,  // 2 Orange = 1
            Bottom = 1 << 2,   // 4 Yellow = 6
            Top = 1 << 3,     // 8 White = 2
            Back = 1 << 4,   // 16 Blue = 0
            Front = 1 << 5,  // 32 Green = 4
            Base = 1 << 6,   // 64 Block base = 5
        }
        public enum Sides
        {
            None = 0,
            Left = 1 << 0,
            Right = 1 << 1,
            Bottom = 1 << 2,
            Top = 1 << 3,
            Back = 1 << 4,
            Front = 1 << 5,
        }

        public static int GetSideMaterialIndex(BlockSide side)
        {
            switch (side)
            {
                case BlockSide.Back:
                    return 0;
                case BlockSide.Right:
                    return 1;
                case BlockSide.Top:
                    return 2;
                case BlockSide.Left:
                    return 3;
                case BlockSide.Front:
                    return 4;
                case BlockSide.Base:
                    return 5;
                case BlockSide.Bottom:
                    return 6;
                default:
                    return -1;
            }
        }
        public static BlockSide GetOppositeSide(BlockSide side)
        {
            switch (side)
            {
                case BlockSide.Top:
                    return BlockSide.Bottom;
                case BlockSide.Right:
                    return BlockSide.Left;
                case BlockSide.Front:
                    return BlockSide.Back;
                case BlockSide.Left:
                    return BlockSide.Right;
                case BlockSide.Back:
                    return BlockSide.Front;
                case BlockSide.Bottom:
                    return BlockSide.Top;
                case BlockSide.Base:
                    return BlockSide.Base;
                default:
                    return BlockSide.None;
            }
        }
        public static BlockSide GetVisibleSides(Vector3Int position, int dimensionLength)
        {
            BlockSide visibleSides = BlockSide.None;
            if (position.x == 0)
            {
                visibleSides |= BlockSide.Left;
            }
            else if (position.x == dimensionLength - 1)
            {
                visibleSides |= BlockSide.Right;
            }
            if (position.y == 0)
            {
                visibleSides |= BlockSide.Bottom;
            }
            else if (position.y == dimensionLength - 1)
            {
                visibleSides |= BlockSide.Top;
            }
            if (position.z == 0)
            {
                visibleSides |= BlockSide.Back;
            }
            else if (position.z == dimensionLength - 1)
            {
                visibleSides |= BlockSide.Front;
            }
            return visibleSides;
        }
        public static Vector3 ToVector3(this CubeDirectionAxis self)
        {
            switch (self)
            {
                case CubeDirectionAxis.X:
                    return Vector3.right;
                case CubeDirectionAxis.Y:
                    return Vector3.up;
                case CubeDirectionAxis.Z:
                    return Vector3.forward;
                default:
                    return Vector3.zero;
            }
        }
        public static Vector3 ToVector3(this Sides self)
        {
            switch (self)
            {
                case Sides.Left:
                    return Vector3.left;
                case Sides.Right:
                    return Vector3.right;
                case Sides.Bottom:
                    return Vector3.down;
                case Sides.Top:
                    return Vector3.up;
                case Sides.Back:
                    return Vector3.back;
                case Sides.Front:
                    return Vector3.forward;
                default:
                    return Vector3.zero;
            }
        }
        public static Block[] Sort(this Block[] blocks, CubeDirectionAxis axis, bool isReverse)
        {
            switch (axis)
            {
                case CubeDirectionAxis.X:
                    return blocks.SortByX(isReverse);
                case CubeDirectionAxis.Y:
                    return blocks.SortByY(isReverse);
                case CubeDirectionAxis.Z:
                    return blocks.SortByZ(isReverse);
                default:
                    return blocks;
            }
        }
        public static Block[] SortByX(this Block[] blocks, bool isReverse = false)
        {
            System.Array.Sort(blocks, (a, b) => a.RealCurrentPosition.x.CompareTo(b.RealCurrentPosition.x));
            if (isReverse)
            {
                System.Array.Reverse(blocks);
            }
            return blocks;
        }
        public static Block[] SortByY(this Block[] blocks, bool isReverse = false)
        {
            System.Array.Sort(blocks, (a, b) => a.RealCurrentPosition.y.CompareTo(b.RealCurrentPosition.y));
            if (isReverse)
            {
                System.Array.Reverse(blocks);
            }
            return blocks;
        }
        public static Block[] SortByZ(this Block[] blocks, bool isReverse = false)
        {
            System.Array.Sort(blocks, (a, b) => a.RealCurrentPosition.z.CompareTo(b.RealCurrentPosition.z));
            if (isReverse)
            {
                System.Array.Reverse(blocks);
            }
            return blocks;
        }
        public static List<Material> ToList(this CubeMaterialPack self)
        {
            List<Material> materials = new List<Material>();
            if (self != null)
            {
                //Blue Orange White Red Green BASE Yellow
                materials.Add(self.BlockMaterialBack); // Blue
                materials.Add(self.BlockMaterialRight); // Orange
                materials.Add(self.BlockMaterialTop); // White
                materials.Add(self.BlockMaterialLeft); // Red
                materials.Add(self.BlockMaterialFront); // Green
                materials.Add(self.BlockMaterialBase); // Base
                materials.Add(self.BlockMaterialBottom); // Yellow
            }
            return materials;
        }
        public static Material[] ToArray(this CubeMaterialPack self)
        {
            Material[] materials = new Material[7];
            if (self != null)
            {
                //Blue Orange White Red Green BASE Yellow
                materials[0] = self.BlockMaterialBack; // Blue
                materials[1] = self.BlockMaterialRight; // Orange
                materials[2] = self.BlockMaterialTop; // White
                materials[3] = self.BlockMaterialLeft; // Red
                materials[4] = self.BlockMaterialFront; // Green
                materials[5] = self.BlockMaterialBase; // Base
                materials[6] = self.BlockMaterialBottom; // Yellow
            }
            return materials;
        }

        public static CubeDirectionAxis ToAxis(this Sides side, Sides to, out bool clockWise)
        {
            clockWise = false;
            switch (side)
            {
                case Sides.Left:
                    switch (to)
                    {
                        case Sides.Top:
                            clockWise = false;
                            return CubeDirectionAxis.Z;
                        case Sides.Bottom:
                            clockWise = true;
                            return CubeDirectionAxis.Z;
                        case Sides.Back:
                            clockWise = false;
                            return CubeDirectionAxis.Y;
                        case Sides.Front:
                            clockWise = true;
                            return CubeDirectionAxis.Y;
                        default:
                            return CubeDirectionAxis.None;
                    }
                case Sides.Right:
                    switch (to)
                    {
                        case Sides.Top:
                            clockWise = true;
                            return CubeDirectionAxis.Z;
                        case Sides.Bottom:
                            clockWise = false;
                            return CubeDirectionAxis.Z;
                        case Sides.Back:
                            clockWise = true;
                            return CubeDirectionAxis.Y;
                        case Sides.Front:
                            clockWise = false;
                            return CubeDirectionAxis.Y;
                        default:
                            return CubeDirectionAxis.None;
                    }
                case Sides.Bottom:
                    switch (to)
                    {
                        case Sides.Left:
                            clockWise = false;
                            return CubeDirectionAxis.Z;
                        case Sides.Right:
                            clockWise = true;
                            return CubeDirectionAxis.Z;
                        case Sides.Back:
                            clockWise = true;
                            return CubeDirectionAxis.X;
                        case Sides.Front:
                            clockWise = false;
                            return CubeDirectionAxis.X;
                        default:
                            return CubeDirectionAxis.None;
                    }
                case Sides.Top:
                    switch (to)
                    {
                        case Sides.Left:
                            clockWise = true;
                            return CubeDirectionAxis.Z;
                        case Sides.Right:
                            clockWise = false;
                            return CubeDirectionAxis.Z;
                        case Sides.Back:
                            clockWise = false;
                            return CubeDirectionAxis.X;
                        case Sides.Front:
                            clockWise = true;
                            return CubeDirectionAxis.X;
                        default:
                            return CubeDirectionAxis.None;
                    }
                case Sides.Back:
                    switch (to)
                    {
                        case Sides.Left:
                            clockWise = true;
                            return CubeDirectionAxis.Y;
                        case Sides.Right:
                            clockWise = false;
                            return CubeDirectionAxis.Y;
                        case Sides.Top:
                            clockWise = true;
                            return CubeDirectionAxis.X;
                        case Sides.Bottom:
                            clockWise = false;
                            return CubeDirectionAxis.X;
                        default:
                            return CubeDirectionAxis.None;
                    }
                case Sides.Front:
                    switch (to)
                    {
                        case Sides.Left:
                            clockWise = false;
                            return CubeDirectionAxis.Y;
                        case Sides.Right:
                            clockWise = true;
                            return CubeDirectionAxis.Y;
                        case Sides.Top:
                            clockWise = false;
                            return CubeDirectionAxis.X;
                        case Sides.Bottom:
                            clockWise = true;
                            return CubeDirectionAxis.X;
                        default:
                            return CubeDirectionAxis.None;
                    }
                default:
                case Sides.None:
                    return CubeDirectionAxis.None;
            }
        }
        public static Sides ToSide(this CubeDirectionAxis axis)
        {
            switch (axis)
            {
                case CubeDirectionAxis.X:
                    return Sides.Left;
                case CubeDirectionAxis.Y:
                    return Sides.Top;
                case CubeDirectionAxis.Z:
                    return Sides.Back;
                default:
                    return Sides.None;
            }
        }
        public static Sides ToSide(this CubeDirectionAxis axis, bool isClockwise)
        {
            switch (axis)
            {
                case CubeDirectionAxis.X:
                    return isClockwise ? Sides.Left : Sides.Right;
                case CubeDirectionAxis.Y:
                    return isClockwise ? Sides.Top : Sides.Bottom;
                case CubeDirectionAxis.Z:
                    return isClockwise ? Sides.Back : Sides.Front;
                default:
                    return Sides.None;
            }
        }
        public static Sides ToSide(this CubeDirectionAxis axis, int index)
        {
            switch (axis)
            {
                case CubeDirectionAxis.X:
                    return index == 0 ? Sides.Left : Sides.Right;
                case CubeDirectionAxis.Y:
                    return index == 0 ? Sides.Top : Sides.Bottom;
                case CubeDirectionAxis.Z:
                    return index == 0 ? Sides.Back : Sides.Front;
                default:
                    return Sides.None;
            }
        }
    }
    [System.Serializable]
    public struct CubeMove
    {
        private int m_LayerIndex;
        private CubeDirectionAxis m_Direction;
        private bool m_Clockwise;

        public int LayerIndex { get => m_LayerIndex; }
        public CubeDirectionAxis Direction { get => m_Direction; }
        public bool Clockwise { get => m_Clockwise; }

        public CubeMove(int layerIndex, CubeDirectionAxis direction, bool clockwise)
        {
            m_LayerIndex = layerIndex;
            m_Direction = direction;
            m_Clockwise = clockwise;
        }
        public CubeMove ReverseMovement()
        {
            return new CubeMove(LayerIndex, Direction, !Clockwise);
        }
        public static CubeMove Random
        {
            get
            {
                return new CubeMove(UnityEngine.Random.Range(0, 3), (CubeDirectionAxis)UnityEngine.Random.Range(1, 4), UnityEngine.Random.Range(0, 2) == 0);
            }
        }
        public static CubeMove None
        {
            get
            {
                return new CubeMove(-1, CubeDirectionAxis.None, false);
            }
        }
    }
    [System.Serializable]
    public enum CubeDirectionAxis
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 3,
    }
}
