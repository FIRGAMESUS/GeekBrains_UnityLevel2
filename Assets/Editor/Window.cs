using UnityEditor;
using UnityEngine;

public class Window : EditorWindow
{
    public GameObject botPref;
    public int objCounter;
    public float radius;

    [MenuItem("Инструменты/ Создание префабов/ Генератор Ботов")]
    public static void Show()
    {
        GetWindow(typeof(Window), false, "Генератор ботов");
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Настройка", EditorStyles.boldLabel);
        botPref = EditorGUILayout.ObjectField("Префаб бота", botPref, typeof(GameObject), true) as GameObject;
        objCounter = EditorGUILayout.IntSlider("Количество ботов", objCounter, 3, 200);
        radius = EditorGUILayout.Slider("Радиус", radius, 10, 100);
        EditorGUILayout.EndVertical();


        if (GUILayout.Button("Создать"))
        {
            if(botPref)
            {
                GameObject Main = new GameObject("Main");
                for (int i = 0; i < objCounter; i++)
                {
                    float angle = i * Mathf.PI * 2 / objCounter;
                    Vector3 pos = (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius);
                    GameObject temp = Instantiate(botPref, pos, Quaternion.identity);
                    temp.transform.parent = Main.transform;
                    temp.name = "Bot (" + i + ")";
                }
            }
        }

   
    }
}
