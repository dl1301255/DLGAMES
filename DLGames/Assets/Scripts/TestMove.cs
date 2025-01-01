using DL.Common;
using DL.InputSys;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL
{
    public class TestMove : MonoBehaviour
    {
        public float RotSpeed;
        public float lockRotZ;
        public float TFlockRotZ;
        public Quaternion rot;
        public float crRot;

        public float speed;
        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                var t = transform.rotation;
                if (transform.rotation.eulerAngles.z >= lockRotZ && transform.rotation.eulerAngles.z <= 180) return;

                crRot += RotSpeed * Time.deltaTime;
                transform.rotation = Quaternion.AngleAxis(crRot + transform.rotation.eulerAngles.z, Vector3.forward);
                crRot = 0;

                rot = transform.rotation;
                TFlockRotZ = t.z;
            }
            if (Input.GetKey(KeyCode.D))
            {
                var t = transform.rotation;
                if (transform.rotation.eulerAngles.z <= 360 - lockRotZ && transform.rotation.eulerAngles.z >= 180) return;

                crRot += RotSpeed * Time.deltaTime;
                transform.rotation = Quaternion.AngleAxis(-crRot + transform.rotation.eulerAngles.z, Vector3.forward);
                crRot = 0;
                rot = transform.rotation;
                TFlockRotZ = t.z;
            }

        }
        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, transform.position.z * speed * Time.fixedDeltaTime),Space.World);
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * speed , Space.World);
                transform.Translate(Vector3.forward * -speed, Space.World);
                EventManager.Instance.SendEvent("test on move" ,gameObject);
            }
        }

        private void Start()
        {
            EventManager.Instance.ListenEvent<GameObject>("test on move", notmove);
        }
        private void OnCollisionEnter(Collision collision)
        {
            //print(collision.gameObject.name);
            //EventManager.Instance.ListenEvent<GameObject>("test on move", notmove);
        }
        private void OnCollisionExit(Collision collision)
        {
            //print(collision.gameObject.name);
            //EventManager.Instance.RemoveEvent<GameObject>("test on move", notmove);
        }

        private void notmove(GameObject t1)
        {
            //transform.Translate(Vector3.forward * -speed, Space.World);
        }

        [Button]
        public void ros()
        {
            print(transform.rotation);
            print(transform.rotation.z);
            print(transform.rotation.eulerAngles);
            print(transform.rotation.normalized);
        }
    }
}
