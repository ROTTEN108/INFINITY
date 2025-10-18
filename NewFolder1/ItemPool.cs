using HKMirror.Reflection;
using HutongGames.PlayMaker.Actions;
using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vasi;
using static Mono.Math.BigInteger;

namespace INFINITY
{
    public static class ItemPool
    {
        public static GameObject HK;
        public static GameObject BeamCollider1;
        public static GameObject Glow;
        public static GameObject CounterFlash;
        public static GameObject Light;
        public static GameObject Line;
        public static GameObject SlashLight;
        public static GameObject SlashLightForPlayer;
        public static GameObject BigSlash;
        public static GameObject BigSlashForPlayer;
        public static GameObject BigDstabForPlayer;
        public static GameObject Nail;
        public static GameObject Plume;
        public static GameObject Plumes;
        public static GameObject Floor;
        public static GameObject Floor1;
        public static GameObject FloorPt;
        public static GameObject FocusBall;
        public static GameObject FocusHit;
        public static GameObject Phase5Arena;
        public static GameObject PhaseFlash;
        public static GameObject BossTitle;
        public static GameObject DashBurst;
        public static GameObject Energy;
        public static GameObject RockPt_Wide;
        public static GameObject RockPt_Thin;
        public static GameObject Boss_Bugs_Pt;
        public static GameObject Boss_Bugs_Pt2;
        public static GameObject Wave;
        public static GameObject Spike;
        public static GameObject Corpse;
        public static GameObject RoarWave;
        public static AudioClip Teleport;
        public static AudioClip Slash;
        public static AudioClip Dir;
        public static AudioClip Land;
        public static AudioClip Explode;

        public static GameObject Scene_Fence;
        public static GameObject Scene_Gate;
        public static GameObject Scene_Light_1;
        public static GameObject Scene_Light_2;
        public static GameObject Scene_Light_3;
        public static GameObject Scene_Light_4;
        public static GameObject Scene_Light_5;
        public static GameObject Scene_Light_6;
        public static GameObject Scene_Bugs_Pt;
        public static GameObject Scene_Fountain;
        public static GameObject Scene_Fountain_Inspect;
        public static GameObject Scene_Particle_1;
        public static GameObject Scene_Particle_2;
        public static GameObject Scene_Shield;

        public static GameObject Scene_StatueOrb;
        public static GameObject Scene_StatueBugs_Pt1;
        public static GameObject Scene_StatueBugs_Pt2;
        public static GameObject Scene_StatueSymb1_prefab;
        public static GameObject Scene_StatueSymb2_prefab;
        public static GameObject Scene_StatueSymb3_prefab;
        public static GameObject Scene_StatueSymb4_prefab;
        public static GameObject Scene_StatueSymb1;
        public static GameObject Scene_StatueSymb2;
        public static GameObject Scene_StatueSymb3;
        public static GameObject Scene_StatueSymb4;

        public static AudioClip BGM1;
        public static AudioClip BGM2;
        public static AudioClip BGM3;

