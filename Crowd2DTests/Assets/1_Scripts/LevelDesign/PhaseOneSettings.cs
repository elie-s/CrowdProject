using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    [CreateAssetMenu(menuName = "Crowd/LD/Phase1")]
    public class PhaseOneSettings : LDPhaseSettings
    {
        public GameObject collectablePrefab;
        public int sequence;

        private Vector2Int lastPos;

        public override void Init()
        {
            base.Init();
            sequence = 0;
            lastPos = Vector2Int.zero;
        }

        public override void Set()
        {
            if (isFinished) return;

            switch (sequence)
            {
                case 0:
                    SequenceOne();
                    sequence++;
                    break;

                case 1:
                    SequenceTwo();
                    sequence++;
                    break;

                case 2:
                    SequenceThree();
                    isFinished = true;
                    break;
            }
        }

        private void SequenceOne()
        {
            int situation = Random.Range(0, 4);

            switch (situation)
            {
                case 0:
                    lastPos = new Vector2Int(0, 1);
                    break;

                case 1:
                    lastPos = new Vector2Int(0, -1);
                    break;

                case 2:
                    lastPos = new Vector2Int(1, 0);
                    break;

                case 3:
                    lastPos = new Vector2Int(-1, 0);
                    break;
            }

            Instantiate(collectablePrefab, grid.RandomPosInCell(lastPos + Vector2Int.one, 0.1f), Quaternion.identity).name = "Collectable 1-1";
        }

        private void SequenceTwo()
        {
            lastPos = new Vector2Int(lastPos.x == 0 ? (Random.value > 0.5f ? 1 : -1) : 0, lastPos.y == 0 ? (Random.value > 0.5f ? 1 : -1) : 0);
            Instantiate(collectablePrefab, grid.RandomPosInCell(lastPos + Vector2Int.one, 0.1f), Quaternion.identity).name = "Collectable 1-2";
        }

        private void SequenceThree()
        {
            Vector2Int pos1 = new Vector2Int(lastPos.x != 0 ? lastPos.x * -1 : 1, lastPos.y != 0 ? lastPos.y * -1 : 1) + Vector2Int.one;
            Vector2Int pos2 = new Vector2Int(lastPos.x != 0 ? lastPos.x * -1 : -1, lastPos.y != 0 ? lastPos.y * -1 : -1) + Vector2Int.one;

            Instantiate(collectablePrefab, grid.RandomPosInCell(pos1, 0.1f), Quaternion.identity).name = "Collectable 1-3-1";
            Instantiate(collectablePrefab, grid.RandomPosInCell(pos2, 0.1f), Quaternion.identity).name = "Collectable 1-3-2";
        }
    }
}