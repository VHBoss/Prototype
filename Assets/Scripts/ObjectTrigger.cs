using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    [SerializeField] private MonoBehaviour TriggerObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(TriggerObject is ITrigger trigger)
            {
                CharacterController characterController = other.GetComponent<CharacterController>();
                trigger.TriggerEnter(characterController);
            }
        }
    }
}
