using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

namespace DialogueEditor
{
    //对整个图的编辑
    [CustomNodeGraphEditor(typeof(DialogueGraph))]
    public class DialogueGrpahEditor : NodeGraphEditor
    {
        DialogueGraph dialogueGraph;
        // private DialogueGraph dialogueGraph;
        public override void OnGUI()
        {
            base.OnGUI();

        }

        public override void OnWindowFocus()
        {
 
        }

        public override void OnWindowFocusLost()
        {
     
        }

        public override void OnDropObjects(Object[] objects)
        {
          //  if (dialogueGraph == null) dialogueGraph = window.graph as DialogueGraph;

            // dialogueGraph.RefreshEventNode();
 
        }
    }

}
