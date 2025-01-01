using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IData 
{
    public void Init();
    public T GetData<T>() where T : class;
    public void Clear();
}
