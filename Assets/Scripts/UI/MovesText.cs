using System;
using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MovesText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            GameManager.Instance.OnMovesUpdated += OnUpdate;
        }

        private void OnUpdate(int moves)
        {
            _text.text = moves == 0 ? string.Empty : $"Moves: {moves}";
        }
    }
}