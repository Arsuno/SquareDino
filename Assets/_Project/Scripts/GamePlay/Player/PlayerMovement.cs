using Mirror;
using UnityEngine;

namespace _Project.Scripts.GamePlay.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NetworkAnimator))]
    public class PlayerMovement : NetworkBehaviour
    {
        private const float _rotationSendInterval = 1f / 30f;

        private static readonly int BlendHash = Animator.StringToHash("Blend");

        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _minPitch = -60f;
        [SerializeField] private float _maxPitch = 80f;

        [SerializeField] private Camera _camera;
        [SerializeField] private Animator _animator;

        private CharacterController _cc;
        private bool _controlEnabled;
        private float _pitch;
        private float _lastRotSentTime;

        private Vector2 _moveAxes;
        private Vector2 _lookDelta;

        [SyncVar] private float _networkYaw;
        [SyncVar] private float _networkPitch;

        private void Awake()
        {
            _cc = GetComponent<CharacterController>();
            
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                SyncRotationFromNetwork();
                return;
            }

            if (!_controlEnabled) return;

            HandleLook();
            HandleMovement();
        }

        public void SetMoveAxes(Vector2 axes) => _moveAxes = Vector2.ClampMagnitude(axes, 1f);
        public void SetLookDelta(Vector2 delta) => _lookDelta = delta;

        public void ToggleCursor()
        {
            bool toLock = Cursor.lockState != CursorLockMode.Locked;
            Cursor.lockState = toLock ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !toLock;
        }

        public void SetControlEnabled(bool enabled)
        {
            if (!isLocalPlayer)
                return;

            _controlEnabled = enabled;

            if (enabled)
                ToggleCursor();
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            if (_camera != null)
                _camera.enabled = true;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            
            if (!isLocalPlayer && _camera != null) 
                _camera.enabled = false;
        }

        private void HandleLook()
        {
            transform.Rotate(0f, _lookDelta.x, 0f, Space.Self);
            _pitch = Mathf.Clamp(_pitch - _lookDelta.y, _minPitch, _maxPitch);
            
            if (_camera != null)
                _camera.transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);

            if (Time.time - _lastRotSentTime >= _rotationSendInterval)
            {
                _lastRotSentTime = Time.time;
                float yaw = transform.eulerAngles.y;
                
                if (isServer)
                {
                    _networkYaw = yaw;
                    _networkPitch = _pitch;
                }
                else
                {
                    CmdUpdateRotation(yaw, _pitch);
                }
            }

            _lookDelta = Vector2.zero;
        }

        private void HandleMovement()
        {
            Vector3 fwd = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;
            Vector3 move = (right * _moveAxes.x + fwd * _moveAxes.y);
            
            if (move.sqrMagnitude > 1f)
                move.Normalize();

            Vector3 velocity = move * _moveSpeed;
            _cc.SimpleMove(velocity);

            if (_animator != null)
                _animator.SetFloat(BlendHash, velocity.magnitude, 0.1f, Time.deltaTime);
        }

        private void SyncRotationFromNetwork()
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0f, _networkYaw, 0f),
                Time.deltaTime * 10f);

            if (_camera != null)
                _camera.transform.localRotation = Quaternion.Lerp(
                    _camera.transform.localRotation,
                    Quaternion.Euler(_networkPitch, 0f, 0f),
                    Time.deltaTime * 10f);
        }

        [Command]
        private void CmdUpdateRotation(float yaw, float pitch)
        {
            _networkYaw = yaw;
            _networkPitch = pitch;
        }
    }
}