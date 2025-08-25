using Mirror;
using UnityEngine;

namespace _Project.Scripts.GamePlay.Player
{
    public class PlayerNetworkActions : NetworkBehaviour
    {
        [SerializeField] private GameObject _cubePrefab;

        [Command]
        public void CmdSpawnCube()
        {
            if (_cubePrefab == null) 
                return;
            
            var pos = transform.position + transform.forward * 2f;
            var go = Instantiate(_cubePrefab, pos, Quaternion.identity);
            
            NetworkServer.Spawn(go);
        }
    }
}