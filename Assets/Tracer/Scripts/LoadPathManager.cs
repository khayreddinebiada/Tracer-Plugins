using UnityEngine;

namespace tracer
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
