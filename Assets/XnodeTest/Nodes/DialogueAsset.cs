using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueEditor
{
    [CreateAssetMenu]
    [System.Serializable]
    public class DialogueAsset : ScriptableObject
    {
        public List<Person> persons = new List<Person>();
    }

    [System.Serializable]//自定义类Unity无法识别序列化，需要使用System.Serializable序列化
    public class Person
    {
        public string Name;
        public AudioClip Voice;
       
     
        public Person()
        {
            Name = string.Empty;
            Voice = null;
     
        }
    }

}

