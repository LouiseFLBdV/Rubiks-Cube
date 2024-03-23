using UnityEngine;

namespace RubicksCubeCreator
{

    [CreateAssetMenu(fileName = "CubePreset", menuName = "Rubicks Cube Creator/CubePreset", order = 0)]
    public class CubePreset : ScriptableObject
    {
        [Min(2)]
        public int Dimension;
        public Block BlockPrefab;
        public CubeMaterialPack Materials;
    }
}
