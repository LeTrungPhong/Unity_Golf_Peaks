﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SurfaceType
{
    Water,
    QuickSand,
    Conveyor,
    Portal,
    RippleWater,
    Wood,
    Jump,
    Conveyor_Tile,
    Ice
}

public class EffectSurfaceManager : MonoBehaviour
{
    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private GameObject effectSurfacePrefab;
    [SerializeField] private Texture effectWater;
    [SerializeField] private Texture effectQuickSand;
    [SerializeField] private Texture effectConveyor;
    [SerializeField] private Texture effectConveyor_Tile;
    [SerializeField] private Texture effectPortal;
    [SerializeField] private Texture effectRippleWater;
    [SerializeField] private Texture effectWood;
    [SerializeField] private Texture effectJump;
    [SerializeField] private Texture effectIce;

    public GameObject[][] effectGameObjectJump;

    // water
    float scrollSpeedX = -0.2f;
    float scrollSpeedY = -0.2f;

    // quick sand
    float scrollSpeedSandX = -0.1f;
    float scrollSpeedSandY = -0.1f;

    // conveyor
    private GameObject effectSurfaceConveyor;
    float scrollSpeedConveyorX = 0.5f;
    float scrollSpeedConveyorY = 0f;
    int directionEffectSurfaceConveyor = 0;

    // portal
    Vector2 scalePortal = new Vector2(1.75f, 1.75f);

    // ripple water
    private GameObject effectSurfaceRippleWater;
    Vector2 scaleDefault = new Vector2(1.0f, 1.0f);
    float scaleSpeedX = 0.2f;
    float scaleSpeedY = 0.2f;

    // jump
    Vector2 scaleJump = new Vector2(2.5f, 2.5f);

    // conveyor_tile
    float scrollSpeedConveyor_TileX = 1.0f;
    float scrollSpeedConveyor_TileY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnEffectSurface(SurfaceType type, int[] index, int numberPortal = 0)
    {
        GameObject effectSurface = SpawnByIndex(index);

        PlayEffect(effectSurface, type, numberPortal);

        if (type == SurfaceType.Jump)
        {
            effectGameObjectJump[index[0]][index[2]] = effectSurface;
        }
    }

    public void PlayEffect(GameObject effectSurface,SurfaceType type, int number = 0)
    {
        Material mat = new Material(Shader.Find("UI/Default"));

        effectSurface.GetComponent<Renderer>().material = mat;

        switch (type)
        {
            case SurfaceType.Water:
                mat.mainTexture = effectWater;
                mat.color = new Color(0f, 0f, 0f, 0.3f);
                StartCoroutine(ScrollTexture(effectSurface.GetComponent<Renderer>().material, scrollSpeedX, scrollSpeedY));
                break;
            case SurfaceType.QuickSand:
                mat.mainTexture = effectQuickSand;
                StartCoroutine(ScrollTexture(effectSurface.GetComponent<Renderer>().material, scrollSpeedSandX, scrollSpeedSandY));
                break;
            case SurfaceType.Conveyor:
                mat.mainTexture = effectConveyor;
                //mat.color = new Color(1f, 1f, 1f, 0.5f);
                mat.color = Color.red;
                if (directionEffectSurfaceConveyor != 0)
                {
                    effectSurface.transform.rotation = Quaternion.Euler(90, 90 * (directionEffectSurfaceConveyor - 1), 0);
                    StartCoroutine(ScrollTexture(effectSurface.GetComponent<Renderer>().material, scrollSpeedConveyorX, scrollSpeedConveyorY));
                }
                break;
            case SurfaceType.Conveyor_Tile:
                mat.mainTexture = effectConveyor;
                mat.color = new Color(1f, 1f, 1f, 0.5f);
                effectSurface.transform.rotation = Quaternion.Euler(90, 90 * (number - 1), 0);
                StartCoroutine(ScrollTexture(effectSurface.GetComponent<Renderer>().material, scrollSpeedConveyor_TileX, scrollSpeedConveyor_TileY));
                break;
            case SurfaceType.Portal:
                //effectPortal.colo
                mat.mainTexture = effectPortal;
                mat.mainTextureScale = scalePortal;
                mat.mainTextureOffset = (Vector2.one - scalePortal) / 2;
                mat.color = PickColor(number);
                break;
            case SurfaceType.RippleWater:
                mat.mainTexture = effectRippleWater;
                StartCoroutine(ScaleTexture(effectSurface.GetComponent<Renderer>().material, scaleSpeedX, scaleSpeedY));
                break;
            case SurfaceType.Wood:
                mat.mainTexture = effectWood;
                break;
            case SurfaceType.Jump:
                mat.mainTexture = effectJump;
                mat.mainTextureScale = scaleJump;
                mat.mainTextureOffset = (Vector2.one - scaleJump) / 2;
                break;
            case SurfaceType.Ice:
                mat.mainTexture = effectIce;
                mat.color = new Color(1f, 1f, 1f, 0.5f);
                break;
        }
    }
    Color PickColor(int index)
    {
        switch (index)
        {
            case 1:
                return Color.red;
            case 2:
                return Color.green;
            case 3:
                return Color.blue;
            case 4:
                return Color.yellow;
            case 5:
                return Color.grey;
            case 6:
                return Color.magenta;
            case 7:
                return Color.black;
            case 8:
                return Color.cyan;
        }
        return Color.white;
    }

