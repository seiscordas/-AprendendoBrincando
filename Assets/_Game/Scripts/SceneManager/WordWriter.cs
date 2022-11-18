using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LearningByPlaying.WordWriterSystem
{
    public class WordWriter : MonoBehaviour
    {
        [SerializeField] private Transform wordPlace;
        [SerializeField] private GameObject charSlot;

        public static WordWriter Instance;
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
                yield return new WaitForSeconds(0.5f);
            }
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
            if (charSlotList.Count > 0)
            {
                foreach (GameObject child in charSlotList)
                {
                    DestroyImmediate(child);
                }
                charSlotList.Clear();
            }
        }
    }
}
