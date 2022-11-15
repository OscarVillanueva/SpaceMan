using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    public Dictionary<string, float> breaks;

    private void Awake()
    {
        // Agregamos un diccionario de breaks en caso de sea de tipo LevelBlock3
        if (gameObject.layer == 7)
        {
            breaks = new Dictionary<string, float>
            {
                { "StartPoint", -5.0f },
                { "IntermidatePoint", -5.0f },
                { "SecondPoint", -8.0f },
                { "DeepPoint", -12.5f },
                { "UpPoint", -10.0f },
                { "LastPoint", -7.0f },
                { "EndPoint", -1.0f }
            };
        }
    }

}
