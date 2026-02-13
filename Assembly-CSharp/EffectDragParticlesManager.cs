// Decompiled with JetBrains decompiler
// Type: EffectDragParticlesManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EffectDragParticlesManager : MonoBehaviour
{
  public static EffectDragParticlesManager instance;
  private ObjectPoolGenericMono<EffectParticlesCursor> _pool;
  [SerializeField]
  private EffectParticlesCursor _prefab;
  [SerializeField]
  private List<SpriteSet> _sprite_sets;
  [SerializeField]
  public float _spawn_interval = 10f;

  private void Awake()
  {
    this._pool = new ObjectPoolGenericMono<EffectParticlesCursor>(this._prefab, ((Component) this).transform);
    EffectDragParticlesManager.instance = this;
  }

  private void Update()
  {
    this.updateSpawn();
    this.updateAnimation();
  }

  private void updateAnimation()
  {
    if (this._pool.countActive() == 0)
      return;
    foreach (EffectParticlesCursor effectParticlesCursor in (IEnumerable<EffectParticlesCursor>) this._pool.getListTotal())
    {
      if (((Behaviour) effectParticlesCursor).isActiveAndEnabled)
        effectParticlesCursor.update();
    }
  }

  private void updateSpawn()
  {
    if (!Config.isDraggingItem() || Config.dragging_item_object == null || !Config.dragging_item_object.spawn_particles_on_drag || (double) Time.frameCount % (double) this._spawn_interval != 0.0)
      return;
    this.spawnNew(Config.dragging_item_object.transform.position);
  }

  public void spawnNew(Vector3 pPos)
  {
    EffectParticlesCursor next = this._pool.getNext();
    next.setFrames(this._sprite_sets.GetRandom<SpriteSet>().sprites);
    next.launch();
    next.getAnimation().setActionFinish(new EffectParticlesCursorDelegate(this.finishingEffectAction));
    Vector2 vector2 = Vector2.op_Implicit(pPos);
    vector2.x += Randy.randomFloat(-1f, 1f);
    vector2.y += Randy.randomFloat(-1f, 1f);
    ((Component) next).transform.position = Vector2.op_Implicit(vector2);
  }

  private void finishingEffectAction(MonoBehaviour pEffectObject)
  {
    this._pool.release(((Component) pEffectObject).GetComponent<EffectParticlesCursor>());
  }
}
