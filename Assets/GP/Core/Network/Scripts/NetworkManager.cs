using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Gameplay;

namespace Networking
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameEvent _OnConnectedToServer;
        [SerializeField] private GameEvent _OnJoinRoomFailed;
        [SerializeField] private GameEvent _OnRoomFulled;

        private void Start()
        {

            //PhotonNetwork.ConnectToMaster("192.168.56.1", 5055, "bc760756-d6bd-48aa-b511-8d3c5cca5aef");
            PhotonNetwork.ConnectUsingSettings();

        }
        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);

        }

        public void CreateRoom(string roomName)
        {
            RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
            PhotonNetwork.CreateRoom(roomName, roomOptions);
            

        }

        public override void OnJoinedRoom()
        {
            Debug.Log(PhotonNetwork.CurrentRoom);
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                _OnRoomFulled.Raise();
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                _OnRoomFulled.Raise();
            }
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            _OnJoinRoomFailed.Raise();
        }
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            _OnConnectedToServer.Raise();
            Debug.Log(PhotonNetwork.ServerAddress);
        }

        
    }
}

