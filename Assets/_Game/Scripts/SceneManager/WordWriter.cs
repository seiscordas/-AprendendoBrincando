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

        [SerializeField] private Transform wordPlace;
        [SerializeField] private GameObject charSlot;
        [SerializeField] private float timeDelay = 0.5f;

        private string word;
        private TextMeshProUGUI charSlotText;
        private List<GameObject> charSlotList = new List<GameObject>();

        private void Awake()
        {
            Instance = this;
        }

        public void StartWordWriter(string word)
        {
            CleanCharSlotList();
            this.word = word;
            StartCoroutine(Write());
        }

        private IEnumerator Write()
        {
            char[] leterList = word.ToCharArray();
            for (int i = 0; i < leterList.Length; i++)
            {
                CharSlotCreator().PutCharInCharSlot(leterList[i]);      
                yield return new WaitForSeconds(timeDelay);
            }
            OnFinishWrite?.Invoke();
        }

        private WordWriter CharSlotCreator()
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

        private void CleanCharSlotList()
        {
            while (wordPlace.transform.childCount > 0)
            {
                DestroyImmediate(wordPlace.transform.GetChild(0).gameObject);
            }
        }
    }
}
