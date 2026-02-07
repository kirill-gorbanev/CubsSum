using Code.UI;
using UnityEngine.EventSystems;

public class CollectItem : Prefab
{
    public override void OnEndDrag(PointerEventData eventData)
    {
        var b = Content?.FindCellReset(this, eventData.pointerEnter) ?? false;
        if (b)
            Content = null;
    }
}