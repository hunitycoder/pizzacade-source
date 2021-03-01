using DG.Tweening;
using UnityEngine.UI;
using Blastproof.Systems.Core;
using Sirenix.OdinInspector;

public class UIBehaviour_Fader : UIBehaviour_Component
{
    private Image img;
    private Image Img => img ?? (img = GetComponentInChildren<Image>(true));

    [Button]
    public override void Activated()
    {
        Img.gameObject.SetActive(true);
        Img.color = Img.color.WithAlpha(0);
        Img.DOFade(.5f, .33f);
        Img.raycastTarget = true;
    }

    [Button]
    public override void Deactivated()
    {
        Img.DOFade(0f, .33f).OnComplete(() => Img.gameObject.SetActive(false));
        Img.raycastTarget = false;
    }

    [Button]
    private void DisableGO()
    {
        Img.gameObject.SetActive(false);
    }
}
