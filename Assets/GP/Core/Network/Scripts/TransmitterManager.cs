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

        public GameManager gameManager;

        [SerializeField] private BoolVariable _isMobile;

        [Title("Win/Lose")]
        [SerializeField] private GameEvent _onLose;
        [SerializeField] private GameEvent _onWin;

        [Title("Player")]
        [SerializeField] private Vector3Variable _playerVRPosition;
        [SerializeField] private QuaternionVariable _playerVRRotation;

        [Title("Elements")]
        [SerializeField] private CallableFunction _switch;
        [SerializeField] private CallableFunction _destroyJammer;

        [Title("Modifiers")]
        [SerializeField] private BoolVariable _shake;
        [SerializeField] private BoolVariable _hidePlayer;
        [SerializeField] private GameEvent _onHidePlayer;
        [SerializeField] private FloatVariable _oxygenTimer;

        [Title("Level Generation")]
        [SerializeField] private LevelVariable _levelHolder;
        [SerializeField] private GameEvent _buildLevel;

        [Title("Level Management")]
        [SerializeField] private GameEvent _changeRoomMobile;
        [SerializeField] private GameEvent _onLevelRestart;
        [SerializeField] private CallableFunction _loadNextScene;
        [SerializeField] private CallableFunction _loadSameScene;
        [SerializeField] private CallableFunction _loadMainMenu;

        [Title("Symbol Door")]
        public SymbolManager symbolManager;
        [SerializeField] private GameEvent _onOpenDoor;
        [SerializeField] private GameEvent _onResetCodes;

        public static TransmitterManager instance;

        private void Awake() => instance = this;

        #region Game Management

        [Button]
        public void SendLoseToAll(int loseType) { photonView.RPC("SendLose", RpcTarget.AllViaServer, loseType); }
        [PunRPC] private void SendLose(int loseType) { gameManager.loseType = (LoseType)loseType; _onLose.Raise(); }

        public void SendWinToAll() => photonView.RPC("SendWin", RpcTarget.AllViaServer);
        [PunRPC] private void SendWin() => _onWin.Raise();

        public void SendRestartToAll() => photonView.RPC("SendRestart", RpcTarget.AllViaServer);
        [PunRPC] private void SendRestart() { SendLoadSameSceneToAll(); }

        public void SendLoadNextSceneToAll() => photonView.RPC("SendLoadNextScene", RpcTarget.AllViaServer);
        [PunRPC] private void SendLoadNextScene() { _loadNextScene.Raise(); }
        public void SendLoadSameSceneToAll() => photonView.RPC("SendLoadSameScene", RpcTarget.AllViaServer);
        [PunRPC] private void SendLoadSameScene() { _loadSameScene.Raise(); }

        public void SendLoadMainMenuToAll() => photonView.RPC("SendLoadMainMenu", RpcTarget.AllViaServer);
        [PunRPC] private void SendLoadMainMenu() { _loadMainMenu.Raise(); }
        #endregion

        #region Player Position
        public void SendPlayerVRPosAndRotToOthers() => photonView.RPC("SendPosAndRot", RpcTarget.Others, _playerVRPosition.Value, _playerVRRotation.Value);
        [PunRPC] private void SendPosAndRot(Vector3 position, Quaternion rotation) { _playerVRPosition.Value = position; _playerVRRotation.Value = rotation; }
        #endregion

        #region Switcher
        public void SendSwicherChangeToOthers(float ID) => photonView.RPC("SendSwitcherChange", RpcTarget.Others, ID);
        [PunRPC] private void SendSwitcherChange(float ID) => _switch.Raise(ID);
        #endregion

        #region Jammer
        public void SendDestroyJammerToOthers(int ID) => photonView.RPC("SendDestroyJammer", RpcTarget.Others, ID);
        [PunRPC] private void SendDestroyJammer(int ID) => _destroyJammer.Raise(ID);
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
            if (!_isMobile.Value) _onResetCodes.Raise();
        }

        public void SendIconsSelectedToOthers(int i, int ID) => photonView.RPC("SendIconsSelected", RpcTarget.Others, i, ID);
        [PunRPC] private void SendIconsSelected(int i, int ID) => symbolManager.iconsSelected[i] = symbolManager.iconsStashed[ID];

        public void SendOnOpenDoorToOther() => photonView.RPC("SendOnOpenDoor", RpcTarget.Others);
        [PunRPC] private void SendOnOpenDoor() => _onOpenDoor.Raise();
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
        public void SendLevelHolderToOthers(LevelVariable levelVariable)
        {
            photonView.RPC("InitLevelHolder", RpcTarget.Others, levelVariable.LevelRoomsData.Count);

            for (int i = 0; i < levelVariable.LevelRoomsData.Count; i++)
            {
                photonView.RPC("SendRoomName", RpcTarget.Others, levelVariable.LevelRoomsData[i].roomName, i);
                photonView.RPC("SendRoomModifier", RpcTarget.Others, levelVariable.LevelRoomsData[i].roomModifier, i);
            }
        }
        [PunRPC] private void InitLevelHolder(int size)
        {
            _levelHolder.LevelRoomsData = new List<RoomData>();
            for (int i = 0; i < size; i++) _levelHolder.LevelRoomsData.Add(new RoomData());
        }
        [PunRPC] private void SendRoomName(string name, int index) { Debug.Log("Send Room Name"); _levelHolder.LevelRoomsData[index].roomName = name; }
        [PunRPC] private void SendRoomModifier(ModifierType modifier, int index) { Debug.Log("Send Room Modifier"); _levelHolder.LevelRoomsData[index].roomModifier = modifier; }

        public void SendBuildLevelToAll() => photonView.RPC("SendBuildLevel", RpcTarget.All);
        [PunRPC] private void SendBuildLevel() => _buildLevel.Raise();

        public void SendRoomChangeToOthers() => photonView.RPC("SendRoomChange", RpcTarget.Others);
        [PunRPC] private void SendRoomChange() => _changeRoomMobile.Raise();
        #endregion
    }
}
