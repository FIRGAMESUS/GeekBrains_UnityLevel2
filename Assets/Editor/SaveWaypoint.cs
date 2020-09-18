using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(PathWaypoint))]
public class SaveWaypoint : Editor
{
    private static XmlSerializer _serializer;
    public List<SVect3> SavingNodes  = new List<SVect3>();

    public static string _directoryName;
    public static string _savingPath;
    public static string SceneName;



    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _serializer = new XmlSerializer(typeof(SVect3[]));

        PathWaypoint Wpath = (PathWaypoint)target;

        if(GUILayout.Button("Сохранить"))
        {
            SceneName = EditorSceneManager.GetActiveScene().name;
            _directoryName = "WaypointData";
            _savingPath = Path.Combine((Application.dataPath + "/" + _directoryName), "WaypointMap" + SceneName + ".xml");

            PlayerPrefs.SetString("Waypoint", _savingPath);
            PlayerPrefs.Save();

            if (Wpath.nodes.Count > 0)
            {
                foreach (Transform T in Wpath.nodes)
                {
                    if(!SavingNodes.Contains(T.position))
                    {
                        SavingNodes.Add(T.position);
                    }
                }
            }

            using (FileStream fs = new FileStream(_savingPath, FileMode.Create))
            {
                _serializer.Serialize(fs, SavingNodes.ToArray());
            }

        }

    }
}
