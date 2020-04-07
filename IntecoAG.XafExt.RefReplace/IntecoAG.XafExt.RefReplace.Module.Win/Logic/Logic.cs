using DevExpress.ExpressApp;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.XafExt.RefReplace.Module.Win.Logic {
public static    class Logic {
     
      

        public static ReferenceTable  FindAllRef(Type type) {
            ReferenceTable table = null;
            List<ReferenceItem>  items = new List<ReferenceItem>();
            var info = XafTypesInfo.CastTypeToTypeInfo(type);
            var b = XafTypesInfo.CastTypeToTypeInfo(typeof(BaseObject));
            Boolean findRef(ITypeInfo i) {
              if (!info.Descendants.Contains(i)&& i.Base.IsAssignableFrom(b)) {
                 //так как дочерние типы "не знают" о свойствах родителя, в таблицу занесем классы, не имеющие родителей
                        return true;
                    
                  
                }
                return false;
            }
            //Point first = Array.Find(points,);

         IEnumerable<ITypeInfo> types =   info.GetDependentTypes(findRef); 
        foreach (var r in types) {
                var t = XafTypesInfo.CastTypeInfoToType(r);
                //найти имя модуля для этого класса и имя этого свойства
                var properties = from p in t.GetProperties()
                                 where p.PropertyType == type
                                 select p;
                foreach (var p in properties)
                {
              

                    ReferenceItem i = new ReferenceItem();
                    i.NameField = p.Name;
                    i.NameModule = p.Module.ToString();
                    items.Add(i);
              
                }
              table = new ReferenceTable();
                table.Items = items;
            }


            return table;
        }
    }
}
