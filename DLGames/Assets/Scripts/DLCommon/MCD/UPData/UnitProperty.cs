using DL.Common;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD.Data
{
    /// <summary>
    /// 继承时 namespace ： DL.MCD.Data
    /// default tag :
    ///     1:UP ID
    ///     2:UPC ID
    /// 
    /// </summary>


    [System.Serializable]
    public class UnitProperty /*: ISaveable*/
    {
        /// <summary>
        /// 用于读取value作用
        /// </summary>
        public string UPName;
        /// <summary>
        /// 用于创建UPC 包含类型
        /// </summary>
        public string UPTypeName;

        /// <summary>
        /// default tag :
        ///     1:This Type
        ///     2:UPC ID
        /// </summary>
        public List<string> tags/* = new List<string>()*/;


        [Multiline(3)]
        public string stringVal;


        public UnitProperty()
        {
            if(!string.IsNullOrEmpty(UPName)) UPName = string.Intern(UPName);
            if (!string.IsNullOrEmpty(UPTypeName)) UPTypeName = string.Intern(UPTypeName);
            if (!string.IsNullOrEmpty(StringVal))  StringVal = string.Intern(StringVal);

            if (tags == null || tags.Count <= 0) return;
            for (int i = 0; i < tags.Count; i++)
            {
                tags[i] = string.Intern(tags[i]);
            }

        }

        public virtual string StringVal
        {
            get => stringVal;
            set => stringVal = value;
        }

        /// <summary>
        /// Get Sub Type
        /// </summary>
        /// <returns></returns>
        public virtual T GetSubType<T>() where T : class
        {
            return this as T;
        }

        /// <summary>
        /// 通过Sting 读取 Val 
        /// </summary>
        public virtual string ReadValueByString()
        {
            if (string.IsNullOrEmpty(StringVal)) return null;
            return StringVal;
        }

        /// <summary>
        /// 通过 String 写入 Val
        /// </summary>
        public virtual void WriteValueByString(string val = null)
        {
            if (!string.IsNullOrEmpty(val)) StringVal = val;
            if (string.IsNullOrEmpty(StringVal)) return;
        }

        public virtual void Init(MCDMgr mgr = null)
        {
            WriteValueByString();
        }
        public virtual void ClearValue()
        {
            stringVal = null;
        }
        public virtual T GetValue<T>() where T : class
        {
            if (string.IsNullOrEmpty(StringVal)) return default;

            var v = GameCommonFactory.Instance.CreateObject<ParamsValue>("DL.Common.ParamsValue");
            if (v == null) return default;
            v.StringValue = GameTool.GetStr(StringVal);
            return v as T;
        }



#if UNITY_EDITOR_WIN || UNITY_STANDALONE
        [Button]
        public virtual void  UpdateValue() 
        {
            WriteValueByString(stringVal);
        }
#endif

    }

    public interface IValue 
    {
    }
    public interface IFloatValue:IValue 
    {
        public float GetValue(string valueName = null);
    }

    public interface IIntValue : IValue
    {
        public int GetValue(string valueName = null);
    }
}
