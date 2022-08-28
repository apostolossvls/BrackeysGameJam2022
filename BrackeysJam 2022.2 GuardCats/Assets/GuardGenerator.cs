using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GenerateGuardMode
{
    HoldAndRelease = 0,
    ClickTwice = 1,
    AutoToClickRelease = 2,
    HoldAndReleaseProtectFirst = 3
}

public enum AimGuardMode
{
    Clock = 0,
    Mouse = 1
}

public class GuardGenerator : MonoBehaviour
{
    public Transform previewPivot;
    public Transform previewTransform;
    public ParticleSystem previewCreatePs;
    public SpriteRenderer[] previewSpritesBlocked;
    public SpriteRenderer[] previewSprites;
    public Image[] energyBars;
    public Transform guardsParent;
    public LayerMask raycastGuards;
    public GameObject[] guardOriginal;
    public float previewRotationSpeed = 5f;
    public int[] ringsCounts = new int[4];
    bool canFit;
    public float energy = 0;
    public float energyRate = 1;
    public float[] energyPerGuard = new float[1] {3f};
    public bool showFirstEnergy = true;
    public GenerateGuardMode generateGuardMode = 0;
    public AimGuardMode aimGuardMode = 0;
    bool clickedOnce = false;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        canFit = true;
        energy = 0;
        clickedOnce = false;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.onGame) return;

        if (aimGuardMode == AimGuardMode.Clock)
        {
            previewPivot.rotation *= Quaternion.Euler(0, 0, previewRotationSpeed * Time.deltaTime);
        }
        else if (aimGuardMode == AimGuardMode.Mouse)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            previewPivot.right = (mousePos - (Vector2)previewPivot.position).normalized;
            previewPivot.localRotation *= Quaternion.Euler(0, 0, -90);
        }

        if (generateGuardMode == GenerateGuardMode.HoldAndRelease)
        {
            if (Input.GetMouseButton(0)) energy += energyRate * Time.deltaTime;
            else if (!Input.GetMouseButtonUp(0)) energy = 0;
        }
        else if (generateGuardMode == GenerateGuardMode.ClickTwice)
        {
            if (clickedOnce)
            {
                energy += energyRate * Time.deltaTime;
            }
            if (Input.GetMouseButtonUp(0)){
                clickedOnce = !clickedOnce;
            }
        }
        else if (generateGuardMode == GenerateGuardMode.AutoToClickRelease)
        {
            energy += energyRate * Time.deltaTime;
        }
        else if (generateGuardMode == GenerateGuardMode.HoldAndReleaseProtectFirst)
        {
            if (Input.GetMouseButtonDown(0)){
                clickedOnce = true;
            }
            if (Input.GetMouseButton(0) || clickedOnce) energy += energyRate * Time.deltaTime;
            //else if (!Input.GetMouseButtonUp(0) && !clickedOnce) energy = 0;
        }

        int i = 0;
        do
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(previewPivot.position, previewPivot.up, 1.55f + i, raycastGuards);
            List<Transform> guards = new List<Transform>();
            List<float> distances = new List<float>();
            for (int j = 0; j < hits.Length; j++)
            {
                if (!guards.Contains(hits[j].transform)) {
                    bool foundDouble = false;
                    for (int w = 0; w < distances.Count; w++)
                    {
                        if (hits[j].distance >= distances[w] - 0.2 && hits[j].distance <= distances[w] + 0.2)
                        {
                            foundDouble = true;
                        }
                    }
                    if (!foundDouble)
                    {
                        guards.Add(hits[j].transform);
                        distances.Add(hits[j].distance);
                    }
                }
            }
            if (guards.Count <= i)
            {
                break;
            }
            i++;
        } while (i < 3);
        previewTransform.localPosition = new Vector3(0, 1.55f + i, 0);

        canFit = CanFit(previewTransform.position);

        CalculatePreviewSprite();

        if (Input.GetMouseButtonUp(0) && canFit)
        {
            CreateGuard();
        }
        else if (generateGuardMode == GenerateGuardMode.HoldAndReleaseProtectFirst && clickedOnce)
        {
            if (CanCreate() != -1 && !Input.GetMouseButton(0))
            {
                CreateGuard();
                clickedOnce = false;
            }
        }
    }

    int CanCreate()
    {
        int index = -1;
        for (int i = energyPerGuard.Length - 1; i >= 0; i--)
        {
            bool result = energy >= energyPerGuard[i];
            index = result ? i : index;
            if (result) break;
        }
        return index;
    }

    int CreateGuard()
    {
        if (!canFit) return -1;

        int index = -1;
        for (int i = energyPerGuard.Length - 1; i >= 0; i--)
        {
            bool result = energy >= energyPerGuard[i];
            index = result ? i : index;
            if (result) break;
        }
        if (index >= 0)
        {
            energy = 0;

            GameObject guard = GameObject.Instantiate(guardOriginal[index], guardsParent);
            guard.GetComponent<Guard>().Setup(previewTransform.position, this, previewPivot);
            guard.SetActive(true);

            previewCreatePs.Emit(14);

            AudioManager.Instance.CreateCat();

            AddRingCount(guard.transform.position);
            
            clickedOnce = false;
        }
        return index;
    }

    void AddRingCount(Vector2 pos)
    {
        int index = DistanceToRingIndex(pos);
        ringsCounts[index]++;
    }

    public void NotifyDeath(Guard guard)
    {
        int index = DistanceToRingIndex(guard.transform.position);
        ringsCounts[index]--;
    }

    int DistanceToRingIndex(Vector2 pos)
    {
        int index = ringsCounts.Length - 1;

        float distance = Vector2.Distance(previewPivot.position, pos);
        index = Mathf.RoundToInt(distance - 1.55f);

        return index;
    }

    bool CanFit(Vector2 pos)
    {
        bool value = true;

        int index = DistanceToRingIndex(pos);
        if (ringsCounts[index] > Mathf.Ceil(2 * (1.55f + (float)index * 1) * Mathf.PI)){
            value = false;
        }

        return value;
    }

    void CalculatePreviewSprite()
    {
        float energyAddUp = 0;
        float energyNeeded = energyPerGuard[0];
        int guardIndex = 0;
        for (int i = 0; i < energyPerGuard.Length; i++)
        {
            if (i>0) energyAddUp += energyPerGuard[i - 1];
            if (energy <= energyPerGuard[i])
            {
                guardIndex = i;
                energyNeeded = energyPerGuard[i];
                break;
            }
        }

        if (showFirstEnergy)
        {
            for (int i = 0; i < energyBars.Length; i++)
            {
                energyBars[i].fillAmount = (energy - energyAddUp) / (energyNeeded - energyAddUp);
            }
        }
        else
        {
            for (int i = 0; i < energyBars.Length; i++)
            {
                if (energyAddUp == 0) energyBars[i].fillAmount = 0;
                else energyBars[i].fillAmount = (energy - energyAddUp) / (energyNeeded - energyAddUp);
                //Debug.Log("[Fill] addup: " + energyAddUp + " | needed: " + energyNeeded);
            }
        }
        for (int i = 0; i < energyBars.Length; i++)
        {
            energyBars[i].gameObject.SetActive(i == guardIndex);
        }

        previewSpritesBlocked[0].gameObject.SetActive(!canFit);

        bool foundMatch = false;
        for (int i = previewSprites.Length - 1; i >= 0; i--)
        {
            bool result = energy >= energyPerGuard[i] && !foundMatch && canFit;
            previewSprites[i].gameObject.SetActive(result);
            foundMatch = result ? true : foundMatch;
        }

        previewSpritesBlocked[1].gameObject.SetActive(!foundMatch && canFit);
            /*
            if (!canFit && i == 0) c.a = 1;
            else if (i > 0)
            {
                float energyDiff;
                if (energyPerGuard.Length == 1) energyDiff = 0;
                else
                {
                    energyDiff = energyPerGuard[i-1] - energyPerGuard[i - 2];
                }
                c.a = (energy - energyDiff) / energyPerGuard[i-1];
            }
            */
    }

    int GetTotalGuards()
    {
        int count = 0;
        for (int i = 0; i < ringsCounts.Length; i++)
        {
            count += ringsCounts[i];
        }
        return count;
    }
}
