using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


namespace DialogueEditor
{

	[CreateNodeMenu("OptionNode")]
	[NodeWidth(300)]
	[NodeTint(24,158, 252)]//Node颜色
	public class OptionNode : Node
	{

		[Input]
		public Empty Input;

		public string title;
	

		[Output(dynamicPortList =true )]
		public List<string> options;

		protected override void Init()
		{
			base.Init();

		}


		public override object GetValue(NodePort port)
		{
			return null; 
		}


		public void GetTitle()
		{
			EventManager.DispatchEvent<string>(NodeEvent.GetOptionTitle.ToString(), title);
		}

		public Node  MoveNext(int index,out Current current)
		{
			
			Node temp = this;
			
			foreach (var port in DynamicOutputs)
			{
				
				if (port.fieldName == "options" +" "+ index.ToString())
				{
					if (!port.IsConnected)
					{
						current = Current.Null;
						
						return temp;
					} 
					//如果连上了，返回连上的Node
					temp = port.Connection.node;

					ChatNode cnode = temp as ChatNode;
					if (cnode != null)
					{
					
						current = Current.Chat;
						return cnode;
					}
					OptionNode onode = temp as OptionNode;
					if (onode != null)
					{
						current = Current.Option;
						return onode;
					}
					current = Current.Option;
					return temp;
				}
			}
			current = Current.Option;
			return temp;
		}
	}

	
}
