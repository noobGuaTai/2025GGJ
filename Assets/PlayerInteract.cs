using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    HashSet<Collision2D> interact;
    string[] layers = { "DoorTrader", "Door" };
    void OnCollisionEnter2D(Collision2D other)
    {
        if(layers.ToList().Any(x => LayerMask.NameToLayer(x) == other.gameObject.layer)){
            interact.Add(other);
        }
    }
    public void OnInteract(InputAction.CallbackContext context){
        if(context.action.IsPressed()){
            interact.ToList().ForEach(x =>
            {
                if( x.gameObject.TryGetComponent<DoorTrader>(out var dt)){
                    dt.Trade();
                }
                if( x.gameObject.TryGetComponent<Door>(out var dr)){
                    dr.TryEnter();
                }
            });

        }   
    }
}
