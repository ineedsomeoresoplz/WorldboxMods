// Decompiled with JetBrains decompiler
// Type: CultureBookButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CultureBookButton : MonoBehaviour
{
  private Book _book;
  public Image cover;
  public Image icon;
  private bool _created;

  private void Start() => this.create();

  private void create()
  {
    if (this._created)
      return;
    this._created = true;
    this.setupTooltip();
  }

  public void setupTooltip()
  {
    TipButton tipButton;
    if (!((Component) this).TryGetComponent<TipButton>(ref tipButton))
      return;
    tipButton.setHoverAction(new TooltipAction(this.showTooltip));
  }

  internal void load(long pBookID) => this.load(World.world.books.get(pBookID));

  internal void load(Book pBook)
  {
    this._book = pBook;
    string pPath1 = $"books/book_icons/{this._book.getAsset().path_icons}{this._book.data.path_icon}";
    string pPath2 = "books/book_covers/" + this._book.data.path_cover;
    Sprite sprite1 = SpriteTextureLoader.getSprite(pPath1);
    Sprite sprite2 = SpriteTextureLoader.getSprite(pPath2);
    this.icon.sprite = sprite1;
    this.cover.sprite = sprite2;
    ((Object) ((Component) this).gameObject).name = this._book.getAsset().id;
  }

  private void showTooltip()
  {
    Tooltip.show((object) ((Component) this).gameObject, "book", new TooltipData()
    {
      book = this._book
    });
  }
}
