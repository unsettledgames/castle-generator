using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTileOnAwake : MonoBehaviour
{
    public string path;
    public string[] tile;
    public string direction;
    public bool instantiateIfLast = false;
    public bool isLast = false;
    public float probability = 100;

    private Vector3 instantiationPos;
    private string toInstantiate;
    private bool called = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (name.Contains("Grass"))
        {
            Debug.Log("UOOOOO");
        }
        
        Init();   
    }

    private void Start()
    {
        if (!called)
        {
            Init();
        }
    }

    private void OnEnable()
    {
        if (!called)
        {
            Init();
        }
    }

    public void Init()
    {
        toInstantiate = path + tile[Random.Range(0, tile.Length)];
        direction = direction.ToLower();
        called = true;

        switch (direction)
        {
            case "top":
                instantiationPos = new Vector3(transform.position.x, transform.position.y + 1);
                break;
            case "bottom":
                instantiationPos = new Vector3(transform.position.x, transform.position.y - 1);
                break;
            case "left":
                instantiationPos = new Vector3(transform.position.x - 1, transform.position.y);
                break;
            case "right":
                instantiationPos = new Vector3(transform.position.x + 1, transform.position.y);
                break;
            default:
                instantiationPos = transform.position;
                break;
        }

        if (!instantiateIfLast && Random.Range(0, 100) <= probability)
        {
            Instantiate();
        }
    }

    public void Instantiate()
    {
        GameObject instantiated = Instantiate(
                (GameObject)Resources.Load(toInstantiate),
                instantiationPos,
                Quaternion.Euler(Vector3.zero)
            );
        instantiated.transform.parent = this.transform;
    }
}
