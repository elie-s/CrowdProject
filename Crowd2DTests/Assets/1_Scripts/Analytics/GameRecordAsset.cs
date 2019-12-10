using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject.Analytics
{
    [CreateAssetMenu(menuName ="Crowd/Analytics/Asset")]
    public class GameRecordAsset : ScriptableObject
    {
        public Recording recording = new Recording();
       
    }
}