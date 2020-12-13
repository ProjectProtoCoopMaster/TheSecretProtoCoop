using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Gameplay;

namespace Networking
{

    public class TransmitterManager : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private Vector3Variable _playerVRPosition;
        [SerializeField] private QuaternionVariable _playerVRRotation;
        [SerializeField] private CallableFunction _switch;
        [SerializeField] private GameEvent _onLose;
        [SerializeField] private GameEvent _onOpenDoor;
        [SerializeField] private GameManager gameManager;
        public SymbolManager symbolManager;

        public void SendPlayerVRPosAndRotToOthers() => photonView.RPC("SendPosAndRot", RpcTarget.Others, _playerVRPosition.Value, _playerVRRotation.Value);


        [PunRPC] private void SendPosAndRot(Vector3 position, Quaternion rotation) { _playerVRPosition.Value = position; _playerVRRotation.Value = rotation; }



        public void SendSwicherChangeToOthers(float ID) => photonView.RPC("SendSwitcherChange", RpcTarget.Others, ID);
        [PunRPC] public void SendSwitcherChange(float ID) => _switch.Raise(ID);
        public void SendLoseToOther(int loseType) => photonView.RPC("SendLose", RpcTarget.Others,loseType);
        [PunRPC] public void SendLose(int loseType) { gameManager.loseType = (GameManager.LoseType)loseType; _onLose.Raise();  }

        public void SendSymbolIDToOther(int value) => photonView.RPC("SendSymbolID", RpcTarget.Others, value);

        [PunRPC]
        public void SendSymbolID(int value) => symbolManager.indexes.Add(value);
        public void SendSetSymbolToOthers() => photonView.RPC("SendSetSymbol", RpcTarget.Others);
        [PunRPC]
        public void SendSetSymbol() { symbolManager.isSymbolLoaded.Value = true; symbolManager.SetSymbols(); }

        public void SendCodeNameToOthers(string[] pickedNames) => photonView.RPC("SendCodeName", RpcTarget.Others, pickedNames);
        [PunRPC]
        public void SendCodeName(string[] pickedNames)
        {
            for (int i = 0; i < 3; i++)
            {
                symbolManager.pickedNames[i] = pickedNames[i];
            }

        }
        public void SendIconsSelectedToOthers(int i,int ID) => photonView.RPC("SendIconsSelected", RpcTarget.Others,i, ID);
        [PunRPC]
        public void SendIconsSelected(int i,int ID)
        {
            symbolManager.iconsSelected[i] = symbolManager.iconsStashed[ID];
        }
        public void SendOnOpenDoorToOther() => photonView.RPC("SendOnOpenDoor", RpcTarget.Others);
        [PunRPC]
        public void SendOnOpenDoor() => _onOpenDoor.Raise();
    }
}

