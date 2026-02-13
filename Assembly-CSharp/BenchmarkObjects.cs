// Decompiled with JetBrains decompiler
// Type: BenchmarkObjects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
public class BenchmarkObjects : MonoBehaviour
{
  public static BenchmarkObjects instance;
  public List<Sprite> some_sprites = new List<Sprite>();
  public TestActorGameObject prefab_unity_object;
  private List<TestActorGameObject> actors_unity = new List<TestActorGameObject>();
  internal List<TestActorSimpleObject> actors_simple = new List<TestActorSimpleObject>();
  internal List<TestActorSimpleObject> actors_simple_visible = new List<TestActorSimpleObject>();
  public int total_unity_objects;
  public int total_simple_objects;
  public int total_simple_objects_visible;

  public BenchmarkObjects() => BenchmarkObjects.instance = this;

  private void Update()
  {
    this.update(Time.deltaTime);
    this.total_unity_objects = this.actors_unity.Count;
    this.total_simple_objects = this.actors_simple.Count;
    this.total_simple_objects_visible = this.actors_simple_visible.Count;
  }

  public void addObjectsSimple(int pAmount = 2000)
  {
    for (int index = 0; index < pAmount; ++index)
    {
      TestActorSimpleObject actorSimpleObject = new TestActorSimpleObject();
      actorSimpleObject.create(this.some_sprites);
      this.actors_simple.Add(actorSimpleObject);
    }
  }

  public void addObjectsUnity(int pAmount = 2000)
  {
    for (int index = 0; index < pAmount; ++index)
    {
      TestActorGameObject testActorGameObject = Object.Instantiate<TestActorGameObject>(this.prefab_unity_object);
      testActorGameObject.create(this.some_sprites);
      ((Component) testActorGameObject).transform.parent = ((Component) this).transform;
      this.actors_unity.Add(testActorGameObject);
    }
  }

  public void killAll()
  {
    foreach (Component component in this.actors_unity)
      Object.Destroy((Object) component.gameObject, 0.01f);
    this.actors_unity.Clear();
    this.actors_simple.Clear();
  }

  public void randomRespawn()
  {
    foreach (TestActorGameObject testActorGameObject in this.actors_unity)
      testActorGameObject.randomRespawn();
    foreach (TestActorSimpleObject actorSimpleObject in this.actors_simple)
      actorSimpleObject.randomRespawn();
  }

  public void update(float pElapsed)
  {
    this.updateKeys();
    this.updateUnityActors(pElapsed);
    this.updateSimpleActors(pElapsed);
    this.updateVisibility(pElapsed);
  }

  private void updateKeys()
  {
    if (Input.GetKeyDown((KeyCode) 49))
      this.addObjectsSimple();
    if (Input.GetKeyDown((KeyCode) 50))
      this.addObjectsUnity();
    if (Input.GetKeyDown((KeyCode) 51))
      this.randomRespawn();
    if (!Input.GetKeyDown((KeyCode) 52))
      return;
    this.killAll();
  }

  private void updateUnityActors(float pElapsed)
  {
    for (int index = 0; index < this.actors_unity.Count; ++index)
      this.actors_unity[index].update(pElapsed);
  }

  private void updateSimpleActors(float pElapsed)
  {
    Parallel.ForEach<TestActorSimpleObject>((IEnumerable<TestActorSimpleObject>) this.actors_simple, World.world.parallel_options, (Action<TestActorSimpleObject>) (pActor => pActor.update(pElapsed)));
  }

  private void updateVisibility(float pElapsed)
  {
    this.actors_simple_visible.Clear();
    float num1 = 8f;
    for (int index = 0; index < this.actors_simple.Count; ++index)
    {
      TestActorSimpleObject actorSimpleObject = this.actors_simple[index];
      double posX = (double) actorSimpleObject.pos_x;
      float posY = actorSimpleObject.pos_y;
      double num2 = (double) num1;
      TileZone zone = World.world.zone_calculator.getZone(Mathf.FloorToInt((float) (posX / num2)), Mathf.FloorToInt(posY / num1));
      if (zone != null && zone.visible)
        this.actors_simple_visible.Add(actorSimpleObject);
    }
  }
}
