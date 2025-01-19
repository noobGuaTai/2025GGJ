using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    HashSet<Collider2D> interact = new();
    string[] layers = { "DoorTrader", "Door" };
    void OnTriggerEnter2D(Collider2D other)
    {
        if (layers.ToList().Any(x => LayerMask.NameToLayer(x) == other.gameObject.layer))
        {
            interact.Add(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (interact.Contains(other))
            interact.Remove(other);
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.action.IsPressed())
        {
            interact.ToList().ForEach(x =>
            {
                if (x.gameObject.TryGetComponent<DoorTrader>(out var dt))
                {
                    dt.Trade();
                }
                if (x.gameObject.TryGetComponent<Door>(out var dr))
                {
                    dr.TryEnter();
                }
            });

        }
    }
}
