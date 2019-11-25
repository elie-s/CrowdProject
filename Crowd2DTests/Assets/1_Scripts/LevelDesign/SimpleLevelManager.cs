using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class SimpleLevelManager : MonoBehaviour
    {
        [SerializeField] private GameObject collectablePrefab = default;
        [SerializeField] private int collectableAmount = 5;
        [SerializeField] private VirtualGrid grid;
        // Start is called before the first frame update
        void Start()
        {
            grid.Update();
            SpawnCollectables();
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void SpawnCollectables()
        {
            List<Vector2Int> pickedCells = new List<Vector2Int>();

            if(collectableAmount > grid.cellCount)
            {
                Debug.LogWarning("collectableAmount > cellcount ! collectableAmount must be clamped between 0 and grid.cellcount");
                collectableAmount = grid.cellCount;
            }

            for (int i = 0; i < collectableAmount; i++)
            {
                Vector2Int cell;

                do
                {
                    cell = new Vector2Int(Random.Range(0, grid.size.x), Random.Range(0, grid.size.y));
                } while (pickedCells.Contains(cell));

                pickedCells.Add(cell);

                Vector2 position = grid.RandomPosInCell(cell);

                //Instantiate(collectablePrefab, new Vector3(cell.x, cell.y, 0), Quaternion.identity, transform).name = "Collectable "+i;
                Instantiate(collectablePrefab, position, Quaternion.identity, transform).name = "Collectable " + i + " - " + cell + position;
            }
        }
    }
}