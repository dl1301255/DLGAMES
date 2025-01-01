using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL
{
    public class rotTest : MonoBehaviour
    {
        Quaternion q;
        private void Start()
        {
            q = Quaternion.Euler(0, 0, 0);
        }
        private void Update()
        {
            transform.rotation = q;
        }
    }
}
