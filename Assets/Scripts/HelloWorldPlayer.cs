using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {

        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn() 
        {
            if (IsOwner)
            {
                SubmitInitialPositionRequestServerRpc();
            }
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                //var randomPosition = GetRandomPositionOnPlane();
                //transform.position = randomPosition;
                //Position.Value = randomPosition;
                //SubmitPositionRequestServerRpc();
            }
            else
            {
                //SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitInitialPositionRequestServerRpc()
        {
            Position.Value = GetRandomPositionOnPlane();
        }
        
        
        [ServerRpc]
        void SubmitPositionServerRPC(string direction){   
            float x = 0;
            float z = 0;         
            switch (direction) {
                case "L":
                    x = -0.1f;
                    break;
                case "R":
                    x = 0.1f;
                    break;
                case "F":
                    z = -0.1f;
                    break;
                case "B":
                    z = 0.1f;
                    break;
            }
            Position.Value = new Vector3(Position.Value.x + x, Position.Value.y, Position.Value.z + z);
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-5f, 5f));
        }
        
        void Update()
        {
            //Debug.Log($"Espaneos: " + horaEspaneo + " " + numSpawns);
            if (IsOwner){
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    SubmitPositionServerRPC("L");
                }
                if (Input.GetKey(KeyCode.RightArrow)) {
                    SubmitPositionServerRPC("R");
                }
                if (Input.GetKey(KeyCode.UpArrow)) {
                    SubmitPositionServerRPC("B");
                }
                if (Input.GetKey(KeyCode.DownArrow)) {
                    SubmitPositionServerRPC("F");
                }

            }

            transform.position = Position.Value;
        }

        void Start() {
        }
    }
}