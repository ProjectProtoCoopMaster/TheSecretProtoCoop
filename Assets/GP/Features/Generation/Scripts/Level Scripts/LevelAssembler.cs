using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public enum Platform { Mobile, VR }

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

        public List<RoomManager> selectedRooms { get; private set; }

        public void BuildLevel()
        {
            if (platform == Platform.VR) assembler = assemblerVR;
            else if (platform == Platform.Mobile) assembler = assemblerMobile;

            foreach (RoomManager _room in assembler.chunks) _room.StartRoom();

            assembler.SetLevel();

            foreach (RoomManager _room in assembler.pickedRooms) _room.room.parent.gameObject.SetActive(false);

            assembler.CreateLevel();

            LevelManager.instance.StartLevel();
        }
    }

    [System.Serializable]
    public abstract class Assembler
    {
        public List<RoomManager> chunks;

        public LevelVariable levelHolder;
        public List<RoomManager> pickedRooms { get; protected set; } = new List<RoomManager>();

        public void SetLevel()
        {
            foreach (RoomManager _room in chunks)
            {
                foreach (RoomData roomData in levelHolder.LevelRooms)
                {
                    if (_room.room.roomName == roomData.roomName)
                    {
                        _room.room.roomModifier = roomData.roomModifier;
                        pickedRooms.Add(_room);
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
        public Transform LevelParent;

        public override void CreateLevel()
        {
            LevelManager.instance.levelRooms.Clear();

            Transform currentAnchor = levelEntranceAnchor;

            RoomVR indexRoomVR;

            for (int i = 0; i < pickedRooms.Count; i++)
            {
                indexRoomVR = (RoomVR)pickedRooms[i].room;

                Vector3 translation = currentAnchor.position - indexRoomVR.entranceAnchor.localPosition;
                indexRoomVR.parent.gameObject.transform.position = translation;

                float angle = currentAnchor.rotation.eulerAngles.y - indexRoomVR.entranceAnchor.localRotation.eulerAngles.y;
                indexRoomVR.parent.gameObject.transform.RotateAround(currentAnchor.position, Vector3.up, angle);

                indexRoomVR.parent.gameObject.transform.parent = LevelParent;

                LevelManager.instance.levelRooms.Add(pickedRooms[i]);

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
        }
    }
}
