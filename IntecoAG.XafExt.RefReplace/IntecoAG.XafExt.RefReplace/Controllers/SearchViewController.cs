using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Helpers;
using IntecoAG.XafExt.RefReplace.BusinessObjects;
using Npgsql;

namespace IntecoAG.XafExt.RefReplace.Controllers {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SearchViewController : ViewController {

        public SearchViewController() {
            InitializeComponent();
            foreach (object current in Enum.GetValues(typeof(Status))) {
                EnumDescriptor ed = new EnumDescriptor(typeof(Status));
             
   
            }

        }



       

        protected override void OnActivated() {
            base.OnActivated();
            ReplaceTable replaceTable = View.CurrentObject as ReplaceTable;
            if (replaceTable != null) {
                if (replaceTable.NewId == null) {
                    showNewAction.Active.SetItemValue("Active", false);
                }


                if (replaceTable.CurrentType != null) {
                    Guid g = Guid.Parse(replaceTable.OldId);
                    var obj = ObjectSpace.GetObjectByKey(replaceTable.CurrentType, g);
                    if (replaceTable.Status == Status.CREATED) {



                    }
                    else {
                        replaceTable.IsPassed = true;
                        var del = replaceTable.CurrentType.GetCustomAttribute<DeferredDeletionAttribute>(false);
                        if (del != null && !replaceTable.CurrentType.IsAssignableFrom(typeof(ISupportRefReplace)) ||
                            replaceTable.CurrentType.IsAssignableFrom(typeof(ISupportRefReplace)) ||
                            ((ISupportRefReplace)obj).IsCanDeleted) {



                        }
                    }
                }
            }


            // Perform various tasks depending on the target View.
        }

 

        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            //List<ChoiceActionItem> l = new List<ChoiceActionItem>();
            RefreshItems();
            
        
        }
        protected override void OnDeactivated() {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void ShowLookupAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
            ReplaceTable replaceTable = View.CurrentObject as ReplaceTable;
            Type t1 = e.PopupWindowViewCurrentObject.GetType();
            ITypeInfo info = XafTypesInfo.CastTypeToTypeInfo(t1);

            IMemberInfo m = info.KeyMember;
            String nameKey = m.Name;
            var y = e.PopupWindowViewCurrentObject.GetType().GetProperty(nameKey).GetValue(e.PopupWindowViewCurrentObject);
            replaceTable.NewId = y.ToString();
            var x = info.DefaultMember;
            object u = null;
            if (x != null) {
                u = e.PopupWindowViewCurrentObject.GetType().GetProperty(x.Name).GetValue(e.PopupWindowViewCurrentObject);
            }

            replaceTable.NewName = u==null? "": u.ToString();
            showNewAction.Active.SetItemValue("Active", true);
            //заполним таблицу атрибутами старого и нового объектов
            var inf = XafTypesInfo.CastTypeToTypeInfo(e.PopupWindowViewCurrentObject.GetType());
            int i = 1;
            replaceTable.Attributes[0].NewValue = replaceTable.NewId;
          foreach (var a in inf.OwnMembers) {
                bool b = false;
                BrowsableAttribute br = a.FindAttribute<BrowsableAttribute>();
                if (br != null) {
                    if (br.Browsable == false) {
                        b = true;
                    }
                }
                if (a.IsPersistent && a.IsProperty&&!b) {
                    if (i >= replaceTable.Attributes.Count) break;
                    replaceTable.Attributes[i].NewValue = a.GetValue(e.PopupWindowViewCurrentObject) == null ? "null" : a.GetValue(e.PopupWindowViewCurrentObject).ToString();
                    i++;
      

                }
   
            }

            if (replaceTable.OldId == replaceTable.NewId) {
                replaceTable.NewId = null;
                replaceTable.NewName = null;


            }
            RefreshItems();


        }


