using DL.Common;
using DL.InputSys;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DL.InputSys
{
    public class CommonInputMgr : InputMgr
    {
        private DLCommonIAA IAA;

        protected override void Disable()
        {
            IAA.Player.Move.started -= inputMove;
            IAA.Player.Move.canceled -= inputMove;

            IAA.Player.Look.started -= InputLook;
            IAA.Player.Look.canceled -= InputLook;

            IAA.Player.Jump.started -= Jump_started;
            IAA.Player.Jump.performed -= Jump_performed;
            IAA.Player.Jump.canceled -= Jump_canceled;
        }

        protected override void Enabled()
        {
            //IAA.Player.Move.started += inputMove;
            IAA.Player.Move.performed += inputMove;
            IAA.Player.Move.canceled += inputMove;

            IAA.Player.Look.started += InputLook;
            IAA.Player.Look.performed += InputLook;
            IAA.Player.Look.canceled += InputLook;


            IAA.Player.Jump.started += Jump_started;
            IAA.Player.Jump.performed += Jump_performed;
            IAA.Player.Jump.canceled += Jump_canceled; ;

            IAA.Player.J.started += J_started;
            IAA.Player.J.started += J_performed;
        }

        private void J_performed(InputAction.CallbackContext obj)
        {
            //EventManager.Instance.SendEvent<int>("PlayerInput_J_Start", 1);
        }

        private void J_started(InputAction.CallbackContext obj)
        {
            EventManager.Instance.SendEvent<int>("PlayerInput_J_Start",0);
            //0°´ÏÂ£¬1Ì§Æð£¬2Ë«»÷
        }

        protected override void Init()
        {

        }

        private void Awake()
        {
            IAA = new DLCommonIAA();
            Asset = IAA.asset;
        }

        public void OnValidate()
        {
        }
        private void InputLook(InputAction.CallbackContext obj)
        {
            EventManager.Instance.SendEvent(InputEventTag._Instance.Look, gameObject, obj.ReadValue<Vector2>());
        }

        private void inputMove(InputAction.CallbackContext obj)
        {
            EventManager.Instance.SendEvent(InputEventTag._Instance.Move, gameObject, obj.ReadValue<Vector2>());
        }

        private void Jump_canceled(InputAction.CallbackContext obj)
        {
            print("Jump_canceled");
        }

        private void Jump_performed(InputAction.CallbackContext obj)
        {
            print("Jump_performed");
        }

        private void Jump_started(InputAction.CallbackContext obj)
        {
            print("Jump_started");
        }

    }
}
