using UnityEngine;

public class Interaction 
{
    public InteractionType type;
    public GameObject interactiveObject;

    public Interaction(InteractionType type, GameObject interactiveObject)
    {
        this.type = type;
        this.interactiveObject = interactiveObject;
    }
}
