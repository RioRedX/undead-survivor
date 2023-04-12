using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class jsonTest : MonoBehaviour
{
    public jsonStructDB jsonA;

    void Start()
    {
        string jsonData = Resources.Load("JSONs/Name").ToString();
        jsonA = JsonUtility.FromJson<jsonStructDB>("{\"Name\":" + jsonData + "}:");
    }

}

[System.Serializable]
public struct jsonStructDB
{
    public jsonStruct[] jsonStructs;
}
