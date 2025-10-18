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
//using WavLib;
using static INFINITY.SceneControl;
using GlobalEnums;
using HKMirror.Hooks.OnHooks;
using UnityEngine.Rendering;
using INFINITY_FOR_PLAYER;
using WavLib;

namespace INFINITY
{
    public class INFINITY : Mod, IMod, Modding.ILogger, IGlobalSettings<Settings>, IMenuMod
    {
        public override string GetVersion()
        {
            return "0.0.11.0";
        }

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            List<IMenuMod.MenuEntry> menus = new List<IMenuMod.MenuEntry>();

            if (toggleButtonEntry != null)
            {
                menus.Add(toggleButtonEntry.Value);
            }
            menus.Add(new IMenuMod.MenuEntry
            {
                Name = OtherLanguage("去除「无极」造成的冻结帧", "Remove the frozen frames"),
                Description = OtherLanguage("拼刀不会冻结时间", "Parry will not freeze time."),
                Values = new string[]
                {
                    Language.Language.Get("MOH_ON", "MainMenu"),
                    Language.Language.Get("MOH_OFF", "MainMenu")
                },
                Loader = (() => (!settings_.freezeOn) ? 1 : 0),
                Saver = delegate (int i)
                {
                    settings_.freezeOn = (i == 0);
                }
            });
            menus.Add(new IMenuMod.MenuEntry
            {
                Name = OtherLanguage("开启残影", "Activate the lingering shadow"),
                Description = OtherLanguage("调整以获得更合适的观感", "Adjust to achieve a more suitable appearance."),
                Values = new string[]
                {
                    Language.Language.Get("MOH_ON", "MainMenu"),
                    Language.Language.Get("MOH_OFF", "MainMenu")
                },
                Loader = (() => (!settings_.afterimageOn) ? 1 : 0),
                Saver = delegate (int i)
                {
                    settings_.afterimageOn = (i == 0);
                }
            });
            menus.Add(new IMenuMod.MenuEntry
            {
                Name = OtherLanguage("启用「无极」权柄", "Activate the power of「INFINITY」"),
                Description = OtherLanguage("获得后才能使用", "It can only be used after obtained."),
                Values = new string[]
                {
                    Language.Language.Get("MOH_ON", "MainMenu"),
                    Language.Language.Get("MOH_OFF", "MainMenu")
                },
                Loader = (() => (!settings_.skillOn) ? 1 : 0),
                Saver = delegate (int i)
                {
                    settings_.skillOn = (i == 0);
                }
            });

            return menus;
        }

        public static Settings settings_ = new Settings();
        public bool ToggleButtonInsideMenu => true;
        public void OnLoadGlobal(Settings settings) => settings_ = settings;
        public Settings OnSaveGlobal() => settings_;
        public static string OtherLanguage(string chinese, string english)
        {
            if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
            {
                return chinese;
            }
            return english;
        }

