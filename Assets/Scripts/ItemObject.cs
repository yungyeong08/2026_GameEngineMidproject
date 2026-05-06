using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemSO date;

    public int GetPoint()
    {
        return date.point;
    }
}
