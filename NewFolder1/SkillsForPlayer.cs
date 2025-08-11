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
using HutongGames.PlayMaker;
using HKMirror.Reflection.InstanceClasses;
using IL.tk2dRuntime.TileMap;
using INFINITY;
using static INFINITY.Skills;
using IL;
using static INFINITY.SkillsControl;
using Steamworks;
using static UnityEngine.ParticleSystem;

namespace INFINITY_FOR_PLAYER
{
    public class TeleportSystemForPlayer : MonoBehaviour
    {
        public void TeleportByAngle(float angle, float distance)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
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
    public class AngleSystemForPlayer : MonoBehaviour
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
            if (HeroController.instance.gameObject.transform.localScale.x > 0)
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
    public class OneSlashLightForPlayer : MonoBehaviour
    {
        public void Start()
        {
            Invoke("GetBall", 1f);
        }
        public void GetBall()
        {
            if (ItemPool.SlashLightForPlayer == null)
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
                            ItemPool.SlashLightForPlayer = Instantiate(obj, gameObject.transform);
                            ItemPool.SlashLightForPlayer.transform.localPosition = new Vector3(2f, 0f, 0.001f);

                            var image = LoadPng("INFINITY.Resources.Spell.png");
                            if (image != null)
                            {
                                ItemPool.SlashLightForPlayer.GetComponent<tk2dSprite>().CurrentSprite.material.mainTexture = image;
                            }
                            else
                            {
                                Modding.Logger.Log("image is null!");
                            }

                            ItemPool.SlashLightForPlayer.LocateMyFSM("Fireball Control").ChangeTransition("Idle", "FINISHED", "Recycle");
                            ItemPool.SlashLightForPlayer.LocateMyFSM("Fireball Control").GetState("Idle").GetAction<Wait>().time = 99999f;

                            ItemPool.SlashLightForPlayer.SetActive(false);

                            break;
                        }
                    }
                }
            }
        }
        public void DelayLight(float delayTime)
        {
            Invoke("Light", delayTime);
        }
        public void Light()
        {
            GameObject lightPrefab = null;

            if(ItemPool.SlashLightForPlayer == null)
            {
                GetBall();
            }
            else
            {
                var light = Instantiate(ItemPool.SlashLightForPlayer, ItemPool.SlashLightForPlayer.transform);

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
    }
    public class SlashColliderControlForPlayer : MonoBehaviour
    {
        public GameObject MainSlash;
        public void Start()
        {
            var slashes = gameObject.transform.Find("Slashes").gameObject;
            var slash2 = slashes.transform.Find("Slash2").gameObject;
            var slash3 = slashes.transform.Find("Slash3").gameObject;
            slash3.transform.localPosition += new Vector3(0, 1000, 0);
            MainSlash = slash2;
        }
        public void SlashOn()
        {
            MainSlash.SetActive(true);
        }
        public void SlashOff()
        {
            Invoke("Off", 0.06f);
        }
        void Off()
        {
            MainSlash.SetActive(false);
        }
    }
    public class LockAngleWaitForAttackForPlayer : MonoBehaviour
    {
        private Vector3 LockLocalPosition;

        public GameObject lockedEnemy;

        public float y = 0f;

        double R1 => random.NextDouble();
        double R2 => random.NextDouble();
        public void StartToLockForPlayer_Dstab(float localY)
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
                gameObject.GetComponent<SkillsForPlayer>().Skill1End();
                return;
            }

            if (lockedEnemy != null)
            {

                if (lockedEnemy.transform.position.x - gameObject.transform.position.x > 0)
                {
                    gameObject.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                }

            }
            else
            {
                gameObject.GetComponent<SkillsForPlayer>().Skill3End();
            }
            y = localY;

            LockLoopForPlayer_Dstab();
        }
        public void LockLoopForPlayer_Dstab()
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
                gameObject.GetComponent<SkillsForPlayer>().Skill1End();
                return;
            }

            if (lockedEnemy != null)
            {
                LockLocalPosition = new Vector3(lockedEnemy.transform.position.x, lockedEnemy.transform.position.y + y, 0);

                gameObject.transform.position += (LockLocalPosition - gameObject.transform.position) / 12f;

            }
            else
            {
                gameObject.GetComponent<SkillsForPlayer>().Skill3End();
            }

            Invoke("LockLoopForPlayer_Dstab", 0.02f);
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
                gameObject.GetComponent<SkillsForPlayer>().Skill1End();
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

                LockLoopForPlayer();

                var glow = Instantiate(INFINITY.ItemPool.Glow, gameObject.transform.position, Quaternion.Euler(0, 0, gameObject.transform.eulerAngles.z + 90f));
                glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 3;
                glow.SetActive(true);
                glow.AddComponent<INFINITY.DelayDestory>();
                glow.GetComponent<INFINITY.DelayDestory>().On(2f);

                gameObject.GetComponent<AudioSource>().PlayOneShot(INFINITY.ItemPool.Teleport, 0.8f);
            }
            else
            {
                gameObject.GetComponent<SkillsForPlayer>().Skill1End();
            }

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
                gameObject.GetComponent<SkillsForPlayer>().Skill1End();
            }
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
                gameObject.GetComponent<SkillsForPlayer>().Skill1End();
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
                var glow = Instantiate(INFINITY.ItemPool.Glow, gameObject.transform.position, Quaternion.Euler(0, 0, gameObject.transform.eulerAngles.z + 90f));
                glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 3;
                glow.SetActive(true);
                glow.AddComponent<INFINITY.DelayDestory>();
                glow.GetComponent<INFINITY.DelayDestory>().On(2f);

                gameObject.GetComponent<AudioSource>().PlayOneShot(INFINITY.ItemPool.Teleport, 0.8f);
            }
            else
            {
                gameObject.GetComponent<SkillsForPlayer>().Skill1End();
            }
        }
        public void EndLock()
        {
            CancelInvoke("LockLoopForPlayer");
            CancelInvoke("LockLoopForPlayer_Dstab");
        }
    }
    public class BigSlashControlForPlayer : MonoBehaviour
    {
        float factor1 = 1f;
        public void On(float scaleFactor)
        {
            factor1 = scaleFactor;
            for (int i = 0; i < gameObject.transform.childCount && gameObject.transform.GetChild(i).name.Contains("Beam"); i++)
            {
                var line = gameObject.transform.GetChild(i).gameObject;
                line.transform.localScale = new Vector3(5f, 10f * Math.Sign(line.transform.localScale.y) * scaleFactor, 1f);
                line.SetActive(true);
            }

            DelayLight(skill2SlashDelayTime);
        }
        public void DelayLight(float delayTime)
        {
            Invoke("Light", delayTime);
        }
        public void Light()
        {
            Invoke("Damage", 0.04f);

            for (int i = 0; i < gameObject.transform.childCount && gameObject.transform.GetChild(i).name.Contains("Beam"); i++)
            {
                var line = gameObject.transform.GetChild(i).gameObject;
                line.transform.localScale = new Vector3(5f, 10f * Math.Sign(line.transform.localScale.y), 1f);
                line.SetActive(false);
            }

            GameObject lightPrefab = ItemPool.SlashLightForPlayer;
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

        }
        public static int skill2DamageFactor = 1;
        public static float skill2SlashDelayTime = 0.2f;
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
                    obj.layer = LayerMask.NameToLayer("Attack");
                    SetDamageEnemy(obj, skill2DamageFactor * 2 * PlayerData.instance.nailDamage, obj.transform.eulerAngles.z, 1f, AttackTypes.Nail);
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
        }
    }
    public class CounterDetect : MonoBehaviour
    {
        public bool counterOn = false;

        public void Start()
        {
            if (gameObject.GetComponent<PolygonCollider2D>())
            {
                gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            }

            gameObject.layer = LayerMask.NameToLayer("Hero Box");

            if(gameObject.GetComponent<DamageHero>())
            {
                gameObject.GetComponent<DamageHero>().damageDealt = 0;
            }
        }
        public void CounterOn()
        {
            if (gameObject.GetComponent<PolygonCollider2D>())
            {
                gameObject.GetComponent<PolygonCollider2D>().enabled = true;
            }
            counterOn = true;
        }
        public void CounterEnd()
        {
            if (gameObject.GetComponent<PolygonCollider2D>())
            {
                gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            }
            counterOn = false;

        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            Modding.Logger.Log(collision.gameObject.name);

            bool flag1 = collision.gameObject.GetComponent<DamageHero>() != null;
            bool flag2 = collision.gameObject.GetComponent<DamageHero>().damageDealt >= 1;
            bool flag3 = collision.gameObject.activeSelf;

            if (counterOn && flag1 && flag2 && flag3)
            {
                CounterEnd();
                gameObject.transform.parent.gameObject.LocateMyFSM("Control").SetState("CS Dir");
            }
        }

    }
    public class DtabForPlayer : MonoBehaviour
    {
        public float landY = 0f;
        public bool stomping = false;
        public void StompDetect(float y)
        {
            landY = y;

            stomping = true;
        }
        public void Update()
        {
            if (stomping && gameObject.transform.position.y <= landY)
            {
                stomping = false;

                gameObject.LocateMyFSM("Control").SetState("SL5");
            }
        }
    }
    public class WaveControlForPlayer : MonoBehaviour
    {
        public float speed = 0f;
        public int shrikeTime = 0;
        public Vector3 orig_Scale = Vector3.one;
        void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Attack");

            SetDamageEnemy(gameObject, 8 * PlayerData.instance.nailDamage, 90 - Math.Sign(gameObject.transform.parent.transform.parent.GetComponent<Rigidbody2D>().velocity.x) * 90, 1, AttackTypes.Spell);

            orig_Scale = gameObject.transform.parent.transform.parent.transform.localScale;
            SpeedDownLoop();
            Invoke("SummonOne", 2f);
            Invoke("DamageReduce", 0.15f);
        }
        public void DamageReduce()
        {
            SetDamageEnemy(gameObject, PlayerData.instance.nailDamage, 90 - Math.Sign(gameObject.transform.parent.transform.parent.GetComponent<Rigidbody2D>().velocity.x) * 90, 1, AttackTypes.Spell);

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
    public class SkillsForPlayer : MonoBehaviour
    {
        public int slashCount = 0;
        public int slashCountMax = 5;

        System.Random random = new System.Random();
        double R1 => random.NextDouble();
        double R2 => random.NextDouble();
        double R3 => random.NextDouble();

        float angle = 0f;

        GameObject CounterBox = null;

        public void GetItems()
        {
            ItemPool.Teleport = gameObject.LocateMyFSM("Control").GetState("Tele Out").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;
            ItemPool.Slash = HeroController.instance.gameObject.LocateMyFSM("Nail Arts").GetState("G Slash").GetAction<AudioPlay>().oneShotClip.Value as AudioClip;
            ItemPool.CounterFlash = Instantiate(gameObject.transform.Find("Counter Flash").gameObject, gameObject.transform);
            ItemPool.Dir = gameObject.LocateMyFSM("Control").GetState("Counter Stance").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;
            ItemPool.DashBurst = gameObject.transform.Find("Dash Burst").gameObject;
        }

        public void Start()
        {

            if (!gameObject.GetComponent<AngleSystemForPlayer>())
                gameObject.AddComponent<AngleSystemForPlayer>();

            if (!gameObject.GetComponent<SlashColliderControlForPlayer>())
                gameObject.AddComponent<SlashColliderControlForPlayer>();

            if (!gameObject.GetComponent<LockAngleWaitForAttackForPlayer>())
                gameObject.AddComponent<LockAngleWaitForAttackForPlayer>();

            if (!gameObject.GetComponent<INFINITY.OneSlashAnyAngle>())
                gameObject.AddComponent<INFINITY.OneSlashAnyAngle>();

            if (!gameObject.GetComponent<INFINITY.OneSlash>())
                gameObject.AddComponent<INFINITY.OneSlash>();

            if (!gameObject.GetComponent<OneSlashLightForPlayer>())
                gameObject.AddComponent<OneSlashLightForPlayer>();

            if (!gameObject.GetComponent<TeleportSystemForPlayer>())
                gameObject.AddComponent<TeleportSystemForPlayer>();

            if (!gameObject.GetComponent<DtabForPlayer>())
                gameObject.AddComponent<DtabForPlayer>();

            if (!gameObject.GetComponent<Fadeimage>())
                gameObject.AddComponent<Fadeimage>();


            gameObject.GetComponent<HealthManager>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

            var slashes = gameObject.transform.Find("Slashes").gameObject;
            var slash1 = slashes.transform.Find("Slash1");
            var slash2 = slashes.transform.Find("Slash2");
            var slash3 = slashes.transform.Find("Slash3");

            var dashBurst = gameObject.transform.Find("Dash Burst").gameObject;
            dashBurst.transform.localScale = new Vector3(-9f, 3, 3);

            slash2.gameObject.layer = LayerMask.NameToLayer("Attack");

            slash1.transform.localScale = new Vector3(-1, 2, 1);
            slash1.localPosition = new Vector3(-4f, 1.2255f, 0.004f);

            slash2.transform.localScale = new Vector3(-3, 1.2f, 1);
            slash2.localPosition = new Vector3(-16f, -0.1f, 0.004f);
            slash2.gameObject.AddComponent<BlockFlash>();
            slash2.GetComponent<PolygonCollider2D>().points = new Vector2[] { new Vector2(-6.7f, 2f), new Vector2(0f, 2.3f), new Vector2(4.6f, 2f), new Vector2(2.4f, 1f), new Vector2(1f, -1f), new Vector2(3f, -2.8f), new Vector2(-7.5f, -2.8f), new Vector2(-8.5f, -1.8f), new Vector2(-8.5f, -0.95f), new Vector2(-7.6f, 0.4f) };
            slash3.transform.localScale = new Vector3(-3, 1.05f, 1);
            slash3.localPosition = new Vector3(-9f, -1f, 0.004f);

            CounterBox = Instantiate(slash1.gameObject, gameObject.transform);

            if (!CounterBox.GetComponent<CounterDetect>())
                CounterBox.gameObject.AddComponent<CounterDetect>();

            CounterBox.SetActive(true);

            gameObject.GetComponent<SlashColliderControlForPlayer>().MainSlash = slash2.gameObject;
            gameObject.GetComponent<SlashColliderControlForPlayer>().MainSlash.GetComponent<DamageHero>().damageDealt = 0;

            Skill1Init();
            Skill2Init();
            Skill3Init();

            GetItems();

        }

        public void HeroDisappear()
        {
            gameObject.GetComponent<LockAngleWaitForAttackForPlayer>().EndLock();
            gameObject.GetComponent<AngleSystemForPlayer>().FaceToPlayerEnd();

            HeroController.instance.parryInvulnTimer = 99999f;
            HeroController.instance.gameObject.LocateMyFSM("Nail Arts").SetState("Regain Control");
            HeroController.instance.gameObject.LocateMyFSM("Spell Control").SetState("SA");
            HeroController.instance.gameObject.GetComponent<MeshRenderer>().enabled = false;
            var glow = Instantiate(INFINITY.ItemPool.Glow, HeroController.instance.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 2;
            glow.SetActive(true);
            glow.AddComponent<INFINITY.DelayDestory>();
            glow.GetComponent<INFINITY.DelayDestory>().On(2f);

            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        public void HeroAppear(float parryInvulnTime)
        {
            gameObject.LocateMyFSM("Control").SetState("Idle");
            gameObject.SetActive(false);
            gameObject.transform.SetParent(HeroController.instance.transform);
            gameObject.transform.localPosition = Vector3.zero;

            HeroController.instance.parryInvulnTimer = parryInvulnTime;
            HeroController.instance.gameObject.LocateMyFSM("Nail Arts").SetState("Regain Control");
            HeroController.instance.gameObject.LocateMyFSM("Spell Control").SetState("Spell End");
            HeroController.instance.gameObject.GetComponent<MeshRenderer>().enabled = true;
            var glow = Instantiate(INFINITY.ItemPool.Glow, HeroController.instance.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 2;
            glow.SetActive(true);
            glow.AddComponent<INFINITY.DelayDestory>();
            glow.GetComponent<INFINITY.DelayDestory>().On(2f);
            //INFINITY.CameraControl.CamClose();
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

        public void GetBigSlashForPlayer()
        {
            if (ItemPool.BigSlashForPlayer == null)
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

                    light1.transform.localScale = new Vector3(4f, 13f, 1f);
                    //light2.transform.localScale = new Vector3(6f, 13f, 1f);
                    light3.transform.localScale = new Vector3(4f, -13f, 1f);
                    //light4.transform.localScale = new Vector3(6f, -13f, 1f);

                    light1.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                    //light2.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                    light3.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                    //light4.transform.eulerAngles = new Vector3(0f, 0f, 270f);

                }

                bigSlash.AddComponent<BigSlashControlForPlayer>();
                ItemPool.BigSlashForPlayer = bigSlash;

                var slashes = gameObject.transform.Find("Slashes").gameObject;
                var Slash2 = slashes.transform.Find("Slash2").gameObject;
                var collision = Instantiate(Slash2, bigSlash.transform);
                collision.transform.localPosition = new Vector3(-15f, 0.4f, 0f);
                collision.transform.localScale = new Vector3(-9, 0.85f, 1);
                collision.SetActive(false);

            }
        }
        public void Skill12BigSlashSummon(float time, bool isParry)
        {
            GetBigSlashForPlayer();

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

            if (ItemPool.CounterFlash)
            {
                var flash = Instantiate(ItemPool.CounterFlash, ItemPool.CounterFlash.transform);
                flash.transform.SetParent(null);
                flash.transform.eulerAngles = new Vector3(0, 0, eulerAngle);
                flash.SetActive(true);
                flash.AddComponent<DelayDestory>();
                flash.GetComponent<DelayDestory>().On(3);
            }
            else
            {
                GetItems();
            }
        }
        public void Skill12BigSlashSummonLoop()
        {
            if (ItemPool.Dir)
            {
                HeroController.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Dir, 1f);
            }

            Invoke("Skill12BigSlashSummonLoop", skill12LoopTime);


            sigh1 *= -1;
            sigh2 *= -1;
            sigh3 *= -1;
            Vector3 pos = new Vector3(0, 0, 0);
            float eul = 0f;
            GameObject closest = null;
            GameObject lockedEnemy = null;
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
                Skill12BigSlashSummonEnd();
                gameObject.GetComponent<SkillsForPlayer>().Skill1End();
                return;
            }

            if (lockedEnemy != null)
            {
                pos = new Vector3(sigh1 * (float)R1 * 13f * bigSlashEdgeFactor, sigh2 * (float)R2 * 9f * bigSlashEdgeFactor, 0f);
                eul = sigh3 * 90f + (float)R3 * 180f;

                CounterFlashSummon(eul);

                GameObject bigslash = Instantiate(ItemPool.BigSlashForPlayer, lockedEnemy.transform.position + pos, Quaternion.Euler(0, 0, eul));
                //bigslash.GetComponent<PolygonCollider2D>().enabled = false;

                if (skill12LoopTime < 0.1f)
                {
                    bigslash.transform.localScale = new Vector3(bigslash.transform.localScale.x, bigslash.transform.localScale.y * (0.8f + (float)R3 * 0.6f), 1f);
                }
                bigslash.GetComponent<BigSlashControlForPlayer>().On(1f);

                if (Mathf.Abs(bigslash.transform.eulerAngles.z - lastEularAngle) <= 15f || (Mathf.Abs(bigslash.transform.eulerAngles.z - lastEularAngle) - 180f <= 15f))
                {
                    bigslash.transform.eulerAngles += new Vector3(0, 0, 15f * sigh1);
                }

                lastEularAngle = bigslash.transform.eulerAngles.z;

                if(skill12LoopTime >= 0.1f)
                {
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

            }
            else
            {
                gameObject.GetComponent<SkillsForPlayer>().Skill1End();
            }



        }
        public void Skill12BigSlashSummonEnd()
        {
            var slashes = gameObject.transform.Find("Slashes").gameObject;
            for (int i = 0; i < slashes.transform.childCount; i++)
            {
                var obj = slashes.transform.GetChild(i).gameObject;
                obj.SetActive(false);
            }
            CancelInvoke("Skill12BigSlashSummonLoop");

            if(skill12LoopTime >= 0.1)
            {
                Skill1End();
            }
            else
            {
                gameObject.LocateMyFSM("Control").SetState("SL12-2");
                Invoke("Skill1End", 0.6f);
            }
        }

        public void WaveSummonForPlayer(Vector3 pos, float speed)
        {
            var wave = Instantiate(ItemPool.Wave, pos, Quaternion.Euler(0, 0, 0));
            wave.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 2f, 0f);
            wave.transform.localScale = new Vector3(wave.transform.localScale.x * Math.Sign(speed) * 1.5f, wave.transform.localScale.y * 3f, wave.transform.localScale.z);
            var core = wave.transform.Find("slash_core").gameObject;
            core.transform.Find("hurtbox").gameObject.GetComponent<DamageHero>().damageDealt = 0;
            core.transform.Find("hurtbox").gameObject.AddComponent<WaveControlForPlayer>();
            core.transform.Find("hurtbox").gameObject.GetComponent<WaveControlForPlayer>().speed = speed;
        }

        public void GetDstabSlashForPlayer()
        {
            if (ItemPool.BigDstabForPlayer == null)
            {
                var slashes = gameObject.transform.Find("Slashes").gameObject;
                var Slash2 = slashes.transform.Find("Slash2").gameObject;

                var collision = Instantiate(Slash2, gameObject.transform);

                collision.GetComponent<PolygonCollider2D>().enabled = false;

                SetDamageEnemy(collision, 7 * PlayerData.instance.nailDamage, 270f, 0f, AttackTypes.Spell);

                collision.transform.localPosition = new Vector3(0.4f, 0f, 0f);
                collision.transform.localEulerAngles = new Vector3(0f, 0f, 270f);
                collision.transform.localScale = new Vector3(-1, 0.85f, 1);
                collision.SetActive(true);

                ItemPool.BigDstabForPlayer = collision;

                if (ItemPool.Line != null)
                {
                    var light1 = Instantiate(ItemPool.Line, collision.transform);
                    //var light2 = Instantiate(ItemPool.Line, bigSlash.transform);
                    var light3 = Instantiate(ItemPool.Line, collision.transform);
                    //var light4 = Instantiate(ItemPool.Line, bigSlash.transform);

                    light1.transform.localPosition = new Vector3(30f, 0f, 0f);
                    //light2.transform.localPosition = new Vector3(30f, 0f, 0f);
                    light3.transform.localPosition = new Vector3(-30f, 0f, 0f);
                    //light4.transform.localPosition = new Vector3(-30f, 0f, 0f);

                    light1.transform.localScale = new Vector3(4f, 13f, 1f);
                    //light2.transform.localScale = new Vector3(6f, 13f, 1f);
                    light3.transform.localScale = new Vector3(4f, -13f, 1f);
                    //light4.transform.localScale = new Vector3(6f, -13f, 1f);

                    light1.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                    //light2.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                    light3.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                    //light4.transform.eulerAngles = new Vector3(0f, 0f, 270f);

                }
            }
        }
        public void DstabBurstOn()
        {
            var burst = Instantiate(ItemPool.DashBurst, gameObject.transform);

            burst.LocateMyFSM("FSM").FsmVariables.GetFsmVector3("Init Pos").Value = new Vector3(0f, 0f, 0f);
            burst.LocateMyFSM("FSM").GetState("Init").RemoveAction(0);
            burst.LocateMyFSM("FSM").GetState("Init").RemoveAction(1);
            burst.LocateMyFSM("FSM").enabled = false;
            burst.SetActive(true);

            burst.transform.SetParent(null);
            burst.transform.localScale = new Vector3(6, 4, 3);
            burst.transform.eulerAngles = new Vector3(0, 0, 90);
            burst.transform.position = gameObject.transform.position + new Vector3(0, 4f, 0);
            burst.AddComponent<DelayDestory>();
            burst.GetComponent<DelayDestory>().On(2f);
        }
        public void Skill1Init()
        {
            gameObject.LocateMyFSM("Control").CopyState("Slash1 Antic", "SA2");
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
                slashCountMax = 5;

                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                slashCount++;
                if (slashCount >= slashCountMax)
                {
                    gameObject.GetComponent<AngleSystemForPlayer>().BossAngleRecover();
                    gameObject.GetComponent<LockAngleWaitForAttackForPlayer>().EndLock();
                    gameObject.LocateMyFSM("Control").SetState("Recover");
                    return;
                }
                angle = (float)R1 * 360f;

                SetDamageEnemy(gameObject.GetComponent<SlashColliderControlForPlayer>().MainSlash, 4 * PlayerData.instance.nailDamage, angle + 90f, 2f);
                
                gameObject.GetComponent<LockAngleWaitForAttackForPlayer>().StartToLockForPlayer(angle, 14f);

            });
            gameObject.LocateMyFSM("Control").GetState("CS2").AddMethod(() =>
            {
                gameObject.GetComponent<SlashColliderControlForPlayer>().SlashOn();

                gameObject.GetComponent<INFINITY.OneSlashAnyAngle>().On(angle, 350f);

                AnimFreezeTime(0.08f, 0.02f);

                gameObject.GetComponent<OneSlashLightForPlayer>().DelayLight(0f);

                gameObject.GetComponent<LockAngleWaitForAttackForPlayer>().EndLock();

            });
            gameObject.LocateMyFSM("Control").GetState("CSR2").AddMethod(() =>
            {
                gameObject.GetComponent<SlashColliderControlForPlayer>().SlashOff();

                angle += (float)R1 * 120f - 60f + 180f;
                slashCount++;

                if (slashCount > slashCountMax)
                {
                    slashCount = 0;

                    gameObject.GetComponent<AngleSystemForPlayer>().BossAngleRecover();

                    gameObject.GetComponent<LockAngleWaitForAttackForPlayer>().EndLock();

                    gameObject.LocateMyFSM("Control").SetState("Recover");

                    Skill1End();

                    return;
                }

                gameObject.GetComponent<LockAngleWaitForAttackForPlayer>().LockOnceForPlayer(angle, 15f);

                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            });
            gameObject.LocateMyFSM("Control").GetState("Recover").AddMethod(() =>
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            });
        }
        public void Skill2Init()
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
                gameObject.GetComponent<AngleSystemForPlayer>().BossAngleRecover();

                if(HeroController.instance.transform.localScale.x > 0)
                {
                    gameObject.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                }
                if (HeroController.instance.transform.localScale.x < 0)
                {
                    gameObject.GetComponent<TeleportSystemForPlayer>().TeleportByAngle(225f, 3f);
                }
                else
                {
                    gameObject.GetComponent<TeleportSystemForPlayer>().TeleportByAngle(135f, 3f);
                }

                CounterBox.GetComponent<CounterDetect>().CounterOn();

            });
            gameObject.LocateMyFSM("Control").GetState("Counter End").AddMethod(() =>
            {
                CounterBox.GetComponent<CounterDetect>().CounterEnd();

                BigSlashControlForPlayer.skill2DamageFactor = 1;
                BigSlashControlForPlayer.skill2SlashDelayTime = 0.2f;;

                Skill12BigSlashSummon(time, false);
            });
            gameObject.LocateMyFSM("Control").GetState("CSlash").AddMethod(() =>
            {
                var dashBurst = gameObject.transform.Find("Dash Burst").gameObject;
                dashBurst.transform.localPosition += new Vector3(3, 0, 0);
                dashBurst.SetActive(true);

                BigSlashControlForPlayer.skill2DamageFactor = 4;
                BigSlashControlForPlayer.skill2SlashDelayTime = 0.7f;

                bigSlashEdgeFactor = 1f;
                bigSlashTimeFactor = 0.6f;
                skill12LoopTime = 0.05f;

                float Angle = 90f;
                if (gameObject.transform.localScale.x < 0)
                {
                    Angle = 270f;
                }
                gameObject.GetComponent<OneSlashAnyAngle>().On(Angle, 350);
                gameObject.GetComponent<OneSlashLightForPlayer>().Light();
                Skill12BigSlashSummon(time, true);

                AnimFreezeTime(0.45f, 0.035f);
            });
            gameObject.LocateMyFSM("Control").GetState("Counter Stance").GetAction<Wait>().time = 0.5f;
        }
        public void Skill3Init()
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
                gameObject.GetComponent<AngleSystemForPlayer>().BossAngleRecover();

                gameObject.GetComponent<LockAngleWaitForAttackForPlayer>().StartToLockForPlayer_Dstab(15);

                AnimFreezeTime(0.3f, 0.05f);


            });
            gameObject.LocateMyFSM("Control").GetState("SD5").AddMethod(() =>
            {
                //DstabBurstOn();

                gameObject.GetComponent<DtabForPlayer>().StompDetect(HeroController.instance.gameObject.transform.position.y - 4);

                gameObject.GetComponent<LockAngleWaitForAttackForPlayer>().EndLock();

                gameObject.transform.SetParent(null);


                WaveSummonForPlayer(gameObject.transform.position + new Vector3(1, -20, 6.2f), 16f);
                WaveSummonForPlayer(gameObject.transform.position + new Vector3(-1, -20, 6.2f), -16f);
            });
            gameObject.LocateMyFSM("Control").GetState("SL5").AddMethod(() =>
            {
                HeroController.instance.gameObject.LocateMyFSM("Nail Arts").SetState("Cyclone End");

                HeroController.instance.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;

                HeroController.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(ItemPool.Land, 1f);

            });
            gameObject.LocateMyFSM("Control").GetState("Focus Wait").AddMethod(() =>
            {
                Skill3End();

                gameObject.SetActive(false);

            });
            gameObject.LocateMyFSM("Control").GetState("SD5").GetAction<SetVelocity2d>().y = -250f;
            gameObject.LocateMyFSM("Control").GetState("TI5").AddMethod(() =>
            {
                HeroController.instance.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

                gameObject.GetComponent<TeleportSystemForPlayer>().TeleportByAngle(180f, 3f);
            });
        }
        public void Skill1()
        {
            GameObject closest = null;
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
                CameraControl.CamAway();
                HeroDisappear();
                gameObject.SetActive(true);
                gameObject.transform.SetParent(null);
                gameObject.LocateMyFSM("Control").SetState("SA2");
            }
        }
        public void Skill1End()
        {
            HeroAppear(1f);
            gameObject.LocateMyFSM("Control").SetState("Recover");
            gameObject.SetActive(false);
            gameObject.transform.SetParent(HeroController.instance.transform);
            gameObject.transform.localPosition = new Vector3(0, -100, 0);
        }

        public void Skill2(float timeFactor, float edgeFactor, float loopTime)
        {
            GameObject closest = null;
            float closestDistance = Mathf.Infinity;
            foreach (GameObject obj in FindObjectsOfType<GameObject>())
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
                CameraControl.CamAway();
                HeroDisappear();
                gameObject.SetActive(true);
                gameObject.transform.SetParent(null);
                gameObject.transform.position = HeroController.instance.transform.position;

                bigSlashEdgeFactor = edgeFactor;
                bigSlashTimeFactor = timeFactor;
                skill12LoopTime = loopTime;

                gameObject.LocateMyFSM("Control").SetState("Counter Antic");
            }
        }
        public void Skill2End()
        {
            HeroAppear(1f);
            gameObject.LocateMyFSM("Control").SetState("Recover");
            gameObject.SetActive(false);
            gameObject.transform.SetParent(HeroController.instance.transform);
            gameObject.transform.localPosition = new Vector3(0, -100, 0);
        }
        public void Skill3()
        {
            GameObject closest = null;
            float closestDistance = Mathf.Infinity;
            foreach (GameObject obj in FindObjectsOfType<GameObject>())
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
                CameraControl.CamAway();

                HeroDisappear();

                HeroController.instance.parryInvulnTimer = 99999f;
                HeroController.instance.gameObject.GetComponent<MeshRenderer>().enabled = false;

                var glow = Instantiate(INFINITY.ItemPool.Glow, HeroController.instance.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                glow.transform.localScale = new Vector3(1.4f, 1.3f, 0.5f) * 2;
                glow.SetActive(true);
                glow.AddComponent<INFINITY.DelayDestory>();
                glow.GetComponent<INFINITY.DelayDestory>().On(2f);
                INFINITY.CameraControl.CamAway();

                gameObject.SetActive(true);
                gameObject.LocateMyFSM("Control").SetState("TI5");
            }
        }
        public void Skill3End()
        {
            HeroAppear(1f);
            gameObject.LocateMyFSM("Control").SetState("Recover");
            gameObject.SetActive(false);
            gameObject.transform.SetParent(HeroController.instance.transform);
            gameObject.transform.localPosition = new Vector3(0, -100, 0);
        }
    }
}
