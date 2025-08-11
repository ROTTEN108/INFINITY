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
using static SceneLoad;
using HKMirror.Reflection.InstanceClasses;
using HKMirror.Hooks.OnHooks;
using static UnityEngine.ParticleSystem;
//using WavLib;

namespace INFINITY
{
    public class SceneControl : MonoBehaviour
    {
        public static GameObject floorPrefab;
        public static GameObject floorEdge;
        public static GameObject symb1;
        public static GameObject symb2;
        public static GameObject symb3;
        public static GameObject symb4;
        public static GameObject haze;
        public static GameObject fountain;
        public static GameObject shield;
        public int shrinkTime = 0;
        public int shrinkTime1 = 0;
        public static bool isInBattle;
        double R1 => INFINITY.random.NextDouble();
        double R2 => INFINITY.random.NextDouble();
        double R3 => INFINITY.random.NextDouble();
        public class SymbControl:MonoBehaviour
        {
            Vector3 LockPos = Vector3.zero;

            public float interval = 0.02f;

            float lastCombinedTime = 0.0f;

            float origY = 0f;

            bool fadeStarted = false;

            bool rised = false;
            void Start()
            {
                origY = gameObject.transform.position.y;
                gameObject.transform.position += new Vector3(0f, -60f, 0f);

                if(gameObject.name == "final_boss_room_0016_tall_symb3 (2)")
                {
                    GameObject[] all = GameObject.FindObjectsOfType<GameObject>();
                    foreach (var obj in all)
                    {
                        if (obj.name == "white_palace_particles")
                        {
                            obj.transform.SetParent(gameObject.transform);
                            obj.transform.localScale /= 5.815f / 2f;
                        }
                        if (obj.name == "glow particles")
                        {
                            obj.transform.SetParent(gameObject.transform);
                            obj.transform.localScale /= 5.815f / 2f;
                        }
                        if (obj.name == "abyss particles")
                        {
                            obj.SetActive(false);
                        }
                    }
                }
            }
            public void On(Vector3 localPos)
            {
                LockPos = localPos;
            }

            public void Rise()
            {
                rised = true;
            }
            void Update()
            {
                if (Time.time - lastCombinedTime > interval)
                {
                    lastCombinedTime = Time.time;
                    if(rised)
                    {
                        gameObject.transform.position += (new Vector3(LockPos.x + HeroController.instance.transform.position.x, origY, LockPos.z) - gameObject.transform.position) * 0.009f;
                    }
                    else
                    {
                        gameObject.transform.position += (new Vector3(LockPos.x + HeroController.instance.transform.position.x, origY - 60f, LockPos.z) - gameObject.transform.position) * 1f;
                    }
                }
                if(fadeStarted)
                {
                    gameObject.transform.localScale += new Vector3(0.0025f, 0.0025f, 0f);
                    gameObject.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.0045f);
                }
            }

