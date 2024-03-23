using UnityEngine;

namespace RubicksCubeCreator
{
    [CreateAssetMenu(fileName = "CubeMaterialPack", menuName = "Rubicks Cube Creator/CubeMaterialPack", order = 1)]
    public class CubeMaterialPack : ScriptableObject
    {
        public Material BlockMaterialBack; // 0
        public Material BlockMaterialBase; // 1
        public Material BlockMaterialFront; //2
        public Material BlockMaterialRight; // 3
        public Material BlockMaterialLeft; // 4
        public Material BlockMaterialTop; // 5
        public Material BlockMaterialBottom; // 6
        public bool HasAllMaterials
        {
            get
            {
                return BlockMaterialBase != null && BlockMaterialTop != null && BlockMaterialBack != null && BlockMaterialRight != null && BlockMaterialFront != null && BlockMaterialLeft != null && BlockMaterialBottom != null;
            }
        }
    }
}
