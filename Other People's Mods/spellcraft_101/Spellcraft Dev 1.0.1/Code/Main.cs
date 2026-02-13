using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NCMS.Utils;
using UnityEngine;
using NCMS;
using ReflectionUtility;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spellcraft{
    [ModEntry]
    class Mody : MonoBehaviour{
        void Awake(){
            Spells.init();
            Items.init();




            //ITEM EFFECTS
       }
        public void Update(){
          Equipment();
        }
        static string civId = "unit_human" + "unit_elf" + "unit_orc" + "unit_dwarf" + "baby_human" + "baby_elf" + "baby_orc" + "baby_dwarf";
        void Equipment(){
          var Units = MapBox.instance.units.getSimpleList();
          foreach(var unit in Units)
            {
              if(civId.Contains(unit.stats.id)){
                var pSlot = unit.equipment.getSlot(EquipmentType.Amulet);
                if(pSlot.data != null){
                  if(pSlot.data.id == "Ruby Artifact"){
                    unit.addTrait("Unknown Spell: Enraging Shield");
                    unit.removeTrait("Rare Spell: Arcanist Shield");

                        } 
                    }
                  }
                }
        }
    }
}