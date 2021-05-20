using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


namespace DialogueEditor
{
    [CreateAssetMenu(fileName = "DiaGraph")]
    public class DialogueGraph : NodeGraph
    {


        public Node MoveNext(out Current current)
        {
          
            List<Node> startnodes = nodes.FindAll((x) =>
              {
                  return x.GetType() == typeof(StartNode);
              });

            for (int i = 0; i < startnodes.Count; i++)
            {
                StartNode tstartNode= startnodes[i] as StartNode;
                if (tstartNode != null)
                {
                    current = Current.Start;
                    return tstartNode ;
                }
            }
            current = Current.Null;
            return null  ;
            
        }

        public DialogueAsset GetAsset()
        {
            List<Node> AssetNodes = nodes.FindAll((x)=>
            {
                return x.GetType() == typeof(AssetNode);
            });

            if (AssetNodes.Count == 1)
            {
                if (AssetNodes[0] != null)
                {
                    AssetNode temp = AssetNodes[0] as AssetNode;
                    return temp.diaAsset;
                }
          
              //  return null;
              //  Debug.LogError("未载入资源");

            }

            Debug.LogError("未创建资源节点或者创建了多个资源节点");
            return null;
        }

        //public void RefreshEventNode()
        //{
        //    List<Node> EventNodes = nodes.FindAll((x) =>
        //    {
        //        return x.GetType() == typeof(EventNode);
        //    });

        //    for (int i = 0; i < EventNodes.Count; i++)
        //    {
        //        Debug.Log("刷新了EventnNode 内的方法");
        //        EventNode temp = EventNodes[i] as EventNode;
        //        temp.RefreshMethods();
        //    }
        //}

    }
}
