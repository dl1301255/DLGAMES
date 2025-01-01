using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

namespace DL.Common
{
	///<summary>
	///不销毁
	///<summary>
	public class DontDestroyOnLoad :MonoSingleton<DontDestroyOnLoad>
	{
		public override void Init()
		{
			base.Init();
			DontDestroyOnLoad(this);
		}
	}
}
