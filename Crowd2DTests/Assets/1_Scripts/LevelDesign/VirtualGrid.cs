using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    [System.Serializable]
    public class VirtualGrid
    {
        public Rect dimensions;
        public Vector2Int size;

        public int cellCount { get; private set; }
        public float cellHeight { get; private set; }
        public float cellWidth { get; private set; }
        public Vector2 cellSize => new Vector2(cellWidth, cellHeight);

        public VirtualGrid(Rect dimensions, Vector2Int gridSize)
        {
            this.dimensions = dimensions;
            this.size = gridSize;
            cellCount = size.x * size.y;
            cellHeight = dimensions.height / (float)size.y;
            cellWidth = dimensions.width / (float)size.x;
        }

        public void Update()
        {
            cellCount = size.x * size.y;
            cellHeight = dimensions.height / (float)size.y;
            cellWidth = dimensions.width / (float)size.x;
        }

        public void Update(Vector2 _position)
        {
            dimensions.x += _position.x;
            dimensions.y += _position.y;

            cellCount = size.x * size.y;
            cellHeight = dimensions.height / (float)size.y;
            cellWidth = dimensions.width / (float)size.x;
        }

        public Vector2 RandomPosInCell(Vector2Int _cell)
        {
            Vector2 cellCenter = new Vector2(_cell.x * cellWidth + cellWidth * 0.5f + dimensions.x, _cell.y * cellHeight + cellHeight * 0.5f + dimensions.y);

            return new Vector2(cellCenter.x + (Random.value - 0.5f) * cellWidth, cellCenter.y + (Random.value - 0.5f) * cellHeight);
        }

        public Vector2 RandomPosInCell(Vector2Int _cell, float _margin)
        {
            Vector2 cellCenter = new Vector2(_cell.x * cellWidth + cellWidth * 0.5f + dimensions.x, _cell.y * cellHeight + cellHeight * 0.5f + dimensions.y);

            return new Vector2(cellCenter.x + (Random.value - 0.5f) * cellWidth * _margin, cellCenter.y + (Random.value - 0.5f) * cellHeight * _margin);
        }

        public Vector2Int WorldToCell(Vector2 _worldPos)
        {
            if (_worldPos.x < dimensions.x || _worldPos.x > dimensions.x + dimensions.width || _worldPos.y < dimensions.y || _worldPos.y > dimensions.y + dimensions.height) return new Vector2Int(-1, -1);

            return new Vector2Int(Mathf.FloorToInt((_worldPos.x-dimensions.x)/cellWidth), Mathf.FloorToInt((_worldPos.y - dimensions.y) / cellHeight));
        }

        public override string ToString()
        {
            return dimensions + "\n" + size + "\n" + cellCount + " - " + cellWidth + " - " + cellHeight;
        }

    }
}