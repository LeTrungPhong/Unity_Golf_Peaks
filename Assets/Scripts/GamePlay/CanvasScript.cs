using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] private Button buttonHint;
    [SerializeField] private Image imageArrowHint;
    [SerializeField] private Button buttonReset;
    [SerializeField] private Image imageBreakHint;
    private bool checkUseHint = false;
    //[SerializeField] private Image imageArrowSelect;

    // Start is called before the first frame update
    void Start()
    {
        //imageArrowHint.rectTransform.DOMoveY(imageArrowHint.rectTransform.position.y + 50, 0.5f)
        //    .SetLoops(-1, LoopType.Yoyo)
        //    .SetEase(Ease.Linear);

        //imageArrowSelect.rectTransform.DOMoveX(imageArrowSelect.rectTransform.position.x + 50, 0.5f)
        //    .SetLoops(-1, LoopType.Yoyo)
        //    .SetEase(Ease.Linear);

        imageArrowHint.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HintArrowUp(Vector3 postHint, Vector3 trans, int MoveY)
    {
        imageArrowHint.gameObject.SetActive(true);
        imageArrowHint.gameObject.transform.position = postHint + trans;
        imageArrowHint.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        imageArrowHint.rectTransform.DOKill();
        imageArrowHint.rectTransform.DOMoveY(imageArrowHint.rectTransform.position.y + MoveY, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
        checkUseHint = true;
    }

    void HintArrowRight(Vector3 postHint, Vector3 trans, int MoveX)
    {
        imageArrowHint.gameObject.SetActive(true);
        imageArrowHint.gameObject.transform.position = postHint + trans;
        imageArrowHint.gameObject.transform.eulerAngles = new Vector3(0, 0, -90);
        imageArrowHint.rectTransform.DOKill();
        imageArrowHint.rectTransform.DOMoveX(imageArrowHint.rectTransform.position.x + MoveX, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
        checkUseHint = true;
    }

    public void HintToReset()
    {
        HintArrowUp(buttonReset.transform.position, - new Vector3(0, 150, 0), 50);
    }

    public void HintToMove(Vector3 postHint)
    {
        HintArrowRight(postHint, - new Vector3(200, 0, 0), 50);
    }

    public void HiddenHint()
    {
        if (checkUseHint == true)
        {
            imageArrowHint.gameObject.SetActive(false);
            imageBreakHint.gameObject.SetActive(true);
            buttonHint.gameObject.SetActive(false);
        }
    }
}
