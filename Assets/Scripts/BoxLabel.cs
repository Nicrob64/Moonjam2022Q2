using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxLabel : MonoBehaviour
{
    public Text title, address1, address2, address3, tracking;
    public Text rejected, qaPassed;
    public void SetDetails(Person p) {
        title.text = (p.Title + " " + p.GivenName + " " + p.Surname);
        address1.text = (p.StreetAddress);
        address2.text = (p.City + ", " + p.State + ", " + p.Country);
        address3.text = p.ZipCode.ToString();
        tracking.text = ("Tracking: " + p.UPS);
        qaPassed.text = "[QA:CHECKED:"+p.UPS.Substring(0, 6).Replace(" ", "")+"]";
    }
}
