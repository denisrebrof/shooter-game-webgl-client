using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Social
{
    public abstract class ShareRecordListener : MonoBehaviour
    {
        [SerializeField] private float delay = 1;
        [SerializeField] private UnityEvent onRecordReached;

        private const string PREFS_KEY = "ShareRecord";

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(delay);
            onRecordReached.Invoke();
            PlayerPrefs.SetInt(PREFS_KEY, 1);
        }

        public void CheckAndShow()
        {
            if (PlayerPrefs.HasKey(PREFS_KEY))
                return;
            
            if (IsRecordReached() && onRecordReached != null)
            {
                StartCoroutine(Delay());
            }
        }

        protected abstract bool IsRecordReached();
    }
}