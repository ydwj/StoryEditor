using DialogueEditor;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XNode;
using DG.Tweening;
using System;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.Animations;



public enum NodeEvent
{ 
    Execute_schat,
    ChangeSelectID,
    GetOptionTitle,
    AllDelay,
    Exeute_Event,
    ChatDelay,
    ChangeGraph


}


/// <summary>
/// 当前应该执行的Node类型
/// </summary>  
public enum Current
{ 
   Null,
   Start,
   Chat,
   Option
}

/// <summary>
/// PlayDialogueGraph的运行状态
/// </summary>
public enum State 
{ 
    OFF,
    ON,
    Pause
}



public class PlayDiaLogueGraph : SerializedMonoBehaviour
{

    #region UI-ChatNode Args
    public RectTransform dialoguePanel;

    public RectTransform TextRoot;
    public Image personimage;
    public Text text;
    public Text Name;


    private string textvalue;
    private ChatType cur_chattype;
    private ChatType lastType;

    public float PlayTextSpeed = 0.2f;
    public float UIspeed = 0.7f;
    [InfoBox("这个值决定GoNext对话框的持续时间")]
    public float GoNextTime = 2f;

   
    private bool isPlayTextfinished ;

    //判断是否需要等待UI展开后播放下一句台词
    private bool isShowUIWait ;
    private bool tempbool ;

    private bool isPaused;

    //是否不需要等待上一帧UI结束
    private bool isNoNeedWaitPack;
    private bool isMoveNext ;

    //之后做存档功能需要在读取过程中获取这两个ID
    private int chatItemID;


    #endregion


    #region OptionNode Args
    List<GameObject> OpUIs = new List<GameObject>();

    private GameObject OptionArea;
    public GameObject Optionprefrab;
    //激活选择项后，当前选择的Id
    public RectTransform OptionBack;
    private int selectID ;
    private Text s0;
    public Text title;
    private OptionNode OpNode;

    private bool isGoNextNode ;
    private bool isInstinate ;
    #endregion



    #region StartNode 



    #endregion
    public AudioSource audioSource;
    //执行的dialogueGraph
    public DialogueGraph dialogueGraph;

    private bool NoDelay = true;

 

    //决定当前应该执行dialogueGraph中的哪种Node,后期可序列化为存档
    //每个节点 的MoveNext函数需要手动更新是否前往下个节点的Current状态
    private Current current = Current.Null;
    private Current lastCurrent = Current.Null;

    private State state = State.OFF;

    private Node currentNode;

    public List<DialogueGraph> dias = new List<DialogueGraph>();

    [InfoBox("在这添加你想触发的方法，并确保他挂载在场景内")]
    [NonSerialized, OdinSerialize]
    public Dictionary<string, UnityEvent> Methods = new Dictionary<string, UnityEvent>();


 

    #region 初始化
    public void InitChatNode()
    {
        isPaused = false;
        isPlayTextfinished = false;

        //判断是否需要等待UI展开后播放下一句台词
        isShowUIWait = false;
        tempbool = true;

        //是否不需要等待上一帧UI结束
        isNoNeedWaitPack = true;
        isMoveNext = true;
        

        //之后做存档功能需要在读取过程中获取这两个ID
        chatItemID = 0;
    }

    public void InitOption()
    {
        selectID = 0;
        isGoNextNode = false;
        isInstinate = true;
    }

    #endregion

    void Awake()
    {
       
        InitChatNode();
        InitOption();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        OptionArea = GameObject.Find("OptionArea");
        ExecuteState();
        //注册切换事件
        EventManager.AddEvent< ChatType,string, Sprite,string >(NodeEvent.Execute_schat.ToString(), Execute_schat);
        EventManager.AddEvent<int>(NodeEvent.ChangeSelectID.ToString(), ChangeSelectID);
        EventManager.AddEvent<string>(NodeEvent.GetOptionTitle.ToString(), GetOptionTitle);
        EventManager.AddEvent<float >(NodeEvent.AllDelay.ToString(), AllDelay);
        EventManager.AddEvent<ChatType, string>(NodeEvent.Exeute_Event.ToString(), Exeute_Event);
        EventManager.AddEvent<ChatType, float>(NodeEvent.ChatDelay.ToString(), ChatDelay);
        

        EventManager.AddEvent(NodeEvent.ChangeGraph.ToString(), ChangeGraph);
        DontDestroyOnLoad(this.gameObject);
        //获取初始Node，开始执行
        if (dialogueGraph)
        {
            currentNode = dialogueGraph.MoveNext(out current);
        }

        


    }

 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
         
