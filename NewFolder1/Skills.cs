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
using UnityEngine.SocialPlatforms;
using static INFINITY.INFINITY;
using UnityStandardAssets.ImageEffects;
using static UnityEngine.GridBrushBase;
using HutongGames.PlayMaker;
using static INFINITY.Skills;
using static Mono.Math.BigInteger;
using HKMirror.Reflection.InstanceClasses;
using IL.tk2dRuntime.TileMap;
using static INFINITY.SceneControl;

namespace INFINITY
{
    public class Phase5Arena:MonoBehaviour
    {
        public float phase4X = 10000;
        public void Place()
        {
            float phase5X = phase4X + 500;
            Instantiate(ItemPool.Phase5Arena, new Vector3(phase5X - 5, 10.01f, 32.4219f), Quaternion.Euler(0, 0, 0));
            SceneControl.fountain.transform.position = new Vector3(phase5X + 32f, 12.8881f, 2.7458f);
            var obj1 = SceneControl.fountain.transform.Find("Fountain Inspect(Clone)").gameObject;
            var text = obj1.transform.Find("New Fountain Prompt").gameObject;
            text.transform.position = new Vector3(-0.6143f, text.transform.position.y, text.transform.position.z);

            obj1.LocateMyFSM("Conversation Control").GetState("Hero Anim").AddMethod(()=>
            {
                fountain.GetComponent<FountainControl>().ParticleBurst();

                if (!settings_.skillGot)
                {
                    settings_.skillGot = true;
                }
                else
                {
                    GameObject[] all = GameObject.FindObjectsOfType<GameObject>();

                    foreach (var obj in all)
                    {
                        if (obj.name.Contains("Boss Scene Controller"))
                        {
                            obj.GetComponent<BossSceneController>().EndBossScene();
                        }
                    }
                }
            });
            obj1.LocateMyFSM("Conversation Control").GetState("Fade Down").AddMethod(()=>
            {
                GameObject[] all = GameObject.FindObjectsOfType<GameObject>();

                foreach (var obj in all)
                {
                    if (obj.name.Contains("Boss Scene Controller"))
                    {
                        obj.GetComponent<BossSceneController>().EndBossScene();
                    }
                }
            });

            for (int i = 0; i < 3; i++)
            {
                Vector3 localPosR = new Vector3(phase5X + 30f + 16.9f + 10f * i, 12.8881f - 4.4836f, 0.0004f);
                Vector3 localPosL = new Vector3(phase5X + 30f - 16.9f - 10f * i, 12.8881f - 4.4836f, 0.0004f);
                var lampR = Instantiate(ItemPool.Scene_Light_4, localPosR, Quaternion.Euler(0, 0, 0));
                var lampL = Instantiate(ItemPool.Scene_Light_4, localPosL, Quaternion.Euler(0, 0, 0));
                lampR.SetActive(true);
                lampL.SetActive(true);
            }

            GameObject[] all = GameObject.FindObjectsOfType<GameObject>();

            foreach (var obj in all)
            {
                if (obj.name.Contains("Plume") && obj != ItemPool.Plume)
                {
                    float xFloor = obj.transform.position.x;
                    if (xFloor > phase5X - 20f)
                    {
                        if (obj.GetComponent<PlumeFall>() != null)
                        {
                            obj.GetComponent<PlumeFall>().On();
                        }
                    }
                }
            }

        }
        public void Phase4InvincibleStart()
        {
            gameObject.GetComponent<HealthManager>().IsInvincible = true;
            gameObject.GetComponent<HealthManager>().enabled = false;
            gameObject.GetComponent<SpriteFlash>().flash(new Color(1, 1, 1, 1), 0.8f, 1f, 100000f, 1f);
        }
        public void Phase4InvincibleEnd()
        {
            gameObject.GetComponent<HealthManager>().IsInvincible = false;
            gameObject.GetComponent<HealthManager>().enabled = true;
            gameObject.GetComponent<SpriteFlash>().CancelFlash();
        }
    }
    public class BossSkillChoice : MonoBehaviour
    {
        private static int[] Phase1SkillChoice = { 1, 2, 3, 4, 5 };
        private static int[] Phase2SkillChoice = { 1, 2, 3, 4, 5, 6, 7, 8};
        private static int[] Phase3SkillChoice = { 1, 2, 3, 4, 5, 6, 7};
        private static int[] Phase4SkillChoice = { 1, 2, 3, 4, 5};
        private static int[] Phase5SkillChoice = { 1, 2, 3 };
        int[] shuffledArray1 = Phase1SkillChoice;
        int[] shuffledArray2 = Phase2SkillChoice;
        int[] shuffledArray3 = Phase3SkillChoice;
        int[] shuffledArray4 = Phase4SkillChoice;
        int[] shuffledArray5 = Phase5SkillChoice;
        int numPhase1 = 10;
        int numPhase2 = 10;
        int numPhase3 = 10;
        int numPhase4 = 10;
        int numPhase5 = 10;
        double R => random.NextDouble();
        public void Phase1Skill()
        {
            numPhase1++;
            if(HardMode == 0)
            {
                if (numPhase1 >= 5)
                {
                    shuffledArray1 = Phase1SkillChoice.OrderBy(x => Guid.NewGuid()).ToArray();
                    numPhase1 = 0;
                }
                switch (shuffledArray1[numPhase1])
                {
                    case 1:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill1();
                            return;
                        }
                    case 2:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill3();
                            return;
                        }
                    case 3:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill11(0.2f);
                            return;
                        }
                    case 4:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill4(0.2f);
                            return;
                        }
                    case 5:
                        {
                            //gameObject.GetComponent<SkillsControl>().Skill12(1f, 0.75f);
                            gameObject.GetComponent<SkillsControl>().Skill12(0.3f, 0.25f, false, 0.3f);
                            return;
                        }
                }
            }
            else if(HardMode == 1)
            {
                if (numPhase1 >= 5)
                {
                    shuffledArray1 = Phase1SkillChoice.OrderBy(x => Guid.NewGuid()).ToArray();
                    numPhase1 = 0;
                }
                switch (shuffledArray1[numPhase1])
                {
                    case 1:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill1();
                            return;
                        }
                    case 2:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill3();
                            return;
                        }
                    case 3:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill11(0.15f);
                            return;
                        }
                    case 4:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill4(0.15f);
                            return;
                        }
                    case 5:
                        {
                            //gameObject.GetComponent<SkillsControl>().Skill12(1f, 0.75f);
                            gameObject.GetComponent<SkillsControl>().Skill12(0.6f, 0.25f, false, 0.2f);
                            return;
                        }
                }
            }
        }
        public void Phase2Skill()
        {
            numPhase2++;
            if(HardMode == 0)
            {
                if (numPhase2 >= 8)
                {
                    shuffledArray2 = Phase2SkillChoice.OrderBy(x => Guid.NewGuid()).ToArray();
                    numPhase2 = 0;
                }
                switch (shuffledArray2[numPhase2])
                {
                    case 1:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill5();
                            return;
                        }
                    case 2:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill1();
                            return;
                        }
                    case 3:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill3();
                            return;
                        }
                    case 4:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill6();
                            return;
                        }
                    case 5:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill11(0.15f);
                            return;
                        }
                    case 6:
                        {
                            if (R > 0.5f)
                            {
                                gameObject.GetComponent<SkillsControl>().Skill12(3f, 0.225f, false, 0.4f);
                            }
                            else
                            {
                                gameObject.GetComponent<SkillsControl>().Skill12(0.25f, 1f, false, 0.1f);
                            }
                            return;
                        }
                    case 7:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill4(0.15f);
                            return;
                        }
                    case 8:
                        {
                            Phase2Skill();
                            return;
                        }
                }
            }
            if(HardMode == 1)
            {
                if (numPhase2 >= 8)
                {
                    shuffledArray2 = Phase2SkillChoice.OrderBy(x => Guid.NewGuid()).ToArray();
                    numPhase2 = 0;
                }
                switch (shuffledArray2[numPhase2])
                {
                    case 1:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill5();
                            return;
                        }
                    case 2:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill1();
                            return;
                        }
                    case 3:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill3();
                            return;
                        }
                    case 4:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill6();
                            return;
                        }
                    case 5:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill11(0.1f);
                            return;
                        }
                    case 6:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill2();
                            return;
                        }
                    case 7:
                        {
                            //gameObject.GetComponent<SkillsControl>().Skill12(1f, 0.75f);

                            //gameObject.GetComponent<SkillsControl>().Skill12(2f, 0.25f, false, 0.2f);
                            if(R > 0.5f)
                            {
                                gameObject.GetComponent<SkillsControl>().skill12isBig = true;
                                gameObject.GetComponent<SkillsControl>().Skill12(1f, 0.25f, false, 0.2f);
                            }
                            else
                            {
                                gameObject.GetComponent<SkillsControl>().Skill12(3.3f, 0.225f, false, 0.3f);
                            }
                            return;
                        }
                    case 8:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill4(0.1f);
                            return;
                        }
                }
            }
        }
        public void Phase3Skill()
        {
            numPhase3++;
            if(HardMode == 0)
            {
                if (numPhase3 >= 7)
                {
                    shuffledArray3 = Phase3SkillChoice.OrderBy(x => Guid.NewGuid()).ToArray();
                    numPhase3 = 0;
                }
                switch (shuffledArray3[numPhase3])
                {
                    case 1:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill1();
                            return;
                        }
                    case 2:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill3();
                            return;
                        }
                    case 3:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill6();
                            return;
                        }
                    case 4:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill4(0.15f);
                            return;
                        }
                    case 5:
                        {
                            //gameObject.GetComponent<SkillsControl>().Skill12(1f, 0.75f);
                            gameObject.GetComponent<SkillsControl>().Skill12(0.3f, 0.225f, false, 0.3f);
                            return;
                        }
                    case 6:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill9();
                            return;
                        }
                    case 7:
                        {
                            Phase3Skill();
                            return;
                        }
                }
            }
            if (HardMode == 1)
            {
                if (numPhase3 >= 7)
                {
                    shuffledArray3 = Phase3SkillChoice.OrderBy(x => Guid.NewGuid()).ToArray();
                    numPhase3 = 0;
                }
                switch (shuffledArray3[numPhase3])
                {
                    case 1:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill2();
                            return;
                        }
                    case 2:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill1();
                            return;
                        }
                    case 3:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill3();
                            return;
                        }
                    case 4:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill6();
                            return;
                        }
                    case 5:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill4(0.1f);
                            return;
                        }
                    case 6:
                        {
                            //gameObject.GetComponent<SkillsControl>().Skill12(1f, 0.75f);
                            gameObject.GetComponent<SkillsControl>().Skill12(3.3f, 0.225f, false, 0.3f);
                            return;
                        }
                    case 7:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill5();
                            return;
                        }
                }
            }
        }
        public void Phase4Skill()
        {
            numPhase4++;
            if(HardMode == 0)
            {
                if (numPhase4 >= 5)
                {
                    shuffledArray4 = Phase4SkillChoice.OrderBy(x => Guid.NewGuid()).ToArray();
                    numPhase4 = 0;
                }
                switch (shuffledArray4[numPhase4])
                {
                    case 1:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill1();
                            return;
                        }
                    case 2:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill3();
                            return;
                        }
                    case 3:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill4(0.15f);
                            return;
                        }
                    case 4:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill12(0.2f, 0.85f, false, 0.3f);
                            return;
                        }
                    case 5:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill10();
                            return;
                        }
                }
            }
            if(HardMode == 1)
            {
                if (numPhase4 >= 5)
                {
                    shuffledArray4 = Phase4SkillChoice.OrderBy(x => Guid.NewGuid()).ToArray();
                    numPhase4 = 0;
                }
                switch (shuffledArray4[numPhase4])
                {
                    case 1:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill1();
                            return;
                        }
                    case 2:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill3();
                            return;
                        }
                    case 3:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill4(0.1f);
                            return;
                        }
                    case 4:
                        {
                            //gameObject.GetComponent<SkillsControl>().Skill12(1f, 0.75f);
                            gameObject.GetComponent<SkillsControl>().Skill12(0.6f, 0.85f, false, 0.2f);
                            return;
                        }
                    case 5:
                        {
                            gameObject.GetComponent<SkillsControl>().Skill5();
                            return;
                        }
                }
            }
        }
        public void Phase5Skill()
        {
            numPhase5++;
            if (HardMode == 0)
            {
                if (numPhase5 >= 3)
                {
                    shuffledArray5 = Phase5SkillChoice.OrderBy(x => Guid.NewGuid()).ToArray();
                    numPhase5 = 0;
                }
                switch (shuffledArray5[numPhase5])
                {
                    case 1:
                    case 2:
                    case 3:
                        {
                            gameObject.GetComponent<SkillsControl>().SkillShort2("SA1", 0.1f, 1f);
                            return;
                        }
                }
            }
            if (numPhase5 >= 3)
            {
                shuffledArray5 = Phase5SkillChoice.OrderBy(x => Guid.NewGuid()).ToArray();
                numPhase5 = 0;
            }
            switch (shuffledArray5[numPhase5])
            {
                case 1:
                    {
                        gameObject.GetComponent<SkillsControl>().Skill2();
                        return;
                    }
                case 2:
                    {
                        gameObject.GetComponent<SkillsControl>().Skill2();
                        return;
                    }
                case 3:
                    {
                        gameObject.GetComponent<SkillsControl>().Skill2();
                        return;
                    }
            }
        }
    }
    public class Skills : MonoBehaviour
    {
        private Shader _shader;
        private GameObject Line;
        private GameObject Line1;
        private GameObject Line2;
        private GameObject Line3;
        private GameObject Line4;
        private GameObject Line5;
        private GameObject Line6;
        private GameObject Line7;
        private GameObject Line8;
        public GameObject MainSlash;
        float closeTime = 0.6f;
        float closeDistance = 5f;
        public static DamageEnemies SetDamageEnemy(GameObject go, int value = 0, float angle = 0f, float magnitudeMult = 0f, AttackTypes type = AttackTypes.Nail)
        {
            DamageEnemies damageEnemies = go.GetComponent<DamageEnemies>() ?? go.AddComponent<DamageEnemies>();
            damageEnemies.attackType = type;
            damageEnemies.circleDirection = false;
            damageEnemies.damageDealt = value;
            damageEnemies.direction = angle;
            damageEnemies.ignoreInvuln = false;
            damageEnemies.magnitudeMult = magnitudeMult;
            damageEnemies.moveDirection = false;
            damageEnemies.specialType = SpecialTypes.None;
            return damageEnemies;
        }
        static private double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }
        static private double DegreesToRadians(double Degrees)
        {
            return Degrees * (Math.PI / 180);
        }
        public void GetBall()
        {
            if(ItemPool.Light == null)
            {
                var gm = GameObject.Find("_GameManager").gameObject;
                if (gm != null)
                {
                    var pool = gm.transform.Find("GlobalPool").gameObject;
                    for (int i = 0; i <= pool.transform.childCount; i++)
                    {
                        var obj = pool.transform.GetChild(i).gameObject;
                        if (obj.name.Contains("Fireball2 Spiral"))
                        {
                            ItemPool.Light = Instantiate(obj, gameObject.transform);
                            ItemPool.Light.SetActive(false);
                            break;
                        }
                    }
                }
                Invoke("GetBall", 0.5f);
            }
            else
            {
                var light = Instantiate(ItemPool.Light, gameObject.transform);
                light.transform.localPosition = new Vector3(2f, 0f, 0.001f);
                light.SetActive(true);
                light.LocateMyFSM("Fireball Control").ChangeTransition("Idle", "FINISHED", "Recycle");
                light.LocateMyFSM("Fireball Control").GetState("Idle").GetAction<Wait>().time = 999999f;
                var image = LoadPng("INFINITY.Resources.Spell.png");
                if (image != null)
                {
                    light.GetComponent<tk2dSprite>().CurrentSprite.material.mainTexture = image;
                }
                else
                {
                    Modding.Logger.Log("image is null!");
                }
                light.SetActive(false);
            }
            /*
            if (ball != null)
            {
                if (ItemPool.SlashLight == null)
                {
                    ItemPool.SlashLight = Instantiate(ball, gameObject.transform);
                }
                ItemPool.SlashLight.transform.localPosition = new Vector3(2f, 0f, 0.001f);
                ItemPool.SlashLight.SetActive(true);
                ItemPool.SlashLight.LocateMyFSM("Fireball Control").ChangeTransition("Idle", "FINISHED", "Recycle");
                ItemPool.SlashLight.LocateMyFSM("Fireball Control").GetState("Idle").GetAction<Wait>().time = 999999f;
                var image = INFINITY.LoadPng("INFINITY.Resources.Spell.png");
                if (image != null)
                {
                    ItemPool.SlashLight.GetComponent<tk2dSprite>().CurrentSprite.material.mainTexture = image;
                }
                else
                {
                    Modding.Logger.Log("image is null!");
                }
                ItemPool.SlashLight.SetActive(false);
            }
            else
            {
                Invoke("GetBall", 0.5f);
            }
            */
            //ball.LocateMyFSM("Fireball Control").GetState("Init").RemoveAction<SetVelocity2d>();
            //ball.LocateMyFSM("Fireball Control").ChangeTransition("Idle", "FINISHED", "Recycle");
            //ball.GetComponent<BoxCollider2D>().size = new Vector2(5, 1);
            //ball.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0.05f);
            //ball.GetComponent<BoxCollider2D>().enabled = false;
            //var image = INFINITY.LoadPng("INFINITY.Resources.Spell.png");
            //ball.GetComponent<MeshRenderer>().material.mainTexture = image;
        }
        public void BigSlashTest()
        {
            GameObject bigslash = Instantiate(ItemPool.BigSlash, HeroController.instance.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            bigslash.GetComponent<BigSlashControl>().On(1f);
        }
        public void GetBigSlash()
        {
            if(ItemPool.BigSlash == null)
            {
                GameObject bigSlash = new GameObject();

                if (ItemPool.Line != null)
                {
                    var light1 = Instantiate(ItemPool.Line, bigSlash.transform);
                    //var light2 = Instantiate(ItemPool.Line, bigSlash.transform);
                    var light3 = Instantiate(ItemPool.Line, bigSlash.transform);
                    //var light4 = Instantiate(ItemPool.Line, bigSlash.transform);

                    light1.transform.localPosition = new Vector3(30f, 0f, 0f);
                    //light2.transform.localPosition = new Vector3(30f, 0f, 0f);
                    light3.transform.localPosition = new Vector3(-30f, 0f, 0f);
                    //light4.transform.localPosition = new Vector3(-30f, 0f, 0f);

                    light1.transform.localScale = new Vector3(6f, 13f, 1f);
                    //light2.transform.localScale = new Vector3(6f, 13f, 1f);
                    light3.transform.localScale = new Vector3(6f, -13f, 1f);
                    //light4.transform.localScale = new Vector3(6f, -13f, 1f);

                    light1.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                    //light2.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                    light3.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                    //light4.transform.eulerAngles = new Vector3(0f, 0f, 270f);

                }

                bigSlash.AddComponent<BigSlashControl>();
                ItemPool.BigSlash = bigSlash;

                var slashes = BOSS.transform.Find("Slashes").gameObject;
                var Slash2 = slashes.transform.Find("Slash2").gameObject;
                var collision = Instantiate(Slash2, bigSlash.transform);
                collision.transform.localPosition = new Vector3(-15f, 0.4f, 0f);
                collision.transform.localScale = new Vector3(-9, 0.85f, 1);
                collision.SetActive(false);
            }
        }
        public class BigSlashControl : MonoBehaviour
        {
            float factor1 = 1f;
            public void On(float scaleFactor)
            {
                factor1 = scaleFactor;
                for (int i = 0;i < gameObject.transform.childCount && gameObject.transform.GetChild(i).name.Contains("Beam"); i++)
                {
                    var line = gameObject.transform.GetChild(i).gameObject;
                    line.transform.localScale = new Vector3(5f, 10f * Math.Sign(line.transform.localScale.y) * scaleFactor, 1f);
                    line.SetActive(true);
                }

                DelayLight(0.7f);
            }
            public void DelayLight(float delayTime)
            {
                Invoke("Light", delayTime);
            }
            public void Light()
            {
                GameObject lightPrefab = null;
                for (int i = 0; i <= BOSS.transform.childCount; i++)
                {
                    var obj = BOSS.transform.GetChild(i).gameObject;
                    if (obj.name.Contains("Fireball2 Spiral"))
                    {
                        lightPrefab = obj;
                        break;
                    }
                }
                var light = Instantiate(lightPrefab, gameObject.transform);
                light.transform.localPosition = new Vector3(-13.5f, 0.4f, 0);
                light.SetActive(true);
                light.GetComponent<BoxCollider2D>().enabled = false;
                light.LocateMyFSM("Fireball Control").GetState("L").RemoveAction<FlipScale>();
                light.LocateMyFSM("Fireball Control").SetState("Idle");
                light.GetComponent<tk2dSpriteAnimator>().Reflect().clipTime = 2.05f;
                StartCoroutine(DelayedExecution(0.02f));
                IEnumerator DelayedExecution(float time)
                {
                    yield return new WaitForSeconds(time);
                    light.transform.localScale = new Vector3(18f, 6f, 1f) * factor1 * gameObject.transform.localScale.y;
                    light.AddComponent<ObjDelayRecycle>();
                    light.GetComponent<ObjDelayRecycle>().DelayRecycle(0.15f);
                    light.GetComponent<ObjDelayRecycle>().DelayRecycle(0.15f);
                    light.GetComponent<tk2dSpriteAnimator>().Reflect().clipTime = 2.05f;
                }

                Invoke("Damage", 0.04f);
            }
            public void Damage()
            {
                HeroController.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Slash, 0.8f);

                for (int i = 0; i <= gameObject.transform.childCount; i++)
                {
                    var obj = gameObject.transform.GetChild(i).gameObject;
                    if (obj.name.Contains("Slash"))
                    {
                        obj.SetActive(true);
                        obj.GetComponent<PolygonCollider2D>().enabled = true;
                        break;
                    }
                }
                Invoke("DamageEnd", 0.15f);
            }
            public void DamageEnd()
            {
                for (int i = 0; i <= gameObject.transform.childCount; i++)
                {
                    var obj = gameObject.transform.GetChild(i).gameObject;
                    if (obj.name.Contains("Slash"))
                    {
                        obj.SetActive(false);
                        obj.GetComponent<PolygonCollider2D>().enabled = false;
                        break;
                    }
                }
                Invoke("Destroy", 3f);
            }

            public void Destroy()
            {
                gameObject.Recycle();
            }
        }
        public void Disappear()
        {
            var glow = Instantiate(ItemPool.Glow, gameObject.transform.position, Quaternion.Euler(0, 0, gameObject.transform.eulerAngles.z + 90f));
            glow.transform.SetParent(null);
            glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 3;
            glow.SetActive(true);
            glow.AddComponent<DelayDestory>();
            glow.GetComponent<DelayDestory>().On(2f);
            gameObject.transform.position += new Vector3(0f, -100f, 0f);
        }
        public void Appear()
        {
        }
        public void SlashCollisionChange()
        {
            var slashes = gameObject.transform.Find("Slashes").gameObject;
            var slash1 = slashes.transform.Find("Slash1");
            var slash2 = slashes.transform.Find("Slash2");
            var slash3 = slashes.transform.Find("Slash3");
            slash1.transform.localScale = new Vector3(-3, 1, 1);
            slash1.localPosition = new Vector3(-12f, -1f, 0.004f);
            slash2.transform.localScale = new Vector3(-3, 1.2f, 1);
            slash2.localPosition = new Vector3(-16f, -0.1f, 0.004f);
            slash2.gameObject.AddComponent<BlockFlash>();
            slash2.GetComponent<PolygonCollider2D>().points = new Vector2[] { new Vector2(-6.7f, 2f), new Vector2(0f, 2.3f), new Vector2(4.6f, 2f), new Vector2(2.4f, 1f), new Vector2(1f, -1f), new Vector2(3f, -2.8f), new Vector2(-7.5f, -2.8f), new Vector2(-8.5f, -1.8f), new Vector2(-8.5f, -0.95f), new Vector2(-7.6f, 0.4f) };
            slash3.transform.localScale = new Vector3(-3, 1.05f, 1);
            slash3.localPosition = new Vector3(-9f, -1f, 0.004f);
            MainSlash = slash2.gameObject;
            if(gameObject.name == "MoonForPlayer")
            {
                MainSlash.layer = LayerMask.NameToLayer("Attack");
                MainSlash.GetComponent<DamageHero>().damageDealt = 0;
            }
        }
        public void Turn()
        {
            bool flag1 = gameObject.transform.position.x - HeroController.instance.transform.position.x > 0;
            bool flag2 = gameObject.transform.localScale.x > 0;
            if ((flag1 && flag2) || (!flag1 && !flag2))
            {
                gameObject.transform.localScale = new Vector3(- gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            }
        }
        public void SetLine()
        {
            Line = Instantiate(ItemPool.Line, gameObject.transform);
            Line.transform.localPosition = new Vector3(30f, 0f, 0.001f);
            Line.transform.localScale = new Vector3(2f, 10f, 1f);
            Line.transform.localEulerAngles = new Vector3(0, 0, -90f);
            Line1 = Instantiate(ItemPool.Line, gameObject.transform);
            Line1.transform.localPosition = new Vector3(30f, 0f, 0.001f);
            Line1.transform.localScale = new Vector3(0.5f, 8f, 1f);
            Line1.transform.localEulerAngles = new Vector3(0, 0, -90f);
            Line2 = Instantiate(ItemPool.Line, gameObject.transform);
            Line2.transform.localPosition = new Vector3(30f, 0f, 0.001f);
            Line2.transform.localScale = new Vector3(0.5f, 8f, 1f);
            Line2.transform.localEulerAngles = new Vector3(0, 0, -90f);
            Line3 = Instantiate(ItemPool.Line, gameObject.transform);
            Line3.transform.localPosition = new Vector3(30f, 0f, 0.001f);
            Line3.transform.localScale = new Vector3(0.5f, 8f, 1f);
            Line3.transform.localEulerAngles = new Vector3(0, 0, -90f);
            Line4 = Instantiate(ItemPool.Line, gameObject.transform);
            Line4.transform.localPosition = new Vector3(30f, 0f, 0.001f);
            Line4.transform.localScale = new Vector3(0.5f, 8f, 1f);
            Line4.transform.localEulerAngles = new Vector3(0, 0, -90f);
            Line5 = Instantiate(ItemPool.Line, gameObject.transform);
            Line5.transform.localPosition = new Vector3(0f, -30f, 0.001f);
            Line5.transform.localScale = new Vector3(0.5f, 8f, 1f);
            Line5.transform.localEulerAngles = new Vector3(0, 0, -180f);
            Line6 = Instantiate(ItemPool.Line, gameObject.transform);
            Line6.transform.localPosition = new Vector3(0f, -30f, 0.001f);
            Line6.transform.localScale = new Vector3(0.5f, 8f, 1f);
            Line6.transform.localEulerAngles = new Vector3(0, 0, -180f);

            Line7 = Instantiate(ItemPool.Line, gameObject.transform);
            Line7.transform.localPosition = new Vector3(0f, -30f, 0.001f);
            Line7.transform.localScale = new Vector3(4f, 18f, 1f);
            Line7.transform.localEulerAngles = new Vector3(0, 0, 180f);
            Line8 = Instantiate(ItemPool.Line, gameObject.transform);
            Line8.transform.localPosition = new Vector3(0f, -30f, 0.001f);
            Line8.transform.localScale = new Vector3(4f, 18f, 1f);
            Line8.transform.localEulerAngles = new Vector3(0, 0, 180f);
            LineDisAppear();
        }
        public void LineAppear(int type)
        {
            LineDisAppear();
            if (type == 0)
            {
                Line.SetActive(true);
            }
            else if(type == 1)
            {
                Line.SetActive(true);
                Line1.SetActive(true);
                Line2.SetActive(true);
            }
            else if(type == 2)
            {
                Line.SetActive(true);
                Line1.SetActive(true);
                Line2.SetActive(true);
                Line3.SetActive(true);
                Line4.SetActive(true);
            }
            else if(type == 3)
            {
                Line5.SetActive(true);
                Line6.SetActive(true);
            }
            else if(type == 4)
            {
                Line1.SetActive(true);
                Line2.SetActive(true);
            }
            else if(type == 5)
            {
                Line7.SetActive(true);
                Line8.SetActive(true);

                LinesPhase3();
            }
            else if(type == 6)
            {
                Line7.SetActive(true);
                Line8.SetActive(true);
                LinesPhase5();
            }
            LinesStartToClose();
            //Invoke("LineDisAppear", 1f);
        }
        public void LineDisAppear()
        {
            Line.SetActive(false);
            Line1.SetActive(false);
            Line2.SetActive(false);
            Line3.SetActive(false);
            Line4.SetActive(false);
            Line5.SetActive(false);
            Line6.SetActive(false);
            CancelInvoke("LinesCloseLoop");
        }
        public void LinesPhase3()
        {
            Line7.transform.SetParent(null);
            Line8.transform.SetParent(null);
            LinesCloseLoopPhase3();
        }
        public void LinesPhase5()
        {
            Line7.GetComponent<tk2dSpriteAnimator>().Pause();
            Line8.GetComponent<tk2dSpriteAnimator>().Pause();

            Line7.transform.position = new Vector3(HeroController.instance.gameObject.transform.position.x, -30f, 0.001f);
            Line8.transform.position = new Vector3(HeroController.instance.gameObject.transform.position.x, -30f, 0.001f);

            Line7.transform.SetParent(null);
            Line8.transform.SetParent(null);

            Invoke("LinesPhase5End", 1000 / (closeDistance * 6 / closeTime));
        }
        public void LinesPhase5End()
        {
            Line7.transform.position = new Vector3(HeroController.instance.gameObject.transform.position.x + 500f, -500f, 0.001f);
            Line8.transform.position = new Vector3(HeroController.instance.gameObject.transform.position.x - 500f, -500f, 0.001f);

        }
        public void LinesStartToClose()
        {
            Line1.transform.localPosition = new Vector3(30f, closeDistance, 0.001f);
            Line2.transform.localPosition = new Vector3(30f, -closeDistance, 0.001f);
            Line3.transform.localPosition = new Vector3(30f, closeDistance * 2, 0.001f);
            Line4.transform.localPosition = new Vector3(30f, -closeDistance * 2, 0.001f);
            Line5.transform.localPosition = new Vector3(closeDistance, -30f, 0.001f);
            Line6.transform.localPosition = new Vector3(-closeDistance, -30f, 0.001f);
            LinesCloseLoop();
        }
        public void LinesCloseLoop()
        {
            Line1.transform.localPosition -= new Vector3(0, closeDistance / (closeTime / 0.02f), 0f);
            Line2.transform.localPosition -= new Vector3(0, -closeDistance / (closeTime / 0.02f), 0f);
            Line3.transform.localPosition -= new Vector3(0, closeDistance * 2 / (closeTime / 0.02f), 0f);
            Line4.transform.localPosition -= new Vector3(0, -closeDistance * 2 / (closeTime / 0.02f), 0f);
            Line5.transform.localPosition -= new Vector3(closeDistance / (closeTime / 0.02f), 0f, 0f);
            Line6.transform.localPosition -= new Vector3(-closeDistance / (closeTime / 0.02f), 0f, 0f);
            Invoke("LinesCloseLoop", 0.02f);
        }
        public void LinesCloseLoopPhase3()
        {
            Line7.transform.position += new Vector3(closeDistance * 6f / (closeTime / 0.02f), 0f, 0f);
            Line8.transform.position += new Vector3(-closeDistance * 6f / (closeTime / 0.02f), 0f, 0f);
            Invoke("LinesCloseLoopPhase3", 0.02f);
        }
        public List<GameObject> floors = new List<GameObject>();
        public void Start()
        {
            if(!gameObject.GetComponent<Fadeimage>())
                gameObject.AddComponent<Fadeimage>();

            if (!gameObject.GetComponent<SceneControl>())
                gameObject.AddComponent<SceneControl>();

            if (!gameObject.GetComponent<SlashBeam>())
                gameObject.AddComponent<SlashBeam>();

            if (!gameObject.GetComponent<OneSlash>())
                gameObject.AddComponent<OneSlash>();

            if (!gameObject.GetComponent<OneSlashLight>())
                gameObject.AddComponent<OneSlashLight>();

            if (!gameObject.GetComponent<LockAngleWaitForAttack>())
                gameObject.AddComponent<LockAngleWaitForAttack>();

            if (!gameObject.GetComponent<OneSlashAnyAngle>())
                gameObject.AddComponent<OneSlashAnyAngle>();

            if (!gameObject.GetComponent<SkillsControl>())
                gameObject.AddComponent<SkillsControl>();

            if (!gameObject.GetComponent<AngleSystem>())
                gameObject.AddComponent<AngleSystem>();

            if (!gameObject.GetComponent<TeleportSystem>())
                gameObject.AddComponent<TeleportSystem>();

            if (!gameObject.GetComponent<SlashColliderControl>())
                gameObject.AddComponent<SlashColliderControl>();

            if (!gameObject.GetComponent<Dtab>())
                gameObject.AddComponent<Dtab>();

            if (!gameObject.GetComponent<BossSkillChoice>())
                gameObject.AddComponent<BossSkillChoice>();

            if (!gameObject.GetComponent<Phase5Arena>())
                gameObject.AddComponent<Phase5Arena>();

            if (!gameObject.GetComponent<BossBurstControl>())
                gameObject.AddComponent<BossBurstControl>();

            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

            GetBall();
            /*
            StartCoroutine(DelayedExecution1(0.5f));
            IEnumerator DelayedExecution1(float time)
            {
                yield return new WaitForSeconds(time);
                GetBall();
            }
            */
            SlashCollisionChange();
            SetLine();
            //ShaderLoader.Load();
            StartCoroutine(DelayedExecution(0.2f));
            IEnumerator DelayedExecution(float time)
            {
                yield return new WaitForSeconds(time);
                CameraControl.BattleStart();
                var obj1 = GameObject.Find("GG_Arena_Prefab").gameObject;
                var floor = obj1.transform.Find("Floor").gameObject;
                //var chains = obj1.transform.Find("Death Break Chains").gameObject;
                if(ItemPool.Floor == null)
                {
                    GameObject[] all = GameObject.FindObjectsOfType<GameObject>();
                    foreach (var obj in all)
                    {
                        if (obj.name.Contains("TileMap Render Data"))
                        {
                            var sceneMap = obj.transform.Find("Scenemap").gameObject;
                            var chunk1 = sceneMap.transform.Find("Chunk 0 1").gameObject;
                            chunk1.GetComponent<MeshRenderer>().enabled = false;
                            EdgeCollider2D[] edgeCollider2Ds = chunk1.GetComponents<EdgeCollider2D>();
                            foreach (var edgeCollider2D in edgeCollider2Ds)
                            {
                                if (edgeCollider2D.edgeCount == 5)
                                {
                                    HeroController.instance.transform.position += new Vector3(0f, 0.5f, 0f);
                                    Vector2[] vectorArray = { new Vector2(-23, 5), new Vector2(-3, 5), new Vector2(-3, -5), new Vector2(-23, -5) };
                                    edgeCollider2D.points = vectorArray;
                                    ItemPool.Floor = chunk1;
                                }
                                else
                                {
                                    edgeCollider2D.enabled = false;
                                }
                            }
                        }
                    }
                }
                var focus = gameObject.transform.Find("Focus Blast").gameObject;
                var pt = focus.transform.Find("Particle_rocks_small").gameObject;
                ItemPool.FloorPt = Instantiate(pt, new Vector3(0, 0, 0), new Quaternion());
                ItemPool.FloorPt.GetComponent<ParticleSystem>().startSpeed = -10;
                ItemPool.FloorPt.GetComponent<ParticleSystem>().emissionRate = 50;
                var fl = Instantiate(floor, new Vector3(-232.59f, 10.01f, 32.4219f), Quaternion.Euler(0, 0, 0));
                var floorCollision = Instantiate(ItemPool.Floor, fl.transform);
                floorCollision.transform.localPosition = new Vector3(14.4782f, -10.01f, 0f);
                fl.AddComponent<FloorFall>();
                var floorPt = Instantiate(ItemPool.FloorPt, fl.transform);
                floorPt.transform.localPosition = new Vector3(3.4764f, -4.6599f, -32.4274f);
                floorPt.SetActive(true);
                pt.GetComponent<ParticleSystem>().Pause();
                ItemPool.Floor1 = fl;

                for (int i = 0; i <= 20; i++)
                {
                    Instantiate(ItemPool.Floor1, new Vector3(-232.59f + i * 20f, 10.01f, 32.4219f), Quaternion.Euler(0, 0, 0));
                }
                gameObject.GetComponent<SceneControl>().FloorAutoPlaceLoop();

                SceneControl.Detect();

            }
            gameObject.GetComponent<DamageHero>().damageDealt = 0;
        }
    }
    public class PlumeFall : MonoBehaviour
    {
        public void On()
        {
            gameObject.GetComponent<tk2dSpriteAnimator>().Resume();
            gameObject.AddComponent<ObjDelayRecycle>().DelayRecycle(3f);
            Invoke("Pt", 0.1f);
        }
        public void OnWithoutPt()
        {
            gameObject.GetComponent<tk2dSpriteAnimator>().Resume();
            gameObject.AddComponent<ObjDelayRecycle>().DelayRecycle(3f);
        }
        public void Pt()
        {
            var pt = Instantiate(ItemPool.Boss_Bugs_Pt, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            pt.transform.localScale = new Vector3(10f, 1f, 5f);
            pt.transform.eulerAngles = new Vector3(0, 0, 90f);
            //pt.transform.localPosition = new Vector3(0f, 0f, 0f);
            pt.GetComponent<ParticleSystem>().loop = false;
            pt.SetActive(true);
            pt.GetComponent<ParticleSystem>().emissionRate = 30;
            pt.GetComponent<ParticleSystem>().maxParticles = 10000;
            pt.GetComponent<ParticleSystem>().startSpeed = 2f;
            pt.AddComponent<DelayDestory>();
            pt.GetComponent<DelayDestory>().On(5f);
            pt.GetComponent<ParticleSystem>().Play();
        }
    }
    public class FloorFall : MonoBehaviour
    {
        Vector3 vec = Vector3.zero;
        GameObject pt = null;
        double R1 => random.NextDouble();
        double R2 => random.NextDouble();
        double R3 => random.NextDouble();
        public void Start()
        {
            //LightPlace();
        }
        public void LightPlace()
        {
            if (R1 > 0.4f)
            {
                if (R1 > 0.88f)
                {
                    //放置灭灯

                    if (R2 > 0.65f)
                    {
                        var light1 = Instantiate(ItemPool.Scene_Light_5, gameObject.transform);
                        light1.transform.localPosition = new Vector3(0.6994f - 7.5f + (float)R3 * 15f, -1.71f, -32.2179f);
                        light1.SetActive(true);
                    }
                    else
                    {
                        var light1 = Instantiate(ItemPool.Scene_Light_6, gameObject.transform);
                        light1.transform.localPosition = new Vector3(0.6994f - 7.5f + (float)R3 * 15f, -1.71f, -32.2179f);
                        light1.SetActive(true);
                    }
                }
                else
                {
                    //放置亮灯

                    if (R2 > 0.65f)
                    {
                        var light1 = Instantiate(ItemPool.Scene_Light_3, gameObject.transform);
                        light1.transform.localPosition = new Vector3(0.6994f - 7.5f + (float)R3 * 15f, -2.21f, -32.2179f);
                        light1.SetActive(true);
                    }
                    else
                    {
                        var light1 = Instantiate(ItemPool.Scene_Light_2, gameObject.transform);
                        light1.transform.localPosition = new Vector3(0.6994f - 7.5f + (float)R3 * 15f, -1.81f, -32.2179f);
                        light1.SetActive(true);
                    }
                }
            }
        }
        public void On()
        {
            FallLoop();
            Invoke("RecycleSelf",8f);
        }
        public void FallLoop()
        {
            Invoke("FallLoop", 0.02f);
            if(vec.y >= -0.1f)
            {
                vec += new Vector3(0f, -0.0004f, 0f);
            }
            gameObject.transform.position += vec;
        }
        public void PtStart()
        {
            pt = gameObject.transform.Find("Particle_rocks_small(Clone)(Clone)").gameObject;
            pt.GetComponent<ParticleSystem>().loop = true;
            pt.GetComponent<ParticleSystem>().Play();
        }
        void PtStop()
        {
            pt.GetComponent<ParticleSystem>().Stop();
        }
        void RecycleSelf()
        {
            gameObject.Recycle();
        }
    }
    public class ObjDelayRecycle : MonoBehaviour
    {
        public void DelayRecycle(float delayTime)
        {
            Invoke("Recycle", delayTime);
        }
        void Recycle()
        {
            gameObject.Recycle();
        }
    }
    public class BlockFlash : MonoBehaviour
    {
        System.Random random = new System.Random();
        double R => random.NextDouble();
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Clash Tink")
            {
                var glow = Instantiate(ItemPool.Glow, (HeroController.instance.gameObject.transform.position + gameObject.transform.position) / 2, Quaternion.Euler(0, 0, (float)R * 360f));
                glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 2;
                glow.SetActive(true);
                glow.AddComponent<DelayDestory>();
                glow.GetComponent<DelayDestory>().On(2f);

                HeroFlashShadow();
            }
        }

        void HeroFlashShadow()
        {
            HeroController.instance.gameObject.GetComponent<SpriteFlash>().flash(new Color(0, 0, 0, 1), 0.9f, 0f, 0.1f, 0.2f);
        }
    }


    public class AngleSystem : MonoBehaviour
    {
        public void FaceToPlayerStart()
        {
            FaceToPlayerLoop();
        }
        public void FaceToAngleOnce(float angle)
        {
            if (gameObject.transform.position.x - HeroController.instance.transform.position.x > 0)
            {
                gameObject.transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            gameObject.transform.localEulerAngles = new Vector3(0, 0, -angle + 90f);
        }
        public void FaceToPlayerEnd()
        {
            CancelInvoke("FaceToPlayerLoop");
        }
        
        private void FaceToPlayerLoop()
        {
            if (gameObject.transform.position.x - HeroController.instance.transform.position.x > 0)
            {
                gameObject.transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            Vector3 direction = HeroController.instance.transform.position - gameObject.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x);
            gameObject.transform.localEulerAngles = new Vector3(0, 0, (float)RadiansToDegrees(angle));
            Invoke("FaceToPlayerLoop", 0.02f);
        }
        public void FaceToPlayerOnce()
        {
            if (gameObject.transform.position.x - HeroController.instance.transform.position.x > 0)
            {
                gameObject.transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            Vector3 direction = HeroController.instance.transform.position - gameObject.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x);
            gameObject.transform.localEulerAngles = new Vector3(0, 0, (float)RadiansToDegrees(angle));
        }
        public void BossAngleRecover()
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            if(HeroController.instance.gameObject.transform.position.x >= gameObject.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            CancelInvoke("LockLoop");
            CancelInvoke("LockLoopForPlayer");
        }
    }
    public class TeleportSystem : MonoBehaviour
    {
        public void TeleportByAngle(float angle, float distance)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            if (gameObject.transform.position.x - HeroController.instance.transform.position.x > 0)
            {
                gameObject.transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            //gameObject.transform.localEulerAngles = new Vector3(0, 0, -angle + 90f);
            float angleR = angle * ((float)Math.PI / 180);
            float x = -(float)Math.Sin(angleR) * distance;
            float y = -(float)Math.Cos(angleR) * distance;
            gameObject.transform.position = HeroController.instance.transform.position + new Vector3(x, y, 0f);

            var glow = Instantiate(ItemPool.Glow, gameObject.transform.position, Quaternion.Euler(0, 0, -angle));
            glow.transform.SetParent(null);
            glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 3;
            glow.SetActive(true);
            glow.AddComponent<DelayDestory>();
            glow.GetComponent<DelayDestory>().On(2f);

            gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Teleport, 0.8f);
        }
        public void TeleportByFloor(float distance)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            gameObject.transform.position = new Vector3(HeroController.instance.transform.position.x + distance, 8.7f, 0f);
            if (gameObject.transform.position.x - HeroController.instance.transform.position.x > 0)
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }

            var glow = Instantiate(ItemPool.Glow, gameObject.transform.position, Quaternion.Euler(0, 0, 90f));
            glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 3;
            glow.SetActive(true);
            glow.AddComponent<DelayDestory>();
            glow.GetComponent<DelayDestory>().On(2f);

            gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Teleport, 0.8f);
        }
    }


    public class SlashBeam : MonoBehaviour
    {
        GameObject BEAM = HeroController.instance.grubberFlyBeamPrefabR;
        public void On(float angle, float speed)
        {
            var beam = Instantiate(BEAM, gameObject.transform);
            beam.transform.localPosition = new Vector3(3.5f, 0, 0);
            beam.transform.localEulerAngles = new Vector3(0f, 0, 0);
            beam.transform.SetParent(null);
            var collider = Instantiate(ItemPool.BeamCollider1, beam.transform);
            collider.SetActive(true);
            collider.AddComponent<BlockFlash>();
            beam.LocateMyFSM("Control").GetState("Inactive").GetAction<Wait>().time = 0.23f;
            beam.LocateMyFSM("Control").GetState("Active").GetAction<Wait>().time = 0f;
            beam.transform.localScale *= 3;
            beam.transform.localScale = new Vector3(beam.transform.localScale.x * gameObject.transform.localScale.x, beam.transform.localScale.y, beam.transform.localScale.z);

            StartCoroutine(DelayedExecution(0.04f));
            IEnumerator DelayedExecution(float time)
            {
                yield return new WaitForSeconds(time);
                float x = (float)Math.Sin(INFINITY.DegreesToRadians(angle)) * speed;
                float y = (float)Math.Cos(INFINITY.DegreesToRadians(angle)) * speed;
                beam.GetComponent<Rigidbody2D>().velocity = new Vector2(x, y) * gameObject.transform.localScale.x;
            }
        }
        public void OnByBossAngle(float speed)
        {
            var beam = Instantiate(BEAM, gameObject.transform);
            beam.transform.localPosition = new Vector3(3.5f, 0, 0);
            beam.transform.localEulerAngles = new Vector3(0f, 0, 0);
            beam.transform.SetParent(null);
            var collider = Instantiate(ItemPool.BeamCollider1, beam.transform);
            collider.SetActive(true);
            collider.AddComponent<BlockFlash>();
            beam.LocateMyFSM("Control").GetState("Inactive").GetAction<Wait>().time = 0.23f;
            beam.LocateMyFSM("Control").GetState("Active").GetAction<Wait>().time = 0f;
            beam.transform.localScale *= 3;
            beam.transform.localScale = new Vector3(beam.transform.localScale.x * gameObject.transform.localScale.x, beam.transform.localScale.y, beam.transform.localScale.z);

            StartCoroutine(DelayedExecution(0.04f));
            IEnumerator DelayedExecution(float time)
            {
                yield return new WaitForSeconds(time);
                float angle = - gameObject.transform.eulerAngles.z + 90f;
                float x = (float)Math.Sin(INFINITY.DegreesToRadians(angle)) * speed;
                float y = (float)Math.Cos(INFINITY.DegreesToRadians(angle)) * speed;
                beam.GetComponent<Rigidbody2D>().velocity = new Vector2(x, y) * gameObject.transform.localScale.x;
            }
        }
    }
    public class OneSlashLight : MonoBehaviour
    {
        public void DelayLight(float delayTime)
        {
            Invoke("Light", delayTime);
        }
        public void Light()
        {
            GameObject lightPrefab = null;
            for (int i = 0; i <= gameObject.transform.childCount; i++)
            {
                var obj = gameObject.transform.GetChild(i).gameObject;
                if (obj.name.Contains("Fireball2 Spiral"))
                {
                    obj.transform.localPosition = new Vector3(2, 0, 0);
                    lightPrefab = obj;
                    break;
                }
            }
            var light = Instantiate(lightPrefab, lightPrefab.transform);
            light.transform.SetParent(null);
            light.SetActive(true);
            light.GetComponent<BoxCollider2D>().enabled = false;
            light.LocateMyFSM("Fireball Control").GetState("L").RemoveAction<FlipScale>();
            light.LocateMyFSM("Fireball Control").SetState("Idle");
            light.GetComponent<tk2dSpriteAnimator>().Reflect().clipTime = 2.05f;
            StartCoroutine(DelayedExecution(0.02f));
            IEnumerator DelayedExecution(float time)
            {
                yield return new WaitForSeconds(time);
                light.transform.localScale = new Vector3(light.transform.localScale.x * 5f, light.transform.localScale.y * 2.5f, 1f);
                light.AddComponent<ObjDelayRecycle>();
                light.GetComponent<ObjDelayRecycle>().DelayRecycle(0.15f);
                light.GetComponent<ObjDelayRecycle>().DelayRecycle(0.15f);
                light.GetComponent<tk2dSpriteAnimator>().Reflect().clipTime = 2.05f;
            }
        }
    }
    public class LockAngleWaitForAttack : MonoBehaviour
    {
        private Vector3 LockLocalPosition;
        public GameObject lockedEnemy;
        Vector3 vec = Vector3.zero;
        float vecReduceFactor = 1.06f;
        double R1 => random.NextDouble();
        double R2 => random.NextDouble();
        public void StartToLock1(float angle, float distance)
        {
            gameObject.GetComponent<AngleSystem>().FaceToAngleOnce(angle);
            float angleR = angle * ((float)Math.PI / 180);
            float x = -(float)Math.Sin(angleR) * distance;
            float y = -(float)Math.Cos(angleR) * distance;
            LockLocalPosition = new Vector3(x, y, 0f);
            LockLoop1();
        }
        public void StartToLock(float angle, float distance)
        {
            gameObject.GetComponent<AngleSystem>().FaceToAngleOnce(angle);
            float angleR = angle * ((float)Math.PI / 180);
            float x = -(float)Math.Sin(angleR) * distance;
            float y = -(float)Math.Cos(angleR) * distance;
            LockLocalPosition = new Vector3(x, y, 0f);
            LockLoop();
        }
        public void StartToLockDtab(float angle, float distance)
        {
            //gameObject.GetComponent<AngleSystem>().FaceToAngleOnce(angle + 90f);
            float angleR = angle * ((float)Math.PI / 180);
            float x = -(float)Math.Sin(angleR) * distance;
            float y = -(float)Math.Cos(angleR) * distance;
            LockLocalPosition = new Vector3(x, y, 0f);
            LockLoopDtab();
        }
        public void StartToLockDtabPhase3()
        {
            float x = 0;
            if (HeroController.instance.gameObject.transform.localScale.x < 0)
            {
                x =  (12 + (float)R2 * 16);
            }
            else
            {
                x =  (-12 - (float)R2 * 16);
            }
            float y = 16;
            LockLocalPosition = new Vector3(x, y, 0f);
            LockLoopDtab();
        }
        public void StartToLockDtabPhase4()
        {
            float x = 0;
            x =  (17 + (float)R2 * 12);
            float y = 16;
            LockLocalPosition = new Vector3(x, y, 0f);
            LockLoopDtab();
        }
        public void StartToLockForPlayer(float angle, float distance)
        {
            GameObject closest = null;
            lockedEnemy = null;
            float closestDistance = Mathf.Infinity;
            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (obj != gameObject && obj != HeroController.instance.gameObject && obj.GetComponent<HealthManager>() != null && !obj.name.Contains("Zote Balloon"))
                {
                    float distance1 = Vector2.Distance(HeroController.instance.gameObject.transform.position, obj.transform.position);

                    if (distance1 < closestDistance)
                    {
                        closest = obj;
                        closestDistance = distance1;
                    }
                }
            }
            if(closest != null)
            {
                lockedEnemy = closest;

            }
            else
            {
                return;
            }
            if (lockedEnemy != null)
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                gameObject.transform.localEulerAngles = new Vector3(0, 0, -angle + 90f);
                float angleR = angle * ((float)Math.PI / 180);
                float x = -(float)Math.Sin(angleR) * distance;
                float y = -(float)Math.Cos(angleR) * distance;
                LockLocalPosition = new Vector3(x, y, 0f);
                gameObject.transform.position = lockedEnemy.transform.position + LockLocalPosition;

                var glow = Instantiate(ItemPool.Glow, gameObject.transform.position, Quaternion.Euler(0, 0, gameObject.transform.eulerAngles.z + 90f));
                glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 3;
                glow.SetActive(true);
                glow.AddComponent<DelayDestory>();
                glow.GetComponent<DelayDestory>().On(2f);
                gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Teleport, 0.8f);
                LockLoopForPlayer();
            }

        }
        public void LockLoop()
        {
            if (gameObject.transform.position.x - HeroController.instance.transform.position.x > 0)
            {
                gameObject.transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            gameObject.transform.position += (HeroController.instance.gameObject.transform.position + LockLocalPosition - gameObject.transform.position) / 32;
            Invoke("LockLoop", 0.02f);
        }
        public void LockLoop1()
        {
            if (gameObject.transform.position.x - HeroController.instance.transform.position.x > 0)
            {
                gameObject.transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            Vector3 a = (HeroController.instance.gameObject.transform.position + LockLocalPosition - gameObject.transform.position) / 5;
            vec += a;
            gameObject.transform.position += vec;
            vec /= vecReduceFactor;
            Invoke("LockLoop", 0.02f);
        }
        public void LockLoopDtab()
        {
            if (gameObject.transform.position.x - HeroController.instance.transform.position.x > 0)
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            gameObject.transform.position += (HeroController.instance.gameObject.transform.position + LockLocalPosition - gameObject.transform.position) / 32;
            Invoke("LockLoopDtab", 0.02f);
        }
        public void LockLoopForPlayer()
        {
            if (lockedEnemy != null)
            {
                if (gameObject.transform.position.x - lockedEnemy.transform.position.x > 0)
                {
                    gameObject.transform.localScale = new Vector3(1, -1, 1);
                }
                else
                {
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                }
                gameObject.transform.position += (lockedEnemy.transform.position + LockLocalPosition - gameObject.transform.position) / 7;
                Invoke("LockLoopForPlayer", 0.02f);
            }
            else
            {
            }
        }
        public void LockOnce(float angle, float distance)
        {
            gameObject.GetComponent<TeleportSystem>().TeleportByAngle(angle, distance);
            gameObject.GetComponent<AngleSystem>().FaceToAngleOnce(angle);
            float angleR = angle * ((float)Math.PI / 180);
            float x = -(float)Math.Sin(angleR) * distance;
            float y = -(float)Math.Cos(angleR) * distance;
            LockLocalPosition = new Vector3(x, y, 0f);
        }
        public void LockOnceForPlayer(float angle, float distance)
        {
            GameObject closest = null;
            lockedEnemy = null;
            float closestDistance = Mathf.Infinity;
            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (obj != gameObject && obj != HeroController.instance.gameObject && obj.GetComponent<HealthManager>() != null && !obj.name.Contains("Zote Balloon"))
                {
                    float distance1 = Vector2.Distance(HeroController.instance.transform.position, obj.transform.position);

                    if (distance1 < closestDistance)
                    {
                        closest = obj;
                        closestDistance = distance1;
                    }
                }
            }
            if (closest != null)
            {
                lockedEnemy = closest;
            }
            else
            {
                return;
            }
            if (lockedEnemy != null)
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                gameObject.transform.localEulerAngles = new Vector3(0, 0, -angle + 90f);
                float angleR = angle * ((float)Math.PI / 180);
                float x = -(float)Math.Sin(angleR) * distance;
                float y = -(float)Math.Cos(angleR) * distance;
                LockLocalPosition = new Vector3(x, y, 0f);
                if (gameObject.transform.position.x - lockedEnemy.transform.position.x > 0)
                {
                    gameObject.transform.localScale = new Vector3(1, -1, 1);
                }
                else
                {
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                }
                gameObject.transform.position = lockedEnemy.transform.position + LockLocalPosition;
                var glow = Instantiate(ItemPool.Glow, gameObject.transform.position, Quaternion.Euler(0, 0, gameObject.transform.eulerAngles.z + 90f));
                glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 3;
                glow.SetActive(true);
                glow.AddComponent<DelayDestory>();
                glow.GetComponent<DelayDestory>().On(2f);

                gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Teleport, 0.8f);
            }
            else
            {
            }
        }
        public void EndLock()
        {
            CancelInvoke("LockLoop");
            CancelInvoke("LockLoop1");
            CancelInvoke("LockLoopDtab");
            CancelInvoke("LockLoopForPlayer");
        }
    }
    public class OneSlashAnyAngle : MonoBehaviour
    {
        public void On(float angle, float speed)
        {
            float angleR = angle * ((float)Math.PI / 180);
            gameObject.GetComponent<OneSlash>().On(angleR, speed, 0f);
        }
    }
    public class OneSlash:MonoBehaviour
    {
        public void On(float angle, float speed, float waitTime)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Slash, 0.8f);
            StartCoroutine(DelayedExecution(waitTime));
            IEnumerator DelayedExecution(float time)
            {
                yield return new WaitForSeconds(time);
                //gameObject.LocateMyFSM("Control").SetState("CSlash");

                float x = (float)Math.Sin(angle) * speed;
                float y = (float)Math.Cos(angle) * speed;
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(x, y);
            }
        }
    }
    public class SlashColliderControl : MonoBehaviour
    {
        GameObject MainSlash;
        public void Start()
        {
            var slashes = gameObject.transform.Find("Slashes").gameObject;
            var slash1 = slashes.transform.Find("Slash1").gameObject;
            var slash2 = slashes.transform.Find("Slash2").gameObject;
            var slash3 = slashes.transform.Find("Slash3").gameObject;
            slash1.transform.localPosition += new Vector3(0, 1000, 0);
            slash3.transform.localPosition += new Vector3(0, 1000, 0);
            MainSlash = slash2;
        }
        public void SlashOn()
        {
            MainSlash.SetActive(true);
            if(ItemPool.Boss_Bugs_Pt != null)
            {
                ItemPool.Boss_Bugs_Pt.GetComponent<ParticleSystem>().emissionRate = 250;
            }
        }
        public void SlashOff()
        {
            Invoke("Off", 0.06f);
        }
        void Off()
        {
            MainSlash.SetActive(false);
            if (ItemPool.Boss_Bugs_Pt != null)
            {
                ItemPool.Boss_Bugs_Pt.GetComponent<ParticleSystem>().emissionRate = 5;
            }
        }
    }
    public class NailSpeedControl : MonoBehaviour
    {
        float speedFactorMax = 1;
        float acceleration = 1;
        float chanseFactor = 0.1f;
        float closeFactor = 6f;
        float angleFactor = 1;
        Vector2 velocityMax = Vector2.zero;
        Vector3 NailClosePosition = Vector2.zero;
        public void NailClose(Vector3 position)
        {
            gameObject.GetComponent<DamageHero>().damageDealt = 0;
            NailClosePosition = position;
            NailCloseLoop();
        }
        public void NailCloseLoop()
        {
            gameObject.transform.position += (NailClosePosition - gameObject.transform.position) / closeFactor;
            Invoke("NailCloseLoop", 0.02f);
        }
        public void NailCloseEnd()
        {
            CancelInvoke("NailCloseLoop");
        }
        public void NailAngleFaceToPlayer(float factor)
        {
            angleFactor = factor;
            NailAngleFaceToPlayerLoop();
        }
        public void NailAngleFaceToPlayerLoop()
        {
            Vector3 direction = HeroController.instance.transform.position - gameObject.transform.position;
            float angle = -Mathf.Atan2(direction.y, direction.x) + Mathf.PI / 2;
            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);
            gameObject.GetComponent<Rigidbody2D>().velocity += (new Vector2(x, y) / 10f - gameObject.GetComponent<Rigidbody2D>().velocity) * angleFactor * 0.35f;
            angleFactor /= 1.2f;
            Invoke("NailAngleFaceToPlayerLoop", 0.02f);
        }
        public void NailAngleFaceToPlayerLoopEnd()
        {
            CancelInvoke("NailAngleFaceToPlayerLoop");
        }

        public void NailChansePlayer()
        {
            NailChanseLoop();
        }
        public void NailChanseLoop()
        {
            float x = HeroController.instance.transform.position.x - gameObject.transform.position.x;
            float y = HeroController.instance.transform.position.y - gameObject.transform.position.y;
            float distance = Vector2.Distance(gameObject.transform.position, HeroController.instance.transform.position);
            gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(x, y) / distance / chanseFactor;
            Invoke("NailChanseLoop", 0.02f);
        }
        public void NailChansePlayerEnd()
        {
            CancelInvoke("NailChanseLoop");
        }
        public void NailSpeedChange(float speedFactor)
        {

        }
        public void NailFireDelay(float delayTime, float speed)
        {
            StartCoroutine(DelayedExecution(delayTime));
            IEnumerator DelayedExecution(float time)
            {
                yield return new WaitForSeconds(time);
                NailFire(speed);
            }
        }
        public void NailFire(float speed)
        {
            gameObject.GetComponent<DamageHero>().damageDealt = 2;
            NailSpeedUpEnd();
            NailCloseEnd();
            NailChansePlayerEnd();
            NailAngleFaceToPlayerLoopEnd();
            float angle = (float)DegreesToRadians(gameObject.transform.eulerAngles.z);
            float x = Mathf.Sin(angle) * speed;
            float y = Mathf.Cos(angle) * speed;
            gameObject.GetComponent<Rigidbody2D>().velocity *= speed * 20f;
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y * (1 + speed / 50f));
        }
        public void NailSpeedUp(float speedFactor, float accelerationFactor)
        {
            velocityMax = gameObject.GetComponent<Rigidbody2D>().velocity * speedFactor;
            acceleration = 1 / accelerationFactor;
            CancelInvoke("NailSpeedUpEnd");
            Invoke("NailSpeedUpEnd", 5f);
            CancelInvoke("NailSpeedUpLoop");
            NailSpeedUpLoop();
        }
        private void NailSpeedUpLoop()
        {
            gameObject.GetComponent<Rigidbody2D>().velocity += (velocityMax - gameObject.GetComponent<Rigidbody2D>().velocity) / acceleration;
            Invoke("NailSpeedUpLoop", 0.02f);
        }
        private void NailSpeedUpEnd()
        {
            CancelInvoke("NailSpeedUpLoop");
        }
    }
    public class Dtab : MonoBehaviour
    {
        System.Random random = new System.Random();
        double R => random.NextDouble();
        double R1 => random.NextDouble();
        public bool stomping = false;
        public float loopTime = 0.1f;
        public int plumeCount = 0;
        public int plumeCount1 = 0;
        Vector3 pos = Vector3.zero;
        public void StompDetect()
        {
            stomping = true;
        }
        public void Update()
        {
            if(stomping && gameObject.transform.position.y <= 6.8f)
            {
                stomping = false;
                if(gameObject.LocateMyFSM("Control").ActiveStateName == "SD9")
                {
                    gameObject.LocateMyFSM("Control").SetState("SL9");
                }
                else if(gameObject.LocateMyFSM("Control").ActiveStateName == "SD5")
                {
                    gameObject.LocateMyFSM("Control").SetState("SL5");
                }
                else if(gameObject.LocateMyFSM("Control").ActiveStateName == "SDPhase4")
                {
                    gameObject.LocateMyFSM("Control").SetState("SLPhase4");
                }
                else if(gameObject.LocateMyFSM("Control").ActiveStateName == "SDPhase3")
                {
                    gameObject.LocateMyFSM("Control").SetState("SLPhase3");
                }
                else if(gameObject.LocateMyFSM("Control").ActiveStateName == "SDPhase5")
                {
                    gameObject.LocateMyFSM("Control").SetState("SLPhase5");
                }
            }
        }
        public void PlumeSummon(float time)
        {
            if(HardMode == 0)
            {
                loopTime = 0.2f;
            }
            else if(HardMode == 1)
            {
                loopTime = 0.1f;
            }
            PlumeSummonLoop();
            Invoke("PlumeSummonEnd", time);
        }
        void PlumeSummonEnd()
        {
            CancelInvoke("PlumeSummonLoop");
        }
        public void PlumeSummonLoop()
        {
            float x = (float)R * 18f - 9f + HeroController.instance.gameObject.transform.position.x; 
            GameObject plume = GameObject.Instantiate(ItemPool.Plume, ItemPool.Plumes.transform);
            plume.transform.position = new Vector3(x, 3.9f, 0);
            plume.transform.localEulerAngles += new Vector3(0, 0, (float)R1 * 30f - 15f);
            plume.LocateMyFSM("FSM").FsmVariables.FindFsmBool("Auto").Value = true;
            plume.LocateMyFSM("FSM").ChangeTransition("Outside Arena?", "OUTSIDE", "Antic");
            plume.SetActive(true);
            plume.AddComponent<ObjDelayRecycle>().DelayRecycle(5f);
            Invoke("PlumeSummonLoop", loopTime);
        }
        bool PlumePhase3Started = false;
        public void PlumeSummon9(float time, bool isPhase3)
        {
            if(isPhase3)
            {       
                PlumePhase3Started = true;
            }
            plumeCount = 0;
            plumeCount1 = 0;
            loopTime = 0.1f;
            pos = gameObject.transform.position;
            PlumeSummonLoop9();
            Invoke("PlumeSummonLoop9More", 1f);
            Invoke("PlumeSummonEnd9", time);
            Invoke("StartSkill1", 2f);
        }
        void PlumeSummonEnd9()
        {
            CancelInvoke("PlumeSummonLoop9");
            CancelInvoke("PlumeSummonLoop9More");
            CancelInvoke("PlumeSummonLoop9Left");
            CancelInvoke("PlumeSummonLoop9LeftMore");
        }
        public void PlumeSummonLoop9()
        {
            float x = pos.x + plumeCount * 4f; 
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
            if(!PlumePhase3Started)
            {
                StartCoroutine(DelayedExecution2(4.5f));
                IEnumerator DelayedExecution2(float time)
                {
                    yield return new WaitForSeconds(time);
                    plume.GetComponent<tk2dSpriteAnimator>().Resume();
                    plume.AddComponent<ObjDelayRecycle>().DelayRecycle(3f);
                }
            }

            PlumeSummonLoop9Left();
            plumeCount++;
            Invoke("PlumeSummonLoop9", loopTime);
        }
        public void PlumeSummonLoop9Left()
        {
            float x = pos.x - plumeCount * 4f; 
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
            if (!PlumePhase3Started)
            {
                StartCoroutine(DelayedExecution2(4.5f));
                IEnumerator DelayedExecution2(float time)
                {
                    yield return new WaitForSeconds(time);
                    plume.GetComponent<tk2dSpriteAnimator>().Resume();
                    plume.AddComponent<ObjDelayRecycle>().DelayRecycle(3f);
                }
            }
        }
        public void PlumeSummonLoop9More()
        {
            float x = pos.x + plumeCount1 * 4f + 2f;
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
            if (!PlumePhase3Started)
            {
                StartCoroutine(DelayedExecution2(3.5f));
                IEnumerator DelayedExecution2(float time)
                {
                    yield return new WaitForSeconds(time);
                    plume.GetComponent<tk2dSpriteAnimator>().Resume();
                    plume.AddComponent<ObjDelayRecycle>().DelayRecycle(3f);
                }
            }

            PlumeSummonLoop9LeftMore();
            plumeCount1++;
            Invoke("PlumeSummonLoop9More", loopTime);
        }
        public void PlumeSummonLoop9LeftMore()
        {
            float x = pos.x - plumeCount1 * 4f - 2f;
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
            if (!PlumePhase3Started)
            {
                StartCoroutine(DelayedExecution2(3.5f));
                IEnumerator DelayedExecution2(float time)
                {
                    yield return new WaitForSeconds(time);
                    plume.GetComponent<tk2dSpriteAnimator>().Resume();
                    plume.AddComponent<ObjDelayRecycle>().DelayRecycle(3f);
                }
            }
        }
        public void StartSkill1()
        {
            gameObject.GetComponent<SkillsControl>().Skill1();
        }
    }
    public class LittleBlastControl:MonoBehaviour
    {
        bool IsChanse = false;
        float ChanseSpeedFactor;
        float SpeedReduceFactor;
        Vector3 ChansePosition;
        public void On(bool isChanse, float chanseSpeedFactor, float speedReduceFactor, Vector3 chansePosition)
        {
            IsChanse = isChanse;
            ChanseSpeedFactor = chanseSpeedFactor;
            SpeedReduceFactor = speedReduceFactor;
            ChansePosition = chansePosition;
            if (isChanse)
            {
                ChanseLoop();
            }
            gameObject.transform.Find("Blast").gameObject.SetActive(false);
            gameObject.transform.Find("Blast").gameObject.SetActive(true);
        }
        void ChanseLoop()
        {
            gameObject.transform.position += (ChansePosition - gameObject.transform.position) * ChanseSpeedFactor;
            ChanseSpeedFactor /= SpeedReduceFactor;
            Invoke("ChanseLoop", 0.02f);
            Invoke("ChanseLoopEnd", 1.3f);
        }
        void ChanseLoopEnd()
        {
            CancelInvoke("ChanseLoop");
            gameObject.transform.Find("Blast").gameObject.SetActive(false);
        }
    }
    public class BossBurstControl: MonoBehaviour
    {
        public void BigOn()
        {
            var burst = Instantiate(ItemPool.DashBurst, gameObject.transform);

            burst.LocateMyFSM("FSM").FsmVariables.GetFsmVector3("Init Pos").Value = new Vector3(-10.5f, -0.2f, 0f);
            burst.LocateMyFSM("FSM").GetState("Init").RemoveAction(0);
            burst.LocateMyFSM("FSM").GetState("Init").RemoveAction(1);

            burst.SetActive(true);
            burst.transform.localScale = new Vector3(-11 * Math.Sign(gameObject.transform.localScale.x), 6 * Math.Sign(gameObject.transform.localScale.y), 3);
            burst.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z);
            burst.AddComponent<DelayDestory>();
            burst.GetComponent<DelayDestory>().On(3f);
        }
        public void SmallOn()
        {
            var burst = Instantiate(ItemPool.DashBurst, gameObject.transform);

            burst.LocateMyFSM("FSM").FsmVariables.GetFsmVector3("Init Pos").Value = new Vector3(-10.5f, -0.2f, 0f);
            burst.LocateMyFSM("FSM").GetState("Init").RemoveAction(0);
            burst.LocateMyFSM("FSM").GetState("Init").RemoveAction(1);

            burst.transform.localPosition += new Vector3(0f, 1f, 0f);
            burst.SetActive(true);
            burst.transform.localScale = new Vector3(-9 * Math.Sign(gameObject.transform.localScale.x), 4 * Math.Sign(gameObject.transform.localScale.y), 3);
            burst.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z);
            burst.AddComponent<DelayDestory>();
            burst.GetComponent<DelayDestory>().On(3f);
        }
        public void DstabOn()
        {
            var burst = Instantiate(ItemPool.DashBurst, gameObject.transform);

            burst.LocateMyFSM("FSM").FsmVariables.GetFsmVector3("Init Pos").Value = new Vector3(0f, 0f, 0f);
            burst.LocateMyFSM("FSM").GetState("Init").RemoveAction(0);
            burst.LocateMyFSM("FSM").GetState("Init").RemoveAction(1);

            burst.SetActive(true);
            burst.transform.localScale = new Vector3(-7 * gameObject.transform.localScale.x, 4, 3);
            burst.transform.eulerAngles = new Vector3(0, 0, 270);
            burst.transform.position = gameObject.transform.position;
            burst.AddComponent<DelayDestory>();
            burst.GetComponent<DelayDestory>().On(3f);

        }
    }
    public class SkillsControl : MonoBehaviour
    {
        System.Random random = new System.Random();
        double R => random.NextDouble();
        double R1 => random.NextDouble();
        double R2 => random.NextDouble();
        float angle = 0;
        public int slashCount = 0;
        public int slashCountMax = 4;
        public bool shrinkStarted = false;
        public bool phase2Updated = false;
        public bool phase3Updated = false;
        public bool phase4Updated = false;
        public bool phase5Updated = false;

        public bool musicEntered = false;

        public void Start()
        {
            BossFsmInit();
            SkillPhase3Init();
            SkillPhase4Init();
            SkillPhase5Init();
            Skill1TurnPhaseInit();
            Skill1Init();
            Skill2Init();
            Skill3Init();
            Skill4Init();
            Skill5Init();
            Skill6Init();
            Skill7Init();
            Skill8Init();
            Skill9Init();
            Skill10Init();
            Skill11Init();
            Skill12Init();
            SkillShort1Init();
            SkillShort2Init();
            SkillShort3Init();
        }
        public void BossFsmInit()
        {
            gameObject.GetComponent<AudioSource>().maxDistance = 500f;
            gameObject.GetComponent<AudioSource>().minDistance = 1f;
            var energy = gameObject.transform.Find("Intro Energy").gameObject;
            var zote = energy.transform.Find("Mighty_Zote_entrance_energy0002").gameObject;
            zote.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            var ball = gameObject.transform.Find("Focus Blast").gameObject;
            var rockPt = ball.transform.Find("Particle_rocks_small").gameObject;

            rockPt.GetComponent<ParticleSystem>().startSpeed = 500;
            rockPt.GetComponent<ParticleSystem>().startSize = 1;

            ItemPool.Energy = energy;
            ItemPool.Energy.SetActive(false);

            if (!ItemPool.Plumes)
            {
                ItemPool.Plumes = new GameObject();
                ItemPool.Plumes.transform.position = new Vector3(0, 0, 0);
            }

            if (!ItemPool.RockPt_Wide)
            {
                ItemPool.RockPt_Wide = Instantiate(rockPt, rockPt.transform.position + new Vector3(-500f, 0f, 0f), Quaternion.Euler(0, 0, 0));
                ItemPool.RockPt_Wide.AddComponent<RockPtControl>();
                ItemPool.RockPt_Wide.SetActive(true);
            }

            if (!ItemPool.RockPt_Thin)
            {
                ItemPool.RockPt_Thin = Instantiate(rockPt, rockPt.transform.position + new Vector3(-500f, 0f, 0f), Quaternion.Euler(0, 0, 0));
                ItemPool.RockPt_Thin.transform.localScale = new Vector3(6f, 1.5f, 1.9216f);
                ItemPool.RockPt_Thin.AddComponent<RockPtControl>();
                ItemPool.RockPt_Thin.GetComponent<ParticleSystem>().startDelay = 0;
                ItemPool.RockPt_Thin.SetActive(true);
            }

            //rockPt.Recycle();

            if(!ItemPool.Teleport)
            {
                ItemPool.Teleport = gameObject.LocateMyFSM("Control").GetState("Tele Out").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;
            }
            if(!ItemPool.Dir)
            {
                ItemPool.Dir = gameObject.LocateMyFSM("Control").GetState("Counter Stance").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;
            }
            if(!ItemPool.Slash)
            {
                ItemPool.Slash = HeroController.instance.gameObject.LocateMyFSM("Nail Arts").GetState("G Slash").GetAction<AudioPlay>().oneShotClip.Value as AudioClip;
            }
            if(!ItemPool.CounterFlash)
            {
                ItemPool.CounterFlash = Instantiate(gameObject.transform.Find("Counter Flash").gameObject, gameObject.transform);
                ItemPool.CounterFlash.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            if(!ItemPool.FocusBall)
            {
                ItemPool.FocusBall = gameObject.transform.Find("Focus Blast").gameObject;
            }
            if(!ItemPool.FocusHit)
            {
                ItemPool.FocusHit = gameObject.transform.Find("Focus Hit").gameObject;
            }
            if(!ItemPool.DashBurst)
            {
                ItemPool.DashBurst = gameObject.transform.Find("Dash Burst").gameObject;
                ItemPool.DashBurst.transform.localScale = new Vector3(-11, 6, 3);
            }
            if(!ItemPool.Corpse)
            {
                ItemPool.Corpse = gameObject.transform.Find("Corpse HK Prime(Clone)").gameObject;
            }


            gameObject.GetComponent<Skills>().MainSlash.SetActive(false);

            gameObject.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Gravity Scale").Value = 0f;
            gameObject.LocateMyFSM("Stun Control").FsmVariables.FindFsmInt("Stun Combo").Value = 99999;
            gameObject.LocateMyFSM("Stun Control").FsmVariables.FindFsmInt("Stun Hit Max").Value = 99999;

            gameObject.LocateMyFSM("Control").CopyState("Slash1 Antic", "SA1");
            gameObject.LocateMyFSM("Control").CopyState("Slash1 Antic", "SA2");
            gameObject.LocateMyFSM("Control").CopyState("Slash1 Antic", "SA3");
            gameObject.LocateMyFSM("Control").GetState("Slash1").RemoveAction(4);
            gameObject.LocateMyFSM("Control").GetState("Slash 2").RemoveAction(4);
            gameObject.LocateMyFSM("Control").GetState("Slash 2").RemoveAction(3);
            gameObject.LocateMyFSM("Control").GetState("Slash2 Recover").RemoveAction<SetPolygonCollider>();
            gameObject.LocateMyFSM("Control").GetState("Set Idle Time").AddMethod(() =>
            {
                ItemPool.Energy.SetActive(false);

                if (ItemPool.Boss_Bugs_Pt != null)
                {
                    ItemPool.Boss_Bugs_Pt.GetComponent<ParticleSystem>().emissionRate = 5;
                }
                //gameObject.GetComponent<SlashColliderControl>().SlashOff();
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                gameObject.GetComponent<AngleSystem>().FaceToPlayerEnd();
                if (gameObject.GetComponent<PhaseControl>().phase != 4)
                {
                    gameObject.GetComponent<Phase5Arena>().Phase4InvincibleEnd();
                }
                gameObject.GetComponent<PhaseControl>().PhaseUpdate();

                if(!phase2Updated && gameObject.GetComponent<PhaseControl>().phase == 2)
                {

                    phase2Updated = true;
                    gameObject.GetComponent<SceneControl>().Phase2();
                    gameObject.GetComponent<Phase5Arena>().Phase4InvincibleStart();
                    SkillTurnPhase();
                    return;
                }
                if(!phase3Updated && gameObject.GetComponent<PhaseControl>().phase == 3)
                {
                    phase3Updated = true;
                    gameObject.GetComponent<SceneControl>().Phase3();
                    gameObject.GetComponent<Phase5Arena>().Phase4InvincibleStart();
                    SkillTurnPhase();
                    return;
                }
                if(!phase4Updated && gameObject.GetComponent<PhaseControl>().phase == 4)
                {
                    phase4Updated = true;
                    gameObject.GetComponent<SceneControl>().Phase4();
                    gameObject.GetComponent<Phase5Arena>().Phase4InvincibleStart();
                    SkillTurnPhase();
                    return;
                }
                if(!phase5Updated && gameObject.GetComponent<PhaseControl>().phase == 5)
                {
                    phase5Updated = true;
                    gameObject.GetComponent<SceneControl>().Phase5();
                    gameObject.GetComponent<Phase5Arena>().Phase4InvincibleStart();

                    gameObject.GetComponent<SkillsControl>().AnimFreezeTime(10000f, 0f);
                    gameObject.transform.position += new Vector3(0f, -300f, 0f);

                    SkillPhase5();
                    return;
                }
                if (HardMode == 0)
                {
                    if (gameObject.GetComponent<PhaseControl>().phase >= 4 && shrinkStarted == false)
                    {
                        gameObject.GetComponent<Phase5Arena>().phase4X = HeroController.instance.transform.position.x;
                        gameObject.GetComponent<Phase5Arena>().Place();
                        Modding.Logger.Log("Phase 4 started!");
                        SkillPhase4();
                        shrinkStarted = true;
                    }
                    else
                    {
                        if (gameObject.GetComponent<PhaseControl>().phase == 1)
                        {
                            gameObject.GetComponent<BossSkillChoice>().Phase1Skill();
                        }
                        if (gameObject.GetComponent<PhaseControl>().phase == 2)
                        {
                            gameObject.GetComponent<BossSkillChoice>().Phase2Skill();
                        }
                        if (gameObject.GetComponent<PhaseControl>().phase == 3)
                        {
                            gameObject.GetComponent<BossSkillChoice>().Phase3Skill();
                        }
                        if (gameObject.GetComponent<PhaseControl>().phase == 4)
                        {
                            gameObject.GetComponent<BossSkillChoice>().Phase4Skill();
                        }
                        if (gameObject.GetComponent<PhaseControl>().phase == 5)
                        {
                            gameObject.GetComponent<BossSkillChoice>().Phase5Skill();
                        }
                    }
                }
                else if (HardMode == 1)
                {
                    if (gameObject.GetComponent<PhaseControl>().phase >= 3 && gameObject.GetComponent<PhaseControl>().phase3Started == false)
                    {
                        gameObject.GetComponent<PhaseControl>().phase3Started = true;
                        Modding.Logger.Log("Phase 3 started!");
                        shrinkStarted = false;
                        SkillPhase3();
                    }
                    else if (gameObject.GetComponent<PhaseControl>().phase >= 4 && shrinkStarted == false)
                    {
                        gameObject.GetComponent<Phase5Arena>().phase4X = HeroController.instance.transform.position.x;
                        gameObject.GetComponent<Phase5Arena>().Place();
                        Modding.Logger.Log("Phase 4 started!");
                        SkillPhase4();
                        shrinkStarted = true;
                    }
                    else
                    {
                        if (gameObject.GetComponent<PhaseControl>().phase == 1)
                        {
                            gameObject.GetComponent<BossSkillChoice>().Phase1Skill();
                        }
                        if (gameObject.GetComponent<PhaseControl>().phase == 2)
                        {
                            gameObject.GetComponent<BossSkillChoice>().Phase2Skill();
                        }
                        if (gameObject.GetComponent<PhaseControl>().phase == 3)
                        {
                            gameObject.GetComponent<BossSkillChoice>().Phase3Skill();
                        }
                        if (gameObject.GetComponent<PhaseControl>().phase == 4)
                        {
                            gameObject.GetComponent<BossSkillChoice>().Phase4Skill();
                        }
                        if (gameObject.GetComponent<PhaseControl>().phase == 5)
                        {
                            gameObject.GetComponent<BossSkillChoice>().Phase5Skill();
                        }
                    }
                }

            });
        }
        public void BossRoarWave()
        {
            var fsmWave = ItemPool.HK.LocateMyFSM("Control").GetState("Intro Roar").GetAction<CreateObject>().gameObject.Value;

            var wave = Instantiate(fsmWave, gameObject.transform.position, new Quaternion());

            wave.GetComponent<DisableAfterTime>().waitTime = 0.16f;

            wave.transform.Find("wave 1").localScale = new Vector3(2.5f, 2.5f, 1f);

            wave.transform.Find("wave 2").GetComponent<SpriteRenderer>().enabled = false;

            wave.SetActive(true);

            wave.AddComponent<ObjDelayRecycle>().DelayRecycle(0.151f);
        }
        public void VelocitySetZero()
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        public void AnimFreezeTime(float time, float delayTime)
        {
            Invoke("AnimFreeze", delayTime);
            Invoke("AnimFreezeEnd", time + delayTime);
        }
        public void AnimFreeze()
        {
            gameObject.GetComponent<tk2dSpriteAnimator>().Pause();
        }
        public void AnimFreezeEnd()
        {
            gameObject.GetComponent<tk2dSpriteAnimator>().Resume();
        }
        public void InvokeSetState(string stateName, float delayTime)
        {
            StartCoroutine(DelayedExecution(delayTime));
            IEnumerator DelayedExecution(float time)
            {
                yield return new WaitForSeconds(time);
                gameObject.LocateMyFSM("Control").SetState(stateName);
            }
        }
        float skill4LoopTime = 0.1f;
        public void Skill4NailSummon(float time)
        {
            Skill4NailSummonLoop();
            Invoke("Skill4NailSummonEnd", time);
        }
        public void Skill4NailSummonLoop()
        {
            var nail = Instantiate(ItemPool.Nail, gameObject.transform);
            nail.transform.localPosition = Vector3.zero;
            nail.transform.localScale = new Vector3(nail.transform.localScale.x, nail.transform.localScale.y * gameObject.transform.localScale.y, 1f);
            nail.transform.SetParent(null);

            Vector3 direction = HeroController.instance.transform.position - gameObject.transform.position;
            //float angle = Mathf.Atan2(direction.y, direction.x) + (float)R * Mathf.PI + Mathf.PI / 2;
            float angle = -(float)R * Mathf.PI / 1.5f + Mathf.PI / 3f;
            //float angle = (float)R * 180f + -gameObject.transform.localScale.x * gameObject.transform.localEulerAngles.z;
            float x = Mathf.Sin(angle) * 22;
            float y = Mathf.Cos(angle) * 22;
            Vector3 position = new Vector3(x, y, 0) + HeroController.instance.transform.position;
            nail.AddComponent<NailSpeedControl>();
            //nail.GetComponent<NailSpeedControl>().NailChansePlayer();
            nail.GetComponent<NailSpeedControl>().NailAngleFaceToPlayer(1f);
            nail.GetComponent<NailSpeedControl>().NailClose(position);
            nail.GetComponent<NailSpeedControl>().NailFireDelay(1f, 150f);
            nail.transform.Find("Beam").gameObject.GetComponent<DeactivateAfter2dtkAnimation>().enabled = false;

            Invoke("Skill4NailSummonLoop", skill4LoopTime);
        }
        public void Skill4NailSummonEnd()
        {
            CancelInvoke("Skill4NailSummonLoop");
        }

        int sigh = 1;
        public void Skill7NailSummon(float time)
        {
            Skill7NailSummonLoop();
            Invoke("Skill7NailSummonEnd", time);
        }
        public void Skill7NailSummonLoop()
        {
            var nail = Instantiate(ItemPool.Nail, gameObject.transform);
            nail.transform.localPosition = Vector3.zero;
            nail.transform.localScale = new Vector3(nail.transform.localScale.x, nail.transform.localScale.y * gameObject.transform.localScale.y, 1f);
            nail.transform.SetParent(null);
            float r = (float)R;
            Vector3 direction = HeroController.instance.transform.position - gameObject.transform.position;
            float angle = -(float)R * Mathf.PI / 1.5f + Mathf.PI / 3f;
            float x = Mathf.Sin(angle) * r * 12;
            float y = Mathf.Cos(angle) * r * 12;
            Vector3 position = new Vector3(x, y, 0) + gameObject.transform.position;
            nail.AddComponent<NailSpeedControl>();
            float distance = Vector2.Distance(gameObject.transform.position, HeroController.instance.gameObject.transform.position);
            Vector2 vector = (HeroController.instance.gameObject.transform.position - gameObject.transform.position) / distance * 0.05f;
            nail.GetComponent<Rigidbody2D>().velocity = vector;
            nail.GetComponent<NailSpeedControl>().NailClose(position);
            nail.GetComponent<NailSpeedControl>().NailFireDelay(1f, 150f);
            nail.transform.Find("Beam").gameObject.GetComponent<DeactivateAfter2dtkAnimation>().enabled = false;
            if (HardMode == 0)
            {
                Invoke("Skill7NailSummonLoop", 0.15f);
            }
            if (HardMode == 1)
            {
                Invoke("Skill7NailSummonLoop", 0.1f);
            }
            /*
            StartCoroutine(ExecuteWithDelay());
            IEnumerator ExecuteWithDelay()
            {
                for (int loopTime = 0; loopTime <= 19; loopTime++)
                {
                    float r = (float)R * 4f;
                    var nail = Instantiate(ItemPool.Nail, gameObject.transform);
                    nail.transform.localPosition = Vector3.zero;
                    nail.transform.localScale = new Vector3(nail.transform.localScale.x, nail.transform.localScale.y * gameObject.transform.localScale.y, 1f);
                    nail.transform.SetParent(null);
                    Vector3 position = new Vector3(sigh * 16f, loopTime * 2f - 20f + r, 0) + HeroController.instance.transform.position;
                    nail.AddComponent<NailSpeedControl>();
                    nail.GetComponent<NailSpeedControl>().NailClose(position);
                    nail.GetComponent<Rigidbody2D>().velocity = new Vector2(-0.05f * sigh, 0);
                    nail.GetComponent<NailSpeedControl>().NailFireDelay(0.9f, 60f);
                    nail.transform.Find("Beam").gameObject.GetComponent<DeactivateAfter2dtkAnimation>().enabled = false;
                    yield return new WaitForSeconds(0.03f);
                }
            }
            sigh *= -1;
            Invoke("Skill7NailSummon", 0.9f);
            Invoke("Skill7NailSummonEnd", 3.5f);
            */
        }
        public void Skill7NailSummonEnd()
        {
            CancelInvoke("Skill7NailSummonLoop");
        }
        public void Skill8NailSummon(float angle)
        {
            float r = (float)R;
            var nail = Instantiate(ItemPool.Nail, gameObject.transform);
            nail.transform.localPosition = new Vector3(-7f, r * 20f - 10f, 0f);
            nail.transform.localScale = new Vector3(nail.transform.localScale.x, nail.transform.localScale.y * gameObject.transform.localScale.y, 1f);
            nail.transform.SetParent(null);
            nail.AddComponent<NailSpeedControl>();
            float distance = Vector2.Distance(gameObject.transform.position, HeroController.instance.gameObject.transform.position);
            //Vector2 vector = (HeroController.instance.gameObject.transform.position - gameObject.transform.position) / distance * 0.05f;
            Vector2 vector = new Vector2(Mathf.Sin((float)DegreesToRadians(angle + 180f)), Mathf.Cos((float)DegreesToRadians(angle + 180f))) * 0.05f;

            nail.GetComponent<Rigidbody2D>().velocity = vector;
            nail.GetComponent<NailSpeedControl>().NailFireDelay(1f, 150f);
            nail.transform.Find("Beam").gameObject.GetComponent<DeactivateAfter2dtkAnimation>().enabled = false;
            Invoke("Skill8NailSummon", 0.1f);
            Invoke("Skill8NailSummonEnd", 2.5f);
            /*
            float r = (float)R;
            float angle = -(float)R * Mathf.PI / 1.5f + Mathf.PI / 3f;
            for (int i = 0;i < 3;i++)
            {
                var nail = Instantiate(ItemPool.Nail, gameObject.transform);
                nail.transform.localPosition = Vector3.zero;
                nail.transform.localScale = new Vector3(nail.transform.localScale.x, nail.transform.localScale.y * gameObject.transform.localScale.y, 1f);
                nail.transform.SetParent(null);
                float x = Mathf.Sin(angle + Mathf.PI * 2 / 3 * i) * 12;
                float y = Mathf.Cos(angle + Mathf.PI * 2 / 3 * i) * 12;
                Vector3 position = new Vector3(x, y, 0) + HeroController.instance.gameObject.transform.position + new Vector3(0, 0.5f, 0);
                nail.AddComponent<NailSpeedControl>();
                float distance = Vector2.Distance(position, HeroController.instance.gameObject.transform.position);
                Vector2 vector = (HeroController.instance.gameObject.transform.position - position) / distance * 0.05f;
                nail.GetComponent<Rigidbody2D>().velocity = vector;
                nail.GetComponent<NailSpeedControl>().NailClose(position);
                nail.GetComponent<NailSpeedControl>().NailFireDelay(1f, 150f);
                nail.transform.Find("Beam").gameObject.GetComponent<DeactivateAfter2dtkAnimation>().enabled = false;
            }
            Invoke("Skill8NailSummon", 0.5f);
            Invoke("Skill8NailSummonEnd", 2.4f);
            StartCoroutine(ExecuteWithDelay());
            IEnumerator ExecuteWithDelay()
            {
                for (int loopTime = 0; loopTime <= 19; loopTime++)
                {
                    float r = (float)R * 4f;
                    var nail = Instantiate(ItemPool.Nail, gameObject.transform);
                    nail.transform.localPosition = Vector3.zero;
                    nail.transform.localScale = new Vector3(nail.transform.localScale.x, nail.transform.localScale.y * gameObject.transform.localScale.y, 1f);
                    nail.transform.SetParent(null);
                    Vector3 position = new Vector3(sigh * 16f, loopTime * 2f - 20f + r, 0) + HeroController.instance.transform.position;
                    nail.AddComponent<NailSpeedControl>();
                    nail.GetComponent<NailSpeedControl>().NailClose(position);
                    nail.GetComponent<Rigidbody2D>().velocity = new Vector2(-0.05f * sigh, 0);
                    nail.GetComponent<NailSpeedControl>().NailFireDelay(0.9f, 60f);
                    nail.transform.Find("Beam").gameObject.GetComponent<DeactivateAfter2dtkAnimation>().enabled = false;
                    yield return new WaitForSeconds(0.03f);
                }
            }
            sigh *= -1;
            Invoke("Skill8NailSummon", 0.9f);
            Invoke("Skill8NailSummonEnd", 3.5f);
            */
        }
        public void Skill8NailSummonEnd()
        {
            CancelInvoke("Skill8NailSummon");
        }

        int nail11Count = 0;
        float r1 = 0;
        float r2 = 0;
        float skill11scaleY = 1;
        float skill11LoopTime = 0.1f;
        Vector3 Skill11Pos = new Vector3(0, 0, 0);
        public void Skill11NailSummon(float time)
        {
            float r1 = (float)R;
            float r2 = (float)R;
            nail11Count = 0;
            Skill11NailSummonLoop();
            Invoke("Skill11NailSummonEnd", time);
        }
        public void Skill11NailSummonLoop()
        {
            Vector3 pos = Vector3.zero;
            if (HeroController.instance.transform.position.x <= Skill11Pos.x)
            {
                pos = new Vector3(13 - (nail11Count * 3 - (float)R * 9), 16 + (float)R * 9, 0);
            }
            else
            {
                pos = new Vector3(-13 + (nail11Count * 3 - (float)R * 9), 16 + (float)R * 9, 0);
            }

            nail11Count++;

            var nail = Instantiate(ItemPool.Nail, Skill11Pos + pos, Quaternion.Euler(0,0,0));
            nail.transform.SetParent(null);

            if(HeroController.instance.transform.position.x <= Skill11Pos.x)
            {
                nail.GetComponent<Rigidbody2D>().velocity = new Vector2(-(5.5f + r1 * 3), -(7.5f + r2)) * 5;
            }
            else
            {
                nail.GetComponent<Rigidbody2D>().velocity = new Vector2(5.5f + r1, -(7.5f + r2)) * 5;
            }
            nail.transform.Find("Beam").gameObject.GetComponent<DeactivateAfter2dtkAnimation>().enabled = false;

            Invoke("Skill11NailSummonLoop", skill11LoopTime);
        }
        public void Skill11NailSummonEnd()
        {
            CancelInvoke("Skill11NailSummonLoop");
        }
        public void Skill12BigSlashSummon(float time)
        {
            Skill12BigSlashSummonLoop();
            Invoke("Skill12BigSlashSummonEnd", time * bigSlashTimeFactor);
        }
        int sigh1 = 1;
        int sigh2 = 1;
        int sigh3 = 1;
        float bigSlashEdgeFactor = 1f;
        float bigSlashTimeFactor = 1f;
        float lastEularAngle = 0f;
        float skill12LoopTime = 0.2f;
        int slashcount = 0;
        public bool skill12isBig = false;
        public bool hardmodeEnd = false;
        public void CounterFlashSummon(float eulerAngle)
        {
            var flash = Instantiate(ItemPool.CounterFlash, ItemPool.CounterFlash.transform);
            flash.transform.SetParent(null);
            flash.transform.eulerAngles = new Vector3 (0, 0, eulerAngle);
            flash.SetActive(true);
            flash.AddComponent<DelayDestory>().On(3f);
        }
        public void Skill12BigSlashSummonLoop()
        {
            HeroController.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Dir, 1f);

            Invoke("Skill12BigSlashSummonLoop", skill12LoopTime);

            gameObject.GetComponent<Skills>().GetBigSlash();

            sigh1 *= -1;
            sigh2 *= -1;
            sigh3 *= -1;
            Vector3 pos = new Vector3(0, 0, 0);
            float eul = 0f;
            if (HeroController.instance.gameObject.transform.position.y <= 11f)
            {
                pos = new Vector3(sigh1 * (float)R1 * 13f * bigSlashEdgeFactor, 1 + (-4f + (float)R2 * 9f) * bigSlashEdgeFactor, 0f);
                eul = sigh3 * 90f - 55f + (float)R * 110f;
            }
            else
            {
                pos = new Vector3(sigh1 * (float)R1 * 13f * bigSlashEdgeFactor, sigh2 * (float)R2 * 9f * bigSlashEdgeFactor, 0f);
                eul = sigh3 * 90f - 55f + (float)R * 110f;
            }
            CounterFlashSummon(eul);

            GameObject bigslash = Instantiate(ItemPool.BigSlash, HeroController.instance.gameObject.transform.position + pos, Quaternion.Euler(0, 0, eul));


            if (skill12LoopTime < 0.1f)
            {
                bigslash.transform.localScale = new Vector3(bigslash.transform.localScale.x, bigslash.transform.localScale.y * (0.8f + (float)R * 0.6f), 1f);
            }
            bigslash.GetComponent<BigSlashControl>().On(1f);

            if(Mathf.Abs(bigslash.transform.eulerAngles.z - lastEularAngle) <= 15f || (Mathf.Abs(bigslash.transform.eulerAngles.z - lastEularAngle) - 180f <= 15f))
            {
                bigslash.transform.eulerAngles += new Vector3(0, 0, 15f * sigh1);
            }

            lastEularAngle = bigslash.transform.eulerAngles.z;

            if (skill12LoopTime > 0.18f)
            {
                AnimFreezeTime(skill12LoopTime - 0.2f, 0f);
            }

            if (slashcount > 2)
            {
                slashcount = 0;
            }
            slashcount++;
            switch (slashcount)
            {
                case 1:
                    {
                        gameObject.LocateMyFSM("Control").SetState("SL12-1");
                        return;
                    }
                case 2:
                    {
                        gameObject.LocateMyFSM("Control").SetState("SL12-2");
                        return;
                    }
                case 3:
                    {
                        gameObject.LocateMyFSM("Control").SetState("SL12-3");
                        return;
                    }
            }


        }
        public void Skill12BigSlashSummonEnd()
        {
            var slashes = gameObject.transform.Find("Slashes").gameObject;
            for(int i = 0; i < slashes.transform.childCount; i++)
            {
                var obj = slashes.transform.GetChild(i).gameObject;
                obj.SetActive(false);
            }
            if(!skill12isBig)
            {
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                AnimFreezeTime(0.8f, 0f);
                gameObject.GetComponent<Skills>().Disappear();
            }
            CancelInvoke("Skill12BigSlashSummonLoop");
        }
        public void Skill1shortNailSummon(float time)
        {
            Skill1shortNailSummonLoop();
            Invoke("Skill1shortNailSummonEnd", time);
        }
        public void Skill1shortNailSummonLoop()
        {
            var nail = Instantiate(ItemPool.Nail, gameObject.transform);
            nail.transform.localPosition = Vector3.zero;
            nail.transform.localScale = new Vector3(nail.transform.localScale.x, nail.transform.localScale.y * gameObject.transform.localScale.y, 1f);
            nail.transform.SetParent(null);

            Vector3 direction = HeroController.instance.transform.position - gameObject.transform.position;
            //float angle = Mathf.Atan2(direction.y, direction.x) + (float)R * Mathf.PI + Mathf.PI / 2;
            float angle = -(float)R * Mathf.PI / 1.5f + Mathf.PI / 3f;
            //float angle = (float)R * 180f + -gameObject.transform.localScale.x * gameObject.transform.localEulerAngles.z;
            float x = Mathf.Sin(angle) * 22;
            float y = Mathf.Cos(angle) * 22;
            Vector3 position = new Vector3(x, y, 0) + HeroController.instance.transform.position;
            nail.AddComponent<NailSpeedControl>();
            //nail.GetComponent<NailSpeedControl>().NailChansePlayer();
            nail.GetComponent<NailSpeedControl>().NailAngleFaceToPlayer(2f);
            nail.GetComponent<NailSpeedControl>().NailClose(position);
            nail.GetComponent<NailSpeedControl>().NailFireDelay(1f, 150f);
            nail.transform.Find("Beam").gameObject.GetComponent<DeactivateAfter2dtkAnimation>().enabled = false;
            if (HardMode == 0)
            {
                Invoke("Skill1shortNailSummonLoop", 0.15f);
            }
            if (HardMode == 1)
            {
                Invoke("Skill1shortNailSummonLoop", 0.1f);
            }
        }
        public void Skill1shortNailSummonEnd()
        {
            CancelInvoke("Skill1shortNailSummonLoop");
        }

        public void Skill2shortBigSlashSummon(float time)
        {
            gameObject.GetComponent<Skills>().GetBigSlash();

            Skill2shortBigSlashSummonLoop();
            Invoke("Skill2shortBigSlashSummonEnd", time * bigSlashTimeFactor);
        }
        public void Skill2shortBigSlashSummonLoop()
        {
            HeroController.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Dir, 1f);

            if (HardMode == 0)
            {
                Invoke("Skill2shortBigSlashSummonLoop", 0.15f);
            }
            else
            {
                Invoke("Skill2shortBigSlashSummonLoop", 0.1f);
            }


            sigh1 *= -1;
            sigh2 *= -1;
            sigh3 *= -1;
            Vector3 pos = new Vector3(0, 0, 0);
            float eul = 0f;
            if (HeroController.instance.gameObject.transform.position.y <= 11f)
            {
                pos = new Vector3(sigh1 * (float)R1 * 13f * bigSlashEdgeFactor, 1 + (-4f + (float)R2 * 9f) * bigSlashEdgeFactor, 0f);
                eul = sigh3 * 90f - 60f + (float)R * 120f;
            }
            else
            {
                pos = new Vector3(sigh1 * (float)R1 * 13f * bigSlashEdgeFactor, sigh2 * (float)R2 * 9f * bigSlashEdgeFactor, 0f);
                eul = sigh3 * 90f - 60f + (float)R * 120f;
            }
            GameObject bigslash = Instantiate(ItemPool.BigSlash, HeroController.instance.gameObject.transform.position + pos, Quaternion.Euler(0, 0, eul));
            bigslash.GetComponent<BigSlashControl>().On(1f);
        }
        public void Skill2shortBigSlashSummonEnd()
        {
            CancelInvoke("Skill2shortBigSlashSummonLoop");
        }
        public void SkillPhase3Init()
        {
            gameObject.LocateMyFSM("Control").CopyState("Tele In", "TIPhase3");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Antic", "STPhase3");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Down", "SDPhase3");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Land", "SLPhase3");
            gameObject.LocateMyFSM("Control").ChangeTransition("TIPhase3", "FINISHED", "STPhase3");
            gameObject.LocateMyFSM("Control").ChangeTransition("STPhase3", "FINISHED", "SDPhase3");
            gameObject.LocateMyFSM("Control").ChangeTransition("SDPhase3", "LAND", "SLPhase3");
            gameObject.LocateMyFSM("Control").ChangeTransition("Plume Gen", "FINISHED", "Burst Pause");
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(0);
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(1);
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(3);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(1);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(2);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(5);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(6);
            gameObject.LocateMyFSM("Control").GetState("Burst Pause").GetAction<Wait>().time = 2.3f;
            gameObject.LocateMyFSM("Control").GetState("STPhase3").AddMethod(() =>
            {
                gameObject.GetComponent<Skills>().LineAppear(3);
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                gameObject.GetComponent<LockAngleWaitForAttack>().StartToLockDtab(180f, 16f);
                AnimFreezeTime(0.3f, 0.05f);
                gameObject.GetComponent<Dtab>().StompDetect();
            });
            gameObject.LocateMyFSM("Control").GetState("SDPhase3").AddMethod(() =>
            {
                gameObject.GetComponent<BossBurstControl>().DstabOn();
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                gameObject.GetComponent<Skills>().LineDisAppear();
            });
            gameObject.LocateMyFSM("Control").GetState("SLPhase3").AddMethod(() =>
            {
                BossRoarWave();

                gameObject.GetComponent<Skills>().LineAppear(5);

                FLysBurst();

                HeroController.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Land, 1f);
                ItemPool.RockPt_Thin.GetComponent<RockPtControl>().On();
                gameObject.GetComponent<Skills>().Disappear();
                gameObject.GetComponent<Dtab>().PlumeSummon9(5f, true);
                gameObject.GetComponent<SceneControl>().PlumeAutoPlaceStart(gameObject.transform.position.x);
                gameObject.GetComponent<SceneControl>().EdgeShrinkStart();
            });
            gameObject.LocateMyFSM("Control").GetState("SDPhase3").GetAction<SetVelocity2d>().y = -250f;
            gameObject.LocateMyFSM("Control").GetState("TIPhase3").AddMethod(() =>
            {
                gameObject.GetComponent<TeleportSystem>().TeleportByAngle(180, 12);
            });
        }
        public void SkillPhase4Init()
        {
            gameObject.LocateMyFSM("Control").CopyState("Tele In", "TIPhase4");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Antic", "STPhase4");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Down", "SDPhase4");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Land", "SLPhase4");
            gameObject.LocateMyFSM("Control").ChangeTransition("TIPhase4", "FINISHED", "STPhase4");
            gameObject.LocateMyFSM("Control").ChangeTransition("STPhase4", "FINISHED", "SDPhase4");
            gameObject.LocateMyFSM("Control").ChangeTransition("SDPhase4", "LAND", "SLPhase4");
            gameObject.LocateMyFSM("Control").ChangeTransition("Plume Gen", "FINISHED", "Burst Pause");
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(0);
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(1);
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(3);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(1);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(2);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(5);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(6);
            gameObject.LocateMyFSM("Control").GetState("Burst Pause").GetAction<Wait>().time = 2.3f;
            gameObject.LocateMyFSM("Control").GetState("STPhase4").AddMethod(() =>
            {
                gameObject.GetComponent<Skills>().LineAppear(3);
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                gameObject.GetComponent<LockAngleWaitForAttack>().StartToLockDtab(180f, 16f);
                AnimFreezeTime(0.8f, 0.05f);
                gameObject.GetComponent<Dtab>().StompDetect();
            });
            gameObject.LocateMyFSM("Control").GetState("SDPhase4").AddMethod(() =>
            {
                gameObject.GetComponent<BossBurstControl>().DstabOn();
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                gameObject.GetComponent<Skills>().LineDisAppear();
            });
            gameObject.LocateMyFSM("Control").GetState("SLPhase4").AddMethod(() =>
            {
                BossRoarWave();

                FLysBurst();

                HeroController.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Land, 1f);
                ItemPool.RockPt_Thin.GetComponent<RockPtControl>().On();

                WaveSummon(gameObject.transform.position.x + 1f, 8f);
                WaveSummon(gameObject.transform.position.x - 1f, -8f);

                gameObject.GetComponent<Skills>().Disappear();

                if (HardMode == 0)
                {
                    gameObject.GetComponent<SceneControl>().EdgeShrinkStart();
                    gameObject.GetComponent<SceneControl>().PlayerEnterPhase5PlaceDetectLoop();
                    gameObject.GetComponent<Phase5Arena>().Phase4InvincibleStart();
                }
                else
                {
                    //gameObject.GetComponent<SceneControl>().EdgeShrinkStartHard();
                    gameObject.GetComponent<SceneControl>().PlayerEnterPhase5PlaceDetectLoop();
                    gameObject.GetComponent<Phase5Arena>().Phase4InvincibleStart();
                }
            });
            gameObject.LocateMyFSM("Control").GetState("SDPhase4").GetAction<SetVelocity2d>().y = -250f;
            gameObject.LocateMyFSM("Control").GetState("TIPhase4").AddMethod(() =>
            {
                gameObject.GetComponent<TeleportSystem>().TeleportByAngle(180, 12);
            });
        }
        public void SkillPhase5Init()
        {
            gameObject.LocateMyFSM("Control").CopyState("Tele In", "TIPhase5");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Antic", "STPhase5");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Down", "SDPhase5");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Land", "SLPhase5");
            gameObject.LocateMyFSM("Control").ChangeTransition("TIPhase5", "FINISHED", "STPhase5");
            gameObject.LocateMyFSM("Control").ChangeTransition("STPhase5", "FINISHED", "SDPhase5");
            gameObject.LocateMyFSM("Control").ChangeTransition("SDPhase5", "LAND", "SLPhase5");
            gameObject.LocateMyFSM("Control").ChangeTransition("Plume Gen", "FINISHED", "Burst Pause");
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(0);
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(1);
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(3);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(1);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(2);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(5);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(6);
            gameObject.LocateMyFSM("Control").GetState("Burst Pause").GetAction<Wait>().time = 2.3f;
            gameObject.LocateMyFSM("Control").GetState("STPhase5").AddMethod(() =>
            {
                gameObject.GetComponent<Skills>().LineAppear(3);
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                gameObject.GetComponent<LockAngleWaitForAttack>().StartToLockDtab(180f, 23f);
                AnimFreezeTime(0.8f, 0.05f);
                gameObject.GetComponent<Dtab>().StompDetect();
            });
            gameObject.LocateMyFSM("Control").GetState("SDPhase5").AddMethod(() =>
            {
                gameObject.GetComponent<BossBurstControl>().DstabOn();
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                gameObject.GetComponent<Skills>().LineDisAppear();
            });
            gameObject.LocateMyFSM("Control").GetState("SLPhase5").AddMethod(() =>
            {
                BossRoarWave();

                FLysBurst();

                HeroController.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Land, 1f);
                ItemPool.RockPt_Thin.GetComponent<RockPtControl>().On();

                WaveSummonP5(gameObject.transform.position.x + 1f, 8f);
                WaveSummonP5(gameObject.transform.position.x - 1f, -8f);

                //gameObject.GetComponent<Skills>().Disappear();
                SkillTurnPhase();
            });
            gameObject.LocateMyFSM("Control").GetState("SDPhase5").GetAction<SetVelocity2d>().y = -250f;
            gameObject.LocateMyFSM("Control").GetState("TIPhase5").AddMethod(() =>
            {
                gameObject.GetComponent<TeleportSystem>().TeleportByAngle(180, 23);
            });
        }

        public void WaveSummonP5(float x, float speed)
        {
            var wave = Instantiate(ItemPool.Wave, new Vector3(x, 6f, 6f), Quaternion.Euler(0, 0, 0));
            wave.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 2f, 0f);
            wave.transform.localScale = new Vector3(wave.transform.localScale.x * Math.Sign(speed) * 1.5f, wave.transform.localScale.y * 3f, wave.transform.localScale.z);
            var core = wave.transform.Find("slash_core").gameObject;
            core.transform.Find("hurtbox").gameObject.AddComponent<WaveControlP5>();
            core.transform.Find("hurtbox").gameObject.GetComponent<WaveControlP5>().speed = speed;
        }

        public void WaveSummon(float x, float speed)
        {
            var wave = Instantiate(ItemPool.Wave, new Vector3(x, 6f, 6f), Quaternion.Euler(0, 0, 0));
            wave.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 2f, 0f);
            wave.transform.localScale = new Vector3(wave.transform.localScale.x * Math.Sign(speed) * 1.5f, wave.transform.localScale.y * 3f, wave.transform.localScale.z);
            var core = wave.transform.Find("slash_core").gameObject;
            core.transform.Find("hurtbox").gameObject.AddComponent<WaveControl>();
            core.transform.Find("hurtbox").gameObject.GetComponent<WaveControl>().speed = speed;
        }
        
        public void WaveLittleSummon(float x, float speed)
        {
            var wave = Instantiate(ItemPool.Wave, new Vector3(x, 3f, 6f), Quaternion.Euler(0, 0, 0));
            wave.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 2f, 0f);
            wave.transform.localScale = new Vector3(wave.transform.localScale.x * Math.Sign(speed), wave.transform.localScale.y * 2f, wave.transform.localScale.z);
            var core = wave.transform.Find("slash_core").gameObject;
            core.transform.Find("hurtbox").gameObject.AddComponent<WaveLittleControl>();
            core.transform.Find("hurtbox").gameObject.GetComponent<WaveLittleControl>().speed = speed;
        }

        public class WaveControlP5 : MonoBehaviour
        {
            public float speed = 0f;
            public int shrikeTime = 0;
            public Vector3 orig_Scale = Vector3.one;
            void Start()
            {
                gameObject.layer = LayerMask.NameToLayer("Attack");
                orig_Scale = gameObject.transform.parent.transform.parent.transform.localScale;
                SpeedDownLoop();
                Invoke("SummonOne", 2f);
            }
            void OnTriggerEnter2D(Collider2D collision)
            {
                if (collision.gameObject.name.Contains("Plume"))
                {
                    collision.GetComponent<PlumeFall>().On();
                }
            }
            public void SpeedDownLoop()
            {
                gameObject.transform.parent.transform.parent.GetComponent<Rigidbody2D>().velocity += (new Vector2(speed, 0) - gameObject.transform.parent.transform.parent.GetComponent<Rigidbody2D>().velocity) * 0.1f;

                Invoke("SpeedDownLoop", 0.02f);
            }
            public void SummonOne()
            {
                var wave = Instantiate(ItemPool.Wave, gameObject.transform.parent.transform.parent.transform.position, Quaternion.Euler(0, 0, 0));
                wave.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 2, 0f);
                wave.transform.localScale = new Vector3(wave.transform.localScale.x * Math.Sign(speed) * 1.5f, wave.transform.localScale.y * 3f, wave.transform.localScale.z);
                //wave.transform.position = gameObject.transform.parent.transform.position;
                var core = wave.transform.Find("slash_core").gameObject;
                core.transform.Find("hurtbox").gameObject.AddComponent<WaveControl>();
                core.transform.Find("hurtbox").gameObject.GetComponent<WaveControl>().speed = speed;
            }
            public void ShrikeLoop()
            {
                gameObject.transform.parent.transform.parent.transform.localScale = new Vector3(orig_Scale.x * (1 - shrikeTime * 0.05f), orig_Scale.y * (1 - shrikeTime * 0.02f), orig_Scale.z);

                shrikeTime++;
                if (shrikeTime <= 20)
                {
                    Invoke("ShrikeLoop", 0.02f);
                }
                else
                {
                    gameObject.transform.parent.transform.parent.transform.position += new Vector3(0, -500, 0);
                }
            }
        }
        public class WaveControl:MonoBehaviour
        {
            public float speed = 0f;
            public int shrikeTime = 0;
            public Vector3 orig_Scale = Vector3.one;
            void Start()
            {
                orig_Scale = gameObject.transform.parent.transform.parent.transform.localScale;

                gameObject.layer = LayerMask.NameToLayer("Attack");
                if (!BOSS.GetComponent<SceneControl>().Phase5Started)
                {
                    Invoke("SummonOne", 2f);
                }
                SpeedDownLoop();
            }
            void OnTriggerEnter2D(Collider2D collision)
            {
                if (collision.gameObject.name.Contains("Plume"))
                {
                    collision.GetComponent<PlumeFall>().On();
                }
            }
            public void SpeedDownLoop()
            {
                if(BOSS.GetComponent<SceneControl>().Phase5Started)
                {
                    ShrikeLoop();
                }
                else
                {
                    Invoke("SpeedDownLoop", 0.02f);
                }
                gameObject.transform.parent.transform.parent.GetComponent<Rigidbody2D>().velocity += (new Vector2(speed, 0) - gameObject.transform.parent.transform.parent.GetComponent<Rigidbody2D>().velocity) * 0.1f;
                
            }
            public void SummonOne()
            {
                Vector3 wavePos = gameObject.transform.parent.transform.parent.transform.position;

                if (wavePos.x < HeroController.instance.transform.position.x - 25f)
                {
                    wavePos = new Vector3(HeroController.instance.transform.position.x - 25, wavePos.y, wavePos.z);
                }

                var wave = Instantiate(ItemPool.Wave, wavePos, Quaternion.Euler(0, 0, 0));
                wave.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 2, 0f);
                wave.transform.localScale = new Vector3(wave.transform.localScale.x * Math.Sign(speed) * 1.5f, wave.transform.localScale.y * 3f, wave.transform.localScale.z);
                //wave.transform.position = gameObject.transform.parent.transform.position;
                var core = wave.transform.Find("slash_core").gameObject;
                core.transform.Find("hurtbox").gameObject.AddComponent<WaveControl>();
                core.transform.Find("hurtbox").gameObject.GetComponent<WaveControl>().speed = speed;
            }
            public void ShrikeLoop()
            {
                gameObject.transform.parent.transform.parent.transform.localScale = new Vector3(orig_Scale.x * (1 - shrikeTime * 0.05f), orig_Scale.y * (1 - shrikeTime * 0.02f), orig_Scale.z);

                shrikeTime++;
                if (shrikeTime <= 20)
                {
                    Invoke("ShrikeLoop", 0.02f);
                }
                else
                {
                    gameObject.transform.parent.transform.parent.transform.position += new Vector3(0, -500, 0);
                }
            }
        }

        public class WaveLittleControl:MonoBehaviour
        {
            public float speed = 0f;
            public int shrikeTime = 0;
            public Vector3 orig_Scale = Vector3.one;
            void Start()
            {
                gameObject.layer = LayerMask.NameToLayer("Attack");
                orig_Scale = gameObject.transform.parent.transform.parent.transform.localScale;
                Invoke("ShrikeLoop", 0.5f);
                SpeedDownLoop();
            }
            void OnTriggerEnter2D(Collider2D collision)
            {
                if (collision.gameObject.name.Contains("Plume"))
                {
                    collision.GetComponent<PlumeFall>().On();
                }
            }
            public void SpeedDownLoop()
            {
                gameObject.transform.parent.transform.parent.GetComponent<Rigidbody2D>().velocity -= gameObject.transform.parent.transform.parent.GetComponent<Rigidbody2D>().velocity * 0.1f;
                Invoke("SpeedDownLoop", 0.02f);
            }
            public void ShrikeLoop()
            {
                gameObject.transform.parent.transform.parent.transform.localScale = new Vector3(orig_Scale.x * (1 - shrikeTime * 0.05f), orig_Scale.y * (1 - shrikeTime * 0.02f), orig_Scale.z);

                shrikeTime++;
                if(shrikeTime <= 20)
                {
                    Invoke("ShrikeLoop", 0.02f);
                }
                else
                {
                    gameObject.transform.parent.transform.parent.transform.position += new Vector3(0, -500, 0);
                }
            }
        }

        private static int[] Blasts = { 1, 2, 3, 4, 5, 6, 7, 8 };
        int[] BlastsArray1 = Blasts;
        int num1 = 10;
        int sigh4 = 1;
        int nextHp = 2000;
        Vector3 EnergyLocalPos = Vector3.zero;
        public void Skill1TurnPhaseInit()
        {
            var scene = GameObject.Find("Battle Scene").gameObject;
            gameObject.LocateMyFSM("Control").CopyState("Focus Charge", "FC_S");
            gameObject.LocateMyFSM("Control").CopyState("Ball Up", "BU_S");
            gameObject.LocateMyFSM("Control").CopyState("Focus Burst", "FB_S");
            gameObject.LocateMyFSM("Control").CopyState("Hit End", "HE_S");
            gameObject.LocateMyFSM("Control").CopyState("Focus Recover", "FR_S");
            gameObject.LocateMyFSM("Control").ChangeTransition("After Tele", "FOCUS", "FC_S");
            gameObject.LocateMyFSM("Control").ChangeTransition("Focus L", "FINISHED", "FC_S");
            gameObject.LocateMyFSM("Control").ChangeTransition("Focus R", "FINISHED", "FC_S");
            gameObject.LocateMyFSM("Control").ChangeTransition("FC_S", "FINISHED", "BU_S");
            gameObject.LocateMyFSM("Control").ChangeTransition("BU_S", "FINISHED", "FB_S");
            gameObject.LocateMyFSM("Control").ChangeTransition("FB_S", "FINISHED", "HE_S");
            gameObject.LocateMyFSM("Control").ChangeTransition("HE_S", "FINISHED", "FR_S");
            gameObject.LocateMyFSM("Control").GetState("FC_S").GetAction<Wait>().time = 1.5f;
            gameObject.LocateMyFSM("Control").GetState("Focus Wait").GetAction<Wait>().time = 1f;
            gameObject.LocateMyFSM("Control").GetState("Focus Burst").RemoveAction(0);
            gameObject.LocateMyFSM("Control").GetState("Focus Recover").AddMethod(() =>
            {
            });
            gameObject.LocateMyFSM("Control").GetState("FC_S").AddMethod(() =>
            {
                ItemPool.Energy.transform.localScale = new Vector3(-2.5f - gameObject.GetComponent<PhaseControl>().phase * 0.3f, 2.5f + gameObject.GetComponent<PhaseControl>().phase * 0.3f, 1f);
                ItemPool.Energy.SetActive(true);

                gameObject.GetComponent<AngleSystem>().BossAngleRecover();

                if(gameObject.GetComponent<PhaseControl>().phase < 5)
                {
                    if (R > 0.5)
                    {
                        gameObject.GetComponent<TeleportSystem>().TeleportByFloor(8f);
                    }
                    else
                    {
                        gameObject.GetComponent<TeleportSystem>().TeleportByFloor(-8f);
                    }
                }
                else
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x + Math.Sign(gameObject.transform.localScale.x), 8.7f, 0f);
                    if (gameObject.transform.position.x - HeroController.instance.transform.position.x > 0)
                    {
                        gameObject.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else
                    {
                        gameObject.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
                StartCoroutine(ExecuteWithDelay());
                IEnumerator ExecuteWithDelay()
                {
                    for (int loopTime = 1; loopTime <= 10; loopTime++)
                    {
                        angle += 180f + (float)R * 180f;
                        float distance = (float)R * 7f + 21f;
                        Vector3 position = new Vector3(Mathf.Sin((float)DegreesToRadians(angle)) * distance, Mathf.Cos((float)DegreesToRadians(angle)) * distance, gameObject.transform.position.z + 0.001f);
                        var blast = Instantiate(Blast1, gameObject.transform.position + position, Quaternion.Euler(0, 0, 0));
                        blast.GetComponent<LittleBlastControl>().On(true, 0.04f, 1f, gameObject.transform.position);
                        blast.AddComponent<ObjDelayRecycle>();
                        blast.GetComponent<ObjDelayRecycle>().DelayRecycle(3f);
                        yield return new WaitForSeconds(0.15f);
                    }
                }
            });
            gameObject.LocateMyFSM("Control").GetState("BU_S").AddMethod(() =>
            {
                ItemPool.RockPt_Wide.GetComponent<RockPtControl>().On();
                ItemPool.FocusBall.transform.position = gameObject.transform.position + new Vector3(0f, 0f, -0.01f);
                ItemPool.DashBurst.transform.localScale = new Vector3(-11, 6, 3);
                ItemPool.FocusBall.SetActive(true);
            });
            gameObject.LocateMyFSM("Control").GetState("FB_S").AddMethod(() =>
            {

                BossRoarWave();

                if (HardMode == 0)
                {
                    hardmodeEnd = true;
                    ItemPool.FocusBall.transform.SetParent(null);
                    EnergyCloseDeley();

                    Invoke("DelayBlast", 0.3f);
                    Skill12(0.5f, 1.2f, true, 0.05f);
                }
                else if(HardMode == 1)
                {
                    //slashCount = 3 - gameObject.GetComponent<PhaseControl>().phase;

                    lock3Slashes = true;
                    gameObject.GetComponent<AngleSystem>().FaceToPlayerOnce();

                    gameObject.GetComponent<BossBurstControl>().BigOn();

                    Invoke("BurstAngleSet", 0.03f);

                    angle = gameObject.transform.eulerAngles.z + 90f;
                    angle *= -1;
                    angle += 180f;

                    ItemPool.Energy.transform.localScale *= 3;
                    lock3Slashes = true;
                    ItemPool.FocusBall.transform.SetParent(null);
                    Invoke("DelayBlast", 0.05f + gameObject.GetComponent<PhaseControl>().phase * 0.15f);
                    slashCount = 5 - gameObject.GetComponent<PhaseControl>().phase;

                    EnergyClose();

                    var glow = Instantiate(ItemPool.Glow, gameObject.transform);
                    glow.transform.SetParent(null);
                    glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 5;
                    glow.SetActive(true);
                    glow.AddComponent<DelayDestory>();
                    glow.GetComponent<DelayDestory>().On(2f);

                    gameObject.LocateMyFSM("Control").SetState("CS2");
                }
            });
        }
        public void BurstAngleSet()
        {
            ItemPool.DashBurst.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z);
        }
        public void EnergyCloseDeley()
        {
            Invoke("EnergyClose", 0.1f);
        }
        public void EnergyClose()
        {
            ItemPool.Energy.SetActive(false);
        }
        public void DelayBlast()
        {
            StartCoroutine(ExecuteWithDelay());
            IEnumerator ExecuteWithDelay()
            {
                for (int loopTime = 1; loopTime <= 8; loopTime++)
                {
                    num1++;
                    if (num1 >= 8)
                    {
                        BlastsArray1 = Blasts.OrderBy(x => Guid.NewGuid()).ToArray();
                        num1 = 0;
                    }

                    float y = 0;
                    if(gameObject.GetComponent<PhaseControl>().phase == 4)
                    {
                        y = 5;
                    }

                    if(BlastsArray1[num1] % 2 == 1)
                    {
                        sigh4 = 1;
                    }
                    else
                    {
                        sigh4 = -1;
                    }

                    Vector3 position = new Vector3(HeroController.instance.transform.position.x - 24f + 6 * BlastsArray1[num1] - 4 + (float)R1 * 8, y + 13f + sigh4 * (1f + (float)R2 * 6f), gameObject.transform.position.z + 0.001f);
                    var blast = Instantiate(Blast1, position, Quaternion.Euler(0, 0, 0));
                    blast.GetComponent<LittleBlastControl>().On(false, chanseSpeedFactor, speedReduceFactor, position);
                    blast.AddComponent<ObjDelayRecycle>();
                    blast.GetComponent<ObjDelayRecycle>().DelayRecycle(3f);
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        public void Skill1Init()
        {
            float angle1 = angle;
            gameObject.LocateMyFSM("Control").CopyState("Slash1 Antic", "SA1-1");
            gameObject.LocateMyFSM("Control").CopyState("CSlash", "CS1");
            gameObject.LocateMyFSM("Control").CopyState("CSlash Recover", "CSR1");
            gameObject.LocateMyFSM("Control").CopyState("Recover", "R1");
            gameObject.LocateMyFSM("Control").GetState("CSR1").RemoveAction<SetVelocity2d>();
            gameObject.LocateMyFSM("Control").GetState("CS1").RemoveAction<SetVelocity2d>();
            gameObject.LocateMyFSM("Control").ChangeTransition("SA1", "FINISHED", "CS1");
            gameObject.LocateMyFSM("Control").ChangeTransition("SA1-1", "FINISHED", "CS1");
            gameObject.LocateMyFSM("Control").ChangeTransition("CS1", "FINISHED", "CSR1");
            gameObject.LocateMyFSM("Control").ChangeTransition("CSR1", "FINISHED", "R1");
            gameObject.LocateMyFSM("Control").ChangeTransition("R1", "FINISHED", "SA1");

            gameObject.LocateMyFSM("Control").GetState("SA1").AddMethod(() =>
            {
                slashCountMax = 3;
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                if (slashCount == 0)
                {
                    angle = (float)R * 360f;
                }
                gameObject.GetComponent<TeleportSystem>().TeleportByAngle(angle, 16);
                angle1 = angle;
                if(gameObject.GetComponent<PhaseControl>().phase > 1 && HardMode != 0)
                {
                    if (R <= 0.3)
                    {
                        SkillShort1("SA1-1");
                        return;
                    }
                    else if (R <= 0.6)
                    {
                        SkillShort3("SA1-1");
                        return;
                    }
                }
                if (HardMode == 0 && gameObject.GetComponent<PhaseControl>().phase == 5)
                {
                    gameObject.GetComponent<SkillsControl>().SkillShort2("SA1-1", 0.1f, 1f);
                }
                else
                {
                    gameObject.GetComponent<LockAngleWaitForAttack>().StartToLock(angle1, 20);
                    gameObject.GetComponent<Skills>().LineAppear(1);
                }

            });
            gameObject.LocateMyFSM("Control").GetState("SA1-1").AddMethod(() =>
            {
                slashCountMax = 3;
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                gameObject.GetComponent<TeleportSystem>().TeleportByAngle(angle1, 16);
                gameObject.GetComponent<LockAngleWaitForAttack>().StartToLock(angle1, 20);
                gameObject.GetComponent<Skills>().LineAppear(1);
            });
            gameObject.LocateMyFSM("Control").GetState("CS1").AddMethod(() =>
            {
                gameObject.GetComponent<BossBurstControl>().SmallOn();
                gameObject.GetComponent<SlashColliderControl>().SlashOn();
                gameObject.GetComponent<OneSlashLight>().DelayLight(0f);
                gameObject.GetComponent<OneSlashAnyAngle>().On(angle1, 350f);
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                gameObject.GetComponent<Skills>().LineDisAppear();

                if (HardMode == 0)
                {
                    AnimFreezeTime(0.16f, 0.02f);
                }
                else
                {
                    AnimFreezeTime(0.08f, 0.02f);
                }
            });
            gameObject.LocateMyFSM("Control").GetState("CSR1").AddMethod(() =>
            {
                gameObject.GetComponent<SlashColliderControl>().SlashOff();
            });
            gameObject.LocateMyFSM("Control").GetState("R1").AddMethod(() =>
            {
                angle *= -1f;
                angle += (float)R * 120f - 60f + 180f;
                slashCount++;
                if (slashCount >= slashCountMax || R >= 0.8)
                {
                    gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                    gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                    slashCount = 0;
                    gameObject.LocateMyFSM("Control").SetState("Recover");
                }
                Invoke("VelocitySetZero", 0.3f);
            });
            gameObject.LocateMyFSM("Control").GetState("Recover").AddMethod(() =>
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            });
        }
        bool lock3Slashes = false;
        bool lock5Slashes = false;
        public void Skill2Init()
        {
            //gameObject.LocateMyFSM("Control").CopyState("Slash1 Antic", "SA2");
            gameObject.LocateMyFSM("Control").CopyState("CSlash", "CS2");
            gameObject.LocateMyFSM("Control").CopyState("CSlash Recover", "CSR2");
            gameObject.LocateMyFSM("Control").CopyState("Recover", "R2");
            gameObject.LocateMyFSM("Control").GetState("CSR2").RemoveAction<SetVelocity2d>();
            gameObject.LocateMyFSM("Control").GetState("CS2").RemoveAction<SetVelocity2d>();
            gameObject.LocateMyFSM("Control").ChangeTransition("SA2", "FINISHED", "CS2");
            gameObject.LocateMyFSM("Control").ChangeTransition("CS2", "FINISHED", "CSR2");
            gameObject.LocateMyFSM("Control").ChangeTransition("CSR2", "FINISHED", "CS2");
            gameObject.LocateMyFSM("Control").ChangeTransition("R2", "FINISHED", "CS2");

            gameObject.LocateMyFSM("Control").GetState("SA2").AddMethod(() =>
            {
                slashCount = 0;
                if (gameObject.GetComponent<PhaseControl>().phase <= 4 && R <= 0.7f)
                {
                    slashCountMax = 3;
                }
                else
                {
                    slashCountMax = 5;
                }
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                slashCount++;
                if (slashCount >= slashCountMax)
                {
                    gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                    gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                    gameObject.LocateMyFSM("Control").SetState("Recover");
                    return;
                }
                angle = (float)R * 360f;


                if (gameObject.name == "MoonForPlayer")
                {
                    slashCountMax = 5;
                    SetDamageEnemy(gameObject.GetComponent<Skills>().MainSlash, 4 * PlayerData.instance.GetInt("nailDamage"), angle + 90f, 2f);
                    //gameObject.GetComponent<Skills>().Turn();
                    gameObject.GetComponent<LockAngleWaitForAttack>().StartToLockForPlayer(angle, 16f);
                    gameObject.GetComponent<Skills>().LineAppear(1);
                }
                else
                {
                    //gameObject.GetComponent<Skills>().Turn();
                    gameObject.GetComponent<LockAngleWaitForAttack>().StartToLock(angle, 20f);
                    gameObject.GetComponent<TeleportSystem>().TeleportByAngle(angle, 16f);
                    gameObject.GetComponent<Skills>().LineAppear(2);
                }


            });
            gameObject.LocateMyFSM("Control").GetState("CS2").AddMethod(() =>
            {
                gameObject.GetComponent<SlashColliderControl>().SlashOn();
                gameObject.GetComponent<OneSlashLight>().DelayLight(0f);
                gameObject.GetComponent<OneSlashAnyAngle>().On(angle, 350f);
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                gameObject.GetComponent<Skills>().LineDisAppear();
                AnimFreezeTime(0.08f, 0.02f);
            });
            gameObject.LocateMyFSM("Control").GetState("CSR2").AddMethod(() =>
            {
                if (lock3Slashes)
                {
                    lock3Slashes = false;
                    slashCountMax = 3;
                }
                else if (lock5Slashes)
                {
                    lock5Slashes = false;
                    slashCountMax = 5;
                }


                if (ItemPool.FocusHit != null)
                {
                    ItemPool.FocusHit.SetActive(false);
                }
                gameObject.GetComponent<SlashColliderControl>().SlashOff();
                angle += (float)R * 120f - 60f + 180f;
                slashCount++;
                if (slashCount > slashCountMax)
                {
                    slashCount = 0;
                    gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                    gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();

                    if (HardMode == 1 && slashCountMax == 3)
                    {
                        Skill12(0.6f, 1.2f, true, 0.05f);
                        //SkillShort2("Focus Wait", 0.8f, 0.15f);
                        return;
                    }
                    else if (HardMode == 1 && gameObject.GetComponent<PhaseControl>().phase == 5)
                    {
                        SkillShort2("Focus Wait", 0.8f, 0.15f);

                        return;
                    }
                    gameObject.LocateMyFSM("Control").SetState("Recover");
                    return;
                }
                if (gameObject.name == "MoonForPlayer")
                {
                    Skills.SetDamageEnemy(gameObject.GetComponent<Skills>().MainSlash, 4 * PlayerData.instance.nailDamage, angle + 90f, 2f);
                    gameObject.GetComponent<LockAngleWaitForAttack>().LockOnceForPlayer(angle, 18f);
                    gameObject.GetComponent<Skills>().LineAppear(0);
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                }
                else
                {
                    gameObject.GetComponent<LockAngleWaitForAttack>().LockOnce(angle, 18f);
                    gameObject.GetComponent<Skills>().LineAppear(0);
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                }
            });
            gameObject.LocateMyFSM("Control").GetState("Recover").AddMethod(() =>
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            });
        }
        public bool lock2Slashes = false;
        public void Skill3Init()
        {
            gameObject.LocateMyFSM("Control").CopyState("Slash1 Antic", "SA31");
            gameObject.LocateMyFSM("Control").CopyState("Slash1 Antic", "SA31-1");
            gameObject.LocateMyFSM("Control").CopyState("Slash1 Antic", "SA3-1");
            gameObject.LocateMyFSM("Control").CopyState("CSlash", "CS31");
            gameObject.LocateMyFSM("Control").CopyState("CSlash Recover", "CSR31");
            gameObject.LocateMyFSM("Control").CopyState("Recover", "R31");
            gameObject.LocateMyFSM("Control").GetState("CSR31").RemoveAction<SetVelocity2d>();
            gameObject.LocateMyFSM("Control").GetState("CS31").RemoveAction<SetVelocity2d>();
            gameObject.LocateMyFSM("Control").ChangeTransition("SA31", "FINISHED", "CS31");
            gameObject.LocateMyFSM("Control").ChangeTransition("SA31-1", "FINISHED", "CS31");
            gameObject.LocateMyFSM("Control").ChangeTransition("CS31", "FINISHED", "CSR31");
            gameObject.LocateMyFSM("Control").ChangeTransition("CSR31", "FINISHED", "R31");

            gameObject.LocateMyFSM("Control").GetState("SA31").AddMethod(() =>
            {
                if (R <= 0.5 && HardMode == 1)
                {
                    Skill12(1f, 0.25f, true, 0.2f);
                    return;
                }
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                angle = -gameObject.transform.localEulerAngles.z + 90f;
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                gameObject.GetComponent<LockAngleWaitForAttack>().StartToLock(angle, 18);
                gameObject.GetComponent<Skills>().LineAppear(1);
            });
            gameObject.LocateMyFSM("Control").GetState("SA31-1").AddMethod(() =>
            {
                gameObject.GetComponent<AngleSystem>().FaceToPlayerEnd();
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                angle = -gameObject.transform.localEulerAngles.z + 90f;
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                gameObject.GetComponent<LockAngleWaitForAttack>().StartToLock(angle, 22);
                gameObject.GetComponent<Skills>().LineAppear(1);
            });
            gameObject.LocateMyFSM("Control").GetState("CS31").AddMethod(() =>
            {
                gameObject.GetComponent<SlashColliderControl>().SlashOn();
                gameObject.GetComponent<BossBurstControl>().SmallOn();
                gameObject.GetComponent<OneSlashLight>().DelayLight(0f);
                gameObject.GetComponent<OneSlashAnyAngle>().On(angle, 350f);
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                gameObject.GetComponent<Skills>().LineDisAppear();
            });
            gameObject.LocateMyFSM("Control").GetState("CSR31").AddMethod(() =>
            {
                gameObject.GetComponent<SlashColliderControl>().SlashOff();
            });

            gameObject.LocateMyFSM("Control").ChangeTransition("SA3", "FINISHED", "Slash1");
            gameObject.LocateMyFSM("Control").ChangeTransition("SA3-1", "FINISHED", "Slash1");
            gameObject.LocateMyFSM("Control").ChangeTransition("Slash 3", "FINISHED", "SA31");
            //gameObject.LocateMyFSM("Control").ChangeTransition("Idle Stance", "FINISHED", "Slash1 Antic");//test

            gameObject.LocateMyFSM("Control").GetState("SA3").AddMethod(()=>
            {
                gameObject.GetComponent<TeleportSystem>().TeleportByAngle((float)R * 160f + 90f, 16f);
                gameObject.GetComponent<Skills>().LineAppear(4);
                gameObject.GetComponent<AngleSystem>().FaceToPlayerStart();
            });
            gameObject.LocateMyFSM("Control").GetState("SA3-1").AddMethod(()=>
            {
                gameObject.GetComponent<Skills>().LineAppear(4);
                //gameObject.GetComponent<AngleSystem>().FaceToPlayerStart();
            });
            gameObject.LocateMyFSM("Control").GetState("Slash1").AddMethod(()=>
            {
                gameObject.GetComponent<SlashBeam>().OnByBossAngle(135f);
                gameObject.GetComponent<Skills>().LineDisAppear();
            });

            gameObject.LocateMyFSM("Control").GetState("Slash 2").AddMethod(()=>
            {
                if (HardMode == 0)
                {
                    gameObject.LocateMyFSM("Control").SetState("Recover");
                }
                else
                {
                    gameObject.GetComponent<SlashBeam>().OnByBossAngle(135f);
                    //gameObject.GetComponent<AngleSystem>().FaceToPlayerEnd();
                }
            });
            gameObject.LocateMyFSM("Control").GetState("Slash 3").AddMethod(()=>
            {
                if(gameObject.GetComponent<PhaseControl>().phase < 2 || lock2Slashes)
                {
                    lock2Slashes = false;
                    gameObject.GetComponent<AngleSystem>().FaceToPlayerEnd();
                    gameObject.LocateMyFSM("Control").SetState("Recover");
                }  
                else
                {
                    gameObject.GetComponent<SlashBeam>().OnByBossAngle(135f);
                    gameObject.GetComponent<AngleSystem>().FaceToPlayerEnd();
                    slashCount = 2;
                }
            });
            gameObject.LocateMyFSM("Control").GetState("Recover").AddMethod(() =>
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                gameObject.GetComponent<AngleSystem>().FaceToPlayerEnd();
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
            });
        }
        public void Skill4Init()
        {
            gameObject.LocateMyFSM("Control").CopyState("SmallShot Antic", "SA4");
            gameObject.LocateMyFSM("Control").GetState("SA4").AddMethod(() =>
            {
                gameObject.GetComponent<TeleportSystem>().TeleportByAngle(180f + 50f * (float)R, 12f);
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();

                Vector3 direction = HeroController.instance.transform.position - gameObject.transform.position;
                gameObject.GetComponent<AngleSystem>().FaceToPlayerStart();
                gameObject.GetComponent<AngleSystem>().FaceToPlayerEnd();
            });
            float time = 0.5f + (float)R * 0.7f;
            gameObject.LocateMyFSM("Control").CopyState("SmallShot LowHigh", "SL4");
            gameObject.LocateMyFSM("Control").GetState("SL4").GetAction<Wait>().time = time + 0.1f;
            gameObject.LocateMyFSM("Control").GetState("SL4").RemoveAction<FlingObjectsFromGlobalPoolTime>();

            gameObject.LocateMyFSM("Control").GetState("SL4").AddMethod(() =>
            {
                gameObject.transform.localEulerAngles = new Vector3(0, 0, (float)RadiansToDegrees(angle));
                Skill4NailSummon(time);
            });
        }
        public void Skill5Init()
        {
            gameObject.LocateMyFSM("Control").CopyState("Tele In", "TI5");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Antic", "ST5");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Down", "SD5");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Land", "SL5");
            gameObject.LocateMyFSM("Control").ChangeTransition("TI5", "FINISHED", "ST5");
            gameObject.LocateMyFSM("Control").ChangeTransition("ST5", "FINISHED", "SD5");
            gameObject.LocateMyFSM("Control").ChangeTransition("SD5", "LAND", "SL5");
            gameObject.LocateMyFSM("Control").ChangeTransition("SL5", "FINISHED", "Focus Wait");
            gameObject.LocateMyFSM("Control").ChangeTransition("Plume Gen", "FINISHED", "Burst Pause");
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(0);
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(1);
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(3);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(1);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(2);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(5);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(6); 
            gameObject.LocateMyFSM("Control").GetState("SL5").RemoveAction(1); 
            gameObject.LocateMyFSM("Control").GetState("Burst Pause").GetAction<Wait>().time = 0.3f;
            gameObject.LocateMyFSM("Control").GetState("ST5").AddMethod(() =>
            {
                var colliders = gameObject.transform.Find("Colliders").gameObject;
                for (int i = 0; i < colliders.transform.childCount; i++)
                {
                    var obj = colliders.transform.GetChild(i).gameObject;
                    if (obj.name == "Dstab Damage")
                    {
                        obj.GetComponent<DamageHero>().damageDealt = 2;
                    }
                }
                gameObject.GetComponent<Skills>().LineAppear(3);
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                if (gameObject.GetComponent<PhaseControl>().phase == 3)
                {
                    gameObject.GetComponent<LockAngleWaitForAttack>().StartToLockDtabPhase3();
                }
                else if(gameObject.GetComponent<PhaseControl>().phase == 4)
                {
                    gameObject.GetComponent<LockAngleWaitForAttack>().StartToLockDtabPhase4();
                }
                else
                {
                    gameObject.GetComponent<LockAngleWaitForAttack>().StartToLockDtab(180f, 16f);
                }
                AnimFreezeTime(0.3f, 0.05f);
                gameObject.GetComponent<Dtab>().StompDetect();
            });
            gameObject.LocateMyFSM("Control").GetState("SD5").AddMethod(() =>
            {
                if (gameObject.GetComponent<PhaseControl>().phase >= 3)
                {
                    gameObject.GetComponent<SceneControl>().Phase3PlumeDetroy();
                }
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                gameObject.GetComponent<Skills>().LineDisAppear();

                gameObject.GetComponent<BossBurstControl>().DstabOn();

                gameObject.GetComponent<DamageHero>().damageDealt = 2;

                WaveLittleSummon(gameObject.transform.position.x + 1f, 8f);
                WaveLittleSummon(gameObject.transform.position.x - 1f, -8f);
            });
            gameObject.LocateMyFSM("Control").GetState("SL5").AddMethod(() =>
            {
                gameObject.GetComponent<DamageHero>().damageDealt = 0;

                FLysBurst();
                HeroController.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Land, 1f);
                ItemPool.RockPt_Thin.GetComponent<RockPtControl>().On();
                gameObject.GetComponent<Skills>().Disappear();

                if (gameObject.GetComponent<PhaseControl>().phase < 3)
                {
                    //gameObject.GetComponent<Dtab>().PlumeSummon(1.5f);
                }
            });
            gameObject.LocateMyFSM("Control").GetState("SD5").GetAction<SetVelocity2d>().y = -250f;
            gameObject.LocateMyFSM("Control").GetState("TI5").AddMethod(() =>
            {
                gameObject.GetComponent<TeleportSystem>().TeleportByAngle(180, 12);
            });
        }
        public void FLysBurst()
        {
            if (ItemPool.Boss_Bugs_Pt2 != null)
            {
                var pt = Instantiate(ItemPool.Boss_Bugs_Pt2, ItemPool.Boss_Bugs_Pt2.transform.position, Quaternion.Euler(0, 0, ItemPool.Boss_Bugs_Pt2.transform.eulerAngles.z));
                //ItemPool.Boss_Bugs_Pt2.GetComponent<ParticleSystem>().Play();
                pt.AddComponent<DelayDestory>();
                pt.GetComponent<DelayDestory>().On(5f);
            }
        }
        public GameObject Blast1;
        public GameObject Blast2;
        public GameObject Blast3;
        public GameObject Blast4;
        public GameObject Blast5;
        public float chanseSpeedFactor = 0.04f;
        public float speedReduceFactor = 1.025f;
        public float time1 = 0.04f;
        /*
        public void Skill6Init()
        {
            var scene = GameObject.Find("Battle Scene").gameObject;
            var blast = scene.transform.Find("Focus Blasts").gameObject;
            Blast1 = blast.transform.Find("HK Prime Blast (1)").gameObject;
            Blast2 = blast.transform.Find("HK Prime Blast (2)").gameObject;
            Blast3 = blast.transform.Find("HK Prime Blast (3)").gameObject;
            Blast4 = blast.transform.Find("HK Prime Blast (4)").gameObject;
            Blast5 = blast.transform.Find("HK Prime Blast (5)").gameObject;
            Blast1.AddComponent<LittleBlastControl>();
            Blast2.AddComponent<LittleBlastControl>();
            Blast3.AddComponent<LittleBlastControl>();
            Blast4.AddComponent<LittleBlastControl>();
            Blast5.AddComponent<LittleBlastControl>();
            gameObject.LocateMyFSM("Control").CopyState("Focus Charge", "FC6");
            gameObject.LocateMyFSM("Control").ChangeTransition("After Tele", "FOCUS", "FC6");
            gameObject.LocateMyFSM("Control").ChangeTransition("Focus L", "FINISHED", "FC6");
            gameObject.LocateMyFSM("Control").ChangeTransition("Focus R", "FINISHED", "FC6");
            gameObject.LocateMyFSM("Control").GetState("FC6").GetAction<Wait>().time = 2f;
            gameObject.LocateMyFSM("Control").GetState("Focus Wait").GetAction<Wait>().time = 1f;
            gameObject.LocateMyFSM("Control").GetState("Focus Burst").RemoveAction(0);
            gameObject.LocateMyFSM("Control").GetState("Focus Recover").AddMethod(() =>
            {
            });
            gameObject.LocateMyFSM("Control").GetState("FC6").AddMethod(() =>
            {
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                if (R > 0.5)
                {
                    gameObject.GetComponent<TeleportSystem>().TeleportByFloor(8f);
                }
                else
                {
                    gameObject.GetComponent<TeleportSystem>().TeleportByFloor(-8f);
                }
                StartCoroutine(ExecuteWithDelay());
                IEnumerator ExecuteWithDelay()
                {
                    for(int loopTime = 1; loopTime <= 50; loopTime++)
                    {
                        gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                        float angle = (float)R * 360f;
                        float distance = (float)R * 24f + 3f;
                        var blast = Instantiate(Blast1, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                        Vector3 position = gameObject.transform.position + new Vector3(Mathf.Sin((float)DegreesToRadians(angle)) * distance, Mathf.Cos((float)DegreesToRadians(angle)) * distance, gameObject.transform.position.z + 0.001f);
                        blast.GetComponent<LittleBlastControl>().On(true, chanseSpeedFactor, speedReduceFactor, position);
                        blast.AddComponent<ObjDelayRecycle>();
                        blast.GetComponent<ObjDelayRecycle>().DelayRecycle(3f);
                        yield return new WaitForSeconds(time1);
                    }
                }
            });
        }
        */
        public void Skill6Init()
        {
            var scene = GameObject.Find("Battle Scene").gameObject;
            var blast = scene.transform.Find("Focus Blasts").gameObject;
            Blast1 = blast.transform.Find("HK Prime Blast (1)").gameObject;
            Blast1.AddComponent<LittleBlastControl>();
            gameObject.LocateMyFSM("Control").CopyState("SmallShot Antic", "SA6-1");
            gameObject.LocateMyFSM("Control").GetState("SA6-1").AddMethod(() =>
            {
                if (R > 0.5)
                {
                    gameObject.GetComponent<TeleportSystem>().TeleportByAngle(245f,9f);
                    gameObject.GetComponent<LockAngleWaitForAttack>().StartToLock(245f, 11f);
                }
                else
                {
                    gameObject.GetComponent<TeleportSystem>().TeleportByAngle(115f, 9f);
                    gameObject.GetComponent<LockAngleWaitForAttack>().StartToLock(115f, 11f);
                }
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                gameObject.GetComponent<AngleSystem>().FaceToPlayerStart();

            });
            float time = 2f;
            gameObject.LocateMyFSM("Control").CopyState("SmallShot LowHigh", "SL6-1");
            gameObject.LocateMyFSM("Control").GetState("SL6-1").GetAction<Wait>().time = time;
            gameObject.LocateMyFSM("Control").GetState("SL6-1").RemoveAction<FlingObjectsFromGlobalPoolTime>();

            gameObject.LocateMyFSM("Control").GetState("SL6-1").AddMethod(() =>
            {
                float angle = 0f;

                if(R > 0.8f)
                {
                    StartCoroutine(ExecuteWithDelay());
                    IEnumerator ExecuteWithDelay()
                    {
                        for (int loopTime = 1; loopTime <= 10; loopTime++)
                        {
                            angle += 180f + (float)R * 180f;
                            float distance = (float)R * 6f + 14f;
                            Vector3 position = new Vector3(Mathf.Sin((float)DegreesToRadians(angle)) * distance, Mathf.Cos((float)DegreesToRadians(angle)) * distance, gameObject.transform.position.z + 0.001f);
                            var blast = Instantiate(Blast1, HeroController.instance.transform.position + position, Quaternion.Euler(0, 0, 0));
                            blast.GetComponent<LittleBlastControl>().On(true, chanseSpeedFactor, speedReduceFactor, HeroController.instance.transform.position);
                            blast.AddComponent<ObjDelayRecycle>();
                            blast.GetComponent<ObjDelayRecycle>().DelayRecycle(3f);

                            var glow = Instantiate(ItemPool.Glow, HeroController.instance.transform.position + position, Quaternion.Euler(0, 0, 0));
                            glow.transform.SetParent(null);
                            glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 2f;
                            glow.SetActive(true);
                            glow.AddComponent<DelayDestory>();
                            glow.GetComponent<DelayDestory>().On(2f);

                            float time = 0.2f;
                            if(HardMode == 0)
                            {
                                time = 0.3f;
                            }
                            else if(HardMode == 1)
                            {
                                time = 0.2f;
                            }

                            yield return new WaitForSeconds(time);
                        }
                    }
                }
                else if(R > 0.4f)
                {
                    StartCoroutine(ExecuteWithDelay());
                    IEnumerator ExecuteWithDelay()
                    {
                        for (int loopTime = 1; loopTime <= 5; loopTime++)
                        {
                            angle += 180f + (float)R * 180f;
                            float distance = (float)R * 6f + 14f;
                            Vector3 position = new Vector3(Mathf.Sin((float)DegreesToRadians(angle)) * distance, Mathf.Cos((float)DegreesToRadians(angle)) * distance, gameObject.transform.position.z + 0.001f);
                            var blast = Instantiate(Blast1, HeroController.instance.transform.position + position, Quaternion.Euler(0, 0, 0));
                            blast.GetComponent<LittleBlastControl>().On(true, chanseSpeedFactor, speedReduceFactor, HeroController.instance.transform.position);
                            blast.AddComponent<ObjDelayRecycle>();
                            blast.GetComponent<ObjDelayRecycle>().DelayRecycle(3f);

                            var glow = Instantiate(ItemPool.Glow, HeroController.instance.transform.position + position, Quaternion.Euler(0, 0, 0));
                            glow.transform.SetParent(null);
                            glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 2f;
                            glow.SetActive(true);
                            glow.AddComponent<DelayDestory>();
                            glow.GetComponent<DelayDestory>().On(2f);

                            float time = 0.2f;
                            if (HardMode == 0)
                            {
                                time = 0.3f;
                            }
                            else if (HardMode == 1)
                            {
                                time = 0.2f;
                            }


                            yield return new WaitForSeconds(time);
                        }
                    }
                    StartCoroutine(DelayedExecution(1f));
                    IEnumerator DelayedExecution(float time)
                    {
                        yield return new WaitForSeconds(time);
                        gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                        gameObject.LocateMyFSM("Control").SetState("SA31-1");
                        //gameObject.GetComponent<AngleSystem>().FaceToPlayerEnd();
                    }
                }
                else
                {
                    StartCoroutine(ExecuteWithDelay());
                    IEnumerator ExecuteWithDelay()
                    {
                        for (int loopTime = 1; loopTime <= 5; loopTime++)
                        {
                            angle += 180f + (float)R * 180f;
                            float distance = (float)R * 6f + 14f;
                            Vector3 position = new Vector3(Mathf.Sin((float)DegreesToRadians(angle)) * distance, Mathf.Cos((float)DegreesToRadians(angle)) * distance, gameObject.transform.position.z + 0.001f);
                            var blast = Instantiate(Blast1, HeroController.instance.transform.position + position, Quaternion.Euler(0, 0, 0));
                            blast.GetComponent<LittleBlastControl>().On(true, chanseSpeedFactor, speedReduceFactor, HeroController.instance.transform.position);
                            blast.AddComponent<ObjDelayRecycle>();
                            blast.GetComponent<ObjDelayRecycle>().DelayRecycle(3f);

                            var glow = Instantiate(ItemPool.Glow, HeroController.instance.transform.position + position, Quaternion.Euler(0, 0, 0));
                            glow.transform.SetParent(null);
                            glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 2f;
                            glow.SetActive(true);
                            glow.AddComponent<DelayDestory>();
                            glow.GetComponent<DelayDestory>().On(2f);

                            float time = 0.2f;
                            if (HardMode == 0)
                            {
                                time = 0.3f;
                            }
                            else if (HardMode == 1)
                            {
                                time = 0.2f;
                            }

                            yield return new WaitForSeconds(time);
                        }
                    }
                    StartCoroutine(DelayedExecution(1f));
                    IEnumerator DelayedExecution(float time)
                    {
                        yield return new WaitForSeconds(time);
                        gameObject.LocateMyFSM("Control").SetState("SA3-1");
                        //gameObject.GetComponent<AngleSystem>().FaceToPlayerEnd();
                    }
                }
            });

            gameObject.LocateMyFSM("Control").GetState("SmallShot Recover").AddMethod(() =>
            {
                gameObject.GetComponent<AngleSystem>().FaceToPlayerEnd();
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
            });
        }
        public void Skill7Init()
        {
            float angle = 0;
            gameObject.LocateMyFSM("Control").CopyState("SmallShot Antic", "SA7");
            gameObject.LocateMyFSM("Control").GetState("SA7").AddMethod(() =>
            {
                gameObject.GetComponent<TeleportSystem>().TeleportByAngle(180f + 50f * (float)R, 11f);
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                if (gameObject.transform.position.x - HeroController.instance.transform.position.x > 0)
                {
                    gameObject.transform.localScale = new Vector3(1, -1, 1);
                }
                else
                {
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                }
                Vector3 direction = HeroController.instance.transform.position - gameObject.transform.position;
                angle = Mathf.Atan2(direction.y, direction.x);
            });
            float time = 0.5f + (float)R * 0.7f;
            gameObject.LocateMyFSM("Control").CopyState("SmallShot LowHigh", "SL7");
            gameObject.LocateMyFSM("Control").GetState("SL7").GetAction<Wait>().time = time + 0.1f;
            gameObject.LocateMyFSM("Control").GetState("SL7").RemoveAction<FlingObjectsFromGlobalPoolTime>();

            gameObject.LocateMyFSM("Control").GetState("SL7").AddMethod(() =>
            {
                gameObject.transform.localEulerAngles = new Vector3(0, 0, (float)RadiansToDegrees(angle));
                Skill7NailSummon(time);
            });
        }
        public void Skill8Init()
        {
            angle = 0;
            gameObject.LocateMyFSM("Control").CopyState("SmallShot Antic", "SA8");
            gameObject.LocateMyFSM("Control").GetState("SA8").AddMethod(() =>
            {
                angle = 120f + 120f * (float)R;
                gameObject.GetComponent<TeleportSystem>().TeleportByAngle(angle, 14f);
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                if (gameObject.transform.position.x - HeroController.instance.transform.position.x > 0)
                {
                    gameObject.transform.localScale = new Vector3(1, -1, 1);
                }
                else
                {
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                }
                Vector3 direction = HeroController.instance.transform.position - gameObject.transform.position;
                float distance = Vector2.Distance(HeroController.instance.transform.position, gameObject.transform.position);
                //angle = Mathf.Atan2(direction.y, direction.x);
                gameObject.GetComponent<LockAngleWaitForAttack>().StartToLock1(angle, distance);
            });
            float time = 0.5f + (float)R * 0.7f;
            gameObject.LocateMyFSM("Control").CopyState("SmallShot LowHigh", "SL8");
            gameObject.LocateMyFSM("Control").GetState("SL8").GetAction<Wait>().time = time + 0.1f;
            gameObject.LocateMyFSM("Control").GetState("SL8").RemoveAction<FlingObjectsFromGlobalPoolTime>();

            gameObject.LocateMyFSM("Control").GetState("SL8").AddMethod(() =>
            {
                //gameObject.transform.localEulerAngles = new Vector3(0, 0, (float)RadiansToDegrees(angle));
                Skill7NailSummon(time);
            });
            gameObject.LocateMyFSM("Control").GetState("SmallShot Recover").AddMethod(() =>
            {
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
            });
        }

        public void Skill9Init()
        {
            gameObject.LocateMyFSM("Control").CopyState("Tele In", "TI9");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Antic", "ST9");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Down", "SD9");
            gameObject.LocateMyFSM("Control").CopyState("Stomp Land", "SL9");
            gameObject.LocateMyFSM("Control").ChangeTransition("TI9", "FINISHED", "ST9");
            gameObject.LocateMyFSM("Control").ChangeTransition("ST9", "FINISHED", "SD9");
            gameObject.LocateMyFSM("Control").ChangeTransition("SD9", "LAND", "SL9");
            gameObject.LocateMyFSM("Control").ChangeTransition("Plume Gen", "FINISHED", "Burst Pause");
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(0);
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(1);
            gameObject.LocateMyFSM("Control").GetState("Plume Gen").RemoveAction(3);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(1);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(2);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(5);
            gameObject.LocateMyFSM("Control").GetState("Plume Up").RemoveAction(6);
            gameObject.LocateMyFSM("Control").GetState("Burst Pause").GetAction<Wait>().time = 2.3f;
            gameObject.LocateMyFSM("Control").GetState("ST9").AddMethod(() =>
            {
                gameObject.GetComponent<Skills>().LineAppear(3);
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                gameObject.GetComponent<LockAngleWaitForAttack>().StartToLockDtab(180f, 16f);
                AnimFreezeTime(0.3f, 0.05f);
                gameObject.GetComponent<Dtab>().StompDetect();
            });
            gameObject.LocateMyFSM("Control").GetState("SD9").AddMethod(() =>
            {
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                gameObject.GetComponent<Skills>().LineDisAppear();
            });
            gameObject.LocateMyFSM("Control").GetState("SL9").AddMethod(() =>
            {
                gameObject.GetComponent<Skills>().LineAppear(5);

                FLysBurst();

                HeroController.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Land, 1f);
                ItemPool.RockPt_Thin.GetComponent<RockPtControl>().On();
                gameObject.GetComponent<Skills>().Disappear();
                gameObject.GetComponent<Dtab>().PlumeSummon9(5f, false);
            });
            gameObject.LocateMyFSM("Control").GetState("SD9").GetAction<SetVelocity2d>().y = -250f;
            gameObject.LocateMyFSM("Control").GetState("TI9").AddMethod(() =>
            {
                gameObject.GetComponent<TeleportSystem>().TeleportByAngle(180, 12);
            });
        }

        public void Skill10Init()
        {
            gameObject.LocateMyFSM("Control").CopyState("Slash1 Antic", "SA10");
            gameObject.LocateMyFSM("Control").CopyState("CSlash", "CS10");
            gameObject.LocateMyFSM("Control").CopyState("CSlash Recover", "CSR10");
            gameObject.LocateMyFSM("Control").CopyState("Recover", "R10");
            gameObject.LocateMyFSM("Control").GetState("CSR10").RemoveAction<SetVelocity2d>();
            gameObject.LocateMyFSM("Control").GetState("CS10").RemoveAction<SetVelocity2d>();
            gameObject.LocateMyFSM("Control").ChangeTransition("SA10", "FINISHED", "CS10");
            gameObject.LocateMyFSM("Control").ChangeTransition("CS10", "FINISHED", "CSR10");
            gameObject.LocateMyFSM("Control").ChangeTransition("CSR10", "FINISHED", "R10");

            gameObject.LocateMyFSM("Control").GetState("SA10").AddMethod(() =>
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                angle = (float)R * 360f;
                gameObject.GetComponent<TeleportSystem>().TeleportByAngle(angle, 16);
                gameObject.GetComponent<LockAngleWaitForAttack>().StartToLock(angle, 20);
                gameObject.GetComponent<Skills>().LineAppear(1);
            });
            gameObject.LocateMyFSM("Control").GetState("CS10").AddMethod(() =>
            {
                gameObject.GetComponent<SlashColliderControl>().SlashOn();
                gameObject.GetComponent<OneSlashLight>().DelayLight(0f);
                gameObject.GetComponent<OneSlashAnyAngle>().On(angle, 350f);
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                gameObject.GetComponent<Skills>().LineDisAppear();
                AnimFreezeTime(0.08f, 0.02f);
            });
            gameObject.LocateMyFSM("Control").GetState("CSR10").AddMethod(() =>
            {
                gameObject.GetComponent<SlashColliderControl>().SlashOff();
            });
            gameObject.LocateMyFSM("Control").GetState("R10").AddMethod(() =>
            {
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                gameObject.GetComponent<LockAngleWaitForAttack>().EndLock();
                gameObject.LocateMyFSM("Control").SetState("Recover");
                Invoke("VelocitySetZero", 0.3f);
            });
        }
        public void Skill11Init()
        {
            gameObject.LocateMyFSM("Control").CopyState("SmallShot Antic", "SA11-1");
            gameObject.LocateMyFSM("Control").GetState("SA11-1").AddMethod(() =>
            {
                if(R > 0.5)
                {
                    gameObject.GetComponent<TeleportSystem>().TeleportByFloor(14f);
                }
                else
                {
                    gameObject.GetComponent<TeleportSystem>().TeleportByFloor(-14f);
                }
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();

            });
            float time = 0.9f;
            gameObject.LocateMyFSM("Control").CopyState("SmallShot LowHigh", "SL11-1");
            gameObject.LocateMyFSM("Control").GetState("SL11-1").GetAction<Wait>().time = time - 0.3f;
            gameObject.LocateMyFSM("Control").GetState("SL11-1").RemoveAction<FlingObjectsFromGlobalPoolTime>();

            gameObject.LocateMyFSM("Control").GetState("SL11-1").AddMethod(() =>
            {
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                Skill11Pos = gameObject.transform.position;
                Skill11NailSummon(time + 0.3f);
            });
        }
        public void Skill12Init()
        {
            //float time = 0.5f + gameObject.GetComponent<PhaseControl>().phase * 0.2f;
            float time = 0.5f;
            gameObject.LocateMyFSM("Control").CopyState("Slash1", "SL12-1");
            gameObject.LocateMyFSM("Control").CopyState("Slash1 Recover", "SR12-1");
            gameObject.LocateMyFSM("Control").CopyState("Slash 2", "SL12-2");
            gameObject.LocateMyFSM("Control").CopyState("Slash2 Recover", "SR12-2");
            gameObject.LocateMyFSM("Control").CopyState("Slash 3", "SL12-3");
            gameObject.LocateMyFSM("Control").CopyState("Slash3 Recover", "SR12-3");

            gameObject.LocateMyFSM("Control").ChangeTransition("SL12-1", "FINISHED", "SR12-1");
            gameObject.LocateMyFSM("Control").ChangeTransition("SL12-2", "FINISHED", "SR12-2");
            gameObject.LocateMyFSM("Control").ChangeTransition("SL12-3", "FINISHED", "SR12-3");
            gameObject.LocateMyFSM("Control").ChangeTransition("SR12-1", "FINISHED", "Recollider");
            gameObject.LocateMyFSM("Control").ChangeTransition("SR12-2", "FINISHED", "Recollider");
            gameObject.LocateMyFSM("Control").ChangeTransition("SR12-3", "FINISHED", "Recollider");
            //gameObject.LocateMyFSM("Control").ChangeTransition("Recollider", "FINISHED", "Focus Wait");


            gameObject.LocateMyFSM("Control").GetState("SL12-1").RemoveAction<SetVelocity2d>();
            gameObject.LocateMyFSM("Control").GetState("SL12-1").RemoveAction(4);
            gameObject.LocateMyFSM("Control").GetState("SL12-1").RemoveAction(5);
            gameObject.LocateMyFSM("Control").GetState("SR12-1").RemoveAction<SetVelocity2d>();
            gameObject.LocateMyFSM("Control").GetState("SL12-2").RemoveAction<SetVelocity2d>();
            gameObject.LocateMyFSM("Control").GetState("SR12-2").RemoveAction<SetVelocity2d>();
            gameObject.LocateMyFSM("Control").GetState("SL12-3").RemoveAction<SetVelocity2d>();
            gameObject.LocateMyFSM("Control").GetState("SL12-3").RemoveAction(3);
            gameObject.LocateMyFSM("Control").GetState("SR12-3").RemoveAction<SetVelocity2d>();

            gameObject.LocateMyFSM("Control").GetState("Counter Antic").AddMethod(() =>
            {
                gameObject.GetComponent<AngleSystem>().BossAngleRecover();
                if (gameObject.GetComponent<PhaseControl>().phase <= 2)
                {
                    if (R > 0.5)
                    {
                        gameObject.GetComponent<TeleportSystem>().TeleportByFloor(14f);
                    }
                    else
                    {
                        gameObject.GetComponent<TeleportSystem>().TeleportByFloor(-14f);
                    }
                }
                else
                {
                    if (R > 0.5)
                    {
                        gameObject.GetComponent<TeleportSystem>().TeleportByAngle(255f, 13f);
                        gameObject.GetComponent<LockAngleWaitForAttack>().StartToLock1(255f, 13f);
                    }
                    else
                    {
                        gameObject.GetComponent<TeleportSystem>().TeleportByAngle(105f, 13f);
                        gameObject.GetComponent<LockAngleWaitForAttack>().StartToLock1(105f, 13f);
                    }
                }
            });
            gameObject.LocateMyFSM("Control").GetState("Counter End").AddMethod(() =>
            {
                if (ItemPool.FocusHit != null)
                {
                    ItemPool.FocusHit.SetActive(false);
                }
                Skill12BigSlashSummon(time);
            });
            gameObject.LocateMyFSM("Control").GetState("Recollider").AddMethod(() =>
            {
                if(hardmodeEnd && HardMode == 0)
                {
                    hardmodeEnd = false;
                    Skill5();
                }
                if(skill12isBig)
                {
                    skill12isBig = false;
                    Skill12(0.6f, 1.2f, false, 0.05f);
                }
            });
            gameObject.LocateMyFSM("Control").GetState("Counter Stance").GetAction<Wait>().time = 0.5f;
            gameObject.LocateMyFSM("Control").GetState("CS Dir").AddMethod(() =>
            {
                gameObject.GetComponent<Skills>().Disappear();
            });
        }

        string WaitingStateName = null;
        public void SkillShort1Init()
        {
            float time = 0.29f;
            gameObject.LocateMyFSM("Control").CopyState("SmallShot LowHigh", "SLshort1");
            gameObject.LocateMyFSM("Control").CopyState("SmallShot Recover", "SRshort1");
            gameObject.LocateMyFSM("Control").GetState("SLshort1").GetAction<Wait>().time = time;
            gameObject.LocateMyFSM("Control").GetState("SLshort1").RemoveAction<FlingObjectsFromGlobalPoolTime>();
            gameObject.LocateMyFSM("Control").ChangeTransition("SLshort1", "FINISHED", "SRshort1");

            gameObject.LocateMyFSM("Control").GetState("SLshort1").AddMethod(() =>
            {
                gameObject.transform.localEulerAngles = new Vector3(0, 0, (float)RadiansToDegrees(angle));
                Skill1shortNailSummon(time);
            });
            gameObject.LocateMyFSM("Control").GetState("SRshort1").AddMethod(() =>
            {
                gameObject.LocateMyFSM("Control").SetState(WaitingStateName);
            });
        }
        public void SkillShort2Init()
        {
            float time = 0.5f;
            gameObject.LocateMyFSM("Control").CopyState("SmallShot LowHigh", "SLshort2");
            gameObject.LocateMyFSM("Control").CopyState("SmallShot Recover", "SRshort2");
            gameObject.LocateMyFSM("Control").GetState("SLshort2").GetAction<Wait>().time = time;
            gameObject.LocateMyFSM("Control").GetState("SLshort2").RemoveAction<FlingObjectsFromGlobalPoolTime>();
            gameObject.LocateMyFSM("Control").ChangeTransition("SLshort2", "FINISHED", "SRshort2");

            gameObject.LocateMyFSM("Control").GetState("SLshort2").AddMethod(() =>
            {
                gameObject.transform.localEulerAngles = new Vector3(0, 0, (float)RadiansToDegrees(angle));
                Skill2shortBigSlashSummon(time);
            });
            gameObject.LocateMyFSM("Control").GetState("SRshort2").AddMethod(() =>
            {
                gameObject.LocateMyFSM("Control").SetState(WaitingStateName);
            });
        }
        public void SkillShort3Init()
        {
            float time = 0.35f;
            gameObject.LocateMyFSM("Control").CopyState("SmallShot LowHigh", "SLshort3");
            gameObject.LocateMyFSM("Control").CopyState("SmallShot Recover", "SRshort3");
            gameObject.LocateMyFSM("Control").GetState("SLshort3").GetAction<Wait>().time = time;
            gameObject.LocateMyFSM("Control").GetState("SLshort3").RemoveAction<FlingObjectsFromGlobalPoolTime>();
            gameObject.LocateMyFSM("Control").ChangeTransition("SLshort3", "FINISHED", "SRshort3");

            gameObject.LocateMyFSM("Control").GetState("SLshort3").AddMethod(() =>
            {
                gameObject.transform.localEulerAngles = new Vector3(0, 0, (float)RadiansToDegrees(angle));
                StartCoroutine(ExecuteWithDelay());
                IEnumerator ExecuteWithDelay()
                {
                    for (int loopTime = 1; loopTime <= 3; loopTime++)
                    {
                        angle += 180f + (float)R * 180f;
                        float distance = (float)R * 6f + 14f;
                        Vector3 position = new Vector3(Mathf.Sin((float)DegreesToRadians(angle)) * distance, Mathf.Cos((float)DegreesToRadians(angle)) * distance, gameObject.transform.position.z + 0.001f);
                        var blast = Instantiate(Blast1, HeroController.instance.transform.position + position, Quaternion.Euler(0, 0, 0));
                        blast.GetComponent<LittleBlastControl>().On(true, chanseSpeedFactor, speedReduceFactor, HeroController.instance.transform.position);
                        blast.AddComponent<ObjDelayRecycle>();
                        blast.GetComponent<ObjDelayRecycle>().DelayRecycle(3f);

                        var glow = Instantiate(ItemPool.Glow, HeroController.instance.transform.position + position, Quaternion.Euler(0, 0, 0));
                        glow.transform.SetParent(null);
                        glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 2f;
                        glow.SetActive(true);
                        glow.AddComponent<DelayDestory>();
                        glow.GetComponent<DelayDestory>().On(2f);

                        yield return new WaitForSeconds(0.1f);
                    }
                }
            });
            gameObject.LocateMyFSM("Control").GetState("SRshort3").AddMethod(() =>
            {
                gameObject.LocateMyFSM("Control").SetState(WaitingStateName);
            });
        }
        public void SkillPhase3()
        {
            gameObject.LocateMyFSM("Control").SetState("TIPhase3");
        }
        public void SkillPhase4()
        {
            gameObject.LocateMyFSM("Control").SetState("TIPhase4");
        }
        public void SkillPhase5()
        {
            gameObject.LocateMyFSM("Control").SetState("TIPhase5");
        }
        public void SkillTurnPhase()
        {
            gameObject.LocateMyFSM("Control").SetState("FC_S");
        }
        //前摇（三）连斩
        public void Skill1()
        {
            gameObject.LocateMyFSM("Control").SetState("SA1");
        }

        //（五）连战尾杀
        public void Skill2()
        {
            gameObject.LocateMyFSM("Control").SetState("SA2");
        }

        //锁头剑气
        public void Skill3()
        {
            gameObject.LocateMyFSM("Control").SetState("SA3");
        }

        //飞剑
        public void Skill4(float loopTime)
        {
            skill4LoopTime = loopTime;
            gameObject.LocateMyFSM("Control").SetState("SA4");
            gameObject.LocateMyFSM("Control").ChangeTransition("L Wave", "LOWHIGH", "SL4");
            gameObject.LocateMyFSM("Control").ChangeTransition("R Wave", "LOWHIGH", "SL4");
        }

        //地刺
        public void Skill5()
        {
            gameObject.LocateMyFSM("Control").SetState("TI5");
            Modding.Logger.Log("Skill 5");
        }

        //星爆
        public void Skill6()
        {
            gameObject.LocateMyFSM("Control").SetState("SA6-1");
            gameObject.LocateMyFSM("Control").ChangeTransition("L Wave", "LOWHIGH", "SL6-1");
            gameObject.LocateMyFSM("Control").ChangeTransition("R Wave", "LOWHIGH", "SL6-1");
            Modding.Logger.Log("Skill 6");
        }

        //持续飞剑
        public void Skill7()
        {
            gameObject.LocateMyFSM("Control").SetState("SA7");
            gameObject.LocateMyFSM("Control").ChangeTransition("L Wave", "LOWHIGH", "SL7");
            gameObject.LocateMyFSM("Control").ChangeTransition("R Wave", "LOWHIGH", "SL7");
            Modding.Logger.Log("Skill 7");
        }

        //追踪飞剑？
        public void Skill8()
        {
            gameObject.LocateMyFSM("Control").SetState("SA8");
            gameObject.LocateMyFSM("Control").ChangeTransition("L Wave", "LOWHIGH", "SL8");
            gameObject.LocateMyFSM("Control").ChangeTransition("R Wave", "LOWHIGH", "SL8");
            Modding.Logger.Log("Skill 8");
        }

        //地刺2
        public void Skill9()
        {
            gameObject.LocateMyFSM("Control").SetState("TI9");
            Modding.Logger.Log("Skill 9");
        }
        //单次蓄力斩
        public void Skill10()
        {
            gameObject.LocateMyFSM("Control").SetState("SA10");
            Modding.Logger.Log("Skill 10");
        }
        //剑雨
        public void Skill11(float loopTime)
        {
            skill11LoopTime = loopTime;
            gameObject.LocateMyFSM("Control").SetState("SA11-1");
            gameObject.LocateMyFSM("Control").ChangeTransition("L Wave", "LOWHIGH", "SL11-1");
            gameObject.LocateMyFSM("Control").ChangeTransition("R Wave", "LOWHIGH", "SL11-1");
            Modding.Logger.Log("Skill 11");
        }
        public void Skill12(float timeFactor, float edgeFactor, bool fast, float loopTime)
        {
            bigSlashEdgeFactor = edgeFactor;
            bigSlashTimeFactor = timeFactor;
            skill12LoopTime = loopTime;
            if (fast)
            {
                gameObject.LocateMyFSM("Control").SetState("Counter End");
            }
            else
            {
                gameObject.LocateMyFSM("Control").SetState("Counter Antic");
            }
            Modding.Logger.Log("Skill 12");
        }

        public void SkillShort1(string fsmStateName)
        {
            WaitingStateName = fsmStateName;
            gameObject.LocateMyFSM("Control").SetState("SLshort1");
        }
        public void SkillShort2(string fsmStateName, float timeFactor, float edgeFactor)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            gameObject.GetComponent<Skills>().Disappear();
            bigSlashEdgeFactor = edgeFactor;
            bigSlashTimeFactor = timeFactor;
            WaitingStateName = fsmStateName;
            gameObject.LocateMyFSM("Control").SetState("SLshort2");
        }
        public void SkillShort3(string fsmStateName)
        {
            WaitingStateName = fsmStateName;
            gameObject.LocateMyFSM("Control").SetState("SLshort3");
        }
    }
}
