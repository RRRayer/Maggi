using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagChanger : MonoBehaviour
{
    public void ChangeTag(string newTag)
    {
        gameObject.tag = newTag;
    }
}
