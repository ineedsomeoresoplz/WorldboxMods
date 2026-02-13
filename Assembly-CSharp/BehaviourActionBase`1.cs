// Decompiled with JetBrains decompiler
// Type: BehaviourActionBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehaviourActionBase<T> : BehaviourElementAI
{
  public bool uses_kingdoms;
  public bool uses_cities;
  public bool uses_books;
  public bool uses_religions;
  public bool uses_languages;
  public bool uses_cultures;
  public bool uses_clans;
  public bool uses_plots;
  public bool uses_families;
  protected bool _has_error_check;

  protected static MapBox world => MapBox.instance;

  public override void create()
  {
    base.create();
    this.setupErrorChecks();
  }

  public virtual void prepare(T pObject)
  {
  }

  public virtual BehResult startExecute(T pObject)
  {
    if (this._has_error_check)
    {
      if (this.shouldRetry(pObject))
        return BehResult.RepeatStep;
      if (this.errorsFound(pObject))
        return BehResult.Stop;
    }
    this.prepare(pObject);
    return this.execute(pObject);
  }

  public virtual BehResult execute(T pObject) => BehResult.Continue;

  protected virtual void setupErrorChecks() => this.setHasErrorCheck();

  private void setHasErrorCheck() => this._has_error_check = true;

  public virtual bool errorsFound(T pObject) => false;

  public virtual bool shouldRetry(T pObject)
  {
    return this.uses_cities && BehaviourActionBase<T>.world.cities.isLocked() || this.uses_kingdoms && BehaviourActionBase<T>.world.kingdoms.isLocked() || this.uses_books && BehaviourActionBase<T>.world.books.isLocked() || this.uses_religions && BehaviourActionBase<T>.world.religions.isLocked() || this.uses_languages && BehaviourActionBase<T>.world.languages.isLocked() || this.uses_cultures && BehaviourActionBase<T>.world.cultures.isLocked() || this.uses_clans && BehaviourActionBase<T>.world.clans.isLocked() || this.uses_plots && BehaviourActionBase<T>.world.plots.isLocked() || this.uses_families && BehaviourActionBase<T>.world.families.isLocked();
  }
}
