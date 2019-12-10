using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject.Analytics
{
    public class Recording
    {
        public List<Record> records;

        private int index;

        public Recording()
        {
            records = new List<Record>();
            index = 0;
        }

        public void Reset()
        {
            index = 0;
        }

        public void Update(Vector2 _position, Color[] _pixels)
        {
            int[] pixelsArray = new int[_pixels.Length];

            for (int i = 0; i < pixelsArray.Length; i++)
            {
                pixelsArray[i] = _pixels[i].a > 0.5f ? 1 : 0;
            }

            records.Add(new Record(Time.time, _position, pixelsArray));
        }

        public Record Next()
        {
            return records[index];
        }
    }

    public struct Record
    {
        public float time;
        public Vector2 position;
        public int[] pixelsArray;

        public Record(float time, Vector2 position, int[] pixelsArray)
        {
            this.time = time;
            this.position = position;
            this.pixelsArray = pixelsArray;
        }

        public Color[] ToColorArray()
        {
            Color[] result = new Color[pixelsArray.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = pixelsArray[i] == 1 ? new Color(1.0f, 1.0f, 1.0f, 1.0f) : new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }

            return result;
        }
    }
}