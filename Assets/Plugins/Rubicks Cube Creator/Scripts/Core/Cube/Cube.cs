using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using static RubicksCubeCreator.CubeUtility;

namespace RubicksCubeCreator
{
    [AddComponentMenu("RubicksCubeCreator/Cube")]

    public class Cube : MonoBehaviour
    {

        public UnityEvent OnCubeChanged;

        public UnityEvent OnCubeSolved;
        public UnityEvent onShuffleEnd = new UnityEvent();

        [SerializeField] private CubePreset m_Preset;

        private CubeCore m_Core;

        #region Move

        [SerializeField] private float m_RotationTime = 0.2f;

        [SerializeField] private AnimationCurve m_RotationCurve;

        private Coroutine m_RotateCoroutine;

        private Stack<CubeMove> cubeMoves = new Stack<CubeMove>();

        private BlocksRotator blocksRotator;

        private bool m_CanMove = true;

        public bool CanMove
        {
            get => m_CanMove;
        }

        #endregion


        public CubePreset Preset => m_Preset;
        public CubeCore Core => m_Core;
        public bool IsSolved => m_Core.IsSolved();

        public Block GetBlockOnThePoint(Vector3 point) => m_Core.GetBlockOnThePoint(point);
        public Color[] GetColorsOfFace(Sides side) => m_Core.GetColorsOfFace(side);
        public Sides GetClosestSideWithNormal(Vector3 normal) => m_Core.GetClosestSideWithNormal(normal);

        /// <summary>
        /// Creates new cube
        /// </summary>
        public void CreateNewCube()
        {
            if (m_Core != null)
            {
                m_Core.Destroy();
                cubeMoves.Clear();
            }
            m_Core = new CubeCore(m_Preset, transform);
            OnCubeChanged?.Invoke();
        }

        private void Awake() => CreateNewCube();

        private void PlayMove(CubeMove move, bool pushMove = false, UnityAction onEnd = null)
        {
            if (pushMove)
                cubeMoves.Push(move);

            if (m_RotateCoroutine != null)
            {
                StopCoroutine(m_RotateCoroutine);
                if (blocksRotator != null)
                {
                    blocksRotator.CompleteAndKill(blocksRotator.Move.Clockwise ? 90f : -90f);
                    blocksRotator = null;
                }
            }
            m_RotateCoroutine = StartCoroutine(RotateInTime(move, onEnd));
        }

        private IEnumerator RotateInTime(CubeMove move, UnityAction onEnd = null)
        {
            var blocks = m_Core.GetRubicksBlocks(move);
            if (move.Direction == CubeDirectionAxis.None) yield break;
            if (blocks.Length == 0) yield break;

            float targetAngle = move.Clockwise ? 90f : -90f;
            float time = 0f;
            blocksRotator = m_Core.GetRotatorForBlocks(blocks, move);

            if (m_RotationTime <= 0)
            {
                blocksRotator.CompleteAndKill(targetAngle);
                blocksRotator = null;
                onEnd?.Invoke();
                OnCubeChanged?.Invoke();
                yield break;
            }

            while (time < m_RotationTime)
            {
                time += Time.deltaTime;
                float angle = Mathf.Lerp(0f, targetAngle, m_RotationCurve.Evaluate(time / m_RotationTime));
                blocksRotator.Rotate(angle);
                yield return null;
            }
            blocksRotator.Kill();
            blocksRotator = null;
            onEnd?.Invoke();
            OnCubeChanged?.Invoke();
            if (IsSolved)
                OnCubeSolved?.Invoke();
        }

        /// <summary>
        /// Plays the move
        /// </summary>
        /// <param name="move"></param>
        public void PlayMove(CubeMove move)
        {
            if (!CanMove) return;
            PlayMove(move, true);
        }

        public void PlayMove(Sides from, Sides to, Block selectedBlock)
        {
            if (!CanMove) return;
            var axis = from.ToAxis(to, out bool clockWise);
            int layerIndex = selectedBlock.GetLayerIndex(axis);
            PlayMove(new CubeMove(layerIndex, axis, clockWise), true);
        }

        /// <summary>
        /// Plays a random move
        /// </summary>
        public void PlayRandomMove()
        {
            if (!CanMove) return;
            PlayMove(CubeMove.Random, true);
        }

        /// <summary>
        /// Shuffels the cube randomly
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public void Shuffle(bool IsAnimated)
        {
            m_CanMove = false;
            StartCoroutine(ShuffleStep(IsAnimated));
        }
        private IEnumerator ShuffleStep(bool animate)
        {
            for (int i = 0; i < 15; i++)
            {
                PlayMove(CubeMove.Random, true);
                if (animate)
                    yield return new WaitForSeconds(m_RotationTime);
            }
            PlayMove(CubeMove.None, false);
            yield return null;
            m_CanMove = true;
            onShuffleEnd.Invoke();
        }

        /// <summary>
        /// Undoes the last move
        /// </summary>
        public void UndoMove()
        {
            if (cubeMoves.Count == 0) return;
            CubeMove move = cubeMoves.Pop();
            PlayMove(move.ReverseMovement(), false);
        }

        /// <summary>
        /// Solves the cube, if it has previous moves
        /// </summary>
        public void Solve()
        {
            m_CanMove = false;
            StartCoroutine(SolveStep());
        }
        private IEnumerator SolveStep()
        {
            while (!IsSolved)
            {
                UndoMove();
                yield return new WaitForSeconds(m_RotationTime);
            }
            m_CanMove = true;
            cubeMoves.Clear();
        }
        public void SolveOneStep()
        {
            m_CanMove = false;
            StartCoroutine(SolveOneStepCoroutine());
        }
        
        private IEnumerator SolveOneStepCoroutine()
        {
            UndoMove();
            yield return new WaitForSeconds(m_RotationTime);
            
            m_CanMove = true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (m_Preset == null) return;
            Vector3 centerOffset = new Vector3((m_Preset.Dimension - 1) / 2f, (m_Preset.Dimension - 1) / 2f, (m_Preset.Dimension - 1) / 2f);
            Gizmos.color = Color.black;
            Gizmos.matrix = transform.localToWorldMatrix;
            for (int i = 0; i < m_Preset.Dimension; i++)
            {
                for (int j = 0; j < m_Preset.Dimension; j++)
                {
                    for (int k = 0; k < m_Preset.Dimension; k++)
                    {
                        Vector3 settedPosition = new Vector3(i, j, k) - centerOffset;
                        Gizmos.DrawWireCube(settedPosition, Vector3.one * 0.9f);
                    }
                }
            }
        }
#endif
    }
}
