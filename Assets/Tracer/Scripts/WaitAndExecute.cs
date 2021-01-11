using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace game.lib
{
    public class WaitAndExecute : MonoBehaviour
    {
        [SerializeField]
        private float _time = 1;
        [SerializeField]
        private UnityEvent _action;

        // Start is called before the first frame update
        void Start()
        {

            StartCoroutine(WaitAndAction());
        }

        IEnumerator WaitAndAction()
        {
            yield return new WaitForSeconds(_time);
            _action.Invoke();
        }
    }
}
