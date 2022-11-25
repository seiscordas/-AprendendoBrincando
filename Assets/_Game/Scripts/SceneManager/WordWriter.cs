using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LearningByPlaying.WordWriterSystem
{
    public class WordWriter : MonoBehaviour
    {
        public static event Action OnFinishWrite;
        public static WordWriter Instance;

        [SerializeField] private float timeDelay = 0.5f;

        private string word;
        private TextMeshProUGUI charSlotText;
        [SerializeField] private List<GameObject> charSlotList = new();

        public List<GameObject> CharSlotList { get => charSlotList; }

        private void Awake()
        {
            Instance = this;
        }

        public void StartWordWriter(string word, GameObject charSlot, Transform wordPlace, float time = 0)
        {
            StopAllCoroutines();
            timeDelay = (time == 0) ? timeDelay : time;
            this.word = word;
            StartCoroutine(Write(charSlot, wordPlace));
        }

        private IEnumerator Write(GameObject charSlot, Transform wordPlace)
        {
            char[] leterList = word.ToCharArray();
            for (int i = 0; i < leterList.Length; i++)
            {
                CharSlotCreator(charSlot, wordPlace).PutCharInCharSlot(leterList[i]);      
                yield return new WaitForSeconds(timeDelay);
            }
            OnFinishWrite?.Invoke();
        }

        private WordWriter CharSlotCreator(GameObject charSlot, Transform wordPlace)
        {
            GameObject newCharSlot = Instantiate(charSlot, wordPlace);
            charSlotText = newCharSlot.GetComponentInChildren<TextMeshProUGUI>();
            charSlotList.Add(newCharSlot);
            return this;
        }

        private void PutCharInCharSlot(char character)
        {
            charSlotText.text = character.ToString();
        }

        public void CleanCharSlotList()
        {
            foreach (var item in charSlotList)
            {
                DestroyImmediate(item.gameObject);
            }
            charSlotList.Clear();
        }
    }
}
