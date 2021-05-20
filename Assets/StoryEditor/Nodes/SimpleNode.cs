using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DialogueEditor
{
    [CreateNodeMenu("SimpleNode1111111")]
    [NodeWidth(400)]
    [NodeTint(73, 236, 209)]//Node颜色
    public class SimpleNode : Node
    {
        [Input] public float value;
        [Output] public float result;
    }
}