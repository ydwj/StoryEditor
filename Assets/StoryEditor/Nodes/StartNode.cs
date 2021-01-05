using DialogueEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


namespace DialogueEditor
{
	[CreateNodeMenu("StartNode")]
	[NodeWidth(200)]

	[NodeTint(255,255,60)]//Node颜色
	public class StartNode : Node
	{


		[Output]
		public Empty Output;

		public AudioClip audioClip;
		protected override void Init()
		{
			base.Init();

		}

		public override object GetValue(NodePort port)
		{
			return null; // Replace this
		}

		public Node  MoveNext(out Current current)
		{
			NodePort exitPort = GetOutputPort("Output");

			if (!exitPort.IsConnected)
			{
				current = Current.Start ;
				Debug.Log("Start节点未连接");
				return this;
			}

			
			Node node = exitPort.Connection.node;

			//性能较低
			//if (node.GetType()==typeof(ChatNode))
			//{
			//	current = Current.Chat; 
			//}


			ChatNode cnode = node as ChatNode;
			if (cnode != null)
			{
				current = Current.Chat;
				return cnode;
			}

			current = Current.Start;

			OptionNode onode = node as OptionNode;
			if (onode != null)
			{
				current = Current.Option;
				return onode;
			}

			return this;
			
		}
	}
}
