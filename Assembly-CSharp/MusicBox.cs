// Decompiled with JetBrains decompiler
// Type: MusicBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using AOT;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

#nullable disable
public class MusicBox : MonoBehaviour
{
  private const int MUSIC_ZONES_SIZE = 3;
  private const int IDLE_SOUND_TIMER_MIN = 5;
  private const int IDLE_SOUND_TIMER_MAX = 12;
  public static MusicBox inst;
  private readonly HashSet<string> _flags_to_enable = new HashSet<string>();
  private EventInstance _music_event;
  internal MusicBoxDebug debug_box;
  private float _timer;
  private const float INTERVAL_UPDATE = 1f;
  public static bool music_on = true;
  public static bool sounds_on = true;
  public static bool debug_sounds = true;
  private VCA _vca_sound_effects;
  private VCA _vca_music;
  private VCA _vca_ui;
  private Bus _bus_master;
  private Bus _bus_idle;
  private float _volume_idle = 1f;
  private EVENT_CALLBACK _music_callback;
  private TimelineInfo _timeline_info;
  private GCHandle _timeline_handle;
  public static bool new_world_on_start_played = false;
  private readonly Dictionary<string, EventInstance> _environment_sounds = new Dictionary<string, EventInstance>();
  private readonly Dictionary<string, EventInstance> _drawing_sounds = new Dictionary<string, EventInstance>();
  private static readonly Dictionary<string, bool> _events_cache = new Dictionary<string, bool>();
  private static readonly Dictionary<string, GUID> _events_guids = new Dictionary<string, GUID>();
  private static GameObject _sound_object;
  private int _tiles_sand;
  private int _tiles_shallow_water;
  public MusicState music_state;
  private MusicBoxLibrary _lib;
  public MusicBoxIdle idle;
  private GameObject _camera_listener;
  private bool _created;

  private static FMOD.Studio.System _studio_system => RuntimeManager.StudioSystem;

  private void Awake() => this.create();

