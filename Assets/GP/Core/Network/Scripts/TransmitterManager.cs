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
        [SerializeField] private IntVariable _sceneID;
        [SerializeField] private QuaternionVariable _playerVRRotation;
        [SerializeField] private CallableFunction _switch;
        [SerializeField] private CallableFunction _destroyJammer;
        [SerializeField] private CallableFunction _loadNextLevel;
        [SerializeField] private BoolVariable _shake;
        [SerializeField] private BoolVariable _hidePlayer;
        [SerializeField] private BoolVariable _isMobile;
        [SerializeField] private FloatVariable _oxygenTimer;
        [SerializeField] private GameEvent _onHidePlayer;
        [SerializeField] private GameEvent _onOpenDoor;
        [SerializeField] private GameEvent _onVictory;
        [SerializeField] private GameEvent _onResetCodes;
        [SerializeField] private LevelVariable _levelHolder;
        [SerializeField] private GameEvent _buildLevel;

        [SerializeField] private GameManager gameManager;
        public SymbolManager symbolManager;

        public static TransmitterManager instance;
        private void Awake()
        {
            instance = this;

        }
        public void SendPlayerVRPosAndRotToOthers() => photonView.RPC("SendPosAndRot", RpcTarget.Others, _playerVRPosition.Value, _playerVRRotation.Value);


        [PunRPC] private void SendPosAndRot(Vector3 position, Quaternion rotation) { _playerVRPosition.Value = position; _playerVRRotation.Value = rotation; }



        public void SendSwicherChangeToOthers(float ID) => photonView.RPC("SendSwitcherChange", RpcTarget.Others, ID);
        [PunRPC] public void SendSwitcherChange(float ID) => _switch.Raise(ID);

        public void SendDestroyJammerToOthers(int ID) { Debug.Log("Jammer"); photonView.RPC("SendDestroyJammer", RpcTarget.Others, ID); }
        [PunRPC] public void SendDestroyJammer(int ID) => _destroyJammer.Raise(ID);
        public void SendLoseToOther(int loseType) { gameManager.RaiseOnLose(loseType); photonView.RPC("SendLose", RpcTarget.Others, loseType); }

        [PunRPC] public void SendLose(int loseType) { gameManager.RaiseOnLose(loseType);  }

        public void SendOnVictoryToOthers() { photonView.RPC("SendOnVictory", RpcTarget.Others); }

        [PunRPC] public void SendOnVictory() { _onVictory.Raise(); }

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
                if (!_isMobile.Value) _onResetCodes.Raise();
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

        public void SendLoadNextSceneToOthers() => photonView.RPC("SendLoadNextScene", RpcTarget.Others);
        [PunRPC]
        public void SendLoadNextScene() => gameManager.LoadNextScene();

        public void SendLoadSameSceneToOthers() {  photonView.RPC("SendLoadSameScene", RpcTarget.Others); }
        [PunRPC]
        public void SendLoadSameScene() { gameManager.LoadSameScene(); Debug.Log("LoadSameScene"); }

        public void SendShakeResultToOthers(bool check) => photonView.RPC("ShakeResult", RpcTarget.Others, check);
        [PunRPC] private void ShakeResult(bool complete) => _shake.Value = complete;

        public void SendHidePlayerToOthers(bool hidePlayer) => photonView.RPC("HidePlayer", RpcTarget.Others, hidePlayer);
        [PunRPC] private void HidePlayer(bool hidePlayer) { _hidePlayer.Value = hidePlayer; _onHidePlayer.Raise(); }

        public void SendOxygenTimerToOthers(float oxygenTimer) => photonView.RPC("SendOxygenTimer", RpcTarget.Others, oxygenTimer);
        [PunRPC] private void SendOxygenTimer(float oxygenTimer) => _oxygenTimer.Value = oxygenTimer;


        public void SendLevelHolderToOthers(LevelVariable levelVariable)
        {
            photonView.RPC("InitLevelHolder", RpcTarget.Others, levelVariable.LevelRooms.Count);

            for (int i = 0; i < levelVariable.LevelRooms.Count; i++)
            {
                photonView.RPC("SendRoomName", RpcTarget.Others, levelVariable.LevelRooms[i].roomName, i);
                photonView.RPC("SendRoomModifier", RpcTarget.Others, levelVariable.LevelRooms[i].roomModifier, i);
            }
        }
        [PunRPC] private void InitLevelHolder(int size)
        {
            _levelHolder.LevelRooms.Clear();
            _levelHolder.LevelRooms.Capacity = size;
        }
        [PunRPC] private void SendRoomName(string name, int index)
        {
            _levelHolder.LevelRooms[index].roomName = name;
        }
        [PunRPC] private void SendRoomModifier(ModifierType modifier, int index)
        {
            _levelHolder.LevelRooms[index].roomModifier = modifier;
        }

        public void SendBuildLevelToOthers() => photonView.RPC("SendBuildLevel", RpcTarget.Others);
        [PunRPC] private void SendBuildLevel() => _buildLevel.Raise();
    }
}

