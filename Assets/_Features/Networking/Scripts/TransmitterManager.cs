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
        [SerializeField] private Vector3Variable playerVRPosition;
        [SerializeField] private CallableFunction _switch;
        [SerializeField] private GameEvent _onLose;
        [SerializeField] private GameEvent _onOpenDoor;
        public SymbolManager symbolManager;

        public void SendPlayerVRPosToOthers(Vector3Variable playerVRPosition) => photonView.RPC("SendVector3", RpcTarget.Others, playerVRPosition.Value);
        [PunRPC] private void SendVector3(Vector3 position) => playerVRPosition.Value = position;

        public void SendSwicherChangeToOthers(float ID) => photonView.RPC("SendSwitcherChange", RpcTarget.Others, ID);
        [PunRPC] public void SendSwitcherChange(float ID) => _switch.Raise(ID);
        public void SendLoseToOther() => photonView.RPC("SendLose", RpcTarget.Others);
        [PunRPC] public void SendLose() => _onLose.Raise();

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

