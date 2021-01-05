
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


namespace DialogueEditor
{
	public class BaseNode : Node
	{

		[Input] public Empty enter;
		[Output] public Empty exit;


		public virtual void MoveNext() { }
		// Return the correct value of an output port when requested
		public override object GetValue(NodePort port)
		{
			return null; // Replace this
		}

		
	}

	[Serializable]
	public class Empty { }
}
