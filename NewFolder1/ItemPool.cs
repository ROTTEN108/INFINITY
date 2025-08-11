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
        public static AudioClip Teleport;
        public static AudioClip Slash;
        public static AudioClip Dir;
        public static AudioClip Land;

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
       
        public void BattleOver()
        {

            Invoke("Music3Enter", 3f);
        }
        void Music3Enter()
        {
            HeroController.instance.gameObject.GetComponent<AudioSource>().pitch = 1f;
            HeroController.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.BGM3, 1f);
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
}
