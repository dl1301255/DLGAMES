//using DL.MCD.Data;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace DL.Common
{
    public class JsonManager : Singleton<JsonManager>
    {
        public T JsonToObj<T>(string json)
        {
            var upc = JsonConvert.DeserializeObject<T>(json);
            return upc;
        }

        /// <summary>
        /// 数据 转为 Json
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fileName">MySave.json</param>
        /// <param name="path">def：persistentDataPath</param>
        public void ObjToJson(object obj, string fileName = null, string path = null)
        {
            var json = JsonConvert.SerializeObject(obj);
            FileStream fs;
            if (path == null && fileName == null)
            {
                fs = new FileStream(Application.persistentDataPath + "/MySave.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
            else if (path != null && fileName == null)
            {
                fs = new FileStream(path + "/MySave.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
            else if (fileName != null && path == null)
            {
                fs = new FileStream(Application.persistentDataPath + fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
            else
            {
                fs = new FileStream(path + fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }


            byte[] b = Encoding.UTF8.GetBytes(json);
            fs.Write(b, 0, b.Length);
            fs.Flush();
            fs.Close();
            fs.Dispose();
        }
    }
}
