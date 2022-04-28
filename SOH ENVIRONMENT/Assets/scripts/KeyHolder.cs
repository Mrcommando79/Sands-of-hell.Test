using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    [SerializeField] List<GameObject> keyImages;
    int keys;

    // Start is called before the first frame update
    void Start()
    {
        keys = 0;
    }

    public void AddKey()
    {
        keys++;
        DisplayKey();
    }

    void DisplayKey()
    {
        keyImages[keys-1].SetActive(true);
    }

    public int GetKeys()
    {
        return keys;
    }

}
