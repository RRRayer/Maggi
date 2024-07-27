using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foryoung : MonoBehaviour
{
    public List<GameObject> gameobjects;
    public Player player;

    private void Update()
    {
        Vector3 nextPos = player.transform.position;
        foreach (GameObject obj in gameobjects)
        {
            Vector3 pos = nextPos;
            pos.y = obj.transform.position.y;
            obj.transform.position = pos;
        }
    }
}
