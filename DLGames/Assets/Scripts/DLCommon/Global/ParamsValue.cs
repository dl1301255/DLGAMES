using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    ///<summary>
    ///
    ///<summary>
    public class ParamsValue
    {
        public float FloatValue;
        public int IntValue;
        public bool BoolValue;
        public string StringValue;
        public ParamsValue(float f = 0,int n = 0,bool b = false,string s = null) 
        {
            FloatValue = f;
            IntValue = n;
            BoolValue = b;
            StringValue = s;
        }
        public virtual void Clear()
        {
            FloatValue = 0;
            IntValue = 0;
            BoolValue = false;
            StringValue = null;
        }
    }

    public class ParamsValueVector
    {
        public Vector2 vector2;
        public Vector3 vector3;
        public  void clear()
        {
            vector2 = Vector2.zero;
            vector3 = Vector3.zero;
        }
    }
}