    GameObject SpawnByIndex(int[] index)
    {
        return Instantiate(
            effectSurfacePrefab,
            new Vector3(index[0] * obstacleManager.obstacleSize, (index[1] - 1 + (float)1 / 2 + 0.01f) * obstacleManager.obstacleSizeY, index[2] * obstacleManager.obstacleSize),
            Quaternion.Euler(90, 0, 0));
    }

    IEnumerator ScrollTexture(Material mat, float speedX, float speedY)
    {
        Vector2 currentOffset = new Vector2();
        while (true)
        {
            currentOffset += new Vector2(speedX, speedY) * Time.deltaTime;
            mat.mainTextureOffset = currentOffset;
            yield return null;
        }
    }

    IEnumerator ScaleTexture(Material mat, float speedX, float speedY)
    {
        Vector2 currentScale = scaleDefault;
        while (true)
        {
            if (mat.GetTextureScale("_MainTex").x < 0.5f)
            {

            }

            currentScale -= new Vector2(scaleSpeedX, scaleSpeedY) * Time.deltaTime;
            mat.mainTextureScale = currentScale;
            mat.mainTextureOffset = (Vector2.one - currentScale) / 2;
            yield return null;
        }
    }

    public void PlayRippleWater(int[] index, int direction)
    {
        effectSurfaceRippleWater = Instantiate(
            effectSurfacePrefab,
            new Vector3(index[0] * obstacleManager.obstacleSize, (index[1] - 1 + (float)1 / 2 + 0.01f) * obstacleManager.obstacleSizeY, index[2] * obstacleManager.obstacleSize),
            Quaternion.Euler(90, 0, 0));
    }

    public void PlayEffectConveyor(int[] index, int direction)
    {
        directionEffectSurfaceConveyor = direction;

        effectSurfaceConveyor = Instantiate(
            effectSurfacePrefab,
            new Vector3(index[0] * obstacleManager.obstacleSize, (index[1] - 1 + (float)1 / 2 + 0.01f) * obstacleManager.obstacleSizeY, index[2] * obstacleManager.obstacleSize),
            Quaternion.Euler(90, 0, 0));

        PlayEffect(effectSurfaceConveyor, SurfaceType.Conveyor);
    }

    public void StopEffectConveyor()
    {
        Destroy(effectSurfaceConveyor);
    }

    public void PlayEffectJump(int indexX, int indexZ)
    {
        if (effectGameObjectJump != null)
        {
            Transform transEffect = effectGameObjectJump[indexX][indexZ].transform;
            effectGameObjectJump[indexX][indexZ].transform.DOMoveY(transEffect.position.y + obstacleManager.highBlockJump, obstacleManager.timeBlockJump)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InBounce);
        }
    }
}