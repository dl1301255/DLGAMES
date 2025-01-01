using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.Common
{
    //public delegate void Fun();
    ///<summary>
    ///
    ///<summary>
    ///		/// <summary>
    /// 查找 符合条件的 T泛型 
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="array">目标物体数组</param>
    /// <param name="condition">判断条件</param>
    /// <returns></returns>
    //本质：查找符合条件的目标，包含 目标列表，判断条件，循环执行 三个 执行模块
    //目标列表：T[] array;
    //判断条件：Func condition； 调用者设计
    //循环执行：for
    //将判断条件 交给 调用者，其他部分 列表，循环 都在 方法内执行；

    public static class ArrayHelper
    {
        //符合条件的目标
        public static T Find<T>(this T[] array, Func<T, bool> condition)//定义一个T型 Find工具 （Func一个 泛型，有返回值，委托）（包含目标数组，bool）
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (condition(array[i]))
                {
                    return array[i];
                }
            }

            return default(T);//泛型 为空 返回值 default（T）
        }

        //符合条件的 数组
        public static T[] FindAll<T>(this T[] array, Func<T, bool> condition) //注意这里需要返回 T型数组 - T[]
        {
            List<T> list = new List<T>();

            for (int i = 0; i < array.Length; i++)
            {
                if (condition(array[i]))
                {
                    list.Add(array[i]);
                }
            }
            return list.ToArray();
        }

        //选取最大值
        /// <summary>
        /// 数组 选取最大值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="Q">比较类型（接口）</typeparam>
        /// <param name="array">数组名</param>
        /// <param name="condition">比较方法名</param>
        /// <returns></returns>
        public static T GetMax<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable//注意这里需要返回 T型数组 - T[]
        {
            T max = array[0];//选取第0个元素
            for (int i = 0; i < array.Length; i++)
            {
                if (condition(max).CompareTo(condition(array[i])) < 0)//开始比较
                {
                    max = array[i];
                }
            }

            return max;
        }

        //求最小值
        public static T GetMin<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable//注意这里需要返回 T型数组 - T[]
        {
            T min = array[0];//选取第0个元素
            for (int i = 0; i < array.Length; i++)
            {
                if (condition(min).CompareTo(condition(array[i])) > 0)//开始比较
                {
                    min = array[i];
                }
            }

            return min;
        }


        //升序
        public static T[] OrderBy<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable //注意这里需要返回 T型数组 - T[]
        {

            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    //if (array[j] > array[j+1]) 
                    if (condition(array[j]).CompareTo(condition(array[j + 1])) > 0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            return array;
        }

        //求降序
        public static T[] OrderDescending<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable //注意这里需要返回 T型数组 - T[]
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    //if (array[j] > array[j+1]) 
                    if (condition(array[j]).CompareTo(condition(array[j + 1])) < 0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            return array;
        }


        //筛选
        public static Q[] Select<T, Q>(this T[] arrary, Func<T, Q> condition)//第一个 T 是 范围类型，Func 中 T=筛选条件合 Q=目标（需要被筛选） 
        {
            Q[] result = new Q[arrary.Length];
            for (int i = 0; i < arrary.Length; i++)
            {
                //筛选的条件，满足 则 保存result里面
                result[i] = condition(arrary[i]);
            }
            return result;
        }

        //+1
        public static T[] Add<T>(this T[] arrary, T value)
        {
            T[] result = new T[arrary.Length + 1];
            arrary.CopyTo(result,0);
            result[arrary.Length] = value;
            return result;
        }
        //-1
    }

}
