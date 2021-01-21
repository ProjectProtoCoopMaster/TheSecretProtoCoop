using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class LevelAssembler : MonoBehaviour
    {
        public Platform platform;

        [ShowIf("platform", Platform.VR)]
        [Title("Assembler VR")]
        public AssemblerVR assemblerVR;

        [ShowIf("platform", Platform.Mobile)]
        [Title("Assembler Mobile")]
        public AssemblerMobile assemblerMobile;

        public Assembler assembler { get; private set; }

        public void BuildLevel()
        {
            if (platform == Platform.VR) assembler = assemblerVR;
            else if (platform == Platform.Mobile) assembler = assemblerMobile;

            assembler.PickRooms();

            assembler.CreateLevel();

            LevelManager.instance.StartLevel();
        }
    }

    [System.Serializable]
    public abstract class Assembler
    {
        public List<RoomManager> roomChunks;

        public LevelVariable levelHolder;
        public List<RoomManager> pickedRooms { get; protected set; } = new List<RoomManager>();

        public Transform LevelParent;

        public void PickRooms()
        {
            foreach (RoomManager roomManager in roomChunks)
            {
                roomManager.StartRoom();

                foreach (RoomData roomData in levelHolder.LevelRoomsData)
                {
                    if (roomManager.room.roomName == roomData.roomName)
                    {
                        roomManager.room.roomModifier = roomData.roomModifier;
                        pickedRooms.Add(roomManager);
                    }
                }
            }
        }

        public abstract void CreateLevel();
    }

    [System.Serializable]
    [HideLabel]
    public class AssemblerVR : Assembler
    {
        public Transform levelEntranceAnchor;
        
        public override void CreateLevel()
        {
            LevelManager.instance.levelRooms.Clear();
            LevelManager.instance.levelRooms = pickedRooms;

            Transform currentAnchor = levelEntranceAnchor;

            RoomVR indexRoomVR;

            for (int i = 0; i < pickedRooms.Count; i++)
            {
                indexRoomVR = (RoomVR)pickedRooms[i].room;

                Vector3 translation = currentAnchor.position - indexRoomVR.entranceAnchor.localPosition;
                indexRoomVR.roomHolder.position = translation;

                float angle = currentAnchor.rotation.eulerAngles.y - indexRoomVR.entranceAnchor.localRotation.eulerAngles.y;
                indexRoomVR.roomHolder.RotateAround(currentAnchor.position, Vector3.up, angle);

                indexRoomVR.roomHolder.parent = LevelParent;
                indexRoomVR.roomHolder.gameObject.SetActive(false);

                currentAnchor = indexRoomVR.exitAnchor;
            }
        }
    }

    [System.Serializable]
    [HideLabel]
    public class AssemblerMobile : Assembler
    {
        public override void CreateLevel()
        {
            LevelManager.instance.levelRooms.Clear();
            LevelManager.instance.levelRooms = pickedRooms;

            RoomMobile indexRoomMobile;

            for (int i = 0; i < pickedRooms.Count; i++)
            {
                indexRoomMobile = (RoomMobile)pickedRooms[i].room;

                indexRoomMobile.roomHolder.parent = LevelParent;
                indexRoomMobile.roomHolder.gameObject.SetActive(false);
            }
        }
    }
}
