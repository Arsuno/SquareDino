using _Project.Scripts.GamePlay.Player.Data;
using Mirror;
using UnityEngine;

namespace _Project.Scripts.GamePlay.Player
{
    public class PlayerInputController : NetworkBehaviour
    {
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerNetworkData _data;
        [SerializeField] private PlayerNetworkActions _actions;

        [SerializeField] private float _mouseSensitivity = 2f;
        [SerializeField] private float _helloCooldown = 0.5f;

        private float _nextHelloTime;

        private void Update()
        {
            if (!isLocalPlayer) return;

            var move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _movement.SetMoveAxes(move);

            var mouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * _mouseSensitivity;
            _movement.SetLookDelta(mouse);

            if (Input.GetKeyDown(KeyCode.P))
                _actions.CmdSpawnCube();

            if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _nextHelloTime)
            {
                _nextHelloTime = Time.time + _helloCooldown;
                _data.CmdSendHello();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                _movement.ToggleCursor();
        }
    }
}