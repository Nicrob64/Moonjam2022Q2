using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class QAController : MonoBehaviour
{
    Transform cameraTarget;
    Transform boxTarget;
    public PackageManager nodejs;
    public PickableItemManager pickable;

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

    public float cameraMoveMaxDelta = 0.001f;
    public float cameraMoveAngleDelta = 0.1f;
    public float boxMoveDuration = 5.0f;
    public float boxTime = 0.0f;
    Vector3 boxStartLoc;

    public float badOrderChance = 0.27f;
    public float itemsWrongChance = 0.34f;
    public Text whatsInBox;
    public Text manifest;

    bool boxMoving = false;
    bool isBadOrder = false;

    public int maxItems = 14;

    public void Start()
    {
        StartCoroutine(NewBox());
    }
    public void Update()
    {
        if (!boxMoving)
        {
            if (Input.GetKey(KeyCode.P))
            {
                StartCoroutine(SubmitBox(true));
            }
            else if (Input.GetKey(KeyCode.O))
            {
                StartCoroutine(SubmitBox(false));
            }
        }

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


            if (cameraTarget)
            {
                Vector3 newPos = Vector3.MoveTowards(Camera.main.transform.position, cameraTarget.position, cameraMoveMaxDelta);
                Camera.main.transform.position = newPos;

                Vector3 newEuler = Vector3.MoveTowards(Camera.main.transform.eulerAngles, cameraTarget.eulerAngles, cameraMoveAngleDelta);
                Camera.main.transform.eulerAngles = newEuler;
            }
        }



    }

    IEnumerator SubmitBox(bool accepted)
    {
        boxTime = 0;
        boxMoving = true;
        boxStartLoc = box.transform.position;
        boxLabel.rejected.enabled = !accepted;
        boxLabel.qaPassed.enabled = accepted;
        boxTarget = accepted ? boxGood : boxSpawn;
        yield return new WaitForSeconds(boxMoveDuration + 1);
      //man that is some shitty code
        boxResultAudioSource.PlayOneShot((accepted != isBadOrder) ? correctClip : incorrectClip);
        StartCoroutine(NewBox());
    }
    
    IEnumerator NewBox()
    {
        boxTime = 0;
        boxMoving = true;
        CreateNewPackage();
        box.transform.position = boxSpawn.position;
        boxStartLoc = boxSpawn.position;
        boxLabel.rejected.enabled = false;
        boxLabel.qaPassed.enabled = false;
        boxTarget = boxTable;
        yield return new WaitForSeconds(boxMoveDuration);
        boxMoving = false;
    }


    void CreateNewPackage()
    {
        Person p = nodejs.GetRandomPerson();
        ListOfPickableItems items = pickable.GenerateNonUniqueItemList((ushort)Random.Range(1, maxItems));
        string itemsString = string.Join(", ", (from x in (items.Items) select x.ItemName));
        manifest.text = string.Format("CUSTOMER NAME\n{0} {1} {2}\n\nCONTACT INFORMATION\nEmail : {10}\nPhone: {11}\n\nADDRESS\n{3}, {4}, {5}, {6}, {7}\n\nITEMS:\n{8}\n\nTRACKING #\n{9}", p.Title, p.GivenName, p.Surname, p.StreetAddress, p.City, p.State, p.Country, p.ZipCode, itemsString, p.UPS, p.EmailAddress, p.TelephoneNumber);

        
        PickableItemInfo replaceItem = items.Items[Random.Range(0, items.Items.Count)];
        pickable.GetRandomSingleItem(replaceItem);

        var rand = new System.Random();
        var randomizedItems = items.Items.OrderBy(_ => rand.Next()).ToList();

        Person label = p;
        if(Random.value < badOrderChance)
        {
            isBadOrder = true;
            //fuck the order up based on
        }
        else
        {
            isBadOrder = false;
            string whatsInBoxString = string.Join(", ", (from x in (randomizedItems) select x.ItemName));
            whatsInBox.text = whatsInBoxString;
        }


        boxLabel.SetDetails(label);

    }
}
