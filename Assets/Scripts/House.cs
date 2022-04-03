using DG.Tweening;
using UnityEngine;

public class House : MonoBehaviour, ITrigger
{
    private CharacterController m_Character;

    public void TriggerEnter(CharacterController character)
    {
        m_Character = character;
        InvokeRepeating("Build", 0, 0.1f);
    }

    public void TriggerExit(CharacterController characterController)
    {
        CancelInvoke();
    }

    private void Build()
    {
        Transform ice = m_Character.Bag.GetIce();
        if (ice != null)
        {
            Transform brick = GetNextBrick();
            if(brick != null)
            {
                ice.SetParent(null);
                ice.DOMove(brick.position, 0.2f);
                ice.DORotate(brick.eulerAngles, 0.2f).OnComplete(() =>
                {
                    brick.gameObject.SetActive(true);
                    ice.gameObject.SetActive(false);
                });
            }
            else
            {
                CancelInvoke();
            }
        }
        else
        {
            CancelInvoke();
        }
    }

    private Transform GetNextBrick()
    {
        foreach (Transform item in transform)
        {
            if (!item.CompareTag("Finish"))
            {
                item.tag = "Finish";
                return item;
            }
        }
        return null;
    }
}
