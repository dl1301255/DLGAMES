using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    public class ColEventSender : MonoBehaviour
    {
        public string EventStartTag;
        public string TargetTag;

        private void OnTriggerEnter(Collider other)
        {
            if (string.IsNullOrEmpty(TargetTag) ||
                other.gameObject.tag != TargetTag ||
                string.IsNullOrEmpty(EventStartTag)) return;

            EventManager.Instance.SendEvent(EventStartTag,other.gameObject);
            print("event sender == " + EventStartTag);
        }
    }
}
