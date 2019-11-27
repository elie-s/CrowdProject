using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public abstract class LDPhaseSettings : ScriptableObject
    {
        public VirtualGrid grid;
        public bool isFinished;
        public System.Action finishedCallback;
        private List<CollectableBehaviour> collectables = new List<CollectableBehaviour>();
        public List<CollectableBehaviour> Collectables { get { return collectables; } private set { collectables = value; } }

        public abstract void Set();
        public virtual void Init(System.Action _callback)
        {
            grid.Update();
            isFinished = false;
            finishedCallback = _callback;
            collectables = new List<CollectableBehaviour>();
        }

        public void AddCollectables(CollectableBehaviour _collectable)
        {
            collectables.Add(_collectable);
        }

        public void RemoveCollectable(CollectableBehaviour _collectable)
        {
            collectables.Remove(_collectable);
            OnCollectableRemoved();
        }

        private void OnCollectableRemoved()
        {
            if (collectables.Count == 0)
            {
                if(isFinished) finishedCallback();
                else Set();
            }
        }
    }
}