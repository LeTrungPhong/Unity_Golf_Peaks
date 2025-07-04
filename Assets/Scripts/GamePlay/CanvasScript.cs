using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] private Button buttonHint;
    [SerializeField] private Image imageArrowHint;
    private RectTransform rectImageArrow;
    [SerializeField] private Button buttonReset;
    [SerializeField] private Image imageBreakHint;

    private GameManager gameManager;
    private bool checkUseHint = false;
    //[SerializeField] private Image imageArrowSelect;

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").gameObject.GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //imageArrowHint.rectTransform.DOMoveY(imageArrowHint.rectTransform.position.y + 50, 0.5f)
        //    .SetLoops(-1, LoopType.Yoyo)
        //    .SetEase(Ease.Linear);

        //imageArrowSelect.rectTransform.DOMoveX(imageArrowSelect.rectTransform.position.x + 50, 0.5f)
        //    .SetLoops(-1, LoopType.Yoyo)
        //    .SetEase(Ease.Linear);

        rectImageArrow = imageArrowHint.GetComponent<RectTransform>();

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

    void HintArrowDown(Vector3 postHint, Vector3 trans, int MoveY)
    {
        imageArrowHint.gameObject.SetActive(true);
        imageArrowHint.gameObject.transform.position = postHint + trans;
        imageArrowHint.gameObject.transform.eulerAngles = new Vector3(0, 0, 180);
        imageArrowHint.rectTransform.DOKill();
        imageArrowHint.rectTransform.DOMoveY(imageArrowHint.rectTransform.position.y + MoveY, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
        checkUseHint = true;
    }

    public void HintToReset()
    {
        HintArrowUp(buttonReset.transform.position, - new Vector3(0, 200, 0), 50);
    }

    public void HintToMove(Vector3 postHint)
    {
        HintArrowDown(postHint, new Vector3(-gameManager.widthButton / 2 + rectImageArrow.sizeDelta.x / 2, +gameManager.heigtButton + rectImageArrow.sizeDelta.y / 2 + 200, 0), -50);
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

    public void DisplayHint()
    {
        imageArrowHint.gameObject.SetActive(false);
        imageBreakHint.gameObject.SetActive(false);
        buttonHint.gameObject.SetActive(true);
        checkUseHint = false;
    }

    public void HintToDirect()
    {
        
    }
}