using System.Collections;
using UnityEngine;
using XNodeEditor;
using UnityEditor;
using JetBrains.Annotations;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using XNode;
using UnityEditorInternal;

using System.Collections.Generic;
using System;

namespace  DialogueEditor
{
    //重置Editor面板
    [CustomNodeEditor(typeof(ChatNode))]
    public class DialogueNodeEditor : NodeEditor
    {
        public ChatNode chatNode;

        public bool ShowSpecial=false ;

        public string fieldName = "chatslist";

      //  public string[] RoleNameArr=new string[] {"111","2222"};
        private DialogueGraph dialogueGraph;

        public  DialogueAsset tAsset;

        SerializedProperty iconProperty;




        public override void OnHeaderGUI()
        {
            
            base.OnHeaderGUI();

        }

        List<object> cacheList = new List<object>();

        public long MilliSeconds()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }

        public void DrawList(ReorderableList list)
        {
            IList l = list.list;

            if (chatNode == null)
                chatNode = target as ChatNode;


            if (dialogueGraph == null)
                dialogueGraph = window.graph as DialogueGraph;

            tAsset = dialogueGraph.GetAsset();

            if (tAsset!=null)
            {
                foreach (var person in tAsset.persons)
                {
                    //Debug.Log("角色名" + person.Name);
                    chatNode.tPersonNameList.Add(person.Name);
                  
                }
            }

            //获取序列化后的chatslist数据
            SerializedProperty arrayData = serializedObject.FindProperty(fieldName);
            bool hasArrayData = arrayData != null && arrayData.isArray;

            list.drawHeaderCallback = (Rect rect) => {
               // Debug.Log("AAAAAAAAAAAAAAAAA");
                string title = "对话列表";
                GUI.Label(rect, title);
            };

            list.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
            {
             

                //if (MilliSeconds() < lastBodyGUITime + 100)
                //{
                //    return;
                //}

                //lastBodyGUITime = MilliSeconds();


                #region 自定义渲染动态节点

                XNode.Node node = serializedObject.targetObject as XNode.Node;
                XNode.NodePort port = node.GetPort(fieldName + " " + index);

                EditorGUI.LabelField(rect, "");
                if (port != null&&chatNode.chatslist[index].chatType==ChatType.option|| chatNode.chatslist[index].chatType==ChatType.jump)
                {
                    Vector2 pos = rect.position + (port.IsOutput ? new Vector2(rect.width + 6, 0) : new Vector2(-36, 0));
                    NodeEditorGUILayout.PortField(pos, port);
                }

                #endregion

                #region 详细定义每个元素的位置
                
            

                    // 设置属性名宽度
                    //EditorGUIUtility.labelWidth = 1;
                    Rect position = rect;
                    position.height = EditorGUIUtility.singleLineHeight;

                    //单个singlechatclass数据
                    SerializedProperty itemData = arrayData.GetArrayElementAtIndex(index);
                    if (chatNode.chatslist[index].chatType == ChatType.cEvent || chatNode.chatslist[index].chatType == ChatType.pause)
                    {


                        //这里用原本的对话内容代替停止的时间


                        Rect typeRect = new Rect(position)
                        {
                            width = 130,
                            y = position.y + 5,
                            height = 20
                        };

                        Rect methodNameRect = new Rect(typeRect)
                        {
                            width = position.width - 140,
                            x = position.x + 140,
                            height = 20
                        };

                        SerializedProperty content_Timeproperty = itemData.FindPropertyRelative("content");
                        SerializedProperty typeproperty = itemData.FindPropertyRelative("chatType");

                        content_Timeproperty.stringValue = EditorGUI.TextArea(methodNameRect, content_Timeproperty.stringValue);

                        SingleChatClass temp = chatNode.chatslist[index];
                        ChatType ty = new ChatType();
                        ty = (ChatType)EditorGUI.EnumPopup(typeRect, temp.chatType);

                        typeproperty.enumValueIndex = (int)ty;
                    }
                    else
                    {
                        Rect iconRect = new Rect(position)
                        {
                            width = 50,
                            height = 50
                        };

                        Rect nameTypeRect = new Rect(position)
                        {
                            width = position.width - 270,
                            x = position.x + 60
                        };

                        Rect typeRect = new Rect(nameTypeRect)
                        {
                            width = position.width - 270,
                            y = nameTypeRect.y + EditorGUIUtility.singleLineHeight + 7,
                            height = 20
                        };
                        Rect contentRect = new Rect(position)
                        {
                            width = position.width - 140,
                            height = 50,
                            x = position.x + 140
                        };

                        iconProperty = itemData.FindPropertyRelative("emoji");
                        SerializedProperty nameProperty = itemData.FindPropertyRelative("name");
                        SerializedProperty contentproperty = itemData.FindPropertyRelative("content");
                        SerializedProperty typeproperty = itemData.FindPropertyRelative("chatType");



                        iconProperty.objectReferenceValue =
                         EditorGUI.ObjectField(iconRect, iconProperty.objectReferenceValue, typeof(Sprite), false);



                        SingleChatClass temp = chatNode.chatslist[index];

                        nameProperty.intValue = EditorGUI.Popup(nameTypeRect, temp.name, chatNode.tPersonNameList.ToArray());

                        ChatType ty = new ChatType();
                        ty = (ChatType)EditorGUI.EnumPopup(typeRect, temp.chatType);

                        typeproperty.enumValueIndex = (int)ty;

                        contentproperty.stringValue =
                            EditorGUI.TextArea(contentRect, contentproperty.stringValue);

                    }
                
              
                #endregion
            };

            list.elementHeightCallback =(int index) => {
                //Debug.Log("CCCCCCCCCCCCCCCCC");
            if (hasArrayData)
              {
                    if (arrayData.arraySize <= index)
                        return EditorGUIUtility.singleLineHeight ;
                  SerializedProperty itemData = arrayData.GetArrayElementAtIndex(index);

                    if (chatNode.chatslist[index].chatType == ChatType.cEvent || chatNode.chatslist[index].chatType == ChatType.pause)
                    {
                        return EditorGUI.GetPropertyHeight(itemData) + 12;
                    }
                    
                    return EditorGUI.GetPropertyHeight(itemData)+36;
              }
              else return EditorGUIUtility.singleLineHeight;
            };
        }


     

