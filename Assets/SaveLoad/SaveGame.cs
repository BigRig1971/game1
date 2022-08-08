using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace StupidHumanGames
{
    public class SaveGame : MonoBehaviour
    {
        [Header("Variables")]
        public List<GameObject> targets = new List<GameObject>();
        public List<GameObject> spawnedTargets = new List<GameObject>();
        public List<GameObject> loadedTargets = new List<GameObject>();


        Dictionary<string, List<string>> listMaster;
        public string saveObject;
        private void Awake()
        {
            listMaster = new Dictionary<string, List<string>>();
        }

        private void Start()
        {

            LoadGameFile();
            LoadSpawnedGameFile();
        }
        private void LateUpdate()
        {

        }
        void Cleanup()
        {
            spawnedTargets.RemoveAll(o => (o == null || o.Equals(null)));
            targets.RemoveAll(o => (o == null || o.Equals(null)));
        }
        public void RemoveId(string id)
        {

            //foreach (GameObject target in spawnedTargets)
            for (int t = spawnedTargets.Count - 1; t >= 0; t--)
            {
                //foreach(string i in GetIdsFromList(spawnedTargets))
                for (var i = 0; i < GetIdsFromList(spawnedTargets).Count; i++)
                {
                    if (id == GetIdsFromList(spawnedTargets)[i])
                    {
                        spawnedTargets.RemoveAt(i);

                    }
                }
            }

        }
        List<string> GetIdsFromList(List<GameObject> list)
        {
            List<string> l = new List<string>();
            foreach (GameObject go in list)
            {
                string id = go.GetComponent<SaveableObject>().goid.ToString();
                l.Add(id);
            }
            return l;
        }
        public void SpawnPrefab(GameObject go, Vector3 pos, Quaternion rot)
        {
            //saveObject = saveObj;
            // List<string> list = new List<string>();
            // list.Add(saveObj);
            GameObject obj = Instantiate(go);
            obj.name = go.name;
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            spawnedTargets.Add(obj);
            //CreateList(saveObj);
        }
        public void CreateList(string listName)
        {
            #region List Check

            var IO = listMaster.ContainsKey(listName);
            if (IO) { return; }
            else
            {
                listMaster.Add(listName, new List<string>());
                Debug.Log("list added: " + listName);
            }

            #endregion
        }
        public void SaveGameFile()
        {

            var save = CreateSaveGameObject();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
            bf.Serialize(file, save);
            file.Close();
        }
        public void SaveSpawnedGameFile()
        {
            Cleanup();

            var save = CreateSpawnedSaveGameObject();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.spawnedSave");
            bf.Serialize(file, save);
            file.Close();
        }
        private Save CreateSaveGameObject()
        {
            var save = new Save();
            for (int i = 0; i < targets.Count; i++)
            {

                var position = targets[i].transform.position;
                var rotation = targets[i].transform.rotation;
                var targetName = targets[i].name;

                Vector3Serializable serializableVector3 = new Vector3Serializable
                {
                    x = position.x,
                    y = position.y,
                    z = position.z
                };
                QuaternionSerializable serializableQuaternion = new QuaternionSerializable
                {
                    x = rotation.x,
                    y = rotation.y,
                    z = rotation.z,
                    w = rotation.w
                };
                Strings strings = new Strings
                {
                    name = targetName
                };

                save.targetPositions.Add(serializableVector3);
                save.targetRotation.Add(serializableQuaternion);
                save.names.Add(strings);
                //Debug.Log("saved: " + targets[i].name);
            }
            return save;
        }
        private Save CreateSpawnedSaveGameObject()
        {


            var save = new Save();
            for (int i = 0; i < spawnedTargets.Count; i++)
            {
                var position = spawnedTargets[i].transform.position;
                var rotation = spawnedTargets[i].transform.rotation;
                var targetName = spawnedTargets[i].name;

                Vector3Serializable serializableVector3 = new Vector3Serializable
                {
                    x = position.x,
                    y = position.y,
                    z = position.z
                };
                QuaternionSerializable serializableQuaternion = new QuaternionSerializable
                {
                    x = rotation.x,
                    y = rotation.y,
                    z = rotation.z,
                    w = rotation.w
                };
                Strings strings = new Strings
                {
                    name = targetName
                };

                save.targetPositions.Add(serializableVector3);
                save.targetRotation.Add(serializableQuaternion);
                save.names.Add(strings);
                //Debug.Log("saved: " + targets[i].name);
            }
            return save;
        }
        public void LoadGameFile()
        {
            if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
                Save save = (Save)bf.Deserialize(file);
                file.Close();

                for (var i = 0; i < save.targetPositions.Count; i++)
                {

                    Vector3 vector3 = new Vector3()
                    {
                        x = save.targetPositions[i].x,
                        y = save.targetPositions[i].y,
                        z = save.targetPositions[i].z
                    };
                    Quaternion quaternion = new Quaternion()
                    {
                        x = save.targetRotation[i].x,
                        y = save.targetRotation[i].y,
                        z = save.targetRotation[i].z,
                        w = save.targetRotation[i].w
                    };
                    Strings targetNames = new Strings()
                    {
                        name = save.names[i].name
                    };
                    targets[i].transform.localPosition = vector3;
                    targets[i].transform.localRotation = quaternion;
                }

            }
            else
            {
                Debug.Log("No game saved!");
            }
        }

        public void LoadSpawnedGameFile()
        {

            List<GameObject> gameObjects = new List<GameObject>();
            // targets.Clear();
            if (File.Exists(Application.persistentDataPath + "/gamesave.spawnedSave"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/gamesave.spawnedSave", FileMode.Open);
                Save save = (Save)bf.Deserialize(file);
                file.Close();

                for (var i = 0; i < save.targetPositions.Count; i++)
                {

                    Vector3 vector3 = new Vector3()
                    {
                        x = save.targetPositions[i].x,
                        y = save.targetPositions[i].y,
                        z = save.targetPositions[i].z
                    };
                    Quaternion quaternion = new Quaternion()
                    {
                        x = save.targetRotation[i].x,
                        y = save.targetRotation[i].y,
                        z = save.targetRotation[i].z,
                        w = save.targetRotation[i].w
                    };
                    Strings targetName = new Strings()
                    {
                        name = save.names[i].name
                    };

                    // Debug.Log("loaded: " + targetNames.name);
                    GameObject go = Resources.Load("SaveablePrefabs/" + targetName.name) as GameObject;
                    SpawnPrefab(go, vector3, quaternion);
                    loadedTargets.Add(go);
                }


            }
            else
            {
                Debug.Log("No game saved!");
            }
        }
        string GetId(GameObject go)
        {
            string id = go.GetComponent<SaveableObject>().goid.ToString();

            return id;
        }

        private void OnApplicationQuit()
        {
            SaveGameFile();
            SaveSpawnedGameFile();
        }

    }
}
