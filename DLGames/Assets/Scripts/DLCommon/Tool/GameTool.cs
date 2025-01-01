
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace DL.Common
{
    ///<summary>
    ///包含所有常用 静态方法 包括复制对象 复制类数据 通过反射创建对象
    ///<summary>
    public static class GameTool
    {

        #region Copy Object
        /// <summary>
        /// 深复制：包含属性 方法 字段 复制
        /// 需要先获取目标obj 后 进行复制
        /// </summary>
        /// <param name="obj">需要被复制的obj</param>
        /// <returns></returns>
        public static object CopyObjDeep(object obj)
        {
            //Copy目标 OBJ
            object targetDeepCopyObj;

            if (obj == null) Debug.Log("copy obj is null");
            //获得 复制源 的字段与属性
            var targetType = obj.GetType();


            //如果 目标类型 是 数值类型 则直接赋值
            if (targetType.IsValueType) { targetDeepCopyObj = obj; }

            else
            {
                targetDeepCopyObj = Activator.CreateInstance(targetType);//实例化目标
                Debug.Log("target:" + targetDeepCopyObj.GetType().Name);
                MemberInfo[] memberCollection = obj.GetType().GetMembers();//获取目标所有的成员 并且储存 MemberInfo 反射中的特殊类型 

                foreach (var member in memberCollection)
                {
                    if (member.MemberType == MemberTypes.Field)//如果当前成员类型是 字段
                    {
                        FieldInfo field = (FieldInfo)member;//将当前member 转换为 fieldinfo 类型储存 到 filedinfo类型 field
                        object fieldValue = field.GetValue(obj);//将当前目标中的 当前field字段 中的obj数据 储存到 fieldvalue中
                        if (fieldValue is ICloneable)
                        {
                            field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            field.SetValue(targetDeepCopyObj, CopyObjDeep(fieldValue));
                        }

                    }
                    if (member.MemberType == MemberTypes.Property)
                    {
                        PropertyInfo myProperty = member as PropertyInfo;
                        MethodInfo methodInfo = myProperty.GetSetMethod(false);
                        if (methodInfo != null)
                        {
                            object propertyValue = myProperty.GetValue(obj, null);

                            if (propertyValue is ICloneable)
                            {
                                myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone());
                            }
                            else
                            {
                                myProperty.SetValue(targetDeepCopyObj, CopyObjDeep(propertyValue));
                            }
                        }
                    }
                }
            }
            return targetDeepCopyObj;
        }

        /// <summary>
        /// 为目标obj 复制进 source属性
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="target">目标</param>
        public static void CopyObjDeep(object source, object target)
        {
            var targetType = target.GetType();

            if (targetType == null)
            {
                targetType = source.GetType();
            }
            //数值类型
            if (targetType.IsValueType == true)
            {
                target = source;
            }
            //引用类型
            else
            {
                if (source == null) return;

                MemberInfo[] memberCollection = source.GetType().GetMembers();

                foreach (var member in memberCollection)
                {
                    if (member.MemberType == MemberTypes.Field)//如果方法类型是 字节
                    {
                        FieldInfo field = member as FieldInfo;//将当前member 转为 fieldInfo类型
                        object fieldValue = field.GetValue(source);
                        if (field is ICloneable)
                        {
                            field.SetValue(target, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            field.SetValue(target, CopyObjDeep(fieldValue));
                        }
                    }
                    else if (member.MemberType == MemberTypes.Property)//如果方法类型是 属性
                    {
                        PropertyInfo myProperty = member as PropertyInfo;
                        MethodInfo info = myProperty.GetSetMethod(false);
                        if (info != null)
                        {
                            object propertyValue = myProperty.GetValue(source, null);
                            if (propertyValue is ICloneable)
                            {
                                myProperty.SetValue(target, (propertyValue as ICloneable).Clone());
                            }
                            else
                            {
                                myProperty.SetValue(target, CopyObjDeep(propertyValue));
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取A类a，B类b ，将a中与b,相同的字段、属性 赋值到a
        /// </summary>
        /// <typeparam name="A">目标类</typeparam>
        /// <typeparam name="B">来源类</typeparam>
        /// <param name="target">目标obj</param>
        /// <param name="source">来源obj</param>
        public static void CopyObjInfo<A, B>(A target, B source)
        {
            Type Typeb = source.GetType();//获得类型  
            Type Typea = target.GetType();

            foreach (FieldInfo sp in Typeb.GetFields())
            {
                foreach (FieldInfo ap in Typea.GetFields())
                {
                    if (ap.Name == sp.Name)//判断属性名是否相同  
                    {
                        ap.SetValue(target, sp.GetValue(source));//获得b对象属性的值复制给a对象的属性  
                    }
                }
            }

            foreach (PropertyInfo sp in Typeb.GetProperties())
            {
                foreach (PropertyInfo ap in Typea.GetProperties())
                {
                    if (ap.Name == sp.Name)//判断属性名是否相同  
                    {
                        ap.SetValue(target, sp.GetValue(source));//获得b对象属性的值复制给a对象的属性  
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="noContain">不希望包含的字段</param>
        public static void MapperObjAddition<A, B>(A target, B source, string[] noContain = null)
        {
            Type Typeb = source.GetType();//获得类型  
            Type Typea = target.GetType();

            foreach (FieldInfo sp in Typeb.GetFields())
            {
                foreach (FieldInfo ap in Typea.GetFields())
                {
                    if (ap.Name == sp.Name)//判断属性名是否相同  
                    {
                        if (noContain != null)
                        {
                            if (noContain.Find(s => s == ap.Name) != null) continue;
                        }

                        if (ap.FieldType == typeof(int))
                        {
                            int? n = (ap.GetValue(target) as int?) + (sp.GetValue(source) as int?);
                            ap.SetValue(target, n);
                        }
                        else if (ap.FieldType == typeof(float))
                        {
                            float? n = (ap.GetValue(target) as float?) + (sp.GetValue(source) as float?);
                            ap.SetValue(target, n);
                        }
                    }
                }
            }

            foreach (PropertyInfo sp in Typeb.GetProperties())
            {
                foreach (PropertyInfo ap in Typea.GetProperties())
                {
                    if (ap.Name == sp.Name)//判断属性名是否相同  
                    {
                        if (noContain != null)
                        {
                            if (noContain.Find(s => s == ap.Name) != null) continue;
                        }

                        if (ap.PropertyType == typeof(int))
                        {
                            int? n = (ap.GetValue(target) as int?) + (sp.GetValue(source) as int?);
                            ap.SetValue(target, n);
                        }
                        else if (ap.PropertyType == typeof(float))
                        {
                            float? n = (ap.GetValue(target) as float?) + (sp.GetValue(source) as float?);
                            ap.SetValue(target, n);
                        }
                    }
                }
            }
        }
        public static void MapperObjSubtraction<A, B>(A target, B source, string[] noContain = null)
        {
            Type Typeb = source.GetType();//获得类型  
            Type Typea = target.GetType();

            foreach (FieldInfo sp in Typeb.GetFields())
            {
                foreach (FieldInfo ap in Typea.GetFields())
                {
                    if (ap.Name == sp.Name)//判断属性名是否相同  
                    {
                        if (noContain != null)
                        {
                            if (noContain.Find(s => s == ap.Name) != null) continue;
                        }

                        if (ap.FieldType == typeof(int))
                        {
                            int? n = (ap.GetValue(target) as int?) - (sp.GetValue(source) as int?);
                            ap.SetValue(target, n);
                        }
                        else if (ap.FieldType == typeof(float))
                        {
                            float? n = (ap.GetValue(target) as float?) - (sp.GetValue(source) as float?);
                            ap.SetValue(target, n);
                        }

                    }
                }
            }

            foreach (PropertyInfo sp in Typeb.GetProperties())
            {
                foreach (PropertyInfo ap in Typea.GetProperties())
                {
                    if (ap.Name == sp.Name)//判断属性名是否相同  
                    {
                        if (noContain != null)
                        {
                            if (noContain.Find(s => s == ap.Name) != null) continue;
                        }

                        if (ap.PropertyType == typeof(int))
                        {
                            int? n = (ap.GetValue(target) as int?) + (sp.GetValue(source) as int?);
                            ap.SetValue(target, n);
                        }
                        else if (ap.PropertyType == typeof(float))
                        {
                            float? n = (ap.GetValue(target) as float?) + (sp.GetValue(source) as float?);
                            ap.SetValue(target, n);
                        }
                    }
                }
            }
        }
        #endregion

        #region Create Method
        public static Dictionary<string, object> CacheCreateMethod = new Dictionary<string, object>();

        /// <summary>
        /// 创建方法 并缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="className"></param>
        /// <returns></returns>
        //public static T CreateMethod<T>(string className, bool cache = true) where T : class
        //{
        //    if (cache)
        //    {
        //        if (CacheCreateMethod.ContainsKey(className))
        //        {
        //            return CacheCreateMethod[className] as T;
        //        }
        //        else
        //        {
        //            object instance = CreateObject(className);
        //            //Type type = Type.GetType(className);//反射1：获取类型：获取反射类型（字符串）
        //            //object instance = Activator.CreateInstance(type);//反射2：创建对象：激活.创建实例化（type）
        //            CacheCreateMethod.Add(className, instance);
        //            return instance as T;
        //        }
        //    }
        //    else
        //    {
        //        object instance = CreateObject(className);
        //        return instance as T;
        //    }
        //}
        #endregion

        /// <summary>
        /// 传入List 返回 随机 不重复 元素List 
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">元素列表</param>
        /// <param name="num">返回元素数量</param>
        /// <returns></returns>
        public static List<T> GetRandomObjList<T>(List<T> list, int num = 1, bool loop = false)
        {
            System.Random random = new System.Random();

            List<T> tempList = new List<T>();
            list.ForEach(i => tempList.Add(i));

            List<T> reList = new List<T>();

            for (int i = 0; i < num; i++)
            {
                if (tempList.Count == 0) break; //如果 列表中没有可获得元素 && 不循环 则结束循环
                else if (tempList.Count == 0 && loop)
                {
                    list.ForEach(s => tempList.Add(s));//如果无元素&&可循环 则重新赋值循环
                }

                int n = random.Next(0, tempList.Count - 1);//随机一个数
                var v = tempList[n];//获取对应索引里面的Obj
                tempList.RemoveAt(n);
                reList.Add(v);//添加这个Obj
            }
            return reList;
        }
        /// <summary>
        /// 传入List 返回List 不重复
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="num"></param>
        /// <param name="removeat">删除不参与的索引int</param>
        /// <returns></returns>
        public static List<T> GetRandomObjList<T>(int removeat, List<T> list, int num = 1, bool loop = false)
        {
            System.Random random = new System.Random();

            List<T> tempList = new List<T>();//创建临时List

            list.ForEach(i => tempList.Add(i));//为 参与计算的临时list 赋值

            tempList.RemoveAt(removeat);//删除不参与计算的索引

            List<T> reList = new List<T>();//需要返回的List

            for (int i = 0; i < num; i++)
            {
                if (tempList.Count == 0) break; //如果 列表中 没有可获得 元素 则结束循环
                else if (tempList.Count == 0 && loop)
                {
                    list.ForEach(s => tempList.Add(s));//如果无元素&&可循环 则重新赋值循环
                }

                int n = random.Next(0, tempList.Count - 1);//随机一个数
                var v = tempList[n];//获取对应索引里面的Obj
                tempList.RemoveAt(n);
                reList.Add(v);//添加这个Obj
            }
            return reList;
        }

        //随机数字 返回bool
        public static bool GetRandomBool(int n)
        {
            System.Random random = new System.Random();
            int x = random.Next(1, 100);
            if (x < n)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 快速创建obj 并返回obj
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static object CreateObject(string typeName)
        {
            object obj = null;

            try
            {
                Type objType = Type.GetType(typeName, true);
                obj = Activator.CreateInstance(objType);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return obj;
        }
        public static int GetRandomNum(int min, int max)
        {
            if (min == max) return min;
            int num = UnityEngine.Random.Range(min, max + 1);
            return num;
        }

        /// <summary>
        /// 获取动态dynamic类 - 通过创建
        /// </summary>
        /// <param name="typeName">命名空间 + 类名</param>
        /// <returns></returns>
        public static dynamic GetDynamicType(string typeName)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Type t = a.GetType(typeName, true);
            dynamic obj = Activator.CreateInstance(t);

            return obj;
        }
        public static dynamic GetDynamicTypeByAs(object obj, string typeName)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Type t = a.GetType(typeName, true);
            dynamic dynamic = Convert.ChangeType(obj, t);
            return dynamic;
        }

        /// <summary>
        /// 转换类
        /// </summary>
        /// <typeparam name="T">目标Type</typeparam>
        /// <param name="obj">目标obj</param>
        /// <param name="data">转为Type</param>
        /// <returns></returns>
        public static bool TryGetType<T>(object obj, out T data) where T : class
        {
            T t = obj as T;

            if (t == null)
            {
                data = null;
                return false;
            }
            else
            {
                data = t;
                return true;
            }


        }

        ///// <summary>
        ///// 获得类型名称字符串
        ///// </summary>
        ///// <param name="proNmae"> 命名空间 项目名</param>
        ///// <param name="namespaceNmae"> 命名空间 集合名</param>
        ///// <param name="name">类名</param>
        ///// <param name="type">尾缀</param>
        ///// <returns></returns>
        //public static string GetStr(string proNmae, string namespaceNmae, string name, string type = null)
        //{
        //    return string.Format("{0}.{1}.{2}{3}", proNmae, namespaceNmae, name, type);//快速编辑名称格式
        //}

        private static StringBuilder tempSB = new StringBuilder();

        /// <summary>
        /// 通过 intern 获取str （缓存池）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetStr(string str)
        {
            return string.Intern(str);
        }
        public static string Intern(this string str)
        {
            return string.Intern(str);
        }
        /// <summary>
        /// 将string[] 合并为 string
        /// </summary>
        /// <param name="arr">string[] , ‘,’</param>
        /// <returns></returns>
        public static string GetStr(params string[] arr)
        {
            return string.Intern(GetStringBuilder(arr, null).ToString());
        }
        /// <summary>
        /// 将string[] 合并为 string ,并加入分隔符
        /// </summary>
        /// <param name="split"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string GetStr(char split, params string[] arr)
        {
            return string.Intern(GetStringBuilder(arr, split).ToString());
        }


        /// <summary>
        /// 获得String builder
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="split"></param>
        /// <param name="newSB"></param>
        /// <returns></returns>
        private static StringBuilder GetStringBuilder(string[] arr, char? split/* = ','*/, bool newSB = false)
        {
            StringBuilder sb;

            if (newSB)
            {
                sb = new StringBuilder();
            }
            else
            {
                sb = tempSB.Clear();
            }

            if (split != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    sb.Append(arr[i]).Append(split);
                }
            }
            else
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    sb.Append(arr[i]);
                }
            }

            return sb;
        }

        /// <summary>
        /// 读取Text文件 无限循环 直到读取完成
        /// </summary>
        /// <param name="path">文件路径：c/test.txt</param>
        /// <returns></returns>
        public static string ReadTextFile(string path, string eveName, object sender)
        {
            UnityWebRequest request = UnityWebRequest.Get(path);//获取连接
            request.SendWebRequest();//执行连接
            while (true)
            {
                if (request.downloadHandler.isDone)//确定接受完成
                {
                    EventManager.Instance.SendEvent(eveName, sender, request.downloadHandler.text);
                    return request.downloadHandler.text; //读取
                }
            }
        }

        private static int objLayerMask;
        public static bool LayerMaskContainObj(LayerMask lm, GameObject obj)
        {
            // 根据Layer数值进行移位获得用于运算的Mask值
            objLayerMask = 1 << obj.layer;
            return (lm.value & objLayerMask) > 0;
        }

        /// <summary>
        /// 拆分一个字符串 并返回 string[]
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="split">拆分符 默认：'_'</param>
        /// <returns></returns>
        public static string[] GetStringToArray(string str, char split = ',')
        {
            if (string.IsNullOrEmpty(str)) return null;

            string[] val = str.Split(split);//信息分隔符 :'_'
            return val;
        }

        public static string RemoveEnd(this string str, string place)
        {
            if (str.Length < place.Length)
            {
                return string.Empty;
            }

            return str.Replace(place, "");
        }


        public static string RemoveEnd(this string str, int len)
        {
            if (str.Length < len)
            {
                return string.Empty;
            }

            return str.Substring(0, str.Length - len);
        }

        public static string ReadFile(string path)
        {
            UnityWebRequest request = UnityWebRequest.Get(path);//获取连接
            request.SendWebRequest();//执行连接
            while (true)
            {
                if (request.downloadHandler.isDone)//确定接受完成
                {
                    return request.downloadHandler.text; //读取
                }
            }
        }

        public static void WriteFile(string path, string json)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            byte[] b = Encoding.UTF8.GetBytes(json);
            fs.Write(b, 0, b.Length);
            fs.Flush();
            fs.Close();
            fs.Dispose();
        }

        public static bool Contains(this LayerMask layerMask, int layer)
        {
            return (layerMask & layer) > 0;
        }
        public static bool Contains(this LayerMask layerMask, string layerName)
        {
            return Contains(layerMask.value, LayerMask.NameToLayer(layerName));
        }
        public static bool BelongsTo(this int layer, LayerMask layerMask)
        {
            return Contains(layerMask, layer);
        }
        public static bool Contains(this int layer, int layer1)
        {
            return (layer & layer1) > 0;
        }
        public static void RemoveIf<T>(this List<T> list, Func<T, bool> condition)
        {
            int num = list.Count;

            for (int i = num - 1; i >= 0; i--)
            {
                if (condition(list[i]))
                {
                    list.Remove(list[i]);
                }
            }
        }

        /// <summary>
        /// 判断list是否为null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> GetThisList<T>(this List<T> list)
        {
            if (list == null || list.Count <= 0) return null;
            else return list;
        }

        public static string GetStrReplace(this string s, string s1 ,string s2 = "")
        {
            return string.Intern(s.Replace(s1, s2));
        }


        public static bool ListIsNullorEmpty<T>(this List<T> list) 
        {
            if (list == null || list.Count <= 0) return true;
            else return false;
        }


        /// <summary>
        /// 变换组件助手类：用于查找子类目 未知名称 组件
        /// </summary>
        /// <param name="currentTF">当前Transform组件</param>
        /// <param name="childName">后代物体的名称</param>
        /// <returns></returns>
        public static Transform FindChildByName(this Transform currentTF, string childName)
        {
            //查找
            Transform childTF = currentTF.Find(childName);//在当前CurrentTF中找到 childName 组件 交给 childTF ；
            if (childTF != null) return childTF;//找到就返回

            //将任务交给子物体 继续查找
            for (int i = 0; i < currentTF.childCount; i++)
            {
                childTF = FindChildByName(currentTF.GetChild(i), childName);//将任务交给子物体
                if (childTF != null) return childTF;
            }
            return null;
        }

        public static Transform FindParentByName(this Transform currentTF, string parentName)
        {
            Transform targetParentTF;
            //查找所有  子物体 
            Transform[] ParentTFs = currentTF.gameObject.GetComponentsInParent<Transform>();
            //遍历子物体 获取名字
            targetParentTF = ParentTFs.Find(s => s.name == parentName);
            if (targetParentTF != null) return targetParentTF;
            return null;
        }

        public static Transform[] FindChildsByName(this Transform currentTF, string childName)
        {
            //List<Transform> targetChildTFs = new List<Transform>();
            Transform[] targetChildTFs;
            //查找所有  子物体 
            Transform[] childTFs = currentTF.gameObject.GetComponentsInChildren<Transform>();
            //遍历子物体 获取名字
            targetChildTFs = childTFs.FindAll(s => s.name == childName);
            return targetChildTFs;
        }

        /// <summary>
        /// 面向目标方向
        /// </summary>
        /// <param name="targetDirection">目标方向</param>
        /// <param name="transform">需要转向的对象</param>
        /// <param name="rotationSpeed">转向速度</param>
        public static void LookAtTarget(Vector3 targetDirection, Transform transform, float rotationSpeed)
        {
            if (targetDirection != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);
            }
        }
    }
}
