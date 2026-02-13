using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReflectionUtility;
namespace Nationalist
{
    class TraitGroup
    {
 
        public static string Nationalist = "Nationalist";
 
        public static void init()
        {

 
            ActorTraitGroupAsset Nationalist = new ActorTraitGroupAsset();
            Nationalist.id = "Nationalist";
            Nationalist.name = "trait_group_Nationalist";
            Nationalist.color = Toolbox.makeColor("#FFFFFF", -1f);
            AssetManager.trait_groups.add(Nationalist);
            addTraitGroupToLocalizedLibrary(Nationalist.id, "Nationalist");
 
 
        }
        private static void addTraitGroupToLocalizedLibrary(string id, string name)
        {
            string language = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "language") as string;
            Dictionary<string, string> localizedText = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "localizedText") as Dictionary<string, string>;
            localizedText.Add("trait_group_" + id, name);
        }
    }
}
