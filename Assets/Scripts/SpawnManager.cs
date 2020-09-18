using System.Collections;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance = null;
    private static XmlSerializer _serializer;

    [SerializeField] private GameObject BotPrefab;

    private string _savingPath;
    private int EnemyCounter;
    private int MaxEnemyC = 6;
    
    IEnumerator SpawnObject(int counter)
    {
        while (counter > EnemyCounter)
        {
            yield return new WaitForSeconds(Random.Range(1, 3));
            GameObject temp = Create(BotPrefab);
            EnemyCounter++;
            Bot tBot = temp.GetComponent<Bot>();

            SVect3[] result;
            using (FileStream fs = new FileStream(_savingPath, FileMode.Open))
            {
                result = (SVect3[])_serializer.Deserialize(fs);
            }

            foreach (Vector3 position in result)
            {
                tBot.WayPoints.Add(position);
            }
        }
    }
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        _savingPath = PlayerPrefs.GetString("Waypoint");
        _serializer = new XmlSerializer(typeof(SVect3[]));

      
    }


    GameObject Create(GameObject prefab)
    {
        float x = Random.Range(-30, 30);
        float z = Random.Range(-30, 30);
        Vector3 pos = new Vector3(x, 5f, z);

        GameObject temp = Instantiate(prefab, pos, Quaternion.identity);
        return temp;
    }

    void Start()
    {
        StartCoroutine(SpawnObject(MaxEnemyC));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