            ChangeState(State.ON);
            ExecuteState();


        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeState(State.OFF );
            ExecuteState();
        }
        
        if (state == State.ON)
        {
            if (NoDelay)
            {
                ExeuteCurrentNode(currentNode);
            }
          
        }
       
    }


    #region 公开调用的方法

    //激活Graph
    public void AwakePlayDialogeGraph()
    {
        ChangeState(State.ON);
       
    }

    //关闭Graph
    public void CloseGraph()
    {
        ChangeState(State.OFF);
     
    }

    //切换到下一个Graph
    int i = 0;
    public void ChangeGraph()
    {
        CloseGraph();
        InitChatNode();
        InitOption();
      //  ChangeState(State.OFF);
     
        dialogueGraph = dias[i];
        i++;
        if (dialogueGraph)
        {
            StartCoroutine(wait1());
        }
    }

    IEnumerator wait1()
    {
        yield return new WaitForSeconds(1f);
    
        ChangeState(State.ON);
       
        currentNode = dialogueGraph.MoveNext(out current);
    }



    #endregion




    #region 允许调用者自己决定是否缩放UI，几秒后唤醒


    public void AllDelay(float seconds)
    {
        PackUpUI();
        StartCoroutine(Delay(seconds));
    }

    IEnumerator Delay(float seconds)
    {
        NoDelay = false;
    
        yield return new WaitForSeconds(seconds);

        NoDelay = true;
        
    }



    #endregion


    /// <summary>
    /// 执行当前的Node 
    /// </summary>
    /// <param name="curNode"></param>
    public void ExeuteCurrentNode(Node curNode)
    {
        switch (current)
        {
            case Current.Null:
                PackUpUI();
              //  Debug.Log("未创建StartNode");
                break;
            case Current.Start:
                PlayStartNode(curNode as StartNode);
               // Debug.Log("进入start节点");

                break;
            case Current.Chat:
                PlayChatNode(curNode as ChatNode);
                break;
            case Current.Option:

                PlayOptionNode(curNode as OptionNode);
                break;
        
            default:
                break;
        }
    }

    public void ExecuteState()
    {
        switch (state)
        {
            case State.OFF:
                PackUpUI();
                break;
            case State.ON:
                ShowUI();
               
                break;
            case State.Pause:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 切换总状态
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(State tempstate)
    {
        switch (tempstate)
        {
            case State.OFF:
                state = State.OFF;
                break;
            case State.ON:
                state = State.ON;
                break;
            case State.Pause:
                state = State.Pause;
                break;
            default:
                break;
        }

      ExecuteState();
    }


    public void PlayStartNode(StartNode node )
    {
       chatItemID = 0;
       currentNode= node.MoveNext(out current);
        if (node.audioClip)
        {
            // audioClip = node.audioClip;

            audioSource.clip = node.audioClip;
       
            return;
        }
        Debug.LogError("未载入打字音乐");
   
    }

    #region 处理ChatNode

    //获取播放这段文字的时间
    public float GetPlayTextTime(string textvlaue)
    {
        return textvlaue.Length * PlayTextSpeed;
    }

    //是否播放完毕，允许点击进入下一段话
    public void FinishPlayText()
    {
    
        audioSource.Stop();
        if (cur_chattype == ChatType.GoNext)
        {
         
            StartCoroutine(GoNextNodeDelay(GoNextTime));
          
            return;
        }

        isPlayTextfinished = true;
    }

    IEnumerator GoNextNodeDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isPlayTextfinished = true;
    }

    private bool clickOnece = false;
    public void ClickGO()
    {
        if (NoDelay&&isPlayTextfinished)
        {
            clickOnece = true;
        }
      
    }
    //在Update中调用
    public void PlayChatNode(ChatNode node)
    {
       
         if (isMoveNext &&isNoNeedWaitPack)
        {
            isMoveNext = false;
            //这里触发的MoveNext会记录lasttype
            //每次更新当前Node

            currentNode= node.MoveNext(chatItemID);
                
        }
        //是否进入下一句话
        if (isPlayTextfinished)
        {
            if (cur_chattype == ChatType.cEvent || cur_chattype == ChatType.pause)
            {
                chatItemID++;
                isPaused = true;
                isPlayTextfinished = false;
                isMoveNext = true;
                return;
            }
            else if (cur_chattype == ChatType.GoNext)
            {
                chatItemID++;
                isNoNeedWaitPack = false;
                PackUpUI();
                isPlayTextfinished = false;
                isMoveNext = true;
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
             
                lastCurrent = current;
                if (cur_chattype == ChatType.singel)
                {
                    isNoNeedWaitPack = false;
                    PackUpUI();
                }
                else if (cur_chattype == ChatType.option)
                {
                  
                    //重置初始值
                    InitChatNode();
                    PackUpUI();
                    //每次只生成一次选项
                    isInstinate = true;
                   // lastType = ChatType.option;
                    current = Current.Option;
                    return;
                    
                }
                else if (cur_chattype == ChatType.jump)
                {
                   
                    InitChatNode();
                    PackUpUI();

                    return;
                }
              

                chatItemID++;
                isPlayTextfinished = false;
                isMoveNext = true;
            }
       
        }
    }
    public void Execute_schat(ChatType curtype, string b, Sprite c,string d )
    {
        text.text = "";
      
       
        textvalue = b;

        //记录上个节点的类型，决定下次是否执行ShowUI

        lastType = cur_chattype;
        cur_chattype = curtype;
      
        personimage.sprite = c;
        Name.text = d;
        StartCoroutine(PlayText());


    }

    public void Exeute_Event(ChatType curtype, string methodName)
    {
        text.text = "";
        lastType = cur_chattype;
        cur_chattype = curtype;
      
       
        UnityEvent unityEvent = null;
        if (Methods.TryGetValue(methodName, out unityEvent))
        {
            unityEvent.Invoke();
            StartCoroutine(PlayText());
            return;
        }
        Debug.LogError("未找需要触发的方法");
      
    }

    //延时调用的事件
    public void ChatDelay(ChatType curtype, float seconds)
    {
        text.text = "";
        //月花
        lastType = cur_chattype;
        cur_chattype = curtype;
      

        AllDelay(seconds);
        StartCoroutine(PlayText());

    }

    IEnumerator PlayText()
    {
        if ( cur_chattype == ChatType.cEvent||cur_chattype==ChatType.pause)
        {
            DealChatstate();
            yield break;
        }

        if (chatItemID == 0)
        {
            ShowUI();
            yield return new WaitForSeconds(UIspeed);
         
        }
        else if (lastType == ChatType.singel || lastType == ChatType.jump || lastType == ChatType.option||lastType==ChatType.GoNext)
        {
            ShowUI();
            yield return new WaitForSeconds(UIspeed);
            
        }
        else if (lastType == ChatType.cEvent || lastType == ChatType.pause)
        {
          
            if (isPaused)
            {
                ShowUI();
                isPaused = false;
            }
            yield return new WaitForSeconds(UIspeed);
        }
       
     
        DealChatstate();
      
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    //判断状态
    public void DealChatstate()
    {
        switch (cur_chattype)
        {
            case ChatType.normal:

                Tweener tweener = text.DOText(textvalue, GetPlayTextTime(textvalue));
                tweener.SetEase(Ease.Linear);
                // audioSource.PlayOneShot(audioClip);
                audioSource.Play();
               
                tweener.OnComplete(FinishPlayText);
                break;
            case ChatType.singel:
                Tweener tweener2 = text.DOText(textvalue, GetPlayTextTime(textvalue));
                tweener2.SetEase(Ease.Linear);
                audioSource.Play();
                tweener2.OnComplete(FinishPlayText);
                break;
            case ChatType.option:

                Tweener tweener3 = text.DOText(textvalue, GetPlayTextTime(textvalue));
                tweener3.SetEase(Ease.Linear);
                audioSource.Play();
                tweener3.OnComplete(FinishPlayText);
                break;
            case ChatType.jump :
                
                Tweener tweener4= text.DOText(textvalue, GetPlayTextTime(textvalue));
                tweener4.SetEase(Ease.Linear);
                audioSource.Play();
                tweener4.OnComplete(FinishPlayText);

                break;
            case ChatType.cEvent:
                FinishPlayText();
      
                break;

            case ChatType.pause:
                FinishPlayText();
 
                break;
            case ChatType.GoNext:
                Tweener tweener5 = text.DOText(textvalue, GetPlayTextTime(textvalue));
                tweener5.SetEase(Ease.Linear);
                audioSource.Play();
                tweener5.OnComplete(FinishPlayText);
                break;
            default:
                break;
        }
    }

    #endregion

    #region UI动效
    public void ShowUI()
    {
        Tweener tweener = TextRoot.DOScale(new Vector3(1, 1, 1), UIspeed);
        tweener.SetEase(Ease.Linear);
        //tweener.OnComplete(InvokeShowUIwait);
    }

    //收起UI
    public void PackUpUI()
    {
        Tweener tweener = TextRoot.DOScale(new Vector3(0, 0, 0), UIspeed);
        tweener.SetEase(Ease.Linear);
        tweener.OnComplete(InvokePackUpUIwait);
    }

    public void ShowOptionUI()
    {
        title.text = "";
       
        Tweener tweener = OptionBack.DOScale(new Vector3(1, 1, 1), UIspeed);
        tweener.SetEase(Ease.Linear);
        tweener.OnComplete (()=>
        {
            
            Tweener tweener2 = title.DOText(textvalue, GetPlayTextTime(textvalue));
            tweener2.SetEase(Ease.Linear);
            isGoNextNode = true;
            tweener2.OnComplete(()=>
            {
                OptionNode temp = currentNode as OptionNode;
             
                for (int i = 0; i < temp.options.Count; i++)
                {
                    OptionUI aaa= OpUIs[i].GetComponentInChildren<OptionUI>();
                    if (aaa.id == i)
                    {
                        Text temp1 = OpUIs[i].GetComponentInChildren<Text>();
                        temp1.text = temp.options[i];
                    }
                    
                }
             
               
            });
        });
    }

    public void PackUpOptionUI()
    { 
        Tweener tweener = OptionBack.DOScale(new Vector3(0, 0, 0), UIspeed);
        tweener.SetEase(Ease.Linear);
        tweener.OnComplete(InvokePackUpUIwait);
    }

    public void InvokePackUpUIwait()
    {
        StartCoroutine(waitPackUpSeconds(UIspeed));
    }

    IEnumerator waitPackUpSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        isNoNeedWaitPack = true;
    }

    #endregion

    #region 处理OptionUI

    public void GetOptionTitle(string title)
    {
        textvalue = title;

    }

    public void ChangeSelectID(int id )
    {
        if (isGoNextNode)
        {
            isGoNextNode = false;
            selectID = id;
            OptionNode temp = currentNode as OptionNode;
            isInstinate = true;
            //获取连接的下一个节点，同时关闭该UI
            lastCurrent = current;
            currentNode = temp.MoveNext(selectID,out current);
            PackUpOptionUI();
          
        }

    
    }

    public void PlayOptionNode(OptionNode node)
    {
        if (isInstinate)
        {
            for (int i = 0; i < OptionArea.transform.childCount; i++)
            {
                Destroy(OptionArea.transform.GetChild(i).gameObject);
            }
            OpUIs.Clear();
            isInstinate = false;
            node.GetTitle();
            for (int i = 0; i < node.options.Count; i++)
            {
                GameObject temp = Instantiate(Optionprefrab, OptionArea.transform);
                OptionUI aaa = temp.GetComponentInChildren<OptionUI>();

                aaa.id = i;

                OpUIs.Add(temp);

            }
        }

        if (isNoNeedWaitPack)
        {

            isNoNeedWaitPack = false;
            ShowOptionUI();
            //展开Option后播放标题
        }
    }

    #endregion





}
