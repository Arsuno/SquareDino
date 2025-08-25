using System;
using _Project.Scripts.GamePlay.Player.Nickname;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.GamePlay.UI
{
    public class NicknameInputWindow : MonoBehaviour
    {
        public event Action<string> NicknameConfirmed;

        [SerializeField] private TMP_InputField _nicknameInputField;
        [SerializeField] private Button _generateButton;
        [SerializeField] private Button _confirmButton;

        private string _currentNickname;

        private void OnEnable()
        {
            _generateButton.onClick.RemoveListener(OnGenerateButtonClicked);
            _confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);

            _generateButton.onClick.AddListener(OnGenerateButtonClicked);
            _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        private void OnDisable()
        {
            _generateButton.onClick.RemoveListener(OnGenerateButtonClicked);
            _confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
        }

        private void OnConfirmButtonClicked()
        {
            if (string.IsNullOrWhiteSpace(_nicknameInputField.text) 
                && !string.IsNullOrWhiteSpace(_currentNickname))
                _nicknameInputField.text = _currentNickname;

            string nickname = _nicknameInputField.text?.Trim();
            NicknameConfirmed?.Invoke(nickname);
            
            gameObject.SetActive(false);
        }

        private void OnGenerateButtonClicked() => GenerateRandomNickname();

        private void GenerateRandomNickname()
        {
            _currentNickname = NicknameGenerator.Generate();
            if (_nicknameInputField != null)
                _nicknameInputField.text = _currentNickname;
        }
    }
}