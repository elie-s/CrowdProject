using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public abstract class LDPhaseSettings : ScriptableObject
    {
        public VirtualGrid grid;
        public bool isFinished;

        public abstract void Set();
        public virtual void Init()
        {
            grid.Update();
            isFinished = false;
        }
    }
}