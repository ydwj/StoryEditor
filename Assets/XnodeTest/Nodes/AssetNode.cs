using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


namespace DialogueEditor
{
	[CreateNodeMenu("AssetNode")]
	[NodeWidth(200)]

	[NodeTint(73, 236, 209)]//Node颜色
	public class AssetNode : Node
	{

		public DialogueAsset diaAsset;
		// Use this for initialization
		protected override void Init()
		{
			base.Init();

		}

		// Return the correct value of an output port when requested
		public override object GetValue(NodePort port)
		{
			return null; // Replace this
		}
	}
}
