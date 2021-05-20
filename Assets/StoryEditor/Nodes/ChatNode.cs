using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using DG.Tweening;
using Microsoft.Win32;
using System;
using UnityEditor;


namespace DialogueEditor
{
	/*
	 * 选择类型
	 * normal：为普通类型，默认向下继续播放，UI不会缩小，适合单人独白
	 * 
	 * single：同样默认继续向下播放，但UI会自动缩小，下一个选项阅读前需要先唤出,只在人物交替点选择一个singel即可，不要连续两个single 
	 * 
	 * option：为选择项,一个ChatNode只允许一个OPtion跳转选项存在,并且需要放在最末尾
	 *
	 * cEvent：如果需要在对话中插入一小段过场动画，或者触发一个事件，可以选择cEvent，造成一小段延时,并自己定义
	 * 
	 * Pause ：可以配合cEvent使用，输入你想要定时的时间后，UI自动收缩，定时时间结束后自动进入下一节点
	 * 
	 * Jump  ：当前chatNode 节点过长的话，可以选择JUMP连接下一chatNode节点，禁止连接其它类型节点
	 * 
	 * GoNext：为特殊对话+过场动画设计，如果你希望UI自动N秒后消失，并进入下一个ChatNode 单项，使用这个
	 * 
	*/
	public enum ChatType
	{ 
		normal,
		singel,
		option,
		cEvent,
		pause,
		jump,
		GoNext
	}


	[CreateNodeMenu("ChatNode")]
	[NodeWidth(400)]
	[NodeTint(73, 236, 209)]//Node颜色
	public class ChatNode : Node		// 块
	{

		//public int ChatNodeID;

	

		public bool IsMax = true;

		public bool lastIsMax = true;
		//如何让其连起来，这里需要涉及到其中的两个特性，一个输出端，一个输入端
		[Input]public Empty Input;

		//[Output(dynamicPortList = true)]
		//[TableList(ShowIndexLabels = true)]
		public List<SingleChatClass> chatslist;

		

		public List<string> tPersonNameList = new List<string>();

		protected override void Init()
        {
            base.Init();

		}


		// Return the correct value of an output port when requested
		public override object GetValue(NodePort port)
		{

			//this .enterId = GetInputValue<Empty>("enterId", this.enterId);
			//if (port.fieldName == "exit")
			//{
			//	return null ;
			//}
			return null;
		}


		public Node MoveNext(int chatItemID)
		{
			Node temp = this;
			for ( int i=0 ; i < chatslist.Count; i++)
            {
				if (i== chatItemID)
				{
					ChatType aa=new ChatType();
					if (chatslist[i].chatType == ChatType.normal)
					{
						aa = ChatType.normal;
					}
					else if (chatslist[i].chatType == ChatType.singel)
					{
						aa = ChatType.singel;
					}
					else if (chatslist[i].chatType == ChatType.option)
					{
						aa = ChatType.option;
						foreach (var port in DynamicPorts)
						{
							//Debug.Log("进入Option" + port.fieldName);
							if (port.fieldName == "chatslist" + " " + i.ToString())
							{
								
								if (!port.IsConnected)
								{
								

									return temp;
								}
								//主循环进入Option状态处理

								//如果连上了，返回连上的Node
								temp = port.Connection.node;


							}
						}
					}
					else if (chatslist[i].chatType == ChatType.jump)
					{
						Debug.Log("进入Jump了，少年啊！");
						aa = ChatType.jump;
						foreach (var port in DynamicPorts)
						{

							if (port.fieldName == "chatslist" + " " + i.ToString())
							{

								if (!port.IsConnected)
								{
									Debug.LogError("Jump节点未连接");

									return temp;
								}
								//主循环进入Option状态处理

								//如果连上了，返回连上的Node
								temp = port.Connection.node;
							}
						}

					}
					else if (chatslist[i].chatType == ChatType.cEvent)
					{
						aa = ChatType.cEvent;
						//这里触发个其他的函数
						string bb2 = chatslist[i].content;
						EventManager.DispatchEvent<ChatType, string>(NodeEvent.Exeute_Event.ToString(), aa, bb2);

						return temp;

					}
					else if (chatslist[i].chatType == ChatType.pause)
					{
						aa = ChatType.pause;
						float f2;
						string bb3 = chatslist[i].content;
						if (!float.TryParse(bb3, out f2))
						{
							Debug.LogError("定时填入的参数无法转换！");
						}
				
						EventManager.DispatchEvent<ChatType, float>(NodeEvent.ChatDelay.ToString(), aa, f2);
						return temp;

					}
					else if (chatslist[i].chatType == ChatType.GoNext)
					{
						aa = ChatType.GoNext;
					}

					string bb = chatslist[i].content;
					Sprite cc = chatslist[i].emoji;
					string dd = tPersonNameList[chatslist[i].name];
				
					EventManager.DispatchEvent<ChatType ,string ,Sprite,string >(NodeEvent.Execute_schat.ToString(),aa,bb,cc,dd);
				
					return temp;
				}
			}

			//Debug.LogError("ID 不存在");
		
			return temp;
		}

	
	}
	[Serializable]
	public class SingleChatClass
	{
		public int name;
		
		public Sprite emoji;

		public ChatType chatType;
		
		public string content;

	}
}


