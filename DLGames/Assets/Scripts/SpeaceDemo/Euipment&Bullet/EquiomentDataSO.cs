using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.DLGame
{
    public class EquiomentDataSO : ScriptableObject
    {
        //¼¼ÄÜÊÍ·ÅÆ÷ bullet
        public GameObject EuipmentPrefab;
        public GameObject BulletObj;

        public AudioClip ac;

        public GameObject Muzzle;
        public Transform firePointTF;
        public Vector3 firePoint;
    }
}