        /// <summary>
        /// 简单渲染
        /// </summary>
        /// <param name="list"></param>
        public void SimpleDrawList(ReorderableList list)
        {
            if (chatNode == null)
                chatNode = target as ChatNode;


          //  Debug.Log("简单渲染中");
            //获取序列化后的chatslist数据
            SerializedProperty arrayData = serializedObject.FindProperty(fieldName);
            bool hasArrayData = arrayData != null && arrayData.isArray;

            list.drawHeaderCallback = (Rect rect) =>
            {
                string title = "对话列表";
                GUI.Label(rect, title);
            };

            list.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
            {
             
                #region 自定义渲染动态节点

                XNode.Node node = serializedObject.targetObject as XNode.Node;
                XNode.NodePort port = node.GetPort(fieldName + " " + index);

                EditorGUI.LabelField(rect, "");
                if (port != null && chatNode.chatslist[index].chatType == ChatType.option || chatNode.chatslist[index].chatType == ChatType.jump)
                {
                    Vector2 pos = rect.position + (port.IsOutput ? new Vector2(rect.width + 6, 0) : new Vector2(-36, 0));
                    NodeEditorGUILayout.PortField(pos, port);
                }
                #endregion
            };

            list.elementHeightCallback = (int index) =>
            {
                if (chatNode.chatslist[index].chatType == ChatType.option || chatNode.chatslist[index].chatType == ChatType.jump)
                {
                    SerializedProperty itemData = arrayData.GetArrayElementAtIndex(index);
                    return EditorGUI.GetPropertyHeight(itemData);
                }
                else
                {
                    return 0;
                }
            };
        }

        long lastBodyGUITime = 0;

        public override void OnBodyGUI()
        {


            if (chatNode == null)
                chatNode = target as ChatNode;


           
           // Debug.Log("操作chatnode中");
            // serializedObject.Update();
            // base.OnBodyGUI();
            // NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("ChatNodeID"));
            foreach (XNode.NodePort Port in target.Ports)
            {
                if (NodeEditorGUILayout.IsDynamicPortListPort(Port)) continue;
                NodeEditorGUILayout.PortField(Port);
            }

            //对对话列表进行序列化
            bool forceReset = false;
            //Debug.Log("我是最优先的吗");
            if (chatNode.lastIsMax != chatNode.IsMax)
            {
                forceReset = true;
                chatNode.lastIsMax = chatNode.IsMax;

            }

            //姓名元素重载判定
            if (tAsset != null)
            {
                tAsset = dialogueGraph.GetAsset();
                bool isNameEqual = true;
                if (chatNode.tPersonNameList.Count == tAsset.persons.Count)
                {
                    for (int i = 0; i < tAsset.persons.Count; i++)
                    {
                        if (chatNode.tPersonNameList[i] != tAsset.persons[i].Name)
                        {
                            isNameEqual = false;
                            chatNode.tPersonNameList[i] = tAsset.persons[i].Name;
                        }
                    }

                    if (!isNameEqual)
                    {
                       
                        forceReset = true;
                    }
                }
                else
                {
                    chatNode.tPersonNameList.Clear();

                    forceReset = true;
                }
            }

            if (chatNode.IsMax)
            {
              //  Debug.Log("重复渲染节点");
                NodeEditorGUILayout.DynamicPortList("chatslist", typeof(SingleChatClass), serializedObject, NodePort.IO.Output, Node.ConnectionType.Override, Node.TypeConstraint.None, DrawList, forceReset);



                if (GUILayout.Button("Minimize", EditorStyles.miniButton))
                {
                    chatNode.IsMax = false;
                }

                //serializedObject.ApplyModifiedProperties();
            }
            else
            {

                NodeEditorGUILayout.DynamicPortList("chatslist", typeof(SingleChatClass), serializedObject, NodePort.IO.Output, Node.ConnectionType.Override, Node.TypeConstraint.None, SimpleDrawList, forceReset);
                if (GUILayout.Button("Maximize", EditorStyles.miniButton))
                {
                    chatNode.IsMax = true;
                }
            }

            #region Old
            //GUI.color = Color.white;
            //serializedObject.Update();
            //if (chatNode == null)
            //chatNode = target as ChatNode;

            //foreach (XNode.NodePort dynamicPort in target.DynamicPorts)
            //{
            //    if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
            //    NodeEditorGUILayout.PortField(dynamicPort);
            //}
            //NodeEditorGUILayout.DynamicPortList("chatslist", typeof(SingleChatClass), serializedObject, NodePort.IO.Output, Node.ConnectionType.Override);
            ////  NodeEditorGUILayout.PortField(Port);

            //NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("chatslist"));

            //aaa.DoLayoutList();
            //serializedObject.ApplyModifiedProperties();
            //base.OnBodyGUI(); 
            #endregion
        }


   
   
    }
}