        public static System.Random random = new System.Random();
        double R => random.NextDouble();
        int choice = 0;
        static public double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }
        static public double DegreesToRadians(double Degrees)
        {
            return Degrees * (Math.PI / 180);
        }

        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("GG_Workshop","GG_Statue_Radiance/Spotlight/Glow Response statue_beam/light_beam_particles 3"),
                ("GG_Workshop","gg_mantis_cage_0002_1 (3)"),
                ("GG_Workshop","gg_radiant_gate (14)"),
                ("GG_Radiance","Boss Control"),
                ("GG_Radiance","Boss Control/White Fader"),
                ("GG_Radiance","Boss Control/Boss Title"),
                ("GG_Soul_Master","Mage Lord"),
                ("GG_Traitor_Lord","Battle Scene/Wave 3/Mantis Traitor Lord"),
                ("GG_Hollow_Knight","Battle Scene/HK Prime/Slashes/Slash1"),
                ("GG_Hollow_Knight","Battle Scene/HK Prime/Slashes/Slash1"),
                ("GG_Hollow_Knight","Battle Scene/HK Prime"),
                ("GG_Hollow_Knight","final_boss_room_0016_tall_symb3 (3)"),
                ("GG_Hollow_Knight","final_boss_room_0016_tall_symb3"),
                ("GG_Hollow_Knight","final_boss_room_0016_tall_symb3 (1)"),
                ("GG_Hollow_Knight","final_boss_room_0016_tall_symb3 (2)"),
                ("GG_Ghost_Markoth_V","Spike Collider"),
                ("Knight_Pickup","Knight"),
                
                ("Crossroads_47","_Scenery/station_pole/Stag_Pole_Tall_Break (2)"),
                ("Crossroads_13","_Scenery/lamp_02"),
                ("Crossroads_19","_Scenery/lamp_01"),
                ("Crossroads_04","_Scenery/vil_lamp_08"),
                ("Crossroads_04","_Scenery/vil_lamp_06"),
                ("Ruins1_28","ruin_lamp_01 (6)"),
                ("Ruins1_27","_Scenery/ruind_fountain"),
                ("Ruins1_27","Fountain Inspect"),
                ("RestingGrounds_04","Binding Shield Activate/Binding Shield Statues"),
            };
        }
        public static int HardMode = 1;
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            ItemPool.BGM1 = LoadAudioClip("INFINITY.Resources.BGM1.wav");
            ItemPool.BGM2 = LoadAudioClip("INFINITY.Resources.BGM2.wav");
            ItemPool.BGM3 = LoadAudioClip("INFINITY.Resources.BGM3.wav");

            var radiance = preloadedObjects["GG_Radiance"]["Boss Control"].transform.Find("Absolute Radiance").gameObject;

            ItemPool.Explode = radiance.LocateMyFSM("Control").GetState("Final Explode").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;

            var beam1 = preloadedObjects["GG_Hollow_Knight"]["Battle Scene/HK Prime/Slashes/Slash1"].gameObject;
            beam1.GetComponent<PolygonCollider2D>().enabled = true;
            beam1.GetComponent<PolygonCollider2D>().points = new Vector2[] {new Vector2(-2.5477f, -0.2735f), new Vector2(-1.6684f, 0.2882f), new Vector2(-0.57f, 0.717f), new Vector2(0.785f, 0.8595f), new Vector2(-0.2458f, -1.2983f), new Vector2(-1.0813f, -1.2427f), new Vector2(-2.0077f, -1.101f), new Vector2(-2.5024f, -0.8201f) };
            beam1.transform.localPosition = new Vector3(0, 0, 0);
            beam1.transform.localScale = new Vector3(1, 1, 1);
            ItemPool.BeamCollider1 = beam1;
            var hk = preloadedObjects["GG_Hollow_Knight"]["Battle Scene/HK Prime"].gameObject;
            var nail = hk.LocateMyFSM("Control").GetState("SmallShot LowHigh").GetAction<FlingObjectsFromGlobalPoolTime>().gameObject.Value;
            var glow = nail.transform.Find("Glow").gameObject;
            var line = nail.transform.Find("Beam").gameObject;
            var plume = hk.LocateMyFSM("Control").GetState("Plume Gen").GetAction<SpawnObjectFromGlobalPool>().gameObject.Value as GameObject;

            var soul = preloadedObjects["GG_Soul_Master"]["Mage Lord"].gameObject;
            var audio1 = soul.LocateMyFSM("Mage Lord").GetState("Quake Land").GetAction<AudioPlaySimple>().oneShotClip;
            ItemPool.Land = audio1.Value as AudioClip;

            ItemPool.Scene_StatueSymb1_prefab = preloadedObjects["GG_Hollow_Knight"]["final_boss_room_0016_tall_symb3 (3)"].gameObject;
            ItemPool.Scene_StatueSymb2_prefab = preloadedObjects["GG_Hollow_Knight"]["final_boss_room_0016_tall_symb3"].gameObject;
            ItemPool.Scene_StatueSymb3_prefab = preloadedObjects["GG_Hollow_Knight"]["final_boss_room_0016_tall_symb3 (1)"].gameObject;
            ItemPool.Scene_StatueSymb4_prefab = preloadedObjects["GG_Hollow_Knight"]["final_boss_room_0016_tall_symb3 (2)"].gameObject;

            var tl = preloadedObjects["GG_Traitor_Lord"]["Battle Scene/Wave 3/Mantis Traitor Lord"].gameObject;
            var wave = tl.LocateMyFSM("Mantis").GetState("Waves").GetAction<SpawnObjectFromGlobalPool>(0).gameObject.Value;
            wave.GetComponent<AutoRecycleSelf>().timeToWait = 2.3f;
            wave.transform.Find("slash_core").gameObject.GetComponent<Animator>().cullingMode = AnimatorCullingMode.AlwaysAnimate;
            wave.transform.Find("Grass").Recycle();
            ItemPool.Wave = wave;

            var spike = preloadedObjects["GG_Ghost_Markoth_V"]["Spike Collider"].gameObject;
            spike.GetComponent<PolygonCollider2D>().points = new Vector2[] { new Vector2(0, 0), new Vector2(5, 0), new Vector2(5, -5), new Vector2(0, -5) };
            spike.GetComponent<DamageHero>().hazardType = 5;
            spike.GetComponent<DamageHero>().damageDealt = 1;
            ItemPool.Spike = spike;

            var bg = preloadedObjects["GG_Radiance"]["Boss Control"].gameObject;
            for(int i = 1;i < bg.transform.childCount;i++)
            {
                var obj = bg.transform.GetChild(i);
                if(obj.name == "White Fader")
                {
                    obj.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
                    obj.localScale = new Vector3(obj.localScale.x * 10f, obj.localScale.y * 10f, obj.localScale.z);
                }
                else if(obj.name == "Boss Title")
                {
                    obj.gameObject.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1);
                    obj.gameObject.transform.Find("Boss Title (1)").GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1);
                }
                else
                {
                    obj.gameObject.Recycle();
                }
            }
            bg.AddComponent<BossTitleControl>();
            //bg.LocateMyFSM("Control").ChangeTransition("Wait for Hero Pos", "FINISHED", "Knight Ready");
            bg.LocateMyFSM("Control").GetState("Wait for Hero Pos").AddMethod(() =>
            {
                bg.LocateMyFSM("Control").SetState("Battle Start");
            });
            bg.LocateMyFSM("Control").GetState("Battle Start").RemoveAction(0);
            bg.LocateMyFSM("Control").GetState("Battle Start").RemoveAction(1);
            bg.LocateMyFSM("Control").GetState("Battle Start").RemoveAction(2);
            bg.LocateMyFSM("Control").GetState("Battle Start").RemoveAction(3);
            bg.LocateMyFSM("Control").GetState("Battle Start").RemoveAction(4);
            bg.LocateMyFSM("Control").GetState("Battle Start").RemoveAction(5);
            //bg.LocateMyFSM("Control").GetState("Title Up").RemoveAction(3);

            MusicCue obj1 = FsmUtil.GetAction<ApplyMusicCue>(bg.LocateMyFSM("Control"), "Title Up", 3).musicCue.Value as MusicCue;

            MusicCue.MusicChannelInfo musicChannelInfo = ReflectionHelper.GetField<MusicCue, MusicCue.MusicChannelInfo[]>(obj1, "channelInfos")[0];

            ReflectionHelper.SetField(musicChannelInfo, "clip", ItemPool.BGM3);

            bg.LocateMyFSM("Control").GetState("Title Up").GetAction<Wait>().time = 1.5f;
            bg.SetActive(true);

            bg.GetComponent<AudioSource>().maxDistance = 99999f;
            bg.GetComponent<AudioSource>().minDistance = 99999f;

            ItemPool.BossTitle = bg.gameObject;
            
            var fence = preloadedObjects["GG_Workshop"]["gg_mantis_cage_0002_1 (3)"].gameObject;
            fence.GetComponent<SpriteRenderer>().color = new Color(0.2128f, 0.2128f, 0.3004f, 1f);
            ItemPool.Scene_Fence = fence;

            var gate = preloadedObjects["GG_Workshop"]["gg_radiant_gate (14)"].gameObject;
            gate.GetComponent<SpriteRenderer>().color = new Color(0.2128f, 0.2128f, 0.3004f, 1f);
            ItemPool.Scene_Gate = gate;

            var shield = preloadedObjects["RestingGrounds_04"]["Binding Shield Activate/Binding Shield Statues"].gameObject;
            shield.transform.localScale = new Vector3(1.4f, 1.4f, 1.1929f);
            for (int i = 1;i < shield.transform.childCount;i++)
            {
                var obj = shield.transform.GetChild(i);
                obj.gameObject.SetActive(true);
                if(obj.name.Contains("Collider"))
                {
                    obj.Recycle();
                }
                if(obj.name.Contains("bit"))
                {
                    obj.localPosition += new Vector3(0.1f, 0f, 0f);
                }
            }
            ItemPool.Scene_Shield = shield;

            var light1 = preloadedObjects["Crossroads_47"]["_Scenery/station_pole/Stag_Pole_Tall_Break (2)"].gameObject;
            ItemPool.Scene_Light_1 = light1;
            ItemPool.Scene_Bugs_Pt = light1.transform.Find("lamp_bug_escape (7)").gameObject;

            var light2 = preloadedObjects["Crossroads_13"]["_Scenery/lamp_02"].gameObject;
            ItemPool.Scene_Light_2 = light2;

            var light3 = preloadedObjects["Crossroads_19"]["_Scenery/lamp_01"].gameObject;
            ItemPool.Scene_Light_3 = light3; 

            var light4 = preloadedObjects["Ruins1_28"]["ruin_lamp_01 (6)"].gameObject;
            ItemPool.Scene_Light_4 = light4;
            
            var light5 = preloadedObjects["Crossroads_19"]["_Scenery/lamp_01"].gameObject;
            ItemPool.Scene_Light_5 = light5; 

            var light6 = preloadedObjects["Ruins1_28"]["ruin_lamp_01 (6)"].gameObject;
            ItemPool.Scene_Light_6 = light6; 

            var fountain = preloadedObjects["Ruins1_27"]["_Scenery/ruind_fountain"].gameObject;
            fountain.transform.Find("fountain_new").localPosition = new Vector3(-42.3f, 13.2491f, -2.881f);
            fountain.transform.Find("ruin_layered_0091_f").gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0.8f, 1f);
            var inspect = preloadedObjects["Ruins1_27"]["Fountain Inspect"].gameObject;
            inspect.LocateMyFSM("Conversation Control").GetState("Up Pause").RemoveAction(0);
            inspect.LocateMyFSM("Conversation Control").GetState("Up Pause").RemoveAction(1);
            inspect.transform.SetParent(fountain.transform);
            inspect.transform.Find("New Fountain Prompt").transform.localPosition = new Vector3(-41.1032f, -7.79f, -0.006f);
            var camLock1 = inspect.transform.Find("CamLock").gameObject;
            camLock1.GetComponent<CameraLockArea>().cameraXMax = 10000;
            camLock1.GetComponent<CameraLockArea>().cameraYMax = 15;
            camLock1.GetComponent<CameraLockArea>().cameraYMin = 15;
            var new_fountain = fountain.transform.Find("fountain_new").gameObject;
            for (int i = 0;i < new_fountain.transform.childCount;i++)
            {
                var obj = new_fountain.transform.GetChild(i);
                if(obj.name != "_0083_fountain")
                {
                    obj.gameObject.SetActive(false);
                }
                else
                {
                    obj.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0.8f, 1f);
                }
            }

            ItemPool.Scene_Fountain = fountain;
            ItemPool.Scene_Fountain_Inspect = inspect;

            ItemPool.Glow = glow;
            ItemPool.Line = line;
            ItemPool.HK = hk;
            ItemPool.Nail = nail;
            ItemPool.Plume = plume;
            /*
            var knight = preloadedObjects["Knight_Pickup"]["Knight"].gameObject;
            var fsm = knight.LocateMyFSM("Spell Control");
            var ball = fsm.GetState("Fireball 2").GetAction<SpawnObjectFromGlobalPool>().gameObject.Value;
            ball.LocateMyFSM("Fireball Control").GetState("Init").RemoveAction<SetVelocity2d>();
            ball.LocateMyFSM("Fireball Control").ChangeTransition("Idle", "FINISHED", "Recycle");
            //ball.GetComponent<BoxCollider2D>().size = new Vector2(5, 1);
            //ball.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0.05f);
            ball.GetComponent<BoxCollider2D>().enabled = false;
            var image = LoadPng("INFINITY.Resources.Spell.png");
            ball.GetComponent<MeshRenderer>().material.mainTexture = image; 
            ball.transform.localScale *= 4f;
            ItemPool.SlashLight = ball;
            */
            On.GameManager.LoadGame += GameManager_LoadGame;
        }

        private void GameManager_LoadGame(On.GameManager.orig_LoadGame orig, GameManager self, int saveSlot, Action<bool> callback)
        {
            if(INFINITY.settings_.on)
            {
                ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
                ModHooks.LanguageGetHook += ModHooks_LanguageGetHook_Workshop;
                On.PlayMakerFSM.Start += PlayMakerFSM_Start;
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
                On.GameManager.FreezeMoment_int += GameManager_FreezeMoment_int;

            }
            else
            {
                ModHooks.HeroUpdateHook -= ModHooks_HeroUpdateHook;
                ModHooks.LanguageGetHook -= ModHooks_LanguageGetHook_Workshop;
                On.PlayMakerFSM.Start -= PlayMakerFSM_Start;
                UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
                On.GameManager.FreezeMoment_int -= GameManager_FreezeMoment_int;
            }

            orig(self, saveSlot, callback);
        }


        public static void GameManager_FreezeMoment_int(On.GameManager.orig_FreezeMoment_int orig, GameManager self, int type)
        {
            if(INFINITY.settings_.freezeOn)
            {

            }
            else
            {
                if (type == 0)
                {
                    self.StartCoroutine(self.FreezeMoment(0.01f, 0.35f, 0.1f, 0f));
                }
                else if (type == 1)
                {
                    self.StartCoroutine(self.FreezeMoment(0.04f, 0.03f, 0.04f, 0f));
                }
                else if (type == 2)
                {
                    self.StartCoroutine(self.FreezeMoment(0.25f, 2f, 0.25f, 0.15f));
                }
                else if (type == 3)
                {
                    self.StartCoroutine(self.FreezeMoment(0.01f, 0.25f, 0.1f, 0f));
                }
                if (type == 4)
                {
                    self.StartCoroutine(self.FreezeMoment(0.01f, 0.25f, 0.1f, 0f));
                }
                if (type == 5)
                {
                    self.StartCoroutine(self.FreezeMoment(0.01f, 0.25f, 0.1f, 0f));
                }
                if (type == 6)
                {
                    self.StartCoroutine(self.FreezeMoment(0.01f, 0.5f, 0.1f, 0f));
                }
            }
        }
        public static void GameManager_FreezeMoment_int_Recover(On.GameManager.orig_FreezeMoment_int orig, GameManager self, int type)
        {
            orig(self, type);
        }

        public class TakeDamageDelayCheck : MonoBehaviour
        {
            public float delayTime = 0.08f;
            public bool canTakeDamage = false;
            public GameObject go = null;
            public CollisionSide damageSide;
            public int damageAmount;
            public int hazardType;
            public void Check()
            {
                Invoke("TakeDamage", delayTime);
            }
            public void Update()
            {
                if (HeroController.instance.parryInvulnTimer > 0)
                {
                    CancelInvoke("TakeDamage");
                }
            }
            public void TakeDamage()
            {
                canTakeDamage = true;
                HeroController.instance.TakeDamage(go, damageSide, damageAmount, hazardType);
            }
        }
        
        public static void HeroController_TakeDamage(On.HeroController.orig_TakeDamage orig, HeroController self, GameObject go, CollisionSide damageSide, int damageAmount, int hazardType)
        {
            bool flag1 = HeroController.instance.gameObject.GetComponent<TakeDamageDelayCheck>().canTakeDamage;
            if(flag1)
            {
                orig(self, go, damageSide, damageAmount, hazardType);
                HeroController.instance.gameObject.GetComponent<TakeDamageDelayCheck>().canTakeDamage = false;
            }
            else
            {
                var check = HeroController.instance.gameObject.GetComponent<TakeDamageDelayCheck>();
                check.go = go;
                check.damageSide = damageSide;
                check.damageAmount = damageAmount;
                check.hazardType = hazardType;
                check.Check();
            }

        }

        /*
        public static AudioClip LoadAudioClip(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            WavData wavData = new WavData();
            wavData.Parse(stream, null);
            stream.Close();
            float[] samples = wavData.GetSamples();
            AudioClip audioClip = AudioClip.Create("audio", samples.Length / wavData.FormatChunk.NumChannels, wavData.FormatChunk.NumChannels, (int)wavData.FormatChunk.SampleRate, false);
            audioClip.SetData(samples, 0);
            return audioClip;
        }
        */
        public static AudioClip LoadAudioClip(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            WavData wavData = new WavData();
            wavData.Parse(stream, null);
            stream.Close();
            float[] samples = wavData.GetSamples();
            AudioClip audioClip = AudioClip.Create("audio", samples.Length / wavData.FormatChunk.NumChannels, wavData.FormatChunk.NumChannels, (int)wavData.FormatChunk.SampleRate, false);
            audioClip.SetData(samples, 0);
            return audioClip;
        }
        public static Texture2D LoadPng(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            Texture2D texture2D;
            MemoryStream memoryStream = new MemoryStream((int)stream.Length);
            stream.CopyTo(memoryStream);
            stream.Close();
            var bytes = memoryStream.ToArray();
            memoryStream.Close();
            texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(bytes, true);
            return texture2D;
        }
        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "GG_Hollow_Knight")
            {
                /*
                GameObject[] all = GameObject.FindObjectsOfType<GameObject>();
                foreach (var obj in all)
                {
                    if (obj.name.Contains("SceneBorder") || obj.name.Contains("clouds") || obj.name.Contains("Lock Zone") || obj.name.Contains("HK_glow_wall") || obj.name.Contains("GG_Arena_PrefabGG") || obj.name.Contains("Edge"))
                    {
                        obj.Recycle();
                    }
                    if (obj.name.Contains("Death Break Chains"))
                    {
                        obj.transform.localScale *= 2;
                    }
                    if (obj.name.Contains("Battle"))
                    {
                        var godseeker = obj.transform.Find("Godseeker Crowd").gameObject;
                        if(godseeker != null)
                        {
                            godseeker.Recycle();
                        }
                    }
                    if (obj.name.Contains("TileMap Render Data"))
                    {
                        var sceneMap = obj.transform.Find("Scenemap").gameObject;
                        var chunk0 = sceneMap.transform.Find("Chunk 0 0").gameObject;
                        var chunk1 = sceneMap.transform.Find("Chunk 0 1").gameObject;
                        chunk0.Recycle();
                        EdgeCollider2D[] edgeCollider2Ds = chunk1.GetComponents<EdgeCollider2D>();
                        foreach(var edgeCollider2D in edgeCollider2Ds)
                        {
                            if(edgeCollider2D.edgeCount == 5)
                            {
                                HeroController.instance.transform.position += new Vector3(0f, 0.5f, 0f);
                                Vector2[] vectorArray = { new Vector2(-200, 5), new Vector2(200, 5), new Vector2(200, -5), new Vector2(-200, -5) };
                                edgeCollider2D.points = vectorArray;
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
                    }
                }
                */
            }
        }
        public static string ModHooks_LanguageGetHook(string key, string sheet, string text)
        {
            if (INFINITY.settings_.on)
            {
                if (key == "FOUNTAIN_PLAQUE_TOP")
                {
                    if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                    {
                        text = "获得";
                    }
                    else
                    {
                        text = "obtained";
                    }
                }
                if (key == "FOUNTAIN_PLAQUE_MAIN")
                {
                    if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                    {
                        text = "「无极」权柄";
                    }
                    else
                    {
                        text = "power of「INFINITY」";
                    }
                }
                if (key == "FOUNTAIN_PLAQUE_DESC")
                {
                    if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                    {
                        text = "来自「无极」的力量，可在设置里关闭。\n开启时获得更大视野，剑技将被替换。";
                    }
                    else
                    {
                        text = "You can use the power from 「INFINITY」. It can be turned off in the settings. \nGain a wider field of vision, And the nail arts will be replaced.";
                    }
                }
                if (key == "ABSOLUTE_RADIANCE_SUPER" && sheet == "Titles")
                {
                    if(HardMode == 1)
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "力量解放";
                        }
                        else
                        {
                            text = "BOUNDLESS";
                        }
                    }
                    else
                    {
                        text = "";
                    }
                }
                if (key == "ABSOLUTE_RADIANCE_MAIN" && sheet == "Titles")
                {
                    if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                    {
                        text = "无极";
                    }
                    else
                    {
                        text = "INFINITY";
                    }
                }
                return text;
            }
            else
                return @text;
        }
        public static string ModHooks_LanguageGetHook_Workshop(string key, string sheet, string text)
        {
            if (INFINITY.settings_.on == true)
            {
                if (HardMode == 0)
                {
                    if (key == "NAME_HK_PRIME" && sheet == "CP3")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "无极";
                        }
                        else
                        {
                            text = "INFINITY";
                        }
                    }
                    if (key == "GG_S_HK" && sheet == "CP3")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "时间尽头的舞者";
                        }
                        else
                        {
                            text = "The Dancer at the End of Time.";
                        }
                    }
                    if (key == "ABSOLUTE_RADIANCE_MAIN")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "无极";
                        }
                        else
                        {
                            text = "INFINITY";
                        }
                    }
                    return text;
                }
                else
                {
                    if (key == "NAME_HK_PRIME" && sheet == "CP3")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "无极";
                        }
                        else
                        {
                            text = "INFINITY";
                        }
                    }
                    if (key == "GG_S_HK" && sheet == "CP3")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "时间尽头的舞者\n[力量解放]";
                        }
                        else
                        {
                            text = "The Dancer at the End of Time.\n[Power Unbounded]";
                        }
                    }
                    if (key == "ABSOLUTE_RADIANCE_MAIN")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "无极";
                        }
                        else
                        {
                            text = "INFINITY";
                        }
                    }
                    return text;
                }
            }
            else
                return @text;
        }
        public static string ModHooks_LanguageGetHook_Recover(string key, string sheet, string text)
        {
            if (INFINITY.settings_.on == true)
            {
                if (key == "ABSOLUTE_RADIANCE_MAIN")
                {
                    if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                    {
                        text = "无上辐光";
                    }
                    else
                    {
                        text = "RADIANCE";
                    }
                }
                return text;
            }
            else
                return @text;
        }
        public static void ModHooks_HeroUpdateHook()
        {
            var hero = HeroController.instance.gameObject;
            var gc = GameCameras.instance.gameObject;
            var cp = gc.transform.Find("CameraParent");
            var cam = cp.transform.Find("tk2dCamera").gameObject;
            if (hero != null && cam != null)
            {
                HeroController.instance.gameObject.AddComponent<SceneSwitchDetector>();
                HeroController.instance.gameObject.AddComponent<SummonMoon>();
                hero.AddComponent<TakeDamageDelayCheck>();
                cam.AddComponent<CamControl>();
                ModHooks.HeroUpdateHook -= ModHooks_HeroUpdateHook;
            }
        }
        public class SummonMoon : MonoBehaviour
        {
            public GameObject HK;
            public void Start()
            {
                var sa = HeroController.instance.gameObject.LocateMyFSM("Spell Control").CopyState("Scream Antic1", "SA");
                sa.RemoveAction(1);
                sa.RemoveAction(2);
                var sb = HeroController.instance.gameObject.LocateMyFSM("Spell Control").CopyState("Scream Burst 1", "SB");
                sb.RemoveAction(0);
                sb.RemoveAction(1);
                sb.RemoveAction(2);
                sb.RemoveAction(3);
                sb.RemoveAction(4);
                sb.RemoveAction(5);
                sb.RemoveAction(6);
                sb.RemoveAction(7);
                sb.RemoveAction(8);
                var er = HeroController.instance.gameObject.LocateMyFSM("Spell Control").CopyState("End Roar", "ER");
                HeroController.instance.gameObject.LocateMyFSM("Spell Control").ChangeTransition("SA", "FINISHED", "ER");
                HeroController.instance.gameObject.LocateMyFSM("Spell Control").ChangeTransition("SB", "FINISHED", "ER");
                HeroController.instance.gameObject.LocateMyFSM("Spell Control").ChangeTransition("ER", "FINISHED", "SA");
                sa.RemoveAction(1);
                sa.RemoveAction(2);
                sa.RemoveAction(6);
                sb.RemoveAction(0);
                sb.RemoveAction(1);
                sb.RemoveAction(2);
                sb.RemoveAction(3);
                sb.RemoveAction(4);
                sb.RemoveAction(5);
                sb.RemoveAction(6);
                sb.RemoveAction(7);
                sb.RemoveAction(8);
                sb.RemoveAction(9);
                er.RemoveAction(1);

                HK = Instantiate(ItemPool.HK, HeroController.instance.transform);

                HK.transform.Find("Colliders").gameObject.Recycle();

                HK.GetComponent<ConstrainPosition>().enabled = false;


                var heroFsm = HeroController.instance.gameObject.LocateMyFSM("Nail Arts");

                heroFsm.GetState("Flash").AddMethod(() =>
                {
                    if (settings_.skillGot && settings_.skillOn && ItemPool.INFINITY_FOR_PLAYER)
                    {
                        ItemPool.INFINITY_FOR_PLAYER.GetComponent<SkillsForPlayer>().Skill3();
                    }
                });
                heroFsm.GetState("Flash 2").AddMethod(() =>
                {
                    if(settings_.skillGot && settings_.skillOn && ItemPool.INFINITY_FOR_PLAYER)
                    {
                        HeroController.instance.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        ItemPool.INFINITY_FOR_PLAYER.GetComponent<SkillsForPlayer>().Skill2(3f, 0.225f, 0.2f);
                    }
                });
                heroFsm.GetState("DSlash Start").AddMethod(() =>
                {
                    if(settings_.skillGot && settings_.skillOn && ItemPool.INFINITY_FOR_PLAYER)
                    {
                        HeroController.instance.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        ItemPool.INFINITY_FOR_PLAYER.GetComponent<SkillsForPlayer>().Skill1();
                    }
                });
                HK.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Right X").Value = 10000f;
                HK.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Left X").Value = -10000f;
                HK.LocateMyFSM("Control").FsmVariables.FindFsmFloat("TeleRange Max").Value = 10000f;
                HK.LocateMyFSM("Control").FsmVariables.FindFsmFloat("TeleRange Min").Value = -10000f;

                HK.LocateMyFSM("Control").CopyState("Tele In", "TI");
                HK.LocateMyFSM("Control").CopyState("Evade Antic", "EA");
                HK.LocateMyFSM("Control").ChangeTransition("TI", "FINISHED", "EA");

                HK.LocateMyFSM("Control").ChangeTransition("EvadeRecover", "FINISHED", "Slash Distance Check");
                HK.LocateMyFSM("Control").ChangeTransition("Tele Out", "FINISHED", "TI");
                HK.LocateMyFSM("Control").GetState("TI").GetAction<SetPosition>().x = HeroController.instance.transform.position.x + 8f;
                HK.LocateMyFSM("Control").GetState("Slash1").RemoveAction<SetVelocity2d>();
                HK.LocateMyFSM("Control").GetState("Slash1").RemoveAction(4);
                HK.LocateMyFSM("Control").GetState("Slash1 Recover").RemoveAction<SetVelocity2d>();
                HK.LocateMyFSM("Control").GetState("Slash 2").RemoveAction<SetVelocity2d>();
                HK.LocateMyFSM("Control").GetState("Slash 2").RemoveAction(4);
                HK.LocateMyFSM("Control").GetState("Slash2 Recover").RemoveAction<SetVelocity2d>();
                HK.LocateMyFSM("Control").GetState("Slash 3").RemoveAction<SetVelocity2d>();
                HK.LocateMyFSM("Control").GetState("Slash 3").RemoveAction(3);
                HK.LocateMyFSM("Control").GetState("Slash3 Recover").RemoveAction<SetVelocity2d>();

                HK.LocateMyFSM("Control").ChangeTransition("Set Phase HP", "FINISHED", "Init");
                HK.LocateMyFSM("Control").ChangeTransition("Init", "FINISHED", "Intro Roar End");

                HK.transform.localPosition = new Vector3(0, -100, 0);
                HK.SetActive(true);

                Invoke("Re", 0.5f);

                HK.LocateMyFSM("Control").gameObject.AddComponent<INFINITY_FOR_PLAYER.SkillsForPlayer>();

                ItemPool.INFINITY_FOR_PLAYER = HK;
            }
            void Re()
            {
                HK.SetActive(false);
            }
        }
        public static GameObject BOSS;
        public static GameObject BeamFly;
        private void PlayMakerFSM_Start(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self)
        {
            orig(self);
            if (INFINITY.settings_.on == true)
            {
                if (self.FsmName == "Spell Control" && self.gameObject.name == "Knight")
                {
                    self.ChangeTransition("Has Fireball?", "CAST", "Inactive");
                }
                if (self.FsmName == "corpse" && self.gameObject.name == "Corpse HK Prime(Clone)")
                {
                    self.GetState("Land").GetAction<Wait>().time = 10000;

                    self.GetState("Init").AddMethod(()=>
                    {
                        if (fountain.transform.Find("Fountain Inspect(Clone)"))
                        {
                            if(fountain.transform.Find("Fountain Inspect(Clone)").gameObject.LocateMyFSM("Conversation Control").GetState("Up Pause").GetAction<TransitionToAudioSnapshot>() != null)
                            {
                                fountain.transform.Find("Fountain Inspect(Clone)").gameObject.LocateMyFSM("Conversation Control").GetState("Up Pause").RemoveAction<TransitionToAudioSnapshot>();
                            }
                            
                            fountain.transform.Find("Fountain Inspect(Clone)").gameObject.SetActive(true);

                            ItemPool.BossTitle.GetComponent<BossTitleControl>().BattleOver();
                        }
                        self.SetState("Land");

                        self.gameObject.transform.position = fountain.transform.position + new Vector3(-8f, 2, -0.05f);

                        self.gameObject.AddComponent<CorpseFlash>();


                        self.gameObject.transform.localScale = new Vector3(1, 1, self.gameObject.transform.localScale.z);

                        self.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);


                });
                }
                if (self.FsmName == "Control" && self.gameObject.name == "Grubberfly BeamR")
                {
                    BeamFly = self.gameObject;
                }
                if (self.FsmName == "Control" && self.gameObject.name == "HK Prime Blast")
                {
                    self.ChangeTransition("Wait", "FINISHED", "Blast");
                }
                if (self.FsmName == "Control" && self.gameObject.name == "HK Prime")
                {
                    BOSS = self.gameObject;


                    if (self.gameObject.GetComponent<Skills>() == null)
                    {
                        self.gameObject.AddComponent<Skills>();
                    }
                    if (self.gameObject.GetComponent<PhaseControl>() == null)
                    {
                        self.gameObject.AddComponent<PhaseControl>();
                    }

                    self.FsmVariables.FindFsmFloat("Right X").Value = 10000f;
                    self.FsmVariables.FindFsmFloat("Left X").Value = -10000f;
                    self.FsmVariables.FindFsmFloat("TeleRange Max").Value = 10000f;
                    self.FsmVariables.FindFsmFloat("TeleRange Min").Value = -10000f;

                    self.GetState("Slash1").RemoveAction<SetVelocity2d>();
                    self.GetState("Slash1 Recover").RemoveAction<SetVelocity2d>();
                    self.GetState("Slash 2").RemoveAction<SetVelocity2d>();
                    self.GetState("Slash2 Recover").RemoveAction<SetVelocity2d>();
                    self.GetState("Slash 3").RemoveAction<SetVelocity2d>();
                    self.GetState("Slash3 Recover").RemoveAction<SetVelocity2d>();
                    self.ChangeTransition("Pause", "FINISHED", "Init");
                    self.GetState("Init").RemoveAction(2);
                    self.GetState("Counter Dir").RemoveAction(0);
                    self.GetState("Counter Dir").RemoveAction(2);
                    self.GetState("Init").AddMethod(() =>
                    {
                        var colliders = self.transform.Find("Colliders").gameObject;
                        for (int i = 0;i< colliders.transform.childCount;i++)
                        {
                            var obj = colliders.transform.GetChild(i).gameObject;
                            if(obj.GetComponent<DamageHero>() != null && obj.name != "Dstab Damage")
                            {
                                obj.GetComponent<DamageHero>().damageDealt = 0;
                            }
                            else if(obj.name == "Dstab Damage")
                            {
                                obj.GetComponent<BoxCollider2D>().size *= 2;
                            }
                        }
                        var slashes = self.transform.Find("Slashes").gameObject;
                        for (int i = 0;i< colliders.transform.childCount;i++)
                        {
                            var obj = colliders.transform.GetChild(i).gameObject;
                            if(obj.GetComponent<DamageHero>() != null && obj.name != "Dash Stab")
                            {
                                obj.GetComponent<DamageHero>().damageDealt = 0;
                            }
                        }
                    });
                }
            }
        }
    }
}
