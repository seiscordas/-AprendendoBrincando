using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LearningByPlaying.WordWriterSystem
{
    public class WordWriter : MonoBehaviour
    {
        public static event Action OnFinishWriteWord;
        public static WordWriter Instance;

        private string word;
        private TextMeshProUGUI charSlotText;
        [SerializeField] private List<GameObject> charSlotList = new();

        public List<GameObject> CharSlotList { get => charSlotList; }

        private void Awake()
        {
            Instance = this;
        }

        public void StartWordWriter(string word, GameObject charSlot, Transform wordPlace, float timeDelay)
        {
            this.word = word;
            StartCoroutine(Write(charSlot, wordPlace, timeDelay));
        }

        private IEnumerator Write(GameObject charSlot, Transform wordPlace, float timeDelay)
        {
            char[] leterList = word.ToCharArray();
            for (int i = 0; i < leterList.Length; i++)
            {
                CharSlotCreator(charSlot, wordPlace).PutCharInCharSlot(leterList[i]);      
                yield return new WaitForSeconds(timeDelay);
            }
            OnFinishWriteWord?.Invoke();
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
