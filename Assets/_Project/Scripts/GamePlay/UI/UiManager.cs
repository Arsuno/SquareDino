using _Project.Scripts.GamePlay.Player;
using _Project.Scripts.GamePlay.Player.Data;
using UnityEngine;

namespace _Project.Scripts.GamePlay.UI
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private NicknameInputWindow _nicknameInputWindow;
        
        private PlayerNetworkData _localPlayer;

        private void OnEnable() => 
            PlayerNetworkData.LocalPlayerReady += OnLocalPlayerReady;

        private void OnDisable()
        {
            PlayerNetworkData.LocalPlayerReady -= OnLocalPlayerReady;
            
            if (_nicknameInputWindow != null)
                _nicknameInputWindow.NicknameConfirmed -= OnNicknameConfirmed;
        }

        private void OnLocalPlayerReady(PlayerNetworkData player) => 
            ShowNicknameWindow(player);

        private void ShowNicknameWindow(PlayerNetworkData localPlayer)
        {
            if (_nicknameInputWindow == null) return;

            _nicknameInputWindow.NicknameConfirmed -= OnNicknameConfirmed;

            _localPlayer = localPlayer;
            _nicknameInputWindow.gameObject.SetActive(true);
            _nicknameInputWindow.NicknameConfirmed += OnNicknameConfirmed;
        }

        private void OnNicknameConfirmed(string nickname)
        {
            if (_localPlayer != null && _localPlayer.isLocalPlayer)
            {
                _localPlayer.CmdSetNickname(nickname);

                var movement = _localPlayer.GetComponent<PlayerMovement>();
                if (movement != null)
                    movement.SetControlEnabled(true);
            }

            _nicknameInputWindow.NicknameConfirmed -= OnNicknameConfirmed;
        }
    }
}