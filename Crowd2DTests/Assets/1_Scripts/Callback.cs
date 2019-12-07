using System.Collections.Generic;

namespace CrowdProject
{
    public class Callback
    {
        private List<System.Action> callbacks;
        private bool enabled;
        private bool locked;

        public Callback()
        {
            callbacks = new List<System.Action>();
            enabled = true;
            locked = false;
        }

        public void Call()
        {
            if (!enabled) return;

            for (int i = 0; i < callbacks.Count; i++)
            {
                callbacks[i]();
            }
        }

        public void SetActive(bool _enable)
        {
            enabled = _enable;
        }

        public void SetLock(bool _enable)
        {
            locked = _enable;
        }

        public void Register(System.Action _callback)
        {
            if (locked) return;

            callbacks.Add(_callback);
        }

        public void Unregister(System.Action _callback)
        {
            if (locked) return;

            callbacks.Remove(_callback);
        }
    }
}