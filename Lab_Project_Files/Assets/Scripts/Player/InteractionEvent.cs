using UnityEngine;
using UnityEngine.Events;

public class InteractionEvent : MonoBehaviour
{
    public UnityEvent evento;

    public void Interact()
    {
        evento.Invoke();
    }
}
