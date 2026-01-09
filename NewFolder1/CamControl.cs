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
        public static void P3()
        {
            var gc = GameCameras.instance.gameObject;
            var cp = gc.transform.Find("CameraParent");
            var cam = cp.transform.Find("tk2dCamera").gameObject;
            cam.GetComponent<CamControl>().DelayP3Change();
        }
        public static void P5()
        {
            var gc = GameCameras.instance.gameObject;
            var cp = gc.transform.Find("CameraParent");
            var cam = cp.transform.Find("tk2dCamera").gameObject;
            cam.GetComponent<CamControl>().CamP5Change();
        }
    }
    public class CamControl : MonoBehaviour
    {
        public static GameObject Cam;
        float camFactor_Battle = 0.637f;
        float camFactor_Battle_Nolines = 0.6f;
        float camFactor_Battle_Nolines_P3 = 0.48f;
        float camFactor_Skill1 = 0.6f;
        float camFactor_Orig = 1f;
        float camAwaySpeed = 1f;
        float camCloseSpeed = 2f;
        float cam_P3Y = 26f;
        int loopTime = 0;
        int loopTimeMax = 300;
        public void Start()
        {
            var gc = GameCameras.instance.gameObject;
            var cp = gc.transform.Find("CameraParent");
            var cam = cp.transform.Find("tk2dCamera").gameObject;
            Cam = cam;

            CamShakeOff();
        }
        
        public void CamStart()
        {
            if (INFINITY.settings_.lineOff)
            {
                camFactor_Battle = camFactor_Battle_Nolines;
            }

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
            Invoke("LoopEnd", 10f);
        }
        public void CamClose()
        {
            loopTime = 0;
            CancelInvoke("CamAwayLoop");
            CamCloseLoop();
            Invoke("LoopEnd", 10f);
        }
        private void CamAwayLoop()
        {
            if(loopTime < loopTimeMax)
            {
                loopTime++;
                Cam.GetComponent<tk2dCamera>().ZoomFactor += (camFactor_Battle - Cam.GetComponent<tk2dCamera>().ZoomFactor) / 32 * camAwaySpeed;
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

        public void CamDown()
        {
            Cam.GetComponent<Camera>().enabled = false;
        }

        public void CamUp()
        {
            Cam.GetComponent<Camera>().enabled = true;
        }

        public void CamShakeOff()
        {

            gameObject.transform.parent.gameObject.LocateMyFSM("CameraShake").enabled = false;
        }
        public void DelayP3Change()
        {
            Invoke("CamP3Change", 7f);
        }
        public void CamP3Change()
        {
            if (INFINITY.settings_.lineOff)
            {
                camFactor_Battle = camFactor_Battle_Nolines_P3;
            }

            CamAway();

            CancelInvoke("CamP3ChangeLoop");

            CamP3ChangeLoop();
        }
        public void CamP3ChangeLoop()
        {
            Invoke("CamP3ChangeLoop", 0.02f);

            if(INFINITY.settings_.lineOff)
            {
                Cam.GetComponent<CameraController>().camTarget.yLockMin += (cam_P3Y - Cam.GetComponent<CameraController>().camTarget.yLockMin) / 24 * camCloseSpeed;
            }
            else
            {
                Cam.GetComponent<CameraController>().camTarget.yLockMin += (cam_P3Y - 5f - Cam.GetComponent<CameraController>().camTarget.yLockMin) / 24 * camCloseSpeed;
            }
        }
        public void CamP5Change()
        {
            if (INFINITY.settings_.lineOff)
            {
                camFactor_Battle = camFactor_Battle_Nolines;
            }

            CancelInvoke("CamP3ChangeLoop");

            Cam.GetComponent<CameraController>().camTarget.yLockMin = 12.5f;

            CamAway();

            Invoke("CamP5ChangeRepeat", 0.1f);
        }
        public void CamP5ChangeRepeat()
        {
            if (INFINITY.settings_.lineOff)
            {
                camFactor_Battle = camFactor_Battle_Nolines;
            }

            CancelInvoke("CamP3ChangeLoop");

            Cam.GetComponent<CameraController>().camTarget.yLockMin = 12.5f;

            CamAway();


        }
        public void CancelAll()
        {

            CancelInvoke("CamP3Change");

            CancelInvoke("CamP3ChangeLoop");

            CancelInvoke("CamP5ChangeRepeat");

            CancelInvoke("CamAwayLoop");

            CancelInvoke("CamCloseLoop");
        }
    }
}
