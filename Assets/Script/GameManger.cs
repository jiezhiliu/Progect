using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour {
    private Transform uiTrans;
    private Button color1Btn;
    private Button color2Btn;
    private Button colorResetBtn;
    private Slider angleSlider;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Color currentPersonColor;
    private Transform person;
    private Transform spine;
    private Transform leftArm;
    private Transform rightArm;
    private float minRightAngle = 90;
    private float maxRightAngle = 180;
    private float minLeftAngle = 0;
    private float maxLeftAngle = 90;
    private Color originalColor;
    private float rotateSpeed = 80.0f;
    private float lastPosX = 0.0f;      // 上一帧的位置
    private float currentPosX = 0.0f;       // 当前帧的位置
    private void Start()
    {
        person = GameObject.Find("Person/Male_Unified").transform;
        skinnedMeshRenderer = person.Find("UMA_Human_Male").GetComponent<SkinnedMeshRenderer>();
        originalColor = skinnedMeshRenderer.materials[0].color;
        spine = person.Find("UMA_Male_Rig/Global/Position/Hips/LowerBack/Spine/Spine1");
        leftArm = spine.Find("LeftShoulder/LeftArm");
        rightArm = spine.Find("RightShoulder/RightArm");
        uiTrans = GameObject.Find("UI").transform;
        color1Btn = uiTrans.Find("Canvas/Panel/color1").GetComponent<Button>();
        color2Btn = uiTrans.Find("Canvas/Panel/color2").GetComponent<Button>();
        colorResetBtn = uiTrans.Find("Canvas/Panel/colorReset").GetComponent<Button>();
        angleSlider = uiTrans.Find("Canvas/Panel/angleSlider").GetComponent<Slider>();
        color1Btn.onClick.AddListener(OnClickColor1Btn);
        color2Btn.onClick.AddListener(OnClickColor2Btn);
        colorResetBtn.onClick.AddListener(OnClickColorResetBtn);
        angleSlider.value = 0.5f;
        angleSlider.onValueChanged.AddListener(OnChangeSliderValue);
    }

    private void OnClickColorResetBtn()
    {
        currentPersonColor = originalColor;
        ChangePersonColor(currentPersonColor);
    }
    private void Update()
    {
#if !UNITY_EDITOR      
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Moved)
                {
                    currentPosX = touch.position.x;
                    if (currentPosX > lastPosX)
                        person.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
                    else
                        person.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
                    lastPosX = currentPosX;
                }
            }
        }
#else
        if (Input.GetAxis("Horizontal") != 0)
        {
            float h = Input.GetAxis("Horizontal");
            if (h > 0)
                person.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            else
                person.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        }
#endif
    }
    private void OnChangeSliderValue(float value)
    {
        if (leftArm == null || rightArm ==null)
            return;
        float angleLeftArm = 0.0f; 
        float angleRightArm = 0.0f;
        angleLeftArm = 90 - value * 90;
        angleRightArm = 90 + value * 90;
        angleLeftArm = Mathf.Clamp(angleLeftArm, minLeftAngle, maxLeftAngle);
        angleRightArm = Mathf.Clamp(angleRightArm, minRightAngle, maxRightAngle);
        leftArm.eulerAngles = new Vector3(leftArm.eulerAngles.x, leftArm.eulerAngles.y, angleLeftArm);
        rightArm.eulerAngles = new Vector3(rightArm.eulerAngles.x, rightArm.eulerAngles.y, angleRightArm);
    }

    private void OnClickColor2Btn()
    {
        //Debug.Log("colorTwo==============");
        currentPersonColor = new Color(UnityEngine.Random.Range(0, 255) / 255.0f, UnityEngine.Random.Range(0, 255) / 255.0f, UnityEngine.Random.Range(0, 255) / 255.0f, UnityEngine.Random.Range(0, 255) / 255.0f);
        ChangePersonColor(currentPersonColor);
    }

    private void OnClickColor1Btn()
    {
        //Debug.Log("colorOne==========");
        currentPersonColor = new Color(UnityEngine.Random.Range(0, 255) / 255.0f, UnityEngine.Random.Range(0, 255) / 255.0f, UnityEngine.Random.Range(0, 255) / 255.0f, UnityEngine.Random.Range(0, 255) / 255.0f);
        ChangePersonColor(currentPersonColor);
    }

    private void ChangePersonColor(Color color)
    {
        for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
        {
            skinnedMeshRenderer.materials[i].color = color;
        }
    }
}
