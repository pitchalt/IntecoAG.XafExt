using DevExpress.ExpressApp;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Common;
using DevExpress.ExpressApp.Xpo;
using Npgsql;
using System.ComponentModel;
using System.Configuration;
using DevExpress.ExpressApp.Model;

namespace IntecoAG.XafExt.RefReplace.BusinessObjects {
    public static class Logic {

        public static void Apply(ReplaceTable replaceTable, IObjectSpace objectSpace) {
            var lizon = ConfigurationManager.ConnectionStrings["ConnectionString"];
            var res = lizon.ConnectionString.Split(new char[] { ';' });
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < res.Length; i++) {
                sb.Append(res[i]);
                if (i < res.Length - 1) {
                    sb.Append(';');
                }

            }

            String s = sb.ToString();
            NpgsqlConnection conn = new NpgsqlConnection(s);
            //ПРИМЕНИТЬ
            conn.Open();
            //ReplaceTable replaceTable = View.CurrentObject as ReplaceTable;
            replaceTable.DateApply = DateTime.Now;
            replaceTable.Status = Status.APPLIED;
            if (replaceTable.NewId != null) {
                var g = System.Guid.Parse(replaceTable.NewId);

                var p = objectSpace.GetObjectByKey(replaceTable.CurrentType, g);

                if (p != null) {
                    if (replaceTable.Replace) {
                        Guid gi = Guid.Parse(replaceTable.OldId);
                        var obj = objectSpace.GetObjectByKey(replaceTable.CurrentType, gi);
                        Guid et = Guid.Parse(replaceTable.NewId);
                        var etalon = objectSpace.GetObjectByKey(replaceTable.CurrentType, et);
                        ((ISupportEtalon)obj).Etalon = etalon;

                    }
                    if (replaceTable.ToDelete) {

                        //в текущий объект добавить gcrecord

                        String sql = "Update " + '"' + replaceTable.NameTable + '"' + " Set " + '"' + "GCRecord" + '"' +
                            " = @val" + " Where " + '"' + replaceTable.KeyPropCurrentType + '"' + " = " + "'" + replaceTable.OldId + "'";

                        NpgsqlCommand cmd = new NpgsqlCommand();
                        cmd.Connection = conn;
                        Random random = new Random();
                        cmd.Parameters.AddWithValue("@val", random.Next(1, 200));
                        cmd.CommandText = sql;

                        cmd.ExecuteNonQuery();


                    }
                    if (replaceTable.ToClose) {
                        //у текущего объекта вызвать метод close
                        Guid gu = Guid.Parse(replaceTable.OldId);
                        var o = objectSpace.GetObjectByKey(replaceTable.CurrentType, gu);
                        ((ISupportRefReplace)o).Close();
                        //((XPBaseObject)o).Save();

                    }
                    foreach (var r in replaceTable.Objects.Where(x => !x.IsAggregated && !x.IsForbidden)) {

                        var id = XafTypesInfo.CastTypeToTypeInfo(p.GetType()).KeyMember.GetValue(p);


                        String sql = "Update " + '"' + r.NameTableOnDB + '"' + " Set " + '"' + r.NamePropOnDB + '"' +
                            " = @val" + " Where " + '"' + r.NamePropOnDB + '"' + " = " + "'" + replaceTable.OldId + "'";

                        NpgsqlCommand cmd = new NpgsqlCommand();
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@val", id);
                        cmd.CommandText = sql;
                        try {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex) {

                        }
                    }
                    replaceTable.Status = Status.APPLIED;
                }
            }
            conn.Close();
            objectSpace.CommitChanges();

        }