        public static GameObject INFINITY_FOR_PLAYER;
    }
    public class DelayDestory : MonoBehaviour
    {
        public void On(float delayTime)
        {
            Invoke("Destory", delayTime);
        }
        void Destory()
        {
            gameObject.Recycle();
        }
    }
    public class BossTitleControl : MonoBehaviour
    {
        public void Start()
        {
            if (gameObject.LocateMyFSM("Control").GetState("Title Up").GetAction<ApplyMusicCue>() != null)
            {
                MusicCue obj = FsmUtil.GetAction<ApplyMusicCue>(gameObject.LocateMyFSM("Control"), "Title Up", 3).musicCue.Value as MusicCue;

                MusicCue.MusicChannelInfo musicChannelInfo = ReflectionHelper.GetField<MusicCue, MusicCue.MusicChannelInfo[]>(obj, "channelInfos")[0];

                if (ItemPool.BGM1 != null && INFINITY.HardMode == 0)
                {
                    ReflectionHelper.SetField(musicChannelInfo, "clip", ItemPool.BGM1);
                }
                if (ItemPool.BGM2 != null && INFINITY.HardMode == 1)
                {
                    ReflectionHelper.SetField(musicChannelInfo, "clip", ItemPool.BGM2);
                }
            }
        }
        public void On()
        {
            if (gameObject.LocateMyFSM("Control").GetState("Title Up").GetAction<ApplyMusicCue>() != null)
            {
                gameObject.LocateMyFSM("Control").GetState("Title Up").GetAction<ApplyMusicCue>().OnEnter();
            }
            for (int i = 1; i < gameObject.transform.childCount; i++)
            {
                var obj = gameObject.transform.GetChild(i);

                if (obj.name == "White Fader")
                {
                    obj.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
                    obj.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    obj.transform.localPosition = new Vector3(88.726f, 18.1699f, -0.0002f);
                }
                else if (obj.name == "Boss Title")
                {
                    obj.gameObject.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1);
                    obj.gameObject.transform.Find("Boss Title (1)").GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1);
                }
            }
            gameObject.LocateMyFSM("Control").SetState("Flash Up");
        }
       
        public void BattleStart()
        {
            Music3Stop();
        }
       
        public void BattleOver()
        {

            Invoke("Music3Enter", 3f);
        }
        void Music3Enter()
        {
            ItemPool.BossTitle.GetComponent<AudioSource>().PlayOneShot(ItemPool.BGM3, 1f);
        }
        void Music3Stop()
        {
            ItemPool.BossTitle.GetComponent<AudioSource>().enabled = false;
            ItemPool.BossTitle.GetComponent<AudioSource>().enabled = true;
        }
    }
    public class RockPtControl: MonoBehaviour
    {
        public void On()
        {
            gameObject.transform.position = new Vector3(INFINITY.BOSS.transform.position.x, 4.98f, 0.006f);
            gameObject.GetComponent<ParticleSystem>().Play();
        }
    }

    public class CorpseFlash : MonoBehaviour
    {
        System.Random random = new System.Random();
        double R1 => random.NextDouble();
        double R2 => random.NextDouble();
        double R3 => random.NextDouble();

        float freezeTime = 4f;

        float loopTime = 0.15f;

        float loopTime_Orig = 0.15f;

        float minTime = 0.02f;

        float factor1 = 4f;

        float sigh = 1;
        public void Start()
        {
            Flash();
        }
        public void Flash()
        {
            loopTime = loopTime_Orig;

            GameManager.instance.StartCoroutine(GameManager.instance.FreezeMoment(0.1f, freezeTime, 0f, 0.15f));

            FlashOnceLoop();

            Invoke("FlashOnceLoopEnd", freezeTime * 0.15f + 0.1f);
        }
        public void FlashOnceLoop()
        {
            var glow = Instantiate(ItemPool.Glow, HeroController.instance.gameObject.transform.position + new Vector3((-15f + (float)R1 * 15f) * sigh, -10f + (float)R2 * 20f, 0f), Quaternion.Euler(0, 0, 0));

            sigh *= -1;

            glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * (0.5f + (float)R3 * 1.5f);

            glow.SetActive(true);
            glow.AddComponent<DelayDestory>();
            glow.GetComponent<DelayDestory>().On(2f);

            Invoke("FlashOnceLoop", loopTime);

            loopTime += (minTime - loopTime) / factor1;

            HeroController.instance.GetComponent<AudioSource>().PlayOneShot(ItemPool.Teleport, 1f);
        }
        public void FlashOnceLoopEnd()
        {
            BigFlash();
            CancelInvoke("FlashOnceLoop");
        }

        public void BigFlash()
        {
            var glow = Instantiate(ItemPool.Glow, HeroController.instance.gameObject.transform.position, Quaternion.Euler(0, 0, 0));

            glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 6f;

            glow.SetActive(true);
            glow.AddComponent<DelayDestory>();
            glow.GetComponent<DelayDestory>().On(2f);

            HeroController.instance.GetComponent<AudioSource>().PlayOneShot(ItemPool.Explode, 1f);

            Invoke("CamDown", 0.2f);
        }

        public void CamDown()
        {
            CamControl.Cam.GetComponent<CamControl>().CamDown();

            Invoke("CamUp", 4f);

            HeroMove();

            Invoke("HeroReady", 4.3f);

            Invoke("HeroGetUp", 4.5f);
        }

        public void HeroMove()
        {
            GameObject left = null;

            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
            {
                if(obj.name.Contains("Floor") && obj.transform.position.x > 300)
                {
                    if(left == null || obj.transform.position.x < left.transform.position.x)
                    {
                        left = obj;
                    }
                }
            }
            HeroController.instance.transform.position = new Vector3(left.transform.position.x - 2f, 6.4f, HeroController.instance.transform.position.z);

            HeroController.instance.gameObject.LocateMyFSM("Dream Return").SetState("Prostrate");

            HeroController.instance.gameObject.LocateMyFSM("Dream Return").enabled = false;

            HeroController.instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            HeroController.instance.GetComponent<Rigidbody2D>().gravityScale = 0f;

        }

        public void HeroReady()
        {
            HeroController.instance.gameObject.LocateMyFSM("Dream Return").enabled = true;
        }

        public void HeroGetUp()
        {
            HeroController.instance.gameObject.LocateMyFSM("Dream Return").SetState("Prostrate");
        }

        public void CamUp()
        {
            HeroController.instance.GetComponent<Rigidbody2D>().gravityScale = 1f;

            CamControl.Cam.GetComponent<CamControl>().CamUp();
        }
    }
}