  internal void create()
  {
    if (this._created)
      return;
    this._created = true;
    MusicBox.inst = this;
    this.debug_box = new MusicBoxDebug();
    this._lib = AssetManager.music_box;
    this.idle = new MusicBoxIdle();
    ScrollWindow.addCallbackHide(new ScrollWindowNameAction(this.hideWindowCallback));
    if (!MusicBox.fmod_disabled)
    {
      try
      {
        this._bus_master = RuntimeManager.GetBus("bus:/");
        if (((Bus) ref this._bus_master).isValid())
          ((Bus) ref this._bus_master).setVolume(0.0f);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("MusicBox failed to init: " + ex?.ToString()));
        MusicBox.music_on = false;
        MusicBox.sounds_on = false;
        return;
      }
      Platform currentPlatform = Settings.Instance.FindCurrentPlatform();
      if (MusicBox.debug_sounds)
      {
        Platform.PropertyAccessor<TriStateBool> propertyAccessor = Platform.PropertyAccessors.LiveUpdate;
        propertyAccessor.Set(currentPlatform, (TriStateBool) 1);
        propertyAccessor = Platform.PropertyAccessors.Overlay;
        propertyAccessor.Set(currentPlatform, (TriStateBool) 2);
      }
      else
      {
        Platform.PropertyAccessor<TriStateBool> propertyAccessor = Platform.PropertyAccessors.LiveUpdate;
        propertyAccessor.Set(currentPlatform, (TriStateBool) 0);
        propertyAccessor = Platform.PropertyAccessors.Overlay;
        propertyAccessor.Set(currentPlatform, (TriStateBool) 0);
      }
      this.createMusicEvent();
      this.assignCallback();
      this.startMusic();
    }
    MusicBox.reserveFlag(MusicBoxLibrary.Neutral_001.id);
    this.clearParams();
    MusicBox._sound_object = new GameObject("musicbox_pan");
    this._camera_listener = new GameObject("fmod_listener");
    this._camera_listener.transform.parent = ((Component) Camera.main).transform;
    this._camera_listener.AddComponent<StudioListener>();
  }

  private void setMusicState(MusicState pState)
  {
    this.music_state = pState;
    if (pState != MusicState.Menu)
      return;
    MusicBox.reserveFlag("Menu");
  }

  private void checkDrawingSounds()
  {
    if (!MusicBox.sounds_on)
      return;
    bool flag = false;
    if (InputHelpers.mouseSupported)
    {
      if (!Input.GetMouseButton(0))
        flag = true;
      else if (!ControllableUnit.isControllingUnit() && World.world.isOverUI())
        flag = true;
    }
    else if (Input.touchCount == 0)
      flag = true;
    if (!flag)
      return;
    MusicBox.inst.stopDrawingSounds();
  }

  private void checkIdleVolume()
  {
    if (World.world.isPaused())
    {
      this._volume_idle -= Time.deltaTime;
      if ((double) this._volume_idle < 0.0)
        this._volume_idle = 0.0f;
    }
    else
    {
      this._volume_idle += Time.deltaTime;
      if ((double) this._volume_idle > 1.0)
        this._volume_idle = 1f;
    }
    if (!((Bus) ref this._bus_idle).isValid())
      this._bus_idle = RuntimeManager.GetBus("bus:/Idle");
    this.checkBusVolume(this._volume_idle, this._bus_idle);
  }

  private void checkVolumes()
  {
    bool flag = ((VCA) ref this._vca_sound_effects).isValid();
    if (!flag)
    {
      this._vca_sound_effects = RuntimeManager.GetVCA("vca:/Sound Effects");
      this._vca_music = RuntimeManager.GetVCA("vca:/Music");
      this._vca_ui = RuntimeManager.GetVCA("vca:/UI");
      this._bus_master = RuntimeManager.GetBus("bus:/");
      if (!flag)
        return;
    }
    this.checkBusVolume("volume_master_sound", this._bus_master);
    this.checkVcaVolume("volume_sound_effects", this._vca_sound_effects);
    this.checkVcaVolume("volume_music", this._vca_music);
    this.checkVcaVolume("volume_ui", this._vca_ui);
  }

  private void checkBusVolume(float pVolume, Bus pBus)
  {
    float num;
    ((Bus) ref pBus).getVolume(ref num);
    if ((double) num == (double) pVolume)
      return;
    ((Bus) ref pBus).setVolume(pVolume);
  }

  private void checkBusVolume(string pOptionParam, Bus pBus)
  {
    float num1 = (float) PlayerConfig.getIntValue(pOptionParam) / 100f;
    float num2;
    ((Bus) ref pBus).getVolume(ref num2);
    if ((double) num2 == (double) num1)
      return;
    ((Bus) ref pBus).setVolume(num1);
  }

  private void checkVcaVolume(string pOptionParam, VCA pVCA)
  {
    float num1 = (float) PlayerConfig.getIntValue(pOptionParam) / 100f;
    float num2;
    ((VCA) ref pVCA).getVolume(ref num2);
    if ((double) num2 == (double) num1)
      return;
    ((VCA) ref pVCA).setVolume(num1);
  }

  public void update(float pElapsed)
  {
    if (MusicBox.fmod_disabled)
      return;
    Bench.bench("music_box", "music_box_total");
    Bench.bench("check_volume", "music_box");
    this.checkVolumes();
    this.checkIdleVolume();
    Bench.benchEnd("check_volume", "music_box");
    Bench.bench("update_idle", "music_box");
    this.idle.update(pElapsed);
    Bench.benchEnd("update_idle", "music_box");
    Bench.bench("update_debug", "music_box");
    this.debug_box.update();
    Bench.benchEnd("update_debug", "music_box");
    Bench.bench("update_drawing", "music_box");
    this.checkDrawingSounds();
    Bench.benchEnd("update_drawing", "music_box");
    Bench.bench("update_fmod_params", "music_box");
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(0.0f, 0.0f, World.world.camera.orthographicSize * 1.5f);
    this._camera_listener.transform.localPosition = vector3;
    this.updateMainFmodParams();
    Bench.benchEnd("update_fmod_params", "music_box");
    if ((double) this._timer > 0.0)
    {
      this._timer -= pElapsed;
    }
    else
    {
      this._timer = 1f;
      Bench.bench("clearParams", "music_box");
      this.clearParams();
      Bench.benchEnd("clearParams", "music_box");
      Bench.bench("drawFmodDebugZones", "music_box");
      this.drawFmodDebugZones();
      Bench.benchEnd("drawFmodDebugZones", "music_box");
      Bench.bench("countZonesUnits", "music_box");
      this.countUnitsInZones();
      Bench.benchEnd("countZonesUnits", "music_box");
      Bench.bench("countSpecialTiles", "music_box");
      this.countSpecialTilesInChunks();
      Bench.benchEnd("countSpecialTiles", "music_box");
      Bench.bench("checkUnitsParams", "music_box");
      this.checkUnitsParams();
      Bench.benchEnd("checkUnitsParams", "music_box");
      Bench.bench("checkCamera", "music_box");
      this.checkCamera();
      Bench.benchEnd("checkCamera", "music_box");
      Bench.bench("music_params_1", "music_box");
      foreach (MusicBoxContainerTiles cListParam in this._lib.c_list_params)
      {
        if (cListParam.enabled)
          this.enableMusicParameter(cListParam.asset.id);
        else
          this.disableMusicParameter(cListParam.asset.id);
      }
      Bench.benchEnd("music_params_1", "music_box");
      Bench.bench("music_params_2", "music_box");
      foreach (MusicBoxContainerUnits boxContainerUnits in this._lib.c_dict_units.Values)
      {
        if (boxContainerUnits.enabled)
          this.enableMusicParameter(boxContainerUnits.asset.id);
        else
          this.disableMusicParameter(boxContainerUnits.asset.id);
      }
      Bench.benchEnd("music_params_2", "music_box");
      Bench.bench("flags", "music_box");
      if (this._flags_to_enable.Any<string>())
      {
        foreach (string pID in this._flags_to_enable)
          this.enableMusicParameter(pID);
        this._flags_to_enable.Clear();
      }
      Bench.benchEnd("flags", "music_box");
      Bench.bench("check_environment", "music_box");
      foreach (MusicBoxContainerTiles cListEnvironment in this._lib.c_list_environments)
        this.checkEnvironmentSound(cListEnvironment);
      Bench.benchEnd("check_environment", "music_box");
      Bench.benchEnd("music_box", "music_box_total");
    }
  }

  private void updateMainFmodParams()
  {
    if (World.world.quality_changer.isLowRes())
    {
      FMOD.Studio.System studioSystem = MusicBox._studio_system;
      ((FMOD.Studio.System) ref studioSystem).setParameterByName("MiniMap", 1f, false);
    }
    else
    {
      FMOD.Studio.System studioSystem = MusicBox._studio_system;
      ((FMOD.Studio.System) ref studioSystem).setParameterByName("MiniMap", 0.0f, false);
    }
    float zoomRatioLow = World.world.quality_changer.getZoomRatioLow();
    float zoomRatioHigh = World.world.quality_changer.getZoomRatioHigh();
    float zoomRatioFull = World.world.quality_changer.getZoomRatioFull();
    FMOD.Studio.System studioSystem1 = MusicBox._studio_system;
    ((FMOD.Studio.System) ref studioSystem1).setParameterByName("Zoom_Low", zoomRatioLow, false);
    studioSystem1 = MusicBox._studio_system;
    ((FMOD.Studio.System) ref studioSystem1).setParameterByName("Zoom_High", zoomRatioHigh, false);
    studioSystem1 = MusicBox._studio_system;
    ((FMOD.Studio.System) ref studioSystem1).setParameterByName("Zoom_Full", zoomRatioFull, false);
  }

  public static void clearAllSounds()
  {
    if (MusicBox.fmod_disabled)
      return;
    MusicBox.inst.idle.clearAllSounds();
    MusicBox.inst.debug_box.clear();
  }

  public void clearParams()
  {
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      MusicBoxContainerCivs boxContainerCivs;
      if (this._lib.c_dict_civs.TryGetValue(kingdom.getSpecies(), out boxContainerCivs))
        boxContainerCivs.kingdom_exists = true;
    }
    this._tiles_sand = 0;
    this._tiles_shallow_water = 0;
    foreach (MusicBoxContainerCivs boxContainerCivs in this._lib.c_dict_civs.Values)
      boxContainerCivs.clear();
    foreach (MusicAsset musicAsset in this._lib.list)
      musicAsset.container_tiles?.clear();
    foreach (MusicBoxContainerUnits boxContainerUnits in this._lib.c_dict_units.Values)
      boxContainerUnits.clear();
    DebugLayer.fmod_zones_to_draw.Clear();
  }

  private void hideWindowCallback(string pWindowID)
  {
  }

  private void assignCallback()
  {
    // ISSUE: method pointer
    this._music_callback = new EVENT_CALLBACK((object) null, __methodptr(beatEventCallback));
    this._timeline_info = new TimelineInfo();
    this._timeline_handle = GCHandle.Alloc((object) this._timeline_info, GCHandleType.Pinned);
    ((EventInstance) ref this._music_event).setUserData(GCHandle.ToIntPtr(this._timeline_handle));
    ((EventInstance) ref this._music_event).setCallback(this._music_callback, (EVENT_CALLBACK_TYPE) 6144);
  }

  public static EventInstance getNewInstance(string pID) => RuntimeManager.CreateInstance(pID);

  public static EventInstance attachToObject(string pID, GameObject pObject, bool pPlay = true)
  {
    if (!MusicBox.sounds_on)
      return new EventInstance();
    EventInstance newInstance = MusicBox.getNewInstance(pID);
    RuntimeManager.AttachInstanceToGameObject(newInstance, pObject.transform, false);
    if (pPlay)
      ((EventInstance) ref newInstance).start();
    return newInstance;
  }

  private void createMusicEvent()
  {
    this._music_event = MusicBox.getNewInstance("event:/MUSIC/ConsolidatedMusicEvent");
  }

  private void startMusic()
  {
    if (!MusicBox.music_on)
      return;
    ((EventInstance) ref this._music_event).start();
  }

  [MonoPInvokeCallback(typeof (EVENT_CALLBACK))]
  private static RESULT beatEventCallback(
    EVENT_CALLBACK_TYPE pType,
    IntPtr pInstancePtr,
    IntPtr pParameterPtr)
  {
    IntPtr num;
    RESULT userData = ((EventInstance) ref MusicBox.inst._music_event).getUserData(ref num);
    if (userData != null)
      Debug.LogError((object) ("Timeline Callback error: " + userData.ToString()));
    else if (num != IntPtr.Zero)
    {
      TimelineInfo target = (TimelineInfo) GCHandle.FromIntPtr(num).Target;
      if (pType == 2048 /*0x0800*/)
      {
        TIMELINE_MARKER_PROPERTIES structure = (TIMELINE_MARKER_PROPERTIES) Marshal.PtrToStructure(pParameterPtr, typeof (TIMELINE_MARKER_PROPERTIES));
        target.lastMarker = structure.name;
        MusicBox.inst.markerReached(StringWrapper.op_Implicit(target.lastMarker));
      }
    }
    return (RESULT) 0;
  }

  private void loadBanks()
  {
  }

  private void checkEnvironmentSound(MusicBoxContainerTiles pContainer)
  {
    MusicAsset asset = pContainer.asset;
    bool flag = true;
    if (asset.mini_map_only)
    {
      if (!World.world.quality_changer.isLowRes())
        flag = false;
    }
    else if (World.world.quality_changer.isLowRes())
      flag = false;
    else if ((double) asset.min_zoom <= (double) World.world.camera.orthographicSize)
      flag = false;
    if (flag && asset.min_tiles_to_play != 0 && pContainer.amount < asset.min_tiles_to_play)
      flag = false;
    pContainer.enabled = flag;
    if (flag)
      this.playEnvironmentSound(pContainer);
    else
      this.stopEnvironmentSound(pContainer);
  }

  public static void playIdleSoundVisibleOnly(string pSoundPath, WorldTile pTile)
  {
    if (!MusicBox.sounds_on)
      return;
    MusicBox.playSoundVisibleOnly(pSoundPath, pTile);
  }

  public static void playSoundVisibleOnly(string pSoundPath, WorldTile pTile)
  {
    if (!MusicBox.sounds_on)
      return;
    MusicBox.playSound(pSoundPath, pTile, true, true);
  }

  public static void playSound(
    string pSoundPath,
    WorldTile pTile,
    bool pGameViewOnly = false,
    bool pVisibleOnly = false)
  {
    if (string.IsNullOrEmpty(pSoundPath) || pVisibleOnly && !pTile.zone.visible)
      return;
    string pSoundPath1 = pSoundPath;
    Vector2Int pos1 = pTile.pos;
    double x = (double) ((Vector2Int) ref pos1).x;
    Vector2Int pos2 = pTile.pos;
    double y = (double) ((Vector2Int) ref pos2).y;
    int num = pGameViewOnly ? 1 : 0;
    MusicBox.playSound(pSoundPath1, (float) x, (float) y, num != 0);
  }

  public static void playSoundWorld(string pSoundPath)
  {
  }

  public static void playSoundUI(string pSoundPath) => MusicBox.playSound(pSoundPath);

  public static EventInstance PlayOneShot(GUID pGuid, Vector3 pPosition, bool pSet3D = true)
  {
    EventInstance instance = RuntimeManager.CreateInstance(pGuid);
    if (pSet3D)
    {
      ((EventInstance) ref instance).set3DAttributes(RuntimeUtils.To3DAttributes(pPosition));
    }
    else
    {
      Vector3 position = ((Component) World.world.move_camera).transform.position;
      float orthographicSize = World.world.move_camera.main_camera.orthographicSize;
      Vector3 vector3;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3).\u002Ector(position.x, position.y, orthographicSize);
      ((EventInstance) ref instance).set3DAttributes(RuntimeUtils.To3DAttributes(vector3));
    }
    ((EventInstance) ref instance).start();
    ((EventInstance) ref instance).release();
    return instance;
  }

  private static bool isEventExists(string pEventPath)
  {
    bool flag;
    if (!MusicBox._events_cache.TryGetValue(pEventPath, out flag))
    {
      FMOD.Studio.System studioSystem = RuntimeManager.StudioSystem;
      EventDescription eventDescription;
      flag = ((FMOD.Studio.System) ref studioSystem).getEvent(pEventPath, ref eventDescription) == 0;
      MusicBox._events_cache.Add(pEventPath, flag);
      if (!flag)
        Debug.LogWarning((object) ("[FMOD] Missing event : " + pEventPath));
      else
        MusicBox._events_guids[pEventPath] = RuntimeManager.PathToGUID(pEventPath);
    }
    return flag;
  }

  public static void playSound(
    string pSoundPath,
    float pX = -1f,
    float pY = -1f,
    bool pGameViewOnly = false,
    bool pVisibleOnly = false)
  {
    if (!MusicBox.sounds_on || pGameViewOnly && World.world.quality_changer.isLowRes() || !MusicBox.isEventExists(pSoundPath))
      return;
    GUID eventsGuid = MusicBox._events_guids[pSoundPath];
    EventInstance? nullable = new EventInstance?();
    try
    {
      nullable = (double) pX == -1.0 || (double) pY == -1.0 ? new EventInstance?(MusicBox.PlayOneShot(eventsGuid, Vector3.zero, false)) : new EventInstance?(MusicBox.PlayOneShot(eventsGuid, new Vector3(pX, pY, 0.0f)));
    }
    catch (EventNotFoundException ex)
    {
    }
    if (!DebugConfig.isOn(DebugOption.OverlaySounds) && !DebugConfig.isOn(DebugOption.OverlaySoundsActive))
      return;
    MusicBox.inst.debug_box.add(pSoundPath.Split('/', StringSplitOptions.None).Last<string>(), pX, pY, nullable.Value);
  }

  public void playEnvironmentSound(MusicBoxContainerTiles pContainer)
  {
    if (!MusicBox.sounds_on)
      return;
    MusicAsset asset = pContainer.asset;
    EventInstance pInstance;
    if (this._environment_sounds.ContainsKey(asset.fmod_path))
    {
      pInstance = this._environment_sounds[asset.fmod_path];
    }
    else
    {
      pInstance = MusicBox.getNewInstance(asset.fmod_path);
      this._environment_sounds.Add(asset.fmod_path, pInstance);
    }
    MusicBox.setPan(pInstance, pContainer.cur_pan.x, pContainer.cur_pan.y);
    if (MusicBox.isPlaying(pInstance))
      return;
    ((EventInstance) ref pInstance).start();
  }

  public void stopEnvironmentSound(MusicBoxContainerTiles pContainer)
  {
    MusicAsset asset = pContainer.asset;
    if (!this._environment_sounds.ContainsKey(asset.fmod_path))
      return;
    EventInstance environmentSound = this._environment_sounds[asset.fmod_path];
    if (!MusicBox.isPlaying(environmentSound))
      return;
    ((EventInstance) ref environmentSound).stop((STOP_MODE) 0);
  }

  public void playDrawingSound(string pSoundPath, float pX = -1f, float pY = -1f)
  {
    if (!MusicBox.sounds_on)
      return;
    EventInstance pInstance;
    if (this._drawing_sounds.ContainsKey(pSoundPath))
    {
      pInstance = this._drawing_sounds[pSoundPath];
    }
    else
    {
      pInstance = MusicBox.getNewInstance(pSoundPath);
      this._drawing_sounds.Add(pSoundPath, pInstance);
    }
    MusicBox.setPan(pInstance, pX, pY);
    ((EventInstance) ref pInstance).setParameterByName("cursor_speed", MapBox.cursor_speed.fmod_speed, false);
    if (MusicBox.isPlaying(pInstance))
      return;
    ((EventInstance) ref pInstance).start();
  }

  public static void setPan(EventInstance pInstance, float pX, float pY)
  {
    if ((double) pX == -1.0 && (double) pY == -1.0)
      return;
    float num = 0.0f;
    MusicBox._sound_object.transform.position = new Vector3(pX, pY, num);
    ATTRIBUTES_3D attributes3D = RuntimeUtils.To3DAttributes(MusicBox._sound_object);
    ((EventInstance) ref pInstance).set3DAttributes(attributes3D);
  }

  public void stopDrawingSound(string pID)
  {
    if (!this._drawing_sounds.ContainsKey(pID))
      return;
    EventInstance drawingSound = this._drawing_sounds[pID];
    if (!MusicBox.isPlaying(drawingSound))
      return;
    ((EventInstance) ref drawingSound).stop((STOP_MODE) 0);
  }

  public void stopDrawingSounds()
  {
    foreach (EventInstance pInstance in this._drawing_sounds.Values)
    {
      if (MusicBox.isPlaying(pInstance))
        ((EventInstance) ref pInstance).stop((STOP_MODE) 0);
    }
  }

  public static bool isPlaying(EventInstance pInstance)
  {
    PLAYBACK_STATE playbackState;
    ((EventInstance) ref pInstance).getPlaybackState(ref playbackState);
    return playbackState != 2;
  }

  private void drawFmodDebugZones()
  {
  }

  private void countUnitsInZones()
  {
    foreach (MapChunk visibleChunk in World.world.zone_camera.getVisibleChunks())
    {
      if (!visibleChunk.objects.isEmpty())
        this.countUnits(visibleChunk);
    }
  }

  private void checkCamera()
  {
    if ((this._tiles_sand < 50 || this._tiles_shallow_water < 50) && this._tiles_sand >= 100 && this._tiles_shallow_water < 20)
      MusicBoxLibrary.Locations_Desert.container_tiles.amount = this._tiles_sand + this._tiles_shallow_water;
    this._lib.c_list_params.Sort(new Comparison<MusicBoxContainerTiles>(MusicBox.sorter));
    float num1 = 0.0f;
    for (int index = 0; index < this._lib.c_list_params.Count; ++index)
    {
      MusicBoxContainerTiles cListParam = this._lib.c_list_params[index];
      cListParam.enabled = false;
      num1 += (float) cListParam.amount;
    }
    float num2 = 0.0f;
    int num3 = 0;
    for (int index = 0; index < this._lib.c_list_params.Count; ++index)
    {
      MusicBoxContainerTiles cListParam = this._lib.c_list_params[index];
      cListParam.calculatePan();
      cListParam.percent = (float) cListParam.amount / num1;
      num2 += cListParam.percent;
      if (cListParam.amount > 50)
      {
        if (num3 >= 2)
          break;
        cListParam.enabled = true;
        ++num3;
      }
    }
  }

  private void checkUnitsParams()
  {
    MusicBoxContainerUnits boxContainerUnits1 = (MusicBoxContainerUnits) null;
    MusicBoxContainerUnits boxContainerUnits2 = (MusicBoxContainerUnits) null;
    foreach (MusicBoxContainerUnits pContainer in this._lib.c_dict_units.Values)
    {
      MusicAssetDelegate specialDelegateUnits = pContainer.asset.special_delegate_units;
      if (specialDelegateUnits != null)
        specialDelegateUnits(pContainer);
      if (pContainer.units > 0)
      {
        if (pContainer.asset.priority == MusicLayerPriority.High)
          boxContainerUnits2 = pContainer;
        else if (pContainer.asset.priority == MusicLayerPriority.Medium)
          boxContainerUnits1 = pContainer;
      }
    }
    if (boxContainerUnits2 != null)
      boxContainerUnits1 = (MusicBoxContainerUnits) null;
    if (boxContainerUnits2 != null || boxContainerUnits1 != null)
    {
      foreach (MusicBoxContainerUnits boxContainerUnits3 in this._lib.c_dict_units.Values)
      {
        if ((boxContainerUnits2 == null || boxContainerUnits3 != boxContainerUnits2) && (boxContainerUnits1 == null || boxContainerUnits3 != boxContainerUnits1))
          boxContainerUnits3.units = 0;
      }
    }
    foreach (MusicBoxContainerUnits boxContainerUnits4 in this._lib.c_dict_units.Values)
    {
      if (boxContainerUnits4.units > 0)
        boxContainerUnits4.enabled = true;
    }
  }

  public static int sorter(MusicBoxContainerTiles pV1, MusicBoxContainerTiles pV2)
  {
    return pV2.amount.CompareTo(pV1.amount);
  }

  private void countSpecialTilesInChunks()
  {
    List<MapChunk> visibleChunks = World.world.zone_camera.getVisibleChunks();
    int index = 0;
    for (int count = visibleChunks.Count; index < count; ++index)
      this.countSpecialTilesForZone(visibleChunks[index]);
  }

  private void countSpecialTilesForZone(MapChunk pChunk)
  {
    List<MusicBoxTileData> simpleData = pChunk.getSimpleData();
    TileTypeBase[] arrayTiles = TileLibrary.array_tiles;
    int index1 = 0;
    for (int count1 = simpleData.Count; index1 < count1; ++index1)
    {
      MusicBoxTileData musicBoxTileData = simpleData[index1];
      TileTypeBase tileTypeBase = arrayTiles[musicBoxTileData.tile_type_id];
      int amount = musicBoxTileData.amount;
      if (amount != 0)
      {
        List<MusicAsset> musicAssets = tileTypeBase.music_assets;
        if (musicAssets != null)
        {
          int index2 = 0;
          for (int count2 = musicAssets.Count; index2 < count2; ++index2)
            musicAssets[index2].container_tiles.count(amount, pChunk.world_center_x, pChunk.world_center_y);
        }
      }
    }
  }

  private void countUnits(MapChunk pChunk)
  {
    foreach (long kingdom1 in pChunk.objects.kingdoms)
    {
      Kingdom kingdom2 = World.world.kingdoms.get(kingdom1);
      if (kingdom2 != null)
      {
        ActorAsset actorAsset = kingdom2.getActorAsset();
        if (actorAsset != null && actorAsset.has_music_theme)
          ++this._lib.c_dict_units[actorAsset.music_theme].units;
      }
    }
  }

  private void enableMusicParameter(string pID) => this.setMusicParameter(pID, 1f);

  private void disableMusicParameter(string pID) => this.setMusicParameter(pID, 0.0f);

  private void setMusicParameter(string pID, float pValue)
  {
    ((EventInstance) ref this._music_event).setParameterByName(pID, pValue, false);
  }

  private void markerReached(string pMarker)
  {
    if (pMarker == "Intro")
      return;
    MusicAsset musicAsset = this._lib.get(pMarker);
    if (musicAsset == null)
      return;
    if (musicAsset.disable_param_after_start)
      this.disableMusicParameter(pMarker);
    if (musicAsset.action == null)
      return;
    musicAsset.action();
  }

  public static void reserveFlag(string pID, bool pValue = true)
  {
    if (!MusicBox.music_on)
      return;
    MusicBox.inst._timer = -1f;
    MusicBox.inst._flags_to_enable.Add(pID);
  }

  public static void debug_fmod(DebugTool pTool)
  {
    if (MusicBox.fmod_disabled)
      return;
    FMOD.Studio.System studioSystem = MusicBox._studio_system;
    Bank[] bankArray;
    ((FMOD.Studio.System) ref studioSystem).getBankList(ref bankArray);
    studioSystem = MusicBox._studio_system;
    EventDescription eventDescription;
    RESULT result = ((FMOD.Studio.System) ref studioSystem).getEvent("event:/MUSIC/ConsolidatedMusicEvent", ref eventDescription);
    int pT2_1 = -1;
    float pT2_2 = -1f;
    PLAYBACK_STATE playbackState = (PLAYBACK_STATE) 3;
    ((EventInstance) ref MusicBox.inst._music_event).getParameterByName("new_world", ref pT2_2);
    ((EventInstance) ref MusicBox.inst._music_event).getTimelinePosition(ref pT2_1);
    ((EventInstance) ref MusicBox.inst._music_event).getPlaybackState(ref playbackState);
    pTool.setText("Zoom_Low:", (object) World.world.quality_changer.getZoomRatioLow());
    pTool.setText("Zoom_High:", (object) World.world.quality_changer.getZoomRatioHigh());
    pTool.setText("Zoom_Full:", (object) World.world.quality_changer.getZoomRatioFull());
    pTool.setSeparator();
    pTool.setText("idle_sim_objects:", (object) MusicBox.inst.idle.CountCurrentSounds());
    pTool.setText("music state:", (object) MusicBox.inst.music_state);
    pTool.setText("IsInitialized:", (object) RuntimeManager.IsInitialized);
    pTool.setText("Banks count:", (object) bankArray.Length);
    pTool.setText("AnySampleDataLoading:", (object) RuntimeManager.AnySampleDataLoading());
    pTool.setText("Bank Master:", (object) RuntimeManager.HasBankLoaded("Master"));
    pTool.setText("Bank Master.strings:", (object) RuntimeManager.HasBankLoaded("Master.strings"));
    pTool.setText("MUSIC_EVENT by name:", (object) result.ToString());
    pTool.setText("tParam_new_world:", (object) pT2_2);
    pTool.setText("tTimelinePos:", (object) pT2_1);
    pTool.setText("getPlaybackState:", (object) playbackState.ToString());
  }

  public void debug_params(DebugTool pTool)
  {
    if (MusicBox.fmod_disabled)
      return;
    float pT2 = 0.0f;
    for (int index = 0; index < this._lib.list.Count; ++index)
    {
      string id = this._lib.list[index].id;
      ((EventInstance) ref MusicBox.inst._music_event).getParameterByName(id, ref pT2);
      if ((double) pT2 == 1.0)
        pTool.setText(id + ":", (object) pT2);
    }
  }

  public void debug_world_params(DebugTool pTool)
  {
    if (MusicBox.fmod_disabled)
      return;
    foreach (MusicBoxContainerCivs boxContainerCivs in this._lib.c_dict_civs.Values)
    {
      if (boxContainerCivs.active)
        pTool.setText(boxContainerCivs.asset.id, (object) $"{boxContainerCivs.buildings.ToString()} {boxContainerCivs.kingdom_exists.ToString()} {boxContainerCivs.active.ToString()}");
    }
    foreach (MusicAsset musicAsset in this._lib.list)
    {
      MusicBoxContainerTiles containerTiles = musicAsset.container_tiles;
      if (containerTiles != null && containerTiles.enabled)
        pTool.setText(containerTiles.asset.id, (object) $"{containerTiles.amount.ToString()} {containerTiles.enabled.ToString()} {containerTiles.percent.ToText()}%");
    }
    pTool.setText("", (object) "");
  }

  public void debug_unit_params(DebugTool pTool)
  {
    if (MusicBox.fmod_disabled || this._lib.c_dict_units.Count == 0)
      return;
    foreach (MusicBoxContainerUnits boxContainerUnits in this._lib.c_dict_units.Values)
    {
      if (boxContainerUnits.units != 0)
        pTool.setText(boxContainerUnits.asset.id, (object) $"{boxContainerUnits.units.ToString()} {boxContainerUnits.enabled.ToString()}");
    }
    pTool.setText("", (object) "");
  }

  private static bool fmod_disabled => !MusicBox.music_on && !MusicBox.sounds_on;
}
