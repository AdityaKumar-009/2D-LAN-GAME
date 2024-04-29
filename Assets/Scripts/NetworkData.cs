using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkData : NetworkBehaviour
{

    /*private readonly NetworkVariable<PlayerNetworkData> _networkStats = new(writePerm: NetworkVariableWritePermission.Owner);
    struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _y, _yRot;

        internal Vector3 Position
        {
            get => new(_x, _y, 0);
            set
            {
                _x = value.x;
                _y = value.y;
            }
        }

        internal Vector3 Rotation
        {
            get => new(0, _yRot, 0);
            set
            {
                _yRot = value.y;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
            serializer.SerializeValue(ref _yRot);
        }
    }

    void Update()
    {
        if (IsOwner)
        {
            _networkStats.Value = new PlayerNetworkData()
            {
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles
            };
        }
        else
        {
            transform.SetPositionAndRotation(_networkStats.Value.Position, Quaternion.Euler(_networkStats.Value.Rotation));
        }

    }*/
/*
    [ServerRpc]
    public void PositionServerRpc()
    {
        PositionClientRpc();
    }

    [ClientRpc]
    public void PositionClientRpc()
    {
        
    }*/

}
