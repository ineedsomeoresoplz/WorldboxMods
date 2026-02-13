// Decompiled with JetBrains decompiler
// Type: FieldInfoList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class FieldInfoList : MonoBehaviour
{
  public static string color_null = "#9F9F9F";
  public static string color_white = Toolbox.colorToHex(Color32.op_Implicit(Toolbox.color_white));
  public static string color_string = "#F3961F";
  public static string color_enum = Toolbox.colorToHex(Color32.op_Implicit(Toolbox.color_plague));
  public static string color_type = Toolbox.colorToHex(Color32.op_Implicit(Toolbox.color_yellow));
  public static string color_collection = FieldInfoList.color_null;
  public static Dictionary<string, string> selected_field_data;
  public KeyValueField field_prefab;
  public InputField search_input_field;
  public Transform fields_transform;
  private ObjectPoolGenericMono<KeyValueField> _pool_fields;
  internal List<FieldInfo> field_infos = new List<FieldInfo>();
  internal Dictionary<string, FieldInfoListItem> fields_collection_data = new Dictionary<string, FieldInfoListItem>();

  public void init<T>() where T : class => this.init<T>((ListPool<string>) null);

  public void init<T>(ListPool<string> pFieldsToLoad) where T : class
  {
    this.checkInitPool();
    this.field_infos.Clear();
    this.fields_collection_data.Clear();
    FieldInfo[] fields = typeof (T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    Array.Reverse<FieldInfo>(fields);
    bool flag = pFieldsToLoad != null && pFieldsToLoad.Count > 0;
    int num = 0;
    foreach (FieldInfo fieldInfo in fields)
    {
      if (!flag || pFieldsToLoad.Contains(fieldInfo.Name))
      {
        this.field_infos.Add(fieldInfo);
        ++num;
      }
    }
    if (!Object.op_Inequality((Object) this.search_input_field, (Object) null))
      return;
    // ISSUE: method pointer
    ((UnityEvent<string>) this.search_input_field.onValueChanged).AddListener(new UnityAction<string>((object) this, __methodptr(setDataSearched)));
  }

  public void checkInitPool()
  {
    if (this._pool_fields == null)
      this._pool_fields = new ObjectPoolGenericMono<KeyValueField>(this.field_prefab, this.fields_transform);
    else
      this.clear();
  }

  public void setData(object pReference)
  {
    foreach (FieldInfo fieldInfo in this.field_infos)
    {
      FieldInfoListItem fieldData = this.getFieldData(fieldInfo, pReference);
      this.fields_collection_data.Add(fieldData.field_name, fieldData);
      this.addRow(fieldData.field_name, fieldData.field_value);
    }
  }

  public FieldInfoListItem getFieldData(FieldInfo pField, object pReference)
  {
    object pEnumerable1 = pField.GetValue(pReference);
    Type fieldType = pField.FieldType;
    Dictionary<string, string> pCollectionData = (Dictionary<string, string>) null;
    string pValue;
    switch (pEnumerable1)
    {
      case null:
        pValue = Toolbox.coloredText("—", FieldInfoList.color_null);
        break;
      case bool flag:
        pValue = Toolbox.coloredText($"{flag}", flag ? "#43FF43" : "#FB2C21");
        break;
      case string str2:
        string str1 = Toolbox.coloredText("\"", FieldInfoList.color_null);
        pValue = Toolbox.coloredText(str1 + str2 + str1, FieldInfoList.color_string);
        break;
      case int num:
        pValue = Toolbox.coloredText($"{num}", FieldInfoList.color_white);
        break;
      case float pFloat:
        pValue = Toolbox.coloredText(pFloat.ToText() + "f", FieldInfoList.color_white);
        break;
      case Vector2 _:
        Vector2 vector2 = (Vector2) pEnumerable1;
        pValue = Toolbox.coloredText($"Vector2({Toolbox.coloredText(vector2.x.ToText() + "f", FieldInfoList.color_white)}, {Toolbox.coloredText(vector2.y.ToText() + "f", FieldInfoList.color_white)})", FieldInfoList.color_collection);
        break;
      case Vector2Int _:
        Vector2Int vector2Int = (Vector2Int) pEnumerable1;
        pValue = Toolbox.coloredText($"Vector2Int({Toolbox.coloredText(((Vector2Int) ref vector2Int).x.ToText(), FieldInfoList.color_white)}, {Toolbox.coloredText(((Vector2Int) ref vector2Int).y.ToText(), FieldInfoList.color_white)})", FieldInfoList.color_collection);
        break;
      case Enum @enum:
        pValue = Toolbox.coloredText($"{fieldType.Name}.{@enum}", FieldInfoList.color_enum);
        break;
      case Array pEnumerable2:
        pCollectionData = this.enumerableToRowsCompacted((IEnumerable) pEnumerable2);
        pValue = Toolbox.coloredText($"Array<{Toolbox.coloredText(fieldType.GetElementType().Name, FieldInfoList.color_type)}>[{Toolbox.coloredText(pEnumerable2.Length.ToString(), FieldInfoList.color_white)}]", FieldInfoList.color_collection);
        break;
      case IList pEnumerable3:
        pCollectionData = this.enumerableToRowsCompacted((IEnumerable) pEnumerable3);
        pValue = Toolbox.coloredText($"List<{Toolbox.coloredText(fieldType.GetGenericArguments()[0].Name, FieldInfoList.color_type)}>[{Toolbox.coloredText(pEnumerable3.Count.ToString(), FieldInfoList.color_white)}]", FieldInfoList.color_collection);
        break;
      case IDictionary pDictionary:
        pCollectionData = this.dictionaryToRows(pDictionary);
        Type[] genericArguments = fieldType.GetGenericArguments();
        pValue = Toolbox.coloredText($"Dictionary<{Toolbox.coloredText(genericArguments[0].Name, FieldInfoList.color_type)}, {Toolbox.coloredText(genericArguments[1].Name, FieldInfoList.color_type)}>[{Toolbox.coloredText(pDictionary.Count.ToString(), FieldInfoList.color_white)}]", FieldInfoList.color_collection);
        break;
      default:
        if (fieldType.IsGenericType && typeof (HashSet<>) == fieldType.GetGenericTypeDefinition())
        {
          pCollectionData = this.enumerableToRows(pEnumerable1 as IEnumerable);
          pValue = Toolbox.coloredText($"HashSet<{Toolbox.coloredText(fieldType.GetGenericArguments()[0].Name, FieldInfoList.color_type)}>[{Toolbox.coloredText(fieldType.GetProperty("Count").GetValue(pEnumerable1).ToString(), FieldInfoList.color_white)}]", FieldInfoList.color_collection);
          break;
        }
        pValue = Toolbox.coloredText(pEnumerable1.GetType().Name, FieldInfoList.color_type);
        break;
    }
    return new FieldInfoListItem(pField.Name, pValue, pCollectionData);
  }

  public KeyValueField addRow(string pName, string pValue)
  {
    KeyValueField next = this._pool_fields.getNext();
    next.name_text.text = pName;
    next.value.text = pValue;
    FieldInfoListItem fieldInfoListItem;
    if (this.fields_collection_data.TryGetValue(pName, out fieldInfoListItem))
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      FieldInfoList.\u003C\u003Ec__DisplayClass18_0 cDisplayClass180 = new FieldInfoList.\u003C\u003Ec__DisplayClass18_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.tCollectionContent = fieldInfoListItem.collection_data;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass180.tCollectionContent == null || cDisplayClass180.tCollectionContent.Count == 0)
      {
        ((Behaviour) ((Component) next.value).GetComponent<TipButton>()).enabled = false;
      }
      else
      {
        ((Behaviour) ((Component) next.value).GetComponent<TipButton>()).enabled = true;
        // ISSUE: method pointer
        next.on_hover_value = new UnityAction((object) cDisplayClass180, __methodptr(\u003CaddRow\u003Eb__0));
        // ISSUE: method pointer
        next.on_hover_value_out = new UnityAction((object) null, __methodptr(hideTooltip));
      }
    }
    return next;
  }

  internal void setDataSearched(string pValue)
  {
    this.clear();
    pValue = pValue.ToLower();
    if (string.IsNullOrEmpty(pValue))
    {
      int pIndex = 0;
      foreach (FieldInfoListItem fieldInfoListItem in this.fields_collection_data.Values)
      {
        this.setOddEvenColor(this.addRow(fieldInfoListItem.field_name, fieldInfoListItem.field_value), pIndex);
        ++pIndex;
      }
    }
    else
    {
      int pIndex = 0;
      foreach (FieldInfoListItem fieldInfoListItem in this.fields_collection_data.Values)
      {
        if (fieldInfoListItem.field_name.ToLower().Contains(pValue))
        {
          this.setOddEvenColor(this.addRow(fieldInfoListItem.field_name, fieldInfoListItem.field_value), pIndex);
          ++pIndex;
        }
      }
    }
  }

  private void setOddEvenColor(KeyValueField pComponent, int pIndex)
  {
    if (pIndex % 2 == 0)
      pComponent.setEvenColor();
    else
      pComponent.setOddColor();
  }

  private Dictionary<string, string> enumerableToRowsCompacted(IEnumerable pEnumerable)
  {
    Dictionary<string, int> dictionary = new Dictionary<string, int>();
    int num1 = 0;
    foreach (object p in pEnumerable)
    {
      string key = p.ToString();
      if (dictionary.ContainsKey(key))
      {
        ++dictionary[key];
      }
      else
      {
        dictionary.Add(key, 1);
        ++num1;
      }
    }
    string hex = Toolbox.colorToHex(Color32.op_Implicit(Toolbox.color_yellow));
    Dictionary<string, string> rowsCompacted = new Dictionary<string, string>();
    int num2 = 0;
    foreach (KeyValuePair<string, int> keyValuePair in dictionary)
    {
      string str = keyValuePair.Value.ToString();
      rowsCompacted.Add(keyValuePair.Key + "    ", Toolbox.coloredText("x      " + str, hex));
      ++num2;
    }
    return rowsCompacted;
  }

  private Dictionary<string, string> enumerableToRows(IEnumerable pEnumerable)
  {
    Dictionary<string, string> rows = new Dictionary<string, string>();
    int num = 0;
    foreach (object p in pEnumerable)
    {
      rows.Add($"[{num}]     ", p.ToString());
      ++num;
    }
    return rows;
  }

  private Dictionary<string, string> dictionaryToRows(IDictionary pDictionary)
  {
    Dictionary<string, string> rows = new Dictionary<string, string>();
    foreach (object key in (IEnumerable) pDictionary.Keys)
      rows.Add($"[\"{key}\"]", pDictionary[key].ToString());
    return rows;
  }

  public void clear() => this._pool_fields.clear();
}
