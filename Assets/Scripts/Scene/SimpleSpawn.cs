using UnityEngine;

public class SimpleSpawn : MonoBehaviour
{
    public GameObject obj;
    //public string spawnTag;
    public float time = 10f;
    public bool spawnOnce = false;

    private float timer = -1f;
    private bool wasSpawn = false;

    void Awake()
    {
        GameObject instance = Instantiate<GameObject>(obj);
        instance.transform.position = transform.position;
        if (instance.name == "Player(Clone)")
            instance.name = "Player";
        if (spawnOnce)
            wasSpawn = true;
    }

    void Update()
    {
        if (Time.time - timer > time && !wasSpawn)
        {
            timer = Time.time;
            GameObject instance = Instantiate<GameObject>(obj);
            instance.transform.position = transform.position;
        }
    }
}
