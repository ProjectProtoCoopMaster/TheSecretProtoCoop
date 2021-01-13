using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public enum Platform { Mobile, VR }

    public class LevelAssembler : MonoBehaviour
    {
        public Platform platform;

        [ShowIf("platform", Platform.VR)]
        public AssemblerVR assemblerVR;
        [ShowIf("platform", Platform.Mobile)]
        public AssemblerMobile assemblerMobile;

        public Assembler assembler { get; private set; }

        public List<RoomManager> selectedRooms { get; private set; }

        void Start()
        {
            if (platform == Platform.VR) assembler = assemblerVR;
            else if (platform == Platform.Mobile) assembler = assemblerMobile;

            assembler.SetLevel();

            foreach (RoomManager room in assembler.pickedRooms) room.roomParent.gameObject.SetActive(false);

            assembler.CreateLevel();
        }
    }

    [System.Serializable]
    public abstract class Assembler
    {
        public List<RoomManager> chunks;

        public LevelVariable levelHolder;
        public List<RoomManager> pickedRooms { get; protected set; }

        public void SetLevel()
        {
            foreach (RoomManager room in chunks)
            {
                foreach (RoomData roomData in levelHolder.LevelRooms)
                {
                    if (room.roomName == roomData.roomName)
                    {
                        room.roomModifier = roomData.roomModifier;
                        pickedRooms.Add(room);
                    }
                }
            }
        }

        public abstract void CreateLevel();
    }

    public class AssemblerVR : Assembler
    {
        public Transform levelEntranceAnchor;
        public Transform LevelParent;

        public override void CreateLevel()
        {
            LevelManager.instance.levelRooms.Clear();

            Transform currentAnchor = levelEntranceAnchor;

            for (int i = 0; i < pickedRooms.Count; i++)
            {
                Vector3 translation = currentAnchor.position - pickedRooms[i].entranceAnchor.localPosition;
                pickedRooms[i].roomParent.gameObject.transform.position = translation;

                float angle = currentAnchor.rotation.eulerAngles.y - pickedRooms[i].entranceAnchor.localRotation.eulerAngles.y;
                pickedRooms[i].roomParent.gameObject.transform.RotateAround(currentAnchor.position, Vector3.up, angle);

                pickedRooms[i].roomParent.gameObject.transform.parent = LevelParent;

                LevelManager.instance.levelRooms.Add(pickedRooms[i]);

                currentAnchor = pickedRooms[i].exitAnchor;
            }
        }
    }
    public class AssemblerMobile : Assembler
    {
        public override void CreateLevel()
        {
            throw new System.NotImplementedException();
        }
    }
}
