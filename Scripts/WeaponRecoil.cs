using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    public Cinemachine.CinemachineFreeLook playerCamera;
    [HideInInspector] public Cinemachine.CinemachineImpulseSource cameraShake;

    public Vector2[] recoilPattern;
    public float verticalRecoil;
    public float horizontalRecoil;

    public float duration;
    float time;
    int index;

    private void Awake()
    {
        cameraShake = GetComponent<CinemachineImpulseSource>();   
    }

    public void Reset()
    {
        index = 0;
    }

    int NextIndex(int index)
    {
        return (index + 1) % recoilPattern.Length;
    }
    public void GenerateRecoil()
    {
        time = duration;
        cameraShake.GenerateImpulse(Camera.main.transform.forward);

        horizontalRecoil = recoilPattern[index].x;
        verticalRecoil = recoilPattern[index].y;

        index = NextIndex(index);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            playerCamera.m_YAxis.Value -= (verticalRecoil * Time.deltaTime) / duration;
            playerCamera.m_XAxis.Value -= (horizontalRecoil * Time.deltaTime) / duration;
            time =- Time.deltaTime;
        }
    }
}