            public void Fade()
            {
                fadeStarted = true;
            }
        }
        public void Phase2()
        {
            if(symb1 != null)
            {
                symb1.GetComponent<SymbControl>().Fade();
            }
            if(haze != null)
            {
                haze.GetComponent<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 0.19f);
            }
            if(ItemPool.PhaseFlash != null)
            {
                ItemPool.PhaseFlash.SetActive(true);
            }
        }
        public void Phase3()
        {
            if(symb2 != null)
            {
                symb2.GetComponent<SymbControl>().Fade();
            }
            if(haze != null)
            {
                haze.GetComponent<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 0.19f);
            }
            if (ItemPool.PhaseFlash != null)
            {
                ItemPool.PhaseFlash.SetActive(true);
            }
        }
        public void Phase4()
        {
            if(symb3 != null)
            {
                symb3.GetComponent<SymbControl>().Fade();
            }
            if(haze != null)
            {
                haze.GetComponent<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 0.19f);
            }
            if (ItemPool.PhaseFlash != null)
            {
                ItemPool.PhaseFlash.SetActive(true);
            }
        }
        public void Phase5()
        {
            if(symb4 != null)
            {
                symb4.GetComponent<SymbControl>().Fade();
            }
            if(haze != null)
            {
                haze.GetComponent<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 0.19f);
            }
            if (ItemPool.PhaseFlash != null)
            {
                ItemPool.PhaseFlash.SetActive(true);
            }
        }
        public void SpikePlace()
        {
            var spike = Instantiate(ItemPool.Spike, new Vector3(-10, -5, 0), Quaternion.Euler(0, 0, 0));
            spike.transform.localScale = new Vector3(1, 1, 1);
            spike.GetComponent<PolygonCollider2D>().points = new Vector2[] { new Vector2(0, 0), new Vector2(5000, 0), new Vector2(5000, -10), new Vector2(0, -10) };
            spike.GetComponent<DamageHero>().damageDealt = 9999;
            spike.SetActive(true);
        }
        public static void Detect()
        {
            if(INFINITY.BOSS != null)
            {
                INFINITY.BOSS.GetComponent<SkillsControl>().AnimFreezeTime(10000f, 0f);
                INFINITY.BOSS.transform.position += new Vector3(0f, -200f, 0f);
            }

            var spike = Instantiate(ItemPool.Spike, new Vector3(-10, -5, 0), Quaternion.Euler(0, 0, 0));
            spike.transform.localScale = new Vector3(1, 1, 1);
            spike.GetComponent<PolygonCollider2D>().points = new Vector2[] { new Vector2(0, 0), new Vector2(5000, 0), new Vector2(5000, -10), new Vector2(0, -10) };
            spike.GetComponent<DamageHero>().damageDealt = 9999;
            spike.SetActive(true);

            fountain = Instantiate(ItemPool.Scene_Fountain, new Vector3(85.4839f, 12.8881f, 2.7458f), Quaternion.Euler(0, 0, 0));
            fountain.SetActive(true);

            if (fountain.GetComponent<FountainControl>() == null)
            {
                fountain.AddComponent<FountainControl>();
            }

            var ball = Instantiate(ItemPool.Scene_Shield, fountain.transform);
            ball.transform.localPosition = new Vector3(31.9745f, -52.5968f, -1.166f);
            shield = ball;

            GameObject[] all = GameObject.FindObjectsOfType<GameObject>();
            foreach (var obj in all)
            {
                if(obj.name == "white_palace_particles")
                {
                    ItemPool.Scene_Particle_1 = Instantiate(obj, fountain.transform);
                    ItemPool.Scene_Particle_1.transform.localPosition = new Vector3(-2.312f, 18.8881f, -1.2943f);
                    ItemPool.Scene_Particle_1.transform.eulerAngles = Vector3.zero;
                    ItemPool.Scene_Particle_1.transform.localScale = new Vector3(16f, 0.5f, 0.1f);
                    ItemPool.Scene_Particle_1.transform.Find("Particle System BG").localScale = new Vector3(1f, 1f, 0.1f);
                    ItemPool.Scene_Particle_1.transform.Find("Particle System BG").gameObject.GetComponent<ParticleSystem>().emissionRate = 25f * 2;
                    ItemPool.Scene_Particle_1.transform.Find("Particle System BG").gameObject.GetComponent<ParticleSystem>().maxParticles = 30 * 2;

                    ItemPool.Scene_Particle_2 = Instantiate(obj, fountain.transform);
                    ItemPool.Scene_Particle_2.transform.localPosition = new Vector3(-2.112f, 16.8881f, -1.2943f);
                    ItemPool.Scene_Particle_2.transform.eulerAngles = Vector3.zero;
                    ItemPool.Scene_Particle_2.transform.localScale = new Vector3(5f, 0.5f, 0.1f);
                    ItemPool.Scene_Particle_2.transform.Find("Particle System BG").localScale = new Vector3(1f, 1f, 0.1f);
                    if(INFINITY.settings_.skillGot)
                    {
                        ItemPool.Scene_Particle_2.transform.Find("Particle System BG").gameObject.GetComponent<ParticleSystem>().emissionRate = 14f * 2;
                    }
                    else
                    {
                        ItemPool.Scene_Particle_2.transform.Find("Particle System BG").gameObject.GetComponent<ParticleSystem>().emissionRate = 0f;
                    }
                    ItemPool.Scene_Particle_2.transform.Find("Particle System BG").gameObject.GetComponent<ParticleSystem>().maxParticles = 10000;
                    ItemPool.Scene_Particle_2.transform.Find("Particle System BG").gameObject.GetComponent<ParticleSystem>().gravityModifier = -1.5f;
                    ItemPool.Scene_Particle_2.transform.Find("Particle System BG").gameObject.GetComponent<ParticleSystem>().startLifetime = 0.1f;
                    ItemPool.Scene_Particle_2.transform.Find("Particle System BG").gameObject.GetComponent<ParticleSystem>().playbackSpeed = 4f;
                    ItemPool.Scene_Particle_2.transform.Find("Particle System BG").gameObject.GetComponent<ParticleSystem>().startSize = 0.8f;
                }
                if(obj.name.Contains("symb"))
                {
                    var symb = obj.AddComponent<SymbControl>();
                    Vector3 pos = obj.transform.position - HeroController.instance.transform.position;
                    if(obj.name == "final_boss_room_0016_tall_symb3 (1)")
                    {
                        pos += new Vector3(-5.5f, 0f, 0f);
                        symb3 = obj;
                    }
                    if(obj.name == "final_boss_room_0016_tall_symb3")
                    {
                        pos += new Vector3(-5.5f, 0f, 0f);
                        symb2 = obj;
                    }
                    if(obj.name == "final_boss_room_0016_tall_symb3 (2)")
                    {
                        symb4 = obj;
                    }
                    if(obj.name == "final_boss_room_0016_tall_symb3 (3)")
                    {
                        symb1 = obj;
                    }
                    symb.On(pos);
                }
                if(obj.name.Contains("Battle Scene"))
                {
                    ItemPool.PhaseFlash = obj.transform.Find("Tendril Flash").gameObject;
                    ItemPool.PhaseFlash.transform.localScale = new Vector3(10000, 200, 1);
                    obj.transform.Find("CameraLockArea Intro").gameObject.Recycle();
                }
                if(obj.name.Contains("Chains"))
                {
                    obj.SetActive(false);
                }
                if(obj.name.Contains("GG_Arena_Prefab"))
                {
                    ItemPool.Phase5Arena = obj;
                    var bg = obj.transform.Find("BG").gameObject;
                    bg.transform.Find("haze2 (3)").localScale = new Vector3(10000f, 6.7845f, 1f);
                    bg.transform.Find("haze2").localScale = new Vector3(10000f, 0.4536f, 6.8671f);
                    for(int i = 0;i < bg.transform.childCount;i++)
                    {
                        var obj1 = bg.transform.GetChild(i);
                        if (obj1.name == "haze2 (3)")
                        {
                            obj1.localScale = new Vector3(10000f, 6.7845f, 1f);
                            haze = obj1.gameObject;
                            haze.GetComponent<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 1f);
                        }
                        else if(obj1.name == "haze2")
                        {
                            obj1.localScale = new Vector3(10000f, 0.4536f, 6.8671f);
                        }
                        else if(obj1.name == "BlurPlane")
                        {
                            obj1.localScale = new Vector3(obj1.transform.localScale.x, obj1.transform.localScale.y, 6000f);
                        }
                        else
                        {
                            obj1.gameObject.SetActive(false);
                        }
                    }
                }
                if (obj.name.Contains("gg_pillar_corner_large") || obj.name.Contains("SceneBorder") || obj.name.Contains("clouds") || obj.name.Contains("Lock Zone") || obj.name.Contains("HK_glow_wall") || obj.name.Contains("GG_Arena_PrefabGG") || obj.name.Contains("Edge") || obj.name.Contains("Enemy Wall"))
                {
                    obj.Recycle();
                }
                if (obj.name.Contains("Death Break Chains"))
                {
                    //obj.transform.localScale *= 2;
                }
                if (obj.name.Contains("Battle"))
                {
                    var godseeker = obj.transform.Find("Godseeker Crowd").gameObject;
                    godseeker?.Recycle();
                    var chunks = obj.transform.Find("HK_Prime_Burst_Chunks").gameObject;
                    chunks?.Recycle();
                }
                if (obj.name.Contains("TileMap Render Data"))
                {
                    var sceneMap = obj.transform.Find("Scenemap").gameObject;
                    var chunk0 = sceneMap.transform.Find("Chunk 0 0").gameObject;
                    var chunk1 = sceneMap.transform.Find("Chunk 0 1").gameObject;
                    chunk1.GetComponent<MeshRenderer>().enabled = false;
                    chunk0.Recycle();
                    EdgeCollider2D[] edgeCollider2Ds = chunk1.GetComponents<EdgeCollider2D>();
                    foreach (var edgeCollider2D in edgeCollider2Ds)
                    {
                        if (edgeCollider2D.edgeCount == 5)
                        {
                            HeroController.instance.transform.position += new Vector3(0f, 0.5f, 0f);
                            Vector2[] vectorArray = { new Vector2(-23, 5), new Vector2(3, 5), new Vector2(3, -5), new Vector2(-23, -5) };
                            edgeCollider2D.points = vectorArray;
                            ItemPool.Floor = chunk1;
                        }
                        else
                        {
                            edgeCollider2D.enabled = false;
                        }
                    }
                }
                if (obj.name.Contains("CameraLockArea"))
                {
                    obj.SetActive(false);
                    obj.SetActive(true);
                    //obj.SetActive(false);
                    var lockArena = obj.GetComponent<CameraLockArea>();
                    var box = obj.GetComponent<BoxCollider2D>();
                    lockArena.cameraXMax = 100000f;
                    lockArena.cameraXMin = -100000f;
                    lockArena.cameraYMax = 1000f;
                    lockArena.cameraYMin = 12.5f;
                    box.size = new Vector2(30000f, 24.5745f);
                }
                if (obj.name.Contains("HK Prime"))
                {
                    obj.GetComponent<ConstrainPosition>().constrainX = false;
                }
            }
        }

        public class FountainControl : MonoBehaviour
        {
            bool detectOn = true;
            bool detectOn1 = true;
            bool detectOn2 = false;
            bool shieldOn = false;
            GameObject bugsPt = null;
            public GameObject title = null;
            public void Start()
            {
                if (ItemPool.BossTitle != null)
                {
                    title = Instantiate(ItemPool.BossTitle, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));

                    title.LocateMyFSM("Control").GetState("Wait for Hero Pos").AddMethod(() =>
                    {
                        title.LocateMyFSM("Control").SetState("Battle Start");
                    });
                    title.LocateMyFSM("Control").GetState("Battle Start").RemoveAction(0);
                    title.LocateMyFSM("Control").GetState("Battle Start").RemoveAction(1);
                    title.LocateMyFSM("Control").GetState("Battle Start").RemoveAction(2);
                    title.LocateMyFSM("Control").GetState("Battle Start").RemoveAction(3);
                    title.LocateMyFSM("Control").GetState("Battle Start").RemoveAction(4);
                    title.LocateMyFSM("Control").GetState("Battle Start").RemoveAction(5);
                    title.LocateMyFSM("Control").GetState("Title Up").GetAction<Wait>().time = 1.5f;
                }

                DetectOn();

                if(ItemPool.Scene_Bugs_Pt != null)
                {
                    bugsPt = Instantiate(ItemPool.Scene_Bugs_Pt, gameObject.transform);
                    bugsPt.transform.localPosition = new Vector3(-2.5835f, -6.0369f, -1.2874f);
                    bugsPt.transform.localScale = new Vector3(8, 1, 1);
                    bugsPt.SetActive(true);
                    bugsPt.GetComponent<ParticleSystem>().emissionRate = 5;
                    bugsPt.GetComponent<ParticleSystem>().maxParticles = 500;
                    bugsPt.GetComponent<ParticleSystem>().loop = true;
                    bugsPt.GetComponent<ParticleSystem>().Play();

                    ItemPool.Boss_Bugs_Pt = Instantiate(ItemPool.Scene_Bugs_Pt, INFINITY.BOSS.transform);
                    ItemPool.Boss_Bugs_Pt.transform.localScale = new Vector3(2f, 1f, 5f);
                    ItemPool.Boss_Bugs_Pt.transform.eulerAngles = new Vector3(0, 0, 90f);
                    ItemPool.Boss_Bugs_Pt.transform.localPosition = new Vector3(-1f, -1f, 0f);
                    ItemPool.Boss_Bugs_Pt.SetActive(true);
                    ItemPool.Boss_Bugs_Pt.GetComponent<ParticleSystem>().emissionRate = 5;
                    ItemPool.Boss_Bugs_Pt.GetComponent<ParticleSystem>().maxParticles = 10000;
                    ItemPool.Boss_Bugs_Pt.GetComponent<ParticleSystem>().loop = true;
                    ItemPool.Boss_Bugs_Pt.GetComponent<ParticleSystem>().Play();

                    ItemPool.Boss_Bugs_Pt2 = Instantiate(ItemPool.Boss_Bugs_Pt, INFINITY.BOSS.transform);
                    ItemPool.Boss_Bugs_Pt2.transform.localScale = new Vector3(70f, 1f, 5f);
                    ItemPool.Boss_Bugs_Pt2.transform.eulerAngles = new Vector3(0, 0, 0f);
                    ItemPool.Boss_Bugs_Pt2.transform.localPosition = new Vector3(0f, -5f, 0f);
                    ItemPool.Boss_Bugs_Pt2.GetComponent<ParticleSystem>().loop = false;
                    ItemPool.Boss_Bugs_Pt2.SetActive(true);
                    ItemPool.Boss_Bugs_Pt2.GetComponent<ParticleSystem>().emissionRate = 2500;
                    ItemPool.Boss_Bugs_Pt2.GetComponent<ParticleSystem>().maxParticles = 10000;
                    ItemPool.Boss_Bugs_Pt2.GetComponent<ParticleSystem>().startSpeed = 20f;
                    ItemPool.Boss_Bugs_Pt2.GetComponent<ParticleSystem>().startLifetime = 1.5f;
                    ItemPool.Boss_Bugs_Pt2.GetComponent<ParticleSystem>().gravityModifier = 0.5f;
                }
            }
            public void DetectOn()
            {
                detectOn = true;
                detectOn1 = true;
            }
            public void DetectOn2()
            {
                detectOn2 = true;
            }
            public void DetectOff()
            {
                detectOn = false;
                detectOn1 = false;
            }
            public void Detect2Off()
            {
                detectOn2 = false;
            }
            public void Update()
            {
                if(detectOn && gameObject.transform.position.x - HeroController.instance.gameObject.transform.position.x <= 11)
                {
                    DetectOff();

                    TitleOn();
                    Invoke("ShieldOff", 5f);
                }
                if(!shieldOn && gameObject.transform.position.x - HeroController.instance.gameObject.transform.position.x <= 20)
                {
                    shieldOn = true;
                    ShieldOn();
                    SymbRise();
                    HazeOn();
                }
                if(detectOn2 && gameObject.transform.position.x - HeroController.instance.gameObject.transform.position.x <= 2)
                {
                    Detect2Off();
                    INFINITY.BOSS.GetComponent<SkillsControl>().AnimFreezeEnd();
                    INFINITY.BOSS.GetComponent<AngleSystem>().FaceToPlayerEnd();
                    INFINITY.BOSS.GetComponent<LockAngleWaitForAttack>().EndLock();
                    INFINITY.BOSS.SetActive(true);
                    INFINITY.BOSS.GetComponent<SkillsControl>().SkillPhase5();
                }
            }
            public void HazeOn()
            {
                if (haze != null)
                {
                    haze.GetComponent<SpriteRenderer>().color += new Color(0f, 0f, 0f, 1f);
                }
            }
            public void SymbRise()
            {
                ParticleBurst();

                GameObject[] all = GameObject.FindObjectsOfType<GameObject>();
                foreach (var obj in all)
                {
                    if (obj.name.Contains("symb") && obj.GetComponent<SymbControl>() != null)
                    {
                        obj.GetComponent<SymbControl>().Rise();
                    }
                }
            }
            public void TitleOn()
            {
                title.GetComponent<BossTitleControl>().On();

                //INFINITY.BOSS.LocateMyFSM("Control").GetState("Intro Roar").GetAction<ApplyMusicCue>().OnEnter();

                Invoke("FountainMove", 1f);
                Invoke("BattleStart", 3f);
            }
            public void FountainMove()
            {
                gameObject.transform.position += new Vector3(0f, -200f, 0f);
            }
            public void BattleStart()
            {

                if (ItemPool.PhaseFlash != null)
                {
                    ItemPool.PhaseFlash.SetActive(true);
                }

                INFINITY.BOSS.GetComponent<SkillsControl>().AnimFreezeEnd();
                INFINITY.BOSS.LocateMyFSM("Control").SetState("Idle Stance");

                for(int i = 0;i < 3;i++)
                {
                    Vector3 localPosR = new Vector3(16.9f + 10f * i, -4.4836f, -2.8909f);
                    Vector3 localPosL = new Vector3(-16.9f - 10f * i, -4.4836f, -2.8909f);
                    var lampR = Instantiate(ItemPool.Scene_Light_4, gameObject.transform.position + localPosR, Quaternion.Euler(0, 0, 0));
                    var lampL = Instantiate(ItemPool.Scene_Light_4, gameObject.transform.position + localPosL, Quaternion.Euler(0, 0, 0));
                }
            }
            public void ParticleBurst()
            {
                ItemPool.Scene_Particle_2.transform.Find("Particle System BG").gameObject.GetComponent<ParticleSystem>().emissionRate = 1000f;
                ParticleReduceLoop();
                Invoke("ParticleReduceLoopEnd", 1f);
            }
            void ParticleReduceLoop()
            {
                ItemPool.Scene_Particle_2.transform.Find("Particle System BG").gameObject.GetComponent<ParticleSystem>().emissionRate -= 20f;
                Invoke("ParticleReduceLoop", 0.02f);
            }
            void ParticleReduceLoopEnd()
            {
                if(INFINITY.settings_.skillGot)
                {
                    ItemPool.Scene_Particle_2.transform.Find("Particle System BG").gameObject.GetComponent<ParticleSystem>().emissionRate = 14;
                }
                else
                {
                    ItemPool.Scene_Particle_2.transform.Find("Particle System BG").gameObject.GetComponent<ParticleSystem>().emissionRate = 0;
                }
                CancelInvoke("ParticleReduceLoop");
            }

            public void ShieldOn()
            {
                shield.SetActive(true);
            }
            public void ShieldOff()
            {
                shield.SetActive(false);
            }
        }
        float x = -32.59f;
        int i1 = 0;
        int i2 = 0;
        int i11 = 0;
        int i21 = 0;
        public void FloorAutoPlaceLoop()
        {
            Modding.Logger.Log("Floor auto place started!");
            if (HeroController.instance.gameObject.transform.position.x >= x + i1 * 20f)
            {
                Modding.Logger.Log("Floor auto place right one");
                i1++;
                Instantiate(ItemPool.Floor1, new Vector3(x + i1 * 20f + 200f, 10.01f, 32.4219f), Quaternion.Euler(0, 0, 0));
            }
            if (HeroController.instance.gameObject.transform.position.x <= x - i2 * 20f)
            {
                Modding.Logger.Log("Floor auto place left one");
                i2++;
                Instantiate(ItemPool.Floor1, new Vector3(x - i2 * 20f - 200f, 10.01f, 32.4219f), Quaternion.Euler(0, 0, 0));
            }
            Invoke("FloorAutoPlaceLoop", 0.5f);
        }
        float plumeStartX = 0f;
        public void PlumeAutoPlaceStart(float startX)
        {
            plumeStartX = startX;
            PlumeAutoPlaceLoop();
            //Invoke("PlumeAutoPlaceLoopMore", 1f);
        }
        public void PlumeAutoPlaceEnd()
        {
            CancelInvoke("PlumeAutoPlaceLoop");
            CancelInvoke("PlumeAutoPlaceLoopMore");
            Invoke("AllPlumeDown", 0.5f);
        }
        public void PlumeAutoPlaceLoop()
        {
            Invoke("PlumeAutoPlaceLoop", 0.1f);

            while (HeroController.instance.gameObject.transform.position.x >= plumeStartX + i11 * 2f)
            {
                i11++;
                float x = plumeStartX + i11 * 2f + 50;
                GameObject plume = GameObject.Instantiate(ItemPool.Plume, ItemPool.Plumes.transform);
                plume.transform.position = new Vector3(x, 2f, 0);

                plume.transform.localEulerAngles += new Vector3(0, 0, 0);
                plume.LocateMyFSM("FSM").FsmVariables.FindFsmBool("Auto").Value = true;
                plume.LocateMyFSM("FSM").ChangeTransition("Outside Arena?", "OUTSIDE", "Antic");
                plume.SetActive(true);
                plume.AddComponent<PlumeFall>();
                StartCoroutine(DelayedExecution1(1.2f));
                IEnumerator DelayedExecution1(float time)
                {
                    yield return new WaitForSeconds(time);
                    plume.GetComponent<tk2dSpriteAnimator>().Pause();
                }
            }
            while (HeroController.instance.gameObject.transform.position.x < plumeStartX + i11 * 2f - 2f)
            {
                i11--;
            }
            while (HeroController.instance.gameObject.transform.position.x <= plumeStartX - i21 * 2f)
            {
                i21++;
                float x = plumeStartX - i21 * 2f - 50;
                GameObject plume = GameObject.Instantiate(ItemPool.Plume, ItemPool.Plumes.transform);
                plume.transform.position = new Vector3(x, 2f, 0);

                plume.transform.localEulerAngles += new Vector3(0, 0, 0);
                plume.LocateMyFSM("FSM").FsmVariables.FindFsmBool("Auto").Value = true;
                plume.LocateMyFSM("FSM").ChangeTransition("Outside Arena?", "OUTSIDE", "Antic");
                plume.SetActive(true);
                plume.AddComponent<PlumeFall>();
                StartCoroutine(DelayedExecution1(1.2f));
                IEnumerator DelayedExecution1(float time)
                {
                    yield return new WaitForSeconds(time);
                    plume.GetComponent<tk2dSpriteAnimator>().Pause();
                }
            }
            while (HeroController.instance.gameObject.transform.position.x >= plumeStartX - i21 * 2f + 2f)
            {
                i21--;
            }

            for (int i = 0; i<ItemPool.Plumes.transform.childCount; i++)
            {
                var obj = ItemPool.Plumes.transform.GetChild(i);
                float x = obj.position.x;
                
                if(x > HeroController.instance.gameObject.transform.position.x + 52)
                {
                    var fall = obj.gameObject.GetComponent<PlumeFall>();
                    if (fall != null)
                    {
                        fall.OnWithoutPt();
                    }
                }
                if(x < HeroController.instance.gameObject.transform.position.x - 52)
                {
                    var fall = obj.gameObject.GetComponent<PlumeFall>();
                    if (fall != null)
                    {
                        fall.OnWithoutPt();
                    }
                }
            }
        }
        public void PlumeAutoPlaceLoopMore()
        {
            Invoke("PlumeAutoPlaceLoopMore", 0.1f);

            if (HeroController.instance.gameObject.transform.position.x >= plumeStartX + i11 * 4f + 2)
            {
                i11++;
                float x = plumeStartX + i11 * 4f;
                GameObject plume = GameObject.Instantiate(ItemPool.Plume, ItemPool.Plumes.transform);
                plume.transform.position = new Vector3(x, 2f, 0);

                plume.transform.localEulerAngles += new Vector3(0, 0, 0);
                plume.LocateMyFSM("FSM").FsmVariables.FindFsmBool("Auto").Value = true;
                plume.LocateMyFSM("FSM").ChangeTransition("Outside Arena?", "OUTSIDE", "Antic");
                plume.SetActive(true);
                plume.AddComponent<PlumeFall>();
                StartCoroutine(DelayedExecution1(1.2f));
                IEnumerator DelayedExecution1(float time)
                {
                    yield return new WaitForSeconds(time);
                    plume.GetComponent<tk2dSpriteAnimator>().Pause();
                }
            }
            if (HeroController.instance.gameObject.transform.position.x <= plumeStartX - i21 * 4f - 2)
            {
                i21++;
                float x = plumeStartX + i11 * 4f;
                GameObject plume = GameObject.Instantiate(ItemPool.Plume, ItemPool.Plumes.transform);
                plume.transform.position = new Vector3(x, 2f, 0);

                plume.transform.localEulerAngles += new Vector3(0, 0, 0);
                plume.LocateMyFSM("FSM").FsmVariables.FindFsmBool("Auto").Value = true;
                plume.LocateMyFSM("FSM").ChangeTransition("Outside Arena?", "OUTSIDE", "Antic");
                plume.SetActive(true);
                plume.AddComponent<PlumeFall>();
                StartCoroutine(DelayedExecution1(1.2f));
                IEnumerator DelayedExecution1(float time)
                {
                    yield return new WaitForSeconds(time);
                    plume.GetComponent<tk2dSpriteAnimator>().Pause();
                }
            }
        }
        static float xEdge = 0f;
        static float xEdgePlumeL = 0f;
        static float xEdgePlumeR = 0f;
        float xEdgeStart = 0f;
        public void EdgeShrinkStart()
        {
            //PlayerEnterPhase5PlaceDetectLoop();

            xEdgeStart = gameObject.transform.position.x - 20;
            CancelInvoke("EdgeShrinkLoop");
            EdgeShrinkLoop();
        }
        public void EdgeShrinkStartHard()
        {
            PlayerEnterPhase5PlaceDetectLoop();

            xEdgeStart = gameObject.transform.position.x;
            CancelInvoke("EdgeShrinkHardLoop");
            EdgeShrinkHardLoop();

        }
        public void EdgeShrinkLoop()
        {
            if(INFINITY.HardMode == 0)
            {
                Invoke("EdgeShrinkLoop", 3f);
            }
            else if(INFINITY.HardMode == 1)
            {
                Invoke("EdgeShrinkLoop", 1f);
            }

            shrinkTime++;
            xEdge = xEdgeStart + 20 * shrinkTime;

            GameObject[] all = GameObject.FindObjectsOfType<GameObject>();

            foreach (var obj in all)
            {
                if (obj.name.Contains("Floor") && obj != ItemPool.Floor1)
                {
                    float xFloor = obj.transform.position.x;
                    if(xFloor <= xEdge && xFloor < INFINITY.BOSS.GetComponent<Phase5Arena>().phase4X + 480)
                    {
                        if(obj.GetComponent<FloorFall>() != null)
                        {
                            obj.GetComponent<FloorFall>().On();
                        }
                    }
                    if (xFloor <= xEdge + 20 && xFloor < INFINITY.BOSS.GetComponent<Phase5Arena>().phase4X + 480)
                    {
                        if (obj.GetComponent<FloorFall>() != null)
                        {
                            obj.GetComponent<FloorFall>().PtStart();
                        }
                    }
                }
            }
        }
        public void AllPlumeDown()
        {
            CancelInvoke("EdgeShrinkHardLoop");

            if(ItemPool.Plumes != null)
            {
                ItemPool.Plumes.transform.position += new Vector3(0, -500, 0);
            }
        }
        public void EdgeShrinkHardLoop()
        {
            Invoke("EdgeShrinkHardLoop", 0.26f);
            xEdgePlumeR = xEdgeStart + 2 * shrinkTime1 + 4;
            xEdgePlumeL = xEdgeStart - 2 * shrinkTime1 - 4;
            shrinkTime1++;

            GameObject[] all = GameObject.FindObjectsOfType<GameObject>();

            foreach (var obj in all)
            {
                if (obj.name.Contains("Plume") && obj != ItemPool.Plume)
                {
                    float xFloor = obj.transform.position.x;
                    if(xFloor <= xEdgePlumeR && xFloor >= xEdgePlumeL && xFloor < INFINITY.BOSS.GetComponent<Phase5Arena>().phase4X + 480)
                    {
                        if(obj.GetComponent<PlumeFall>() != null)
                        {
                            obj.GetComponent<PlumeFall>().On();
                        }
                    }
                }
            }
        }
        public bool Phase5Started = false;
        public void PlayerEnterPhase5PlaceDetectLoop()
        {
            if(HeroController.instance.gameObject.transform.position.x >= gameObject.GetComponent<Phase5Arena>().phase4X + 490)
            {
                gameObject.GetComponent<Phase5Arena>().Phase4InvincibleEnd();
                Phase5Started = true;
                PlumeAutoPlaceEnd();

                fountain.GetComponent<FountainControl>().DetectOn2(); ;
                gameObject.GetComponent<Skills>().LineAppear(6);

            }
            else
            {
                Invoke("PlayerEnterPhase5PlaceDetectLoop", 0.2f);
                Phase5Started = false;
            }
        }
        float destroyX = 0;
        public void Phase3PlumeDetroy()
        {
            float destroyX = gameObject.transform.position.x;

            GameObject[] all = GameObject.FindObjectsOfType<GameObject>();

            foreach (var obj in all)
            {
                if (obj.name.Contains("Plume") && obj != ItemPool.Plume)
                {
                    float xFloor = obj.transform.position.x;
                    if (xFloor <= destroyX + 4 && xFloor >= destroyX - 4)
                    {
                        if (obj.GetComponent<PlumeFall>() != null)
                        {
                            obj.GetComponent<PlumeFall>().On();
                        }
                    }
                }
            }
            Invoke("Phase3PlumeDetroyRepeat", 0.3f);
        }
        public void Phase3PlumeDetroyRepeat()
        {
            GameObject[] all = GameObject.FindObjectsOfType<GameObject>();

            foreach (var obj in all)
            {
                if (obj.name.Contains("Plume") && obj != ItemPool.Plume)
                {
                    float xFloor = obj.transform.position.x;
                    if (xFloor <= destroyX + 6 && xFloor >= destroyX - 6)
                    {
                        if (obj.GetComponent<PlumeFall>() != null)
                        {
                            obj.GetComponent<PlumeFall>().On();
                        }
                    }
                }
            }
        }
        public class StatueSwich : MonoBehaviour
        {
            void Start()
            {
                var toggle = gameObject.GetComponent<BossStatueDreamToggle>().Reflect();

                if (INFINITY.HardMode == 1)
                {
                    StartCoroutine(toggle.Fade(true));
                }
            }
            public void OnTriggerEnter2D(Collider2D collision)
            {
                if (collision.tag == "Dream Attack")
                {
                    Switch();
                }
            }

            void Switch()
            {
                var toggle = gameObject.GetComponent<BossStatueDreamToggle>().Reflect();

                bool flag = false;

                if (INFINITY.HardMode == 0)
                {
                    flag = true;

                    INFINITY.HardMode = 1;

                    if(ItemPool.Scene_StatueBugs_Pt1)
                    {
                        ItemPool.Scene_StatueBugs_Pt1.GetComponent<ParticleSystem>().loop = true;
                        ItemPool.Scene_StatueBugs_Pt1.GetComponent<ParticleSystem>().Play();
                    }
                    if(ItemPool.Scene_StatueBugs_Pt2)
                    {
                        ItemPool.Scene_StatueBugs_Pt2.GetComponent<ParticleSystem>().Play();
                    }
                }
                else if (INFINITY.HardMode == 1)
                {
                    flag = false;

                    INFINITY.HardMode = 0;

                    if (ItemPool.Scene_StatueBugs_Pt1)
                    {
                        ItemPool.Scene_StatueBugs_Pt1.GetComponent<ParticleSystem>().loop = false;
                    }
                }

                if (toggle.dreamImpactPoint && toggle.dreamImpactPrefab)
                {
                    toggle.dreamImpactPrefab.Spawn(toggle.dreamImpactPoint.position).transform.localScale = toggle.dreamImpactScale;
                }
                if (toggle.dreamBurstEffect)
                {
                    toggle.dreamBurstEffect.SetActive(flag);
                }
                if (toggle.dreamBurstEffectOff)
                {
                    toggle.dreamBurstEffectOff.SetActive(!flag);
                }
                StartCoroutine(toggle.Fade(flag));
            }
        }

        public static GameObject switch1 = null;

        public class SymbControlWorkshop : MonoBehaviour
        {
            Vector3 ShortPos = Vector3.zero;
            Vector3 TallPos = Vector3.zero;
            Vector3 MiddlePos = Vector3.zero;
            Vector3 AimPos = Vector3.zero;
            double R1 => INFINITY.random.NextDouble();

            float r = 0f;
            public void Start()
            {
                ShortPos = gameObject.transform.position += new Vector3(0, -10, 0);
                TallPos = ShortPos + new Vector3(0, 10, 0);
                MiddlePos = ShortPos + new Vector3(0, 6, 0);

                if (INFINITY.HardMode == 0)
                {
                    gameObject.transform.position = MiddlePos;
                }
                else
                {
                    gameObject.transform.position = TallPos;
                }

                Loop();
                FastLoop();
                Invoke("FastLoopEnd", 0.5f);
            }
            public void FastLoop()
            {
                gameObject.transform.position = AimPos;

                Invoke("FastLoop", 0.02f);
            }
            public void FastLoopEnd()
            {
                CancelInvoke("FastLoop");
            }
            public void UpStart()
            {
                if (AimPos != TallPos)
                {
                    r = (float)R1;
                    AimPos = TallPos;
                }
            }
            public void DownStart()
            {
                if(AimPos != ShortPos)
                {
                    r = (float)R1;
                    AimPos = ShortPos;
                }
            }
            public void MiddleStart()
            {
                if (AimPos != MiddlePos)
                {
                    r = (float)R1;
                    AimPos = MiddlePos;
                }
            }
            void Loop()
            {
                gameObject.transform.position += (AimPos - gameObject.transform.position) * (float)(0.015f + r * 0.02);

                Invoke("Loop", 0.02f);
            }

            public void Update()
            {
                float distanceX = Math.Abs(gameObject.transform.position.x - HeroController.instance.transform.position.x);
                if(distanceX <= 8.5f)
                {
                    if(INFINITY.HardMode == 0)
                    {
                        MiddleStart();
                    }
                    else
                    {
                        UpStart();
                    }
                }
                else
                {
                    DownStart();
                }
            }
        }
        public class SceneSwitchDetector : MonoBehaviour
        {
            public void WorkshopChange()
            {
                GameObject[] all = GameObject.FindObjectsOfType<GameObject>();

                ItemPool.Scene_StatueSymb1 = Instantiate(ItemPool.Scene_StatueSymb1_prefab, new Vector3(181.6192f, 38.6782f, 2.504f), Quaternion.Euler(0, 0, 0));
                ItemPool.Scene_StatueSymb2 = Instantiate(ItemPool.Scene_StatueSymb2_prefab, new Vector3(182.9035f, 39.4328f, 2.504f), Quaternion.Euler(0, 0, 0));
                ItemPool.Scene_StatueSymb3 = Instantiate(ItemPool.Scene_StatueSymb3_prefab, new Vector3(186.4786f, 39.4328f, 2.504f), Quaternion.Euler(0, 0, 0));
                ItemPool.Scene_StatueSymb4 = Instantiate(ItemPool.Scene_StatueSymb4_prefab, new Vector3(184.6307f, 39.4328f, 2.504f), Quaternion.Euler(0, 0, 0));

                ItemPool.Scene_StatueSymb1.SetActive(true);
                ItemPool.Scene_StatueSymb2.SetActive(true);
                ItemPool.Scene_StatueSymb3.SetActive(true);
                ItemPool.Scene_StatueSymb4.SetActive(true);

                ItemPool.Scene_StatueSymb1.transform.localScale = new Vector3(-1.1f, 1, 1);
                ItemPool.Scene_StatueSymb2.transform.localScale = new Vector3(1, 1, 1);
                ItemPool.Scene_StatueSymb3.transform.localScale = new Vector3(1, 1, 1);
                ItemPool.Scene_StatueSymb4.transform.localScale = new Vector3(1, 1, 1);

                ItemPool.Scene_StatueSymb1.AddComponent<SymbControlWorkshop>();
                ItemPool.Scene_StatueSymb2.AddComponent<SymbControlWorkshop>();
                ItemPool.Scene_StatueSymb3.AddComponent<SymbControlWorkshop>();
                ItemPool.Scene_StatueSymb4.AddComponent<SymbControlWorkshop>();

                foreach (var obj in all)
                {
                    if (obj.name == "GG_Statue_HollowKnight")
                    {
                        var b = obj.transform.Find("Base").gameObject;
                        b.transform.Find("Plaque").gameObject.SetActive(false);
                        b.transform.Find("Statue").gameObject.SetActive(false);
                    }
                    if (obj.name == "GG_Statue_Defender")
                    {
                        switch1 = Instantiate(obj.transform.Find("dream_version_switch").gameObject, new Vector3(180.9889f, 35.86f, 0.3f), Quaternion.Euler(0, 0, 0));
                        switch1.transform.Find("GG_statue_plinth_dream").gameObject.SetActive(false);
                        switch1.transform.Find("Statue Pt").gameObject.SetActive(false);
                        var lit = switch1.transform.Find("lit_pieces").gameObject;
                        lit.transform.Find("plinth_glow").gameObject.SetActive(false);
                        lit.transform.Find("Base Glow").gameObject.SetActive(false);

                        var orb = switch1.transform.Find("GG_statue_plinth_orb_off").gameObject;
                        ItemPool.Scene_StatueOrb = Instantiate(orb, new Vector3(183.1351f, 34.863f, 2.3611f), Quaternion.Euler(0, 0, 0));
                        ItemPool.Scene_StatueOrb.AddComponent<StatueSwich>();

                        if (ItemPool.Scene_Bugs_Pt != null)
                        {
                            ItemPool.Scene_StatueBugs_Pt1 = Instantiate(ItemPool.Scene_Bugs_Pt, new Vector3(183.8015f, 35.7129f, 3.404f), Quaternion.Euler(0, 0, 0));
                            ItemPool.Scene_StatueBugs_Pt1.transform.localScale = new Vector3(3f, 1f, 5f);
                            ItemPool.Scene_StatueBugs_Pt1.transform.eulerAngles = new Vector3(0, 0, 0f);
                            ItemPool.Scene_StatueBugs_Pt1.SetActive(true);
                            ItemPool.Scene_StatueBugs_Pt1.GetComponent<ParticleSystem>().emissionRate = 3;
                            ItemPool.Scene_StatueBugs_Pt1.GetComponent<ParticleSystem>().maxParticles = 50;

                            if (INFINITY.HardMode == 1)
                            {
                                ItemPool.Scene_StatueBugs_Pt1.GetComponent<ParticleSystem>().loop = true;
                                ItemPool.Scene_StatueBugs_Pt1.GetComponent<ParticleSystem>().Play();
                            }
                            else
                            {
                                ItemPool.Scene_StatueBugs_Pt1.GetComponent<ParticleSystem>().loop = false;
                            }

                            ItemPool.Scene_StatueBugs_Pt2 = Instantiate(ItemPool.Scene_Bugs_Pt, new Vector3(183.8015f, 33.7129f, 3.404f), Quaternion.Euler(0, 0, 0));
                            ItemPool.Scene_StatueBugs_Pt2.transform.localScale = new Vector3(20f, 1f, 5f);
                            ItemPool.Scene_StatueBugs_Pt2.transform.eulerAngles = new Vector3(0, 0, 0f);
                            ItemPool.Scene_StatueBugs_Pt2.GetComponent<ParticleSystem>().loop = false;
                            ItemPool.Scene_StatueBugs_Pt2.SetActive(true);
                            ItemPool.Scene_StatueBugs_Pt2.GetComponent<ParticleSystem>().emissionRate = 1000;
                            ItemPool.Scene_StatueBugs_Pt2.GetComponent<ParticleSystem>().maxParticles = 10000;
                            ItemPool.Scene_StatueBugs_Pt2.GetComponent<ParticleSystem>().startSpeed = 25f;
                            ItemPool.Scene_StatueBugs_Pt2.GetComponent<ParticleSystem>().startLifetime = 5f;
                            ItemPool.Scene_StatueBugs_Pt2.GetComponent<ParticleSystem>().gravityModifier = 0f;
                        }
                    }
                }
                if (switch1 == null)
                {
                    Invoke("WorkshopChange", 0.5f);
                }
            }
            void Start()
            {
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
                //Invoke("WorkshopChange",2f);
                //UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            }
            void OnSceneLoaded(Scene scene, LoadSceneMode mode)
            {
                if(INFINITY.settings_.skillGot && INFINITY.settings_.skillOn)
                {
                    CamControl.Cam.GetComponent<CamControl>().CamChange();
                }
                else
                {
                    CamControl.Cam.GetComponent<CamControl>().CamRecover();
                }
                if (scene.name == "GG_Hollow_Knight")
                {
                    isInBattle = true;

                    On.GameManager.FreezeMoment_int += INFINITY.GameManager_FreezeMoment_int;

                    ModHooks.LanguageGetHook -= INFINITY.ModHooks_LanguageGetHook_Recover;
                    ModHooks.LanguageGetHook -= INFINITY.ModHooks_LanguageGetHook_Workshop;
                    ModHooks.LanguageGetHook += INFINITY.ModHooks_LanguageGetHook;
                    On.HeroController.TakeDamage += INFINITY.HeroController_TakeDamage;

                    CamControl.Cam.GetComponent<CamControl>().CamAway();
                }
                else
                {

                    On.GameManager.FreezeMoment_int -= INFINITY.GameManager_FreezeMoment_int;
                    //On.GameManager.FreezeMoment_int += INFINITY.GameManager_FreezeMoment_int_Recover;

                    if (scene.name == "GG_Workshop")
                    {
                        isInBattle = false;

                        ModHooks.LanguageGetHook -= INFINITY.ModHooks_LanguageGetHook_Recover;
                        ModHooks.LanguageGetHook -= INFINITY.ModHooks_LanguageGetHook;
                        ModHooks.LanguageGetHook += INFINITY.ModHooks_LanguageGetHook_Workshop;

                        CamControl.Cam.GetComponent<CamControl>().CamClose();
                        Invoke("WorkshopChange", 0.5f);
                    }
                    if (scene.name == "GG_Radiance")
                    {
                        isInBattle = false;

                        ModHooks.LanguageGetHook -= INFINITY.ModHooks_LanguageGetHook;
                        ModHooks.LanguageGetHook -= INFINITY.ModHooks_LanguageGetHook_Workshop;
                        ModHooks.LanguageGetHook += INFINITY.ModHooks_LanguageGetHook_Recover;

                        Modding.Logger.Log("Text Replaced.");
                    }
                    else
                    {
                        On.HeroController.TakeDamage -= INFINITY.HeroController_TakeDamage;
                    }
                }
            }
        }

    }
}