        private void simpleTestAction1_Execute(object sender, SimpleActionExecuteEventArgs e) {

            if (e.CurrentObject is ReplaceTable) return;
            Type t1 = e.CurrentObject.GetType();
          var att=  XafTypesInfo.CastTypeToTypeInfo(t1).FindAttribute<PersistentAttribute>();
          var con =  ConfigurationManager.ConnectionStrings["ConnectionString"];
          var key=  XafTypesInfo.CastTypeToTypeInfo(t1).KeyMember;
          var kp=  key.FindAttribute<PersistentAttribute>();

            IObjectSpace objectSpace = Application.CreateObjectSpace();
            string noteListViewId = Application.FindLookupListViewId(typeof(ReplaceTable));
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(ReplaceTable), noteListViewId);
             Type type = View.CurrentObject.GetType();

            var module = Application.Modules.FindModule<RefReplaceModule>();
            ReferenceTable rep = Logic.FindAllRef(objectSpace, View.CurrentObject, Application.Model, module);

            ReplaceTable replaceTable = objectSpace.CreateObject<ReplaceTable>();
            replaceTable.NameTable = att == null ? t1.Name : att.MapTo;
            replaceTable.KeyPropCurrentType = kp == null ? key.Name : kp.MapTo;
            //singleChoiceAction1.Items.Remove(singleChoiceAction1.Items.Last());
            List<ReferenceItem> items = new List<ReferenceItem>();
            items.AddRange(rep.Items);
            replaceTable.Items.AddRange(items);
            List<ObjItem> objs = new List<ObjItem>();
            objs.AddRange(rep.Objects);
            replaceTable.Objects.AddRange(objs);
            replaceTable.DateCreate = rep.DateCreate;
            replaceTable.CurrentType = t1;

            

            ITypeInfo info = XafTypesInfo.CastTypeToTypeInfo(t1);
            if (!info.IsPersistent) {
                return;
            }

            IMemberInfo m = info.KeyMember;
            String nameKey = m == null ? null : m.Name;
            //найти значение поля с таким именем
            
                var y = e.CurrentObject.GetType().GetProperty(nameKey).GetValue(e.CurrentObject);
                replaceTable.OldId = y.ToString();

            //заполним странные атрибуты для appereance

            Guid g = Guid.Parse(replaceTable.OldId);
            var obj = ObjectSpace.GetObjectByKey(replaceTable.CurrentType, g);
            var d = XafTypesInfo.CastTypeToTypeInfo(t1).FindAttribute<DeferredDeletionAttribute>();
            if (d != null) {
                replaceTable.DeferredDel = true;
            }

          
            replaceTable.IsCanDeleted = true;
            

            if (obj is ISupportRefReplace) {
                replaceTable.SupportRef = true;
                replaceTable.IsCanClose = ((ISupportRefReplace)obj).IsCanClose;
                replaceTable.IsCanDeleted = ((ISupportRefReplace)obj).IsCanDeleted;
            }
            if (obj is ISupportEtalon) {
                replaceTable.SupportEtalon = true;
            }

            var x = info.DefaultMember;
            String u = e.CurrentObject.ToString();
            if (x != null) {
                u = e.CurrentObject.GetType().GetProperty(x.Name).GetValue(e.CurrentObject) == null ? "" :
                     e.CurrentObject.GetType().GetProperty(x.Name).GetValue(e.CurrentObject).ToString();
            }

