using Maggi.StateMachine.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

//public enum InteractionType { General, NonPossession, Possession, 
//    None, Light, Heavy, Wall, Point, SieveWheel, Globe, Normal, Key};

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;

    // Events for the different interaction types
    [ReadOnly] public InteractionType currentInteractionType;
    [ReadOnly] public GameObject currentInteractiveObject;
    [ReadOnly] public bool pullInput = false;
    [ReadOnly] public bool pushInput = false;

    private LinkedList<Interaction> _potentialInteractions = new LinkedList<Interaction>(); // To store the objects we the player could potentially interact with

    private void OnEnable()
    {
        _inputReader.PullEvent += OnPullInitiated;
        _inputReader.PullCancelEvent += OnPullCancelInitiated;
        _inputReader.PushEvent += OnPushInitiated;
        _inputReader.PushCancelEvent += OnPushCancelInitiated;
    }

    private void OnDisable()
    {
        _inputReader.PullEvent -= OnPullInitiated;
        _inputReader.PullCancelEvent -= OnPullCancelInitiated;
        _inputReader.PushEvent -= OnPushInitiated;
        _inputReader.PushCancelEvent -= OnPushCancelInitiated;
    }

    private void OnPullInitiated()
    {
        pullInput = true;

        if (_potentialInteractions.Count == 0 ) return;
        if (currentInteractiveObject != null) return;

        currentInteractionType = _potentialInteractions.First.Value.type;
        currentInteractiveObject = _potentialInteractions.First.Value.interactiveObject;

        // Pull Effect
        if (currentInteractiveObject.TryGetComponent(out ToggleEffect effect))
        {
            effect.ToggleMaterial(true);
        }
        else
        {
            //Debug.LogWarning("There is no ToggleEffect _ InteractionManager.cs");
        }
    }

    private void OnPullCancelInitiated()
    {
        pullInput = false;
    }

    private void OnPushInitiated()
    {
        pushInput = true;

        // Push Effect
        if (currentInteractiveObject != null)
        {
            if (currentInteractiveObject.TryGetComponent(out ToggleEffect effect))
            {
                effect.ToggleMaterial(false);
            }
            else
            {
                //Debug.LogWarning("There is no ToggleEffect _ InteractionManager.cs");
            }
        }
    }

    private void OnPushCancelInitiated()
    {
        pushInput = false;
    }

    public void OnTriggerChangeDetected(bool entered, GameObject obj)
    {
        if (entered)
            AddPotentialInteraction(obj);
        else
            RemovePotentialInteraction(obj);
    }

    private void AddPotentialInteraction(GameObject obj)
    {
        Interaction newPotentialInteraction = new Interaction(InteractionType.None, obj);

        if (obj.TryGetComponent(out InteractiveObject io))
        {
            newPotentialInteraction.type = io.m_Type;
        }

        if (newPotentialInteraction.type != InteractionType.None)
        {
            _potentialInteractions.AddFirst(newPotentialInteraction);
        }
    }

    private void RemovePotentialInteraction(GameObject obj)
    {
        LinkedListNode<Interaction> currentNode = _potentialInteractions.First;
        while (currentNode != null)
        {
            if (currentNode.Value.interactiveObject == obj)
            {
                _potentialInteractions.Remove(currentNode);
                break;
            }
            currentNode = currentNode.Next;
        }
    }
}
