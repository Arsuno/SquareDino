using System;
using _Project.Scripts.GamePlay.Player.Nickname;
using Mirror;
using UnityEngine;

namespace _Project.Scripts.GamePlay.Player.Data
{
    public class PlayerNetworkData : NetworkBehaviour
    {
        public static event Action<PlayerNetworkData> LocalPlayerReady;

        [SerializeField] private PlayerNicknameView _playerNicknameView;

        [SyncVar(hook = nameof(OnNicknameChanged))]
        private string _nickname;

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!string.IsNullOrEmpty(_nickname))
                _playerNicknameView?.SetNickname(_nickname);
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            LocalPlayerReady?.Invoke(this); // сообщаем сцене
        }

        [Command(requiresAuthority = true)]
        public void CmdSetNickname(string nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname)) return;
            _nickname = nickname.Trim();
        }

        [Command(requiresAuthority = true)]
        public void CmdSendHello()
        {
            string msg = $"Привет от {_nickname}";
            RpcReceiveHello(msg);
        }

        [ClientRpc] private void RpcReceiveHello(string message) => 
            Debug.Log(message);

        private void OnNicknameChanged(string oldValue, string newValue) =>
            _playerNicknameView?.SetNickname(newValue);
    }
}