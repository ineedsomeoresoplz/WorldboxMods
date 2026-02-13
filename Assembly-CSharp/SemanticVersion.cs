// Decompiled with JetBrains decompiler
// Type: SemanticVersion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
public sealed class SemanticVersion : 
  IComparable,
  IComparable<SemanticVersion>,
  IEquatable<SemanticVersion>
{
  private const RegexOptions _flags = RegexOptions.ExplicitCapture | RegexOptions.Compiled;
  private static readonly Regex _semanticVersionRegex = new Regex("^(?<Version>\\d+(\\s*\\.\\s*\\d+){0,3})(?<Release>-([0]\\b|[0]$|[0][0-9]*[A-Za-z-]+|[1-9A-Za-z-][0-9A-Za-z-]*)+(\\.([0]\\b|[0]$|[0][0-9]*[A-Za-z-]+|[1-9A-Za-z-][0-9A-Za-z-]*)+)*)?(?<Metadata>\\+[0-9A-Za-z-]+(\\.[0-9A-Za-z-]+)*)?$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
  private static readonly Regex _strictSemanticVersionRegex = new Regex("^(?<Version>([0-9]|[1-9][0-9]*)(\\.([0-9]|[1-9][0-9]*)){2})(?<Release>-([0]\\b|[0]$|[0][0-9]*[A-Za-z-]+|[1-9A-Za-z-][0-9A-Za-z-]*)+(\\.([0]\\b|[0]$|[0][0-9]*[A-Za-z-]+|[1-9A-Za-z-][0-9A-Za-z-]*)+)*)?(?<Metadata>\\+[0-9A-Za-z-]+(\\.[0-9A-Za-z-]+)*)?$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
  private readonly string _originalString;
  private string _normalizedVersionString;

  public SemanticVersion(string version)
    : this(SemanticVersion.Parse(version))
  {
    this._originalString = version;
  }

  public SemanticVersion(int major, int minor, int build, int revision)
    : this(new Version(major, minor, build, revision))
  {
  }

  public SemanticVersion(int major, int minor, int build, string specialVersion)
    : this(new Version(major, minor, build), specialVersion)
  {
  }

  public SemanticVersion(int major, int minor, int build, string specialVersion, string metadata)
    : this(new Version(major, minor, build), specialVersion, metadata)
  {
  }

  public SemanticVersion(Version version)
    : this(version, string.Empty)
  {
  }

  public SemanticVersion(Version version, string specialVersion)
    : this(version, specialVersion, (string) null, (string) null)
  {
  }

  public SemanticVersion(Version version, string specialVersion, string metadata)
    : this(version, specialVersion, metadata, (string) null)
  {
  }

  private SemanticVersion(
    Version version,
    string specialVersion,
    string metadata,
    string originalString)
  {
    this.Version = !(version == (Version) null) ? SemanticVersion.NormalizeVersionValue(version) : throw new ArgumentNullException(nameof (version));
    this.SpecialVersion = specialVersion ?? string.Empty;
    this.Metadata = metadata;
    this._originalString = string.IsNullOrEmpty(originalString) ? version.ToString() + (!string.IsNullOrEmpty(specialVersion) ? "-" + specialVersion : (string) null) + (!string.IsNullOrEmpty(metadata) ? "+" + metadata : (string) null) : originalString;
  }

  internal SemanticVersion(SemanticVersion semVer)
  {
    this._originalString = semVer.ToOriginalString();
    this.Version = semVer.Version;
    this.SpecialVersion = semVer.SpecialVersion;
    this.Metadata = semVer.Metadata;
  }

  public Version Version { get; private set; }

  public string SpecialVersion { get; private set; }

  public string Metadata { get; private set; }

  public string[] GetOriginalVersionComponents()
  {
    if (string.IsNullOrEmpty(this._originalString))
      return SemanticVersion.SplitAndPadVersionString(this.Version.ToString());
    int length = this._originalString.IndexOfAny(new char[2]
    {
      '-',
      '+'
    });
    return SemanticVersion.SplitAndPadVersionString(length == -1 ? this._originalString : this._originalString.Substring(0, length));
  }

  private static string[] SplitAndPadVersionString(string version)
  {
    string[] sourceArray = version.Split('.', StringSplitOptions.None);
    if (sourceArray.Length == 4)
      return sourceArray;
    string[] destinationArray = new string[4]
    {
      "0",
      "0",
      "0",
      "0"
    };
    Array.Copy((Array) sourceArray, 0, (Array) destinationArray, 0, sourceArray.Length);
    return destinationArray;
  }

  public static SemanticVersion Parse(string version)
  {
    if (string.IsNullOrEmpty(version))
      throw new ArgumentException("Value cannot be null or an empty string", nameof (version));
    SemanticVersion semanticVersion;
    if (!SemanticVersion.TryParse(version, out semanticVersion))
      throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "'{0}' is not a valid version string.", (object) version), nameof (version));
    return semanticVersion;
  }

  public static bool TryParse(string version, out SemanticVersion value)
  {
    return SemanticVersion.TryParseInternal(version, SemanticVersion._semanticVersionRegex, out value);
  }

  public static bool TryParseStrict(string version, out SemanticVersion value)
  {
    return SemanticVersion.TryParseInternal(version, SemanticVersion._strictSemanticVersionRegex, out value);
  }

  private static bool TryParseInternal(string version, Regex regex, out SemanticVersion semVer)
  {
    semVer = (SemanticVersion) null;
    if (string.IsNullOrEmpty(version))
      return false;
    Match match = regex.Match(version.Trim());
    Version result;
    if (!match.Success || !Version.TryParse(match.Groups["Version"].Value, out result))
      return false;
    semVer = new SemanticVersion(SemanticVersion.NormalizeVersionValue(result), SemanticVersion.RemoveLeadingChar(match.Groups["Release"].Value), SemanticVersion.RemoveLeadingChar(match.Groups["Metadata"].Value), version.Replace(" ", ""));
    return true;
  }

  private static string RemoveLeadingChar(string s)
  {
    return s != null && s.Length > 0 ? s.Substring(1, s.Length - 1) : s;
  }

  public static SemanticVersion ParseOptionalVersion(string version)
  {
    SemanticVersion optionalVersion;
    SemanticVersion.TryParse(version, out optionalVersion);
    return optionalVersion;
  }

  private static Version NormalizeVersionValue(Version version)
  {
    return new Version(version.Major, version.Minor, Math.Max(version.Build, 0), Math.Max(version.Revision, 0));
  }

  public int CompareTo(object obj)
  {
    if (obj == null)
      return 1;
    SemanticVersion other = obj as SemanticVersion;
    return !(other == (SemanticVersion) null) ? this.CompareTo(other) : throw new ArgumentException("Type to compare must be an instance of SemanticVersion.", nameof (obj));
  }

  public int CompareTo(SemanticVersion other)
  {
    if ((object) other == null)
      return 1;
    int num = this.Version.CompareTo(other.Version);
    if (num != 0)
      return num;
    bool flag1 = string.IsNullOrEmpty(this.SpecialVersion);
    bool flag2 = string.IsNullOrEmpty(other.SpecialVersion);
    if (flag1 & flag2)
      return 0;
    if (flag1)
      return 1;
    return flag2 ? -1 : SemanticVersion.CompareReleaseLabels((IEnumerable<string>) this.SpecialVersion.Split('.', StringSplitOptions.None), (IEnumerable<string>) other.SpecialVersion.Split('.', StringSplitOptions.None));
  }

  public static bool operator ==(SemanticVersion version1, SemanticVersion version2)
  {
    return (object) version1 == null ? (object) version2 == null : version1.Equals(version2);
  }

  public static bool operator !=(SemanticVersion version1, SemanticVersion version2)
  {
    return !(version1 == version2);
  }

  public static bool operator <(SemanticVersion version1, SemanticVersion version2)
  {
    if (version1 == (SemanticVersion) null)
      throw new ArgumentNullException(nameof (version1));
    return version1.CompareTo(version2) < 0;
  }

  public static bool operator <=(SemanticVersion version1, SemanticVersion version2)
  {
    return version1 == version2 || version1 < version2;
  }

  public static bool operator >(SemanticVersion version1, SemanticVersion version2)
  {
    if (version1 == (SemanticVersion) null)
      throw new ArgumentNullException(nameof (version1));
    return version2 < version1;
  }

  public static bool operator >=(SemanticVersion version1, SemanticVersion version2)
  {
    return version1 == version2 || version1 > version2;
  }

  public override string ToString()
  {
    if (this.IsSemVer2())
      return this.ToNormalizedString();
    int length = this._originalString.IndexOf('+');
    return length > -1 ? this._originalString.Substring(0, length) : this._originalString;
  }

  public string ToNormalizedString()
  {
    if (this._normalizedVersionString == null)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.Version.Major).Append('.').Append(this.Version.Minor).Append('.').Append(Math.Max(0, this.Version.Build));
      if (this.Version.Revision > 0)
        stringBuilder.Append('.').Append(this.Version.Revision);
      if (!string.IsNullOrEmpty(this.SpecialVersion))
        stringBuilder.Append('-').Append(this.SpecialVersion);
      this._normalizedVersionString = stringBuilder.ToString();
    }
    return this._normalizedVersionString;
  }

  public string ToFullString()
  {
    string fullString = this.ToNormalizedString();
    if (!string.IsNullOrEmpty(this.Metadata))
      fullString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}+{1}", (object) fullString, (object) this.Metadata);
    return fullString;
  }

  public string ToOriginalString() => this._originalString;

  public bool IsSemVer2()
  {
    if (!string.IsNullOrEmpty(this.Metadata))
      return true;
    return !string.IsNullOrEmpty(this.SpecialVersion) && this.SpecialVersion.Contains(".");
  }

  public bool Equals(SemanticVersion other)
  {
    return (object) other != null && this.Version.Equals(other.Version) && this.SpecialVersion.Equals(other.SpecialVersion, StringComparison.OrdinalIgnoreCase);
  }

  public override bool Equals(object obj)
  {
    SemanticVersion other = obj as SemanticVersion;
    return (object) other != null && this.Equals(other);
  }

  public override int GetHashCode()
  {
    int hashCode = this.Version.GetHashCode();
    if (this.SpecialVersion != null)
      hashCode = hashCode * 4567 + this.SpecialVersion.GetHashCode();
    return hashCode;
  }

  private static int CompareReleaseLabels(
    IEnumerable<string> version1,
    IEnumerable<string> version2)
  {
    int num = 0;
    using (IEnumerator<string> enumerator1 = version1.GetEnumerator())
    {
      using (IEnumerator<string> enumerator2 = version2.GetEnumerator())
      {
        bool flag1 = enumerator1.MoveNext();
        for (bool flag2 = enumerator2.MoveNext(); flag1 | flag2; flag2 = enumerator2.MoveNext())
        {
          if (!flag1 & flag2)
            return -1;
          if (flag1 && !flag2)
            return 1;
          num = SemanticVersion.CompareRelease(enumerator1.Current, enumerator2.Current);
          if (num != 0)
            return num;
          flag1 = enumerator1.MoveNext();
        }
        return num;
      }
    }
  }

  private static int CompareRelease(string version1, string version2)
  {
    int result1 = 0;
    int result2 = 0;
    bool flag1 = int.TryParse(version1, out result1);
    bool flag2 = int.TryParse(version2, out result2);
    return !(flag1 & flag2) ? (!(flag1 | flag2) ? StringComparer.OrdinalIgnoreCase.Compare(version1, version2) : (!flag1 ? 1 : -1)) : result1.CompareTo(result2);
  }
}
