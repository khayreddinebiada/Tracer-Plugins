using UnityEngine;

namespace path
{
    public class LoadPathManager : MonoBehaviour
    {
        public TracerInfo tracerInfo;

        public void LoadPath()
        {
            tracerInfo.LoadPath(transform);
        }
    }
}
