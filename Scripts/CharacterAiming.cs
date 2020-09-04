using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;

public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15;
    public float adsSpeed = 0.3f;
    bool aimDone = false;
    public Rig aimLayer;
    RaycastWeapon weapon;

    public CinemachineFreeLook idleCamera;
    public CinemachineFreeLook aimCamera;
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        weapon = GetComponentInChildren<RaycastWeapon>();
    }

    void FixedUpdate()
    {
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            aimLayer.weight += Time.deltaTime / adsSpeed;

            if (aimDone == false)
            {
                doAimCameraTransition();

                aimDone = true;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                weapon.StartFiring();
            }

            if (weapon.isFiring)
            {
                weapon.UpdateFiring(Time.deltaTime);
            }
        }
        else
        {
            aimLayer.weight -= Time.deltaTime / adsSpeed;

            if (aimDone == true)
            {
                stopAimCameraTransition();
                aimDone = false;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            weapon.StopFiring();
        }
    }

    void doAimCameraTransition()
    {
        aimCamera.GetComponent<CinemachineFreeLook>().m_XAxis = idleCamera.GetComponent<CinemachineFreeLook>().m_XAxis;
        aimCamera.GetComponent<CinemachineFreeLook>().m_YAxis = idleCamera.GetComponent<CinemachineFreeLook>().m_YAxis;

        aimCamera.Priority = 11;
    }

    void stopAimCameraTransition()
    {
        idleCamera.GetComponent<CinemachineFreeLook>().m_XAxis = aimCamera.GetComponent<CinemachineFreeLook>().m_XAxis;
        idleCamera.GetComponent<CinemachineFreeLook>().m_YAxis = aimCamera.GetComponent<CinemachineFreeLook>().m_YAxis;

        aimCamera.Priority = 9;
    }
}
