using System;
using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            GameManager.Instance.OnLevelUpdated += OnUpdate;
        }

        private void OnUpdate(int moves)
        {
            _text.text = "Level: " + moves;
        }
    }
}