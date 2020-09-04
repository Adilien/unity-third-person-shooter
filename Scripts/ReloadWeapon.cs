using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class ReloadWeapon : MonoBehaviour
{
    public Animator rigController;
    public WeaponAnimationEvents animationEvents;
    public Transform leftHand;
    public RaycastWeapon raycastWeapon;
    public bool reloading = false;
    public AmmoWidget ammoWidget;

    GameObject magHand;

    // Start is called before the first frame update
    void Start()
    {
        animationEvents.WeaponAnimationEvent.AddListener(onAnimationEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !Input.GetMouseButton(1))
        {
            reloading = true;
            rigController.SetTrigger("reloadWeapon");
        }

        if (raycastWeapon.isFiring)
        {
            ammoWidget.Refresh(raycastWeapon.ammoCount);
        }
    }

    void onAnimationEvent(string eventName)
    {
        Debug.Log(eventName);
        switch (eventName)
        {
            case "detachMag":
                DetachMag();
                break;

            case "dropMag":
                DropMag();
                break;

            case "refillMag":
                RefillMag();
                break;

            case "attachMag":
                AttachMag();
                break;  
        }
    }

    void DetachMag()
    {
        magHand = Instantiate(raycastWeapon.magazine, leftHand, true);
        raycastWeapon.magazine.SetActive(false);
    }

    void DropMag()
    {
        GameObject droppedMag = Instantiate(magHand, magHand.transform.position, magHand.transform.rotation);
        droppedMag.AddComponent<Rigidbody>();
        droppedMag.AddComponent<BoxCollider>();
        magHand.SetActive(false);
    }

    void RefillMag()
    {
        magHand.SetActive(true);
    }

    void AttachMag()
    {
        raycastWeapon.magazine.SetActive(true);
        Destroy(magHand);
        raycastWeapon.ammoCount = raycastWeapon.clipSize;
        rigController.ResetTrigger("reloadWeapon");
        reloading = false;
        ammoWidget.Refresh(raycastWeapon.ammoCount);
    }
}
