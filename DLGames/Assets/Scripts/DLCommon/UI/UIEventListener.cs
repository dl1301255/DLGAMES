using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DL.UGUI.FrameWork
{
    //定义委托数据类型
    #region
    public delegate void PointerEventHandler(PointerEventData eventData);
	public delegate void BaseEventHandler(BaseEventData eventData);
	public delegate void AxisEventHandler(AxisEventData eventData); 
	#endregion
	///<summary>
	///UI事件监听器 类似EventTrigger
	///<summary>
	public class UIEventListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, 
		IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IDropHandler, IEndDragHandler, IScrollHandler, IUpdateSelectedHandler, 
		ISubmitHandler, IEventSystemHandler, ICancelHandler,IMoveHandler,IDeselectHandler,ISelectHandler
	{
		public string id;

        //EventTrigger
        //声明事件
        #region
        public event PointerEventHandler PointerClick;
		public event PointerEventHandler PointerDown;
		public event PointerEventHandler PointerUP;
		public event PointerEventHandler PointerEnter;
		public event PointerEventHandler PointerExit;
		public event PointerEventHandler InitializePotentialDrag;
		public event PointerEventHandler BeginDrag;
		public event PointerEventHandler Drag;
		public event PointerEventHandler EndDrag;
		public event PointerEventHandler Drop;
		public event PointerEventHandler Scroll;
		public event BaseEventHandler UpdateSelected;
		public event BaseEventHandler Select;
		public event BaseEventHandler Deselect;
		public event BaseEventHandler Submit;
		public event BaseEventHandler Cancel;
		public event AxisEventHandler Move;
        #endregion



        //为物体 添加Listener 首先要获取物体 - 如果没有脚本则ADD脚本
        public static UIEventListener GetListener(Transform tf)
		{
			UIEventListener uiEvent = tf.GetComponent<UIEventListener>();
			if (uiEvent == null) uiEvent = tf.gameObject.AddComponent<UIEventListener>();//注意"uievent ="要加上 不然uievent 还是空
			return uiEvent;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (BeginDrag != null) BeginDrag(eventData);
		}

		public void OnCancel(BaseEventData eventData)
		{
			if (Cancel != null) Cancel(eventData);
		}

		public void OnDeselect(BaseEventData eventData)
		{
			if (Deselect != null) Deselect(eventData);
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (Drag != null) Drag(eventData);
		}

		public void OnDrop(PointerEventData eventData)
		{
			if (Drop != null) Drop(eventData);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (EndDrag != null) EndDrag(eventData);
		}

		public void OnInitializePotentialDrag(PointerEventData eventData)
		{
			if (InitializePotentialDrag != null) InitializePotentialDrag(eventData);
		}

		public void OnMove(AxisEventData eventData)
		{
			if (Move != null) Move(eventData);
		}

		//继承接口
		public void OnPointerClick(PointerEventData eventData)
		{
			if (PointerClick != null) PointerClick(eventData);
		}

		/// <summary>
		/// 清空点击事件绑定
		/// </summary>
		public void ClearPointerClick() 
		{
			PointerClick = null;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (PointerDown != null) PointerDown(eventData);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (PointerEnter != null) PointerEnter(eventData);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (PointerExit!= null) PointerExit(eventData);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (PointerUP != null) PointerUP(eventData);
		}

		public void OnScroll(PointerEventData eventData)
		{
			if (Scroll != null) Scroll(eventData);
		}

		public void OnSelect(BaseEventData eventData)
		{
			if (Select != null) Select(eventData);
		}

		public void OnSubmit(BaseEventData eventData)
		{
			if (Submit != null) Submit(eventData);
		}

		public void OnUpdateSelected(BaseEventData eventData)
		{
			if (UpdateSelected != null) UpdateSelected(eventData);
		}
	}
}
