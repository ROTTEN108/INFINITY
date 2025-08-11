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
using static UnityEngine.TouchScreenKeyboard;

namespace INFINITY
{
    public class PhaseControl : MonoBehaviour
    {
        public int phase = 0;
        public bool phase1Started = false;
        public bool phase2Started = false;
        public bool phase3Started = false;
        public bool phase4Started = false;
        public bool phase5Started = false;

        int phase1Hp_N = 1100;
        int phase2Hp_N = 950;
        int phase3Hp_N = 500;
        int phase4Hp_N = 200;

        int phase1Hp_H = 2200;
        int phase2Hp_H = 2000;
        int phase3Hp_H = 1100;
        int phase4Hp_H = 600;
        public void Start()
        {
            phase = 0;
            if(INFINITY.HardMode == 0)
            {
                gameObject.GetComponent<HealthManager>().hp = phase1Hp_N;
            }
            if(INFINITY.HardMode == 1)
            {
                gameObject.GetComponent<HealthManager>().hp = phase1Hp_H;
            }
        }
        public void Update()
        {
            if (INFINITY.HardMode == 0)
            {
                if (gameObject.GetComponent<HealthManager>().hp < phase4Hp_N && phase < 4)
                {
                    gameObject.GetComponent<HealthManager>().hp = phase4Hp_N;
                    gameObject.GetComponent<Phase5Arena>().Phase4InvincibleStart();
                }
                else if (gameObject.GetComponent<HealthManager>().hp < phase3Hp_N && phase < 3)
                {
                    gameObject.GetComponent<HealthManager>().hp = phase3Hp_N;
                    gameObject.GetComponent<Phase5Arena>().Phase4InvincibleStart();
                }
                else if (gameObject.GetComponent<HealthManager>().hp < phase2Hp_N && phase < 2)
                {
                    gameObject.GetComponent<HealthManager>().hp = phase2Hp_N;
                    gameObject.GetComponent<Phase5Arena>().Phase4InvincibleStart();
                }
            }
            if (INFINITY.HardMode == 1)
            {
                if (gameObject.GetComponent<HealthManager>().hp < phase4Hp_H && phase < 4)
                {
                    gameObject.GetComponent<HealthManager>().hp = phase4Hp_H;
                    gameObject.GetComponent<Phase5Arena>().Phase4InvincibleStart();
                }
                else if (gameObject.GetComponent<HealthManager>().hp < phase3Hp_H && phase < 3)
                {
                    gameObject.GetComponent<HealthManager>().hp = phase3Hp_H;
                    gameObject.GetComponent<Phase5Arena>().Phase4InvincibleStart();
                }
                else if (gameObject.GetComponent<HealthManager>().hp < phase2Hp_H && phase < 2)
                {
                    gameObject.GetComponent<HealthManager>().hp = phase2Hp_H;
                    gameObject.GetComponent<Phase5Arena>().Phase4InvincibleStart();
                }
            }
        }
        public void PhaseUpdate()
        {
            if (INFINITY.HardMode == 0)
            {
                if (gameObject.GetComponent<HealthManager>().hp <= phase4Hp_N && gameObject.GetComponent<SceneControl>().Phase5Started)
                {
                    phase = 5;
                }
                else if(gameObject.GetComponent<HealthManager>().hp <= phase4Hp_N)
                {
                    phase = 4;
                }
                else if (gameObject.GetComponent<HealthManager>().hp <= phase3Hp_N)
                {
                    phase = 3;
                }
                else if (gameObject.GetComponent<HealthManager>().hp <= phase2Hp_N)
                {
                    phase = 2;
                }
                else if (gameObject.GetComponent<HealthManager>().hp <= phase1Hp_N)
                {
                    phase = 1;
                }
            }
            if (INFINITY.HardMode == 1)
            {
                if (gameObject.GetComponent<HealthManager>().hp <= phase4Hp_H && gameObject.GetComponent<SceneControl>().Phase5Started)
                {
                    phase = 5;
                }
                else if (gameObject.GetComponent<HealthManager>().hp <= phase4Hp_H)
                {
                    phase = 4;
                }
                else if (gameObject.GetComponent<HealthManager>().hp <= phase3Hp_H)
                {
                    phase = 3;
                }
                else if (gameObject.GetComponent<HealthManager>().hp <= phase2Hp_H)
                {
                    phase = 2;
                }
                else if (gameObject.GetComponent<HealthManager>().hp <= phase1Hp_H)
                {
                    phase = 1;
                }
            }
        }
    }
}
