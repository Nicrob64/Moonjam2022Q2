using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
public class QAController : MonoBehaviour
{

    public InfractionWriter infractionWriter;
    public Cutscene narrator;

    public int quotaRemaining = 20;
    public int failuresTillGameOver = 5;
    int failures = 0;

    Transform cameraTarget;
    Transform boxTarget;
    public PackageManager nodejs;
    public PickableItemManager pickable;

    public TMP_Text remainingMesh;

    public Transform mainState;
    public Transform lookAtBox;
    public Transform lookAtManifest;
    public Transform lookAtContents;

    public Transform boxSpawn;
    public Transform boxTable;
    public Transform boxGood;

    public BoxLabel boxLabel;
    public GameObject box;

    public AudioSource boxResultAudioSource;
    public AudioClip correctClip;
    public AudioClip incorrectClip;
    public AudioClip stamp;

    public AudioSource conveyerLeft;
    public AudioSource conveyerRight;

    public float cameraMoveMaxDelta = 0.001f;
    public float cameraMoveAngleDelta = 0.1f;
    public float boxMoveDuration = 5.0f;
    public float boxTime = 0.0f;
    Vector3 boxStartLoc;

    public float badOrderChance = 0.27f;
    public float itemsWrongChance = 0.50f;
    public Text whatsInBox;
    public Text manifest;

    bool boxMoving = false;
    bool isBadOrder = false;
    string reasonForBadOrder = "Bad at code";

    public int maxItems = 14;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(NewBox());