            replaceTable.OldName = u;
            var mems = XafTypesInfo.CastTypeToTypeInfo(e.CurrentObject.GetType()).OwnMembers;
            SimpleAttribute attribute = new SimpleAttribute(replaceTable.Session);
            attribute.NameAtt = nameKey;
            attribute.LocalName = nameKey;
            attribute.NameType = y.GetType().ToString();
            attribute.OldValue = replaceTable.OldId;
            replaceTable.Attributes.Add(attribute);
            foreach (var mem in mems) {
                bool b = false;
                BrowsableAttribute br =mem.FindAttribute<BrowsableAttribute>();
                if (br != null) {
                   if(br.Browsable == false) {
                        b = true;
                    }
                }
                if (mem.IsProperty &&mem.IsPersistent&&!b) {
                    SimpleAttribute a = new SimpleAttribute(replaceTable.Session);
                    a.NameAtt = mem.Name;
                   var n = Application.Model.BOModel.GetNode(replaceTable.CurrentType.FullName);

            
                    var val = n.GetNode("OwnMembers").GetNode(mem.Name);

                    a.NameType = mem.MemberType.ToString();
                    a.LocalName = val.GetValue<String>("Caption");

                    a.OldValue = mem.GetValue(e.CurrentObject) == null ? "null" : mem.GetValue(e.CurrentObject).ToString();
                    replaceTable.Attributes.Add(a);
                }
            }

      
            View v = Application.CreateDetailView(objectSpace, replaceTable);
           
      
            e.ShowViewParameters.CreatedView = v;
   
        }
        private void RefreshItems() {
        
            ReplaceTable replaceTable = View.CurrentObject as ReplaceTable;

            if (replaceTable != null) {
                ISecurityUserWithRoles user = SecuritySystem.CurrentUser as ISecurityUserWithRoles;
                Boolean isAdm = false;
                foreach (var role in user.Roles) {
                    if (role.Name!=null && role.Name.Contains("Admin")) {
                        isAdm = true;break;
                    }
                    //if (role.IsAdministrative) {
                    //    isAdm = true; break;
                    //}
                }
                var d = Frame.GetController<DeleteObjectsViewController>();
                if (d != null) {

                    d.DeleteAction.Active.SetItemValue("Active", isAdm);
                }
                passAction.Active.SetItemValue("Active", false);
                ApplyAction.Active.SetItemValue("Active", false);
                RejectedAction.Active.SetItemValue("Active", false);
                if (replaceTable.Status == Status.CREATED && replaceTable.NewId!=null) {
                    passAction.Active.SetItemValue("Active", true);
                }
                if (replaceTable.Status == Status.PASSED && replaceTable.NewId != null) {
                    if (isAdm) {
                        ApplyAction.Active.SetItemValue("Active", true);
                        RejectedAction.Active.SetItemValue("Active", true);
                    }
                }
                if (replaceTable.Status == Status.APPLIED && replaceTable.NewId != null) {
                    d.DeleteAction.Active.SetItemValue("Active", false);
                    passAction.Active.SetItemValue("Active", false);
                    ApplyAction.Active.SetItemValue("Active", false);
                    RejectedAction.Active.SetItemValue("Active", false);
                }
                if (replaceTable.Status == Status.REJECTED && replaceTable.NewId != null) {
                    passAction.Active.SetItemValue("Active", false);
                    ApplyAction.Active.SetItemValue("Active", false);
                    RejectedAction.Active.SetItemValue("Active", false);
                }
            }
        }


        private void ShowLookupAction_CustomizeTemplate(object sender, DevExpress.ExpressApp.CustomizeTemplateEventArgs e) {
            ((ILookupPopupFrameTemplate)e.Template).IsSearchEnabled = true;
        }

        private void ShowLookupAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            ReplaceTable replaceTable = View.CurrentObject as ReplaceTable;
            Type t1 = replaceTable.CurrentType;
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            string noteListViewId = Application.FindLookupListViewId(t1);
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, t1, noteListViewId, true, CollectionSourceMode.Normal);
            //IModelNode n = Application.Model.Views;
            //IModelListView p = n.GetNode(noteListViewId) as IModelListView;
            e.Context = TemplateContext.LookupWindow;
            e.View = Application.CreateListView(noteListViewId, collectionSource, true);
         
        }

        private void ApplyAction_Execute(object sender, SimpleActionExecuteEventArgs e) {

            ObjectSpace.CommitChanges();

            IObjectSpace objectSpace = View is ListView ? Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);

            foreach (Object obj in objectsToProcess) {
                ReplaceTable objInNewObjectSpace = (ReplaceTable)objectSpace.GetObject(obj);
                Logic.Apply(objInNewObjectSpace, objectSpace);
            }

            if (View is ListView) {
                View.Refresh();
            }
            RefreshItems();
        }

      

        private void showOldAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
           
        }

        private void showOldAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            ReplaceTable replaceTable = View.CurrentObject as ReplaceTable;
            String id = replaceTable.OldId;
            var g = System.Guid.Parse(id);
            IObjectSpace os = Application.CreateObjectSpace();
            var o = os.GetObjectByKey(replaceTable.CurrentType, g);
            e.View = Application.CreateDetailView(os, o, false);
            e.View.AllowEdit.SetItemValue("Info.AllowEdit", true);

        }

        private void showNewAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            
            ReplaceTable replaceTable = View.CurrentObject as ReplaceTable;
            if (replaceTable.NewId == null) { return; }
            String id = replaceTable.NewId;
            var g = System.Guid.Parse(id);
            IObjectSpace os = Application.CreateObjectSpace();
            var o = os.GetObjectByKey(replaceTable.CurrentType, g);
            e.View = Application.CreateDetailView(os, o, false);
            e.View.AllowEdit.SetItemValue("Info.AllowEdit", true);
        }

        private void singleChoiceAction1_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            IObjectSpace objectSpace = View is ListView ?
         Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
           
                foreach (Object obj in objectsToProcess) {
                    ReplaceTable objInNewObjectSpace = (ReplaceTable)objectSpace.GetObject(obj);
                    objInNewObjectSpace.Status = (Status)e.SelectedChoiceActionItem.Data;
                }
            

            if (View is ListView) {
                View.Refresh();
            }
            RefreshItems();

        }

        private void passAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace objectSpace = View is ListView ?
     Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);

            foreach (Object obj in objectsToProcess) {
                ReplaceTable objInNewObjectSpace = (ReplaceTable)objectSpace.GetObject(obj);
                objInNewObjectSpace.Status = Status.PASSED;
                objInNewObjectSpace.DatePassed = DateTime.Now;
                objInNewObjectSpace.IsPassed = true;

            }


            if (View is ListView) {
                View.Refresh();
            }
            RefreshItems();

            ReplaceTable replaceTable = View.CurrentObject as ReplaceTable;
    
            var n = Application.Model.BOModel.GetNode(typeof(ReplaceTable).FullName);
   
            Guid g = Guid.Parse(replaceTable.OldId);
            var obj1 = ObjectSpace.GetObjectByKey(replaceTable.CurrentType, g);
            var del = replaceTable.CurrentType.GetCustomAttribute<DeferredDeletionAttribute>();
            //var h = XafTypesInfo.CastTypeToTypeInfo(replaceTable.CurrentType).FindAttribute<DeferredDeletionAttribute>();
            if (del != null && !replaceTable.CurrentType.IsAssignableFrom(typeof(ISupportRefReplace)) ||
                replaceTable.CurrentType.IsAssignableFrom(typeof(ISupportRefReplace)) ||
                ((ISupportRefReplace)obj1).IsCanDeleted) {
              //  AppearanceAttribute attEnable = new AppearanceAttribute("");
              //  attEnable.Enabled = true;
              //var a=  XafTypesInfo.CastTypeToTypeInfo(typeof(ReplaceTable)).FindMember("ToDelete").FindAttribute<AppearanceAttribute>();
              //var b=  XafTypesInfo.CastTypeToTypeInfo(typeof(ReplaceTable)).FindMember("Replace").FindAttribute<AppearanceAttribute>();
              //  if (a == null) {
              //      XafTypesInfo.CastTypeToTypeInfo(typeof(ReplaceTable)).FindMember("ToDelete").AddAttribute(attEnable);
              //  }
              //  else a.Enabled = true;
              //  if (b == null) {
              //      XafTypesInfo.CastTypeToTypeInfo(typeof(ReplaceTable)).FindMember("Replace").AddAttribute(attEnable);
              //  }
              //  else b.Enabled = true;
         
           

            }
       
            
            
        }

        private void RejectedAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            IObjectSpace objectSpace = View is ListView ?
     Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);

            foreach (Object obj in objectsToProcess) {
                ReplaceTable objInNewObjectSpace = (ReplaceTable)objectSpace.GetObject(obj);
                objInNewObjectSpace.Status = Status.REJECTED;
                objInNewObjectSpace.DateRejected = DateTime.Now;
            }


            if (View is ListView) {
                View.Refresh();
            }
            
            RefreshItems();
        }
    
    }
}
