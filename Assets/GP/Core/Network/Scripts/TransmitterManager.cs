using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Gameplay;
using Sirenix.OdinInspector;

namespace Networking
{
    public class TransmitterManager : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;

        [SerializeField] private BoolVariable _isMobile;

        [Title("Player")]
        [SerializeField] private Vector3Variable _playerVRPosition;
        [SerializeField] private QuaternionVariable _playerVRRotation;

        [Title("Elements")]
        public SwitcherManager switcherManager;
        public JammerManager jammerManager;

        [Title("Modifiers")]
        [SerializeField] private BoolVariable _shake;
        [SerializeField] private BoolVariable _hidePlayer;
        [SerializeField] private GameEvent _onHidePlayer;
        [SerializeField] private FloatVariable _oxygenTimer;

        [Title("Level Generation")]
        public LevelGenerator levelGenerator;
        [SerializeField] private LevelVariable _levelHolder;

        [Title("Symbol Door")]
        public SymbolManager symbolManager;

        public static TransmitterManager instance;
        void OnEnable() { if (instance == null) instance = this; }

        #region Game Management
        public void SendLoseToAll(int loseType) { photonView.RPC("SendLose", RpcTarget.AllViaServer, loseType); }
        [PunRPC] private void SendLose(int loseType) { GameManager.instance.Lose((LoseType)loseType); }

        public void SendWinToAll() => photonView.RPC("SendWin", RpcTarget.AllViaServer);
        [PunRPC] private void SendWin() { GameManager.instance.Win(); }

        public void SendRestartToAll() => photonView.RPC("SendRestart", RpcTarget.AllViaServer);
        [PunRPC] private void SendRestart() { LevelManager.instance.RestartLevel(); }

        public void SendLoadMainMenuToAll() => photonView.RPC("SendLoadMainMenu", RpcTarget.AllViaServer);
        [PunRPC] private void SendLoadMainMenu() { GameManager.instance.LoadMainMenu(); }
        #endregion

        #region Player Position
        public void SendPlayerVRPosAndRotToOthers() => photonView.RPC("SendPosAndRot", RpcTarget.Others, _playerVRPosition.Value, _playerVRRotation.Value);
        [PunRPC] private void SendPosAndRot(Vector3 position, Quaternion rotation) { _playerVRPosition.Value = position; _playerVRRotation.Value = rotation; }
        #endregion

        #region Switcher
        public void SendSwicherChangeToOthers(float ID) => photonView.RPC("SendSwitcherChange", RpcTarget.Others, ID);
        [PunRPC] private void SendSwitcherChange(float ID) { Debug.Log("Switch Changed with ID :" + ID); switcherManager.RaiseSwitch(ID); }
        #endregion

        #region Jammer
        public void SendDestroyJammerToOthers(int ID) => photonView.RPC("SendDestroyJammer", RpcTarget.Others, ID);
        [PunRPC] private void SendDestroyJammer(int ID) { jammerManager.DestroyJammer(ID); }
        #endregion

        #region Symbol Door
        public void SendSymbolIDToOther(int value) => photonView.RPC("SendSymbolID", RpcTarget.Others, value);
        [PunRPC] private void SendSymbolID(int value) => symbolManager.indexes.Add(value);

        public void SendSetSymbolToOthers() => photonView.RPC("SendSetSymbol", RpcTarget.Others);
        [PunRPC] private void SendSetSymbol() { symbolManager.isSymbolLoaded.Value = true; symbolManager.SetSymbols(); }

        public void SendCodeNameToOthers(string[] pickedNames) => photonView.RPC("SendCodeName", RpcTarget.Others, pickedNames);
        [PunRPC]
        private void SendCodeName(string[] pickedNames)
        {
            for (int i = 0; i < 3; i++) { symbolManager.pickedNames[i] = pickedNames[i]; }
            if (!_isMobile.Value) symbolManager.onResetCodes.Raise();
        }

        public void SendIconsSelectedToOthers(int i, int ID) => photonView.RPC("SendIconsSelected", RpcTarget.Others, i, ID);
        [PunRPC] private void SendIconsSelected(int i, int ID) => symbolManager.iconsSelected[i] = symbolManager.iconsStashed[ID];

        public void SendOnOpenDoorToOther() => photonView.RPC("SendOnOpenDoor", RpcTarget.Others);
        [PunRPC] private void SendOnOpenDoor() => symbolManager.onOpenDoor.Raise();
        #endregion

        #region Modifiers
        public void SendShakeResultToOthers(bool check) => photonView.RPC("ShakeResult", RpcTarget.Others, check);
        [PunRPC] private void ShakeResult(bool check) => _shake.Value = check;

        public void SendHidePlayerToOthers(bool hidePlayer) => photonView.RPC("HidePlayer", RpcTarget.Others, hidePlayer);
        [PunRPC] private void HidePlayer(bool hidePlayer) { _hidePlayer.Value = hidePlayer; _onHidePlayer.Raise(); }

        public void SendOxygenTimerToOthers(float oxygenTimer) => photonView.RPC("SendOxygenTimer", RpcTarget.Others, oxygenTimer);
        [PunRPC] private void SendOxygenTimer(float oxygenTimer) => _oxygenTimer.Value = oxygenTimer;
        #endregion

        #region Level
        public void SendBuildLevelToOther(LevelVariable levelVariable)
        {
            Debug.Log("Send Level Holder VR");
            foreach(RoomData roomData in levelVariable.LevelRoomsData) { Debug.Log("with room :" + roomData.roomName + "and modifier :" + roomData.roomModifier); }

            string[] _names = new string[levelVariable.LevelRoomsData.Count];
            int[] _types = new int[levelVariable.LevelRoomsData.Count];

            for (int i = 0; i < levelVariable.LevelRoomsData.Count; i++)
            {
                _names[i] = levelVariable.LevelRoomsData[i].roomName;
                _types[i] = (int)levelVariable.LevelRoomsData[i].roomModifier;
            }

            photonView.RPC("SendBuildLevel", RpcTarget.Others, levelVariable.LevelRoomsData.Count, _names as object, _types as object);
        }
        [PunRPC] private void SendBuildLevel(int size, string[] names, int[] modifierTypes)
        {
            Debug.Log("Send Level Holder Mobile");

            _levelHolder.LevelRoomsData = new List<RoomData>();
            for (int i = 0; i < size; i++)
            {
                Debug.Log("with room :" + names[i] + "and modifier :" + (ModifierType)modifierTypes[i]);
                _levelHolder.LevelRoomsData.Add(new RoomData { roomName = names[i], roomModifier = (ModifierType)modifierTypes[i] });
            }

            LevelManager.instance.BuildLevel();
        }

        public void SendRoomChangeToAll() => photonView.RPC("SendRoomChange", RpcTarget.AllViaServer);
        [PunRPC] private void SendRoomChange() { LevelManager.instance.ChangeRoom(); }
        #endregion
    }
}
