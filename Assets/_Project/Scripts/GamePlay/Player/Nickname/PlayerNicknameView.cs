using TMPro;
using UnityEngine;

namespace _Project.Scripts.GamePlay.Player.Nickname
{
    public class PlayerNicknameView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _labelNickname;
        public void SetNickname(string nickname)
        {
            if (_labelNickname == null) return;
            _labelNickname.text = string.IsNullOrWhiteSpace(nickname) ? "Unknown" : nickname;
        }
    }
}