        if (SceneTransitionHelper.Instance.TransitionReason == TransitionReason.Retry)
        {
            narrator.shouldRun = false;
        }
    }
    public void Update()
    {
       /* if (!boxMoving)
        {
            if (Input.GetKey(KeyCode.P))
            {
                StartCoroutine(SubmitBox(true));
            }
            else if (Input.GetKey(KeyCode.O))
            {
                StartCoroutine(SubmitBox(false));
            }
        }*/

        if (boxMoving)
        {
            boxTime += Time.deltaTime;
            box.transform.position = Vector3.Lerp(boxStartLoc, boxTarget.position, boxTime / boxMoveDuration);
            Camera.main.transform.LookAt(box.transform);
        }
        else
        {
            if (Input.GetKey(KeyCode.Q))
            {
                cameraTarget = lookAtContents;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                cameraTarget = lookAtBox;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                cameraTarget = lookAtManifest;
            }
            else
            {
                cameraTarget = mainState;
            }
            
        }

    }


    private void FixedUpdate()
    {
        if (boxMoving) { return; }
        if (cameraTarget)
        {
            Vector3 newPos = Vector3.MoveTowards(Camera.main.transform.position, cameraTarget.position, cameraMoveMaxDelta * Time.fixedDeltaTime);
            Camera.main.transform.position = newPos;


            Vector3 newEuler = Vector3.MoveTowards(Camera.main.transform.eulerAngles, cameraTarget.eulerAngles, cameraMoveAngleDelta * Time.fixedDeltaTime);
            Camera.main.transform.eulerAngles = newEuler;
        }
    }

    public void Submit(bool accepted)
    {
        StartCoroutine(SubmitBox(accepted));
    }

    IEnumerator SubmitBox(bool accepted)
    {
        
        boxLabel.rejected.gameObject.SetActive(!accepted);
        boxLabel.qaPassed.gameObject.SetActive(accepted);
        boxResultAudioSource.PlayOneShot(stamp);
        yield return new WaitForSeconds(1.0f);
        if (!accepted)
        {
            conveyerLeft.Play();
        }
        else
        {
            conveyerRight.Play();
        }
        boxTime = 0;
        boxMoving = true;
        boxStartLoc = box.transform.position;
        boxTarget = accepted ? boxGood : boxSpawn;
        yield return new WaitForSeconds(boxMoveDuration + 1);
        conveyerLeft.Pause();
        conveyerRight.Pause();
      //man that is some shitty code
        boxResultAudioSource.PlayOneShot((accepted != isBadOrder) ? correctClip : incorrectClip);
        if(accepted == isBadOrder)
        {
            failures++;
            if (!accepted)
            {
                reasonForBadOrder = "Failure to send perfectly fine order to our dear customer.";
            }
            StartCoroutine(infractionWriter.Infraction(reasonForBadOrder));


            if(failures >= failuresTillGameOver)
            {
                SceneTransitionHelper.Instance.FromScene = FromScene.QA;
                SceneTransitionHelper.Instance.TransitionReason = TransitionReason.GameOverFuckedUpTooManyOrders;
                SceneManager.LoadScene("GameOver");
                yield break;
            }
        }
        else
        {
            quotaRemaining--;
            remainingMesh.text = quotaRemaining.ToString();
            if (quotaRemaining <= 0)
            {
                SceneTransitionHelper.Instance.FromScene = FromScene.QA;
                SceneTransitionHelper.Instance.TransitionReason = TransitionReason.PromotionToManager;
                SceneManager.LoadScene("Promotion");
                yield break;
            }
        }
        StartCoroutine(NewBox());
    }
    
    IEnumerator NewBox()
    {
        conveyerLeft.Play();
        yield return new WaitForSeconds(0.4f);

        boxTime = 0;
        boxMoving = true;
        CreateNewPackage();
        box.transform.position = boxSpawn.position;
        boxStartLoc = boxSpawn.position;
        boxLabel.rejected.gameObject.SetActive(false);
        boxLabel.qaPassed.gameObject.SetActive(false);
        boxTarget = boxTable;
        yield return new WaitForSeconds(boxMoveDuration);
        conveyerLeft.Pause();
        boxMoving = false;
    }


    void CreateNewPackage()
    {
        Person p = nodejs.GetRandomPerson();
        ListOfPickableItems items = pickable.GenerateNonUniqueItemList((ushort)Random.Range(1, maxItems));
        string itemsString = string.Join(", ", (from x in (items.Items) select x.ItemName));
        manifest.text = string.Format("CUSTOMER NAME\n{0} {1} {2}\n\nCONTACT INFORMATION\nEmail : {10}\nPhone: {11}\n\nADDRESS\n{3}, {4}, {5}, {6}, {7}\n\nITEMS:\n{8}\n\nTRACKING #\n{9}", p.Title, p.GivenName, p.Surname, p.StreetAddress, p.City, p.State, p.Country, p.ZipCode, itemsString, p.UPS, p.EmailAddress, p.TelephoneNumber);


        if (Random.value < badOrderChance)
        {
            isBadOrder = true;
            if(Random.value < itemsWrongChance)
            {
                FuckUpItemsInBox(items);
                DontFuckUpLabel(p);
            }
            else
            {
                FuckUpLabel(p);
                DontFuckUpItemsInBox(items);
            }
            Debug.Log(reasonForBadOrder);
        }
        else
        {
            isBadOrder = false;
            DontFuckUpLabel(p);
            DontFuckUpItemsInBox(items);
        }
    }

    void DontFuckUpLabel(Person p)
    {
        boxLabel.SetDetails(p);
    }

    void DontFuckUpItemsInBox(ListOfPickableItems items)
    {
        var rand = new System.Random();
        var randomizedItems = items.Items.OrderBy(_ => rand.Next()).ToList();
        string whatsInBoxString = string.Join(", ", (from x in (randomizedItems) select x.ItemName));
        whatsInBox.text = whatsInBoxString;
    }

    void FuckUpItemsInBox(ListOfPickableItems items)
    {
        PickableItemInfo replaceItem = items.Items[Random.Range(0, items.Items.Count)];
        PickableItemInfo added = pickable.GetRandomSingleItem(replaceItem);
        ListOfPickableItems otherList = pickable.GenerateNonUniqueItemList((ushort)Random.Range(1, maxItems));

        int result = Random.Range(0, 4);
        switch (result)
        {
            case 0:
                items.Items.Remove(replaceItem);
                items.Items.Add(added);
                reasonForBadOrder = string.Format("Incorrect package contents. \n\n Item [{0}] replaced by [{1}].", replaceItem.ItemName, added.ItemName);
                break;
            case 1:
                if (items.Items.Count > 1)
                {
                    reasonForBadOrder = string.Format("Incorrect package contents. \n\n Missing item [{0}].", items.Items[0].ItemName);
                    items.Items.RemoveAt(0);  
                }
                break;
            case 2:
                added = pickable.GetRandomSingleItem();
                items.Items.Add(added);
                reasonForBadOrder = string.Format("Incorrect package contents. \n\n Additional item [{0}].", added.ItemName);
                break;
            case 3:
                items = otherList;
                reasonForBadOrder = "Incorrect package contents. \n\n Wrong package list.";
                break;
        }

        var rand = new System.Random();
        var randomizedItems = items.Items.OrderBy(_ => rand.Next()).ToList();
        string whatsInBoxString = string.Join(", ", (from x in (randomizedItems) select x.ItemName));
        whatsInBox.text = whatsInBoxString;

    }

    void FuckUpLabel(Person p)
    {
        Person p2 = nodejs.GetRandomPerson();
        if(p2 == p)
        {
            p2 = nodejs.GetRandomPerson();
        }


        int fieldToFuckUp = Random.Range(0, 6);
        switch (fieldToFuckUp)
        {
            case 0:
                reasonForBadOrder = string.Format("Incorrect shipping label. \n\n Wrong name. [{0} {1}] -> [{2} {3}].", p.GivenName, p.Surname, p2.GivenName, p2.Surname);
                p.GivenName = p2.GivenName;
                p.Surname = p2.Surname;
                     break;
            case 1:
                reasonForBadOrder = string.Format("Incorrect shipping label. \n\n Wrong street address. [{0}] -> [{1}]", p.StreetAddress, p2.StreetAddress);
                p.StreetAddress = p2.StreetAddress;
                break;
            case 2:
                reasonForBadOrder = string.Format("Incorrect shipping label. \n\n  Wrong city. [{0}] -> [{1}]", p.City, p2.City);
                p.City = p2.City;
                break;
            case 3:
                if(p.ZipCode == p2.ZipCode)
                {
                    p2.ZipCode = p2.ZipCode + 1;
                }
                reasonForBadOrder = string.Format("Incorrect shipping label. \n\n Wrong zipcode. [{0}] -> [{1}]", p.ZipCode, p2.ZipCode);
                p.ZipCode = p2.ZipCode;
                break;
            case 4:
                reasonForBadOrder = string.Format("Incorrect shipping label. \n\n Wrong tracking code. [{0}] -> [{1}]", p.UPS, p2.UPS);
                p.UPS = p2.UPS;
                break;
            case 5:
                reasonForBadOrder = string.Format("Incorrect shipping label. \n\n Wrong state. [{0}] -> [{1}]", p.State, p2.State);
                p.State = p2.State;
                break;

        }
        boxLabel.SetDetails(p);
    }
}