        public static ReferenceTable FindAllRef(IObjectSpace os, Object current, IModelApplication model, RefReplaceModule module) {
            Type type = current.GetType();
            //NpgsqlConnection conn = new NpgsqlConnection("Server='localhost';User Id=pg_adm;Password='flesh*token=across';Database=ermdevmes;");
            var lizon =  ConfigurationManager.ConnectionStrings["ConnectionString"];
            var res =  lizon.ConnectionString.Split(new char[] { ';' });
            StringBuilder sb = new StringBuilder();
            for (int i=1; i<res.Length; i++) {
                sb.Append(res[i]);
                if (i < res.Length - 1) {
                    sb.Append(';');
                }
               
            }
            
            String s = sb.ToString();
            NpgsqlConnection conn = new NpgsqlConnection(s);
            conn.Open();
            Object currentObj = current;

            ReferenceTable table = os.CreateObject<ReferenceTable>();
            table.DateCreate = DateTime.Now;
            var info = XafTypesInfo.CastTypeToTypeInfo(type);

            ISupportRefReplace refreplace_support = current as ISupportRefReplace;

            var forbidden_types = module.ForbiddenTypesGet(info);
            var forbidden_members = module.ForbiddenMembersGet(info);

            //ITypeInfo b = XafTypesInfo.CastTypeToTypeInfo(typeof(BaseObject));
            Boolean findRef(ITypeInfo i) {
                return !i.IsInterface && i.IsPersistent;
            }

            IEnumerable<ITypeInfo> allTypes = info.GetDependentTypes(findRef);
            foreach (var r in allTypes) {

                var t = XafTypesInfo.CastTypeInfoToType(r);
                var h = from w in r.OwnMembers
                        where w.IsPersistent && w.MemberType == type
                        select w;
               
                foreach (var f in h) {
                    String nameField = "";
//                    f.Per
                    var alias = f.FindAttribute<PersistentAliasAttribute>();
                    var persik = f.FindAttribute<PersistentAttribute>();
                    if (alias == null) {
                        if (persik != null) {
                            nameField = persik.MapTo == null ? f.Name : persik.MapTo;
                        }


                    }
                    else {
                        nameField = alias.AliasExpression; //РАЗОБРАТЬСЯ!!!
                    }
                    if (alias == null && persik == null) {
                        nameField = f.Name;
                        
                    }

                    String nameTable = "";
                    var persatt = r.FindAttribute<PersistentAttribute>();
                    if (persatt == null) {
                        var map = r.FindAttribute<MapInheritanceAttribute>();
                        if (map != null) {
                            //вероятно, хранится в таблице родителя
                            if (map.MapType == MapInheritanceType.ParentTable) {
                                var att = r.Base.FindAttribute<PersistentAttribute>();
                                nameTable = att == null ? r.Base.Name : att.MapTo;

                            }
                        }
                        else { nameTable = r.Name; }
                    }
                    else {
                        nameTable = persatt.MapTo;
                    }
                    ReferenceItem i = os.CreateObject<ReferenceItem>();
                    i.NamePropOnDB = nameField;

                    i.NameProp = f.Name;
                    i.IsAggregated = f.IsAggregated;
                    i.IsForbidden = forbidden_types.Contains(r) || forbidden_members.Contains(f);
                    i.NameType = r.FullName;
                    i.NameModule = t.Module.ToString();
                    i.NameTable = nameTable == "" ? r.Name : nameTable;
                    i.Type = t;
                    table.Items.Add(i);
                }


            }
            String id = XafTypesInfo.CastTypeToTypeInfo(currentObj.GetType()).KeyMember.GetValue(currentObj).ToString();
            foreach (var i in table.Items) {
                Type t = i.Type;
                String tableName = i.NameTable;
                var class_type_info = XafTypesInfo.CastTypeToTypeInfo(i.Type);

                var keyMember = class_type_info.Members.FirstOrDefault(x => x.IsKey);
                var pers = keyMember.FindAttribute<PersistentAttribute>();
                var pers_key_name = pers == null ? keyMember.Name : pers.MapTo;

                var inf = XafTypesInfo.CastTypeToTypeInfo(t);
                //var del = inf.FindAttribute<DeferredDeletionAttribute>();
                var del = inf.FindAttribute<DeferredDeletionAttribute>(false);
                string DelField = "";
                if (del != null) {
                    DelField = " AND " + '"' + "GCRecord" + '"' + " is NULL";
                }
                if (tableName == "FmBudgetOrderIdDoc") {

                }

                String sql = @"Select " + '"' + pers_key_name+ '"' + @" From public." + '"' + tableName + '"' + " Where " + '"' + i.NamePropOnDB + '"' + " = '" +
                id + "'" + DelField;




                using (var cmd = new NpgsqlCommand()) {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    using (var reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {

                            ObjItem oi = os.CreateObject<ObjItem>();
                            oi.NamePropOnDB = i.NamePropOnDB;

                            oi.IsAggregated = i.IsAggregated;
                            oi.IsForbidden = i.IsForbidden;

                            oi.NameType = i.NameType;
                            oi.NameProp = i.NameProp;
                            oi.NameTableOnDB = tableName;
                            var ord = reader.GetOrdinal(pers_key_name);
                            var oid = reader.GetValue(ord);
                            oi.ID = oid.ToString();
                            var d = model.BOModel.GetNode(oi.NameType);
                            oi.NameTableLocal = d.GetValue<String>("Caption");

                            var p = d.GetNode("OwnMembers").GetNode(oi.NameProp);
                            if (p != null) {
                                oi.NamePropLocal = p.GetValue<String>("Caption");
                            }
                      
                            table.Objects.Add(oi);
                        }
                    }

                }
              
            }
            conn.Close();
            return table;
        }
    }
}
