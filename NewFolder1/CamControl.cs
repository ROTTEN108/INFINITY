using HutongGames.PlayMaker.Actions;
using Modding;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vasi;
using System.Collections.Generic;
using HKMirror.Reflection;
using System.Collections;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine.PlayerLoop;

namespace INFINITY
{
    public static class CameraControl
    {
        public static void CamAway()
        {
            var gc = GameCameras.instance.gameObject;
            var cp = gc.transform.Find("CameraParent");
            var cam = cp.transform.Find("tk2dCamera").gameObject;
            cam.GetComponent<CamControl>().CamAway();
        }
        public static void CamClose()
        {
            var gc = GameCameras.instance.gameObject;
            var cp = gc.transform.Find("CameraParent");
            var cam = cp.transform.Find("tk2dCamera").gameObject;
            cam.GetComponent<CamControl>().CamClose();
        }
        public static void BattleStart()
        {

            var gc = GameCameras.instance.gameObject;
            var cp = gc.transform.Find("CameraParent");
            var cam = cp.transform.Find("tk2dCamera").gameObject;
            cam.GetComponent<CamControl>().CamStart();
        }
    }
    public class CamControl : MonoBehaviour
    {
        public static GameObject Cam;
        float camFactor_Battle = 0.6f;
        float camFactor_Skill1 = 0.6f;
        float camFactor_Orig = 1f;
        float camAwaySpeed = 1f;
        float camCloseSpeed = 2f;
        int loopTime = 0;
        int loopTimeMax = 75;
        public void Start()
        {
            var gc = GameCameras.instance.gameObject;
            var cp = gc.transform.Find("CameraParent");
            var cam = cp.transform.Find("tk2dCamera").gameObject;
            Cam = cam;
        }
        public void CamStart()
        {
            Cam.GetComponent<CameraController>().xLimit = 10000;
            Cam.GetComponent<CameraController>().yLimit = 10000;
            Invoke("CamStartRepeat", 1f);
        }
        public void CamStartRepeat()
        {
            Cam.GetComponent<CameraController>().xLimit = 10000;
            Cam.GetComponent<CameraController>().yLimit = 10000;
        }
        public void CamChange()
        {
            Cam.GetComponent<tk2dCamera>().ZoomFactor = camFactor_Skill1;
        }
        public void CamRecover()
        {
            Cam.GetComponent<tk2dCamera>().ZoomFactor = 1f;
        }
        public void CamAway()
        {
            loopTime = 0;
            CancelInvoke("CamCloseLoop");
            CamAwayLoop();
            Invoke("LoopEnd", 5f);
        }
        public void CamClose()
        {
            loopTime = 0;
            CancelInvoke("CamAwayLoop");
            CamCloseLoop();
            Invoke("LoopEnd", 5f);
        }
        private void CamAwayLoop()
        {
            if(loopTime < loopTimeMax)
            {
                loopTime++;
                Cam.GetComponent<tk2dCamera>().ZoomFactor += (camFactor_Skill1 - Cam.GetComponent<tk2dCamera>().ZoomFactor) / 32 * camAwaySpeed;
                Invoke("CamAwayLoop", 0.02f);
            }
        }
        private void CamCloseLoop()
        {
            if (loopTime < loopTimeMax)
            {
                loopTime++;
                Cam.GetComponent<tk2dCamera>().ZoomFactor += (camFactor_Orig - Cam.GetComponent<tk2dCamera>().ZoomFactor) / 32 * camCloseSpeed;
                Invoke("CamCloseLoop", 0.02f);
            }
            else
            {
                Cam.GetComponent<tk2dCamera>().ZoomFactor = camFactor_Orig;
            }
        }
        private void LoopEnd()
        {
            CancelInvoke("CamCloseLoop");
            CancelInvoke("CamAwayLoop");
        }
    }
}
