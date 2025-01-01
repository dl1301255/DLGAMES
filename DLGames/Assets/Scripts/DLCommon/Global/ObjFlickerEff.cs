using DL.Common;
using DL.MCD;
using DL.MCD.Data;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.DLGame
{
    public class ObjFlickerEff : MonoBehaviour,IExecutor
    {
        public ObjFlickerEff()
        {
            FlickerColor = Color.red;
            delay = 20;
            repeatNum = 15;
        }

        public Color OGColor;
        public Color FlickerColor;
        public int delay;
        public int repeatNum;
        private bool onFlickerColor;
        //…¡À∏÷¥––÷–
        public bool OnFlicker;
        Renderer render;
        public TimerNode tn;

        [Button]
        public void Flicker()
        {
            if (render == null)
            {
                render = gameObject.GetComponent<Renderer>();
            }
            
            if (render == null) return;

            if (OnFlicker)
            {
                ResetFlicker();
            }
            else
            {
                OGColor = render.materials[0].color;
                onFlickerColor = false;
                ChangeColor();
                tn = TimerManager.Instance.doLoop_repeat(this, delay, ChangeColor, repeatNum, TimeUpdataType.Update);
                OnFlicker = true;
            }        
        }

        private void ResetFlicker()
        {
            render.materials[0].color = OGColor;
            onFlickerColor = false;
            tn.onRepeatNum = 0;
            ChangeColor();
            OnFlicker = true;
        }

        private void ChangeColor()
        {
            if (onFlickerColor)
            {
                render.materials[0].color = OGColor;
                onFlickerColor = false;
                if (tn.onRepeatNum == repeatNum) OnFlicker = false;
            }
            else
            {
                render.materials[0].color = FlickerColor;
                onFlickerColor = true;
            }

        }

        public void Init()
        {
            throw new System.NotImplementedException();
        }

        public void Execute(object obj = null)
        {
           // Flicker();
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public bool OnUse()
        {
            throw new System.NotImplementedException();
        }

        public void Execute(params object[] obj)
        {
            return;
        }

        public void Close(object obj = null)
        {
            throw new System.NotImplementedException();
        }

        public void Close(params object[] obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
