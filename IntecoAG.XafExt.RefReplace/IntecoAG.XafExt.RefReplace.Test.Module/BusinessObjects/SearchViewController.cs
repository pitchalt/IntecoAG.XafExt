using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.Helpers;
using IntecoAG.XafExt.RefReplace.Module.Win.Logic;
using IntecoAG.XafExt.RefReplace.Test.Module.BusinessObjects;

namespace IntecoAG.XafExt.RefReplace.Controllers {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SearchViewController : ViewController {

        public SearchViewController() {
            InitializeComponent();
            foreach (object current in Enum.GetValues(typeof(Status))) {
                EnumDescriptor ed = new EnumDescriptor(typeof(Status));
                ChoiceActionItem item = new ChoiceActionItem(ed.GetCaption(current), current);
                singleChoiceAction1.Items.Add(item);
            }

        }
  

          
        

        protected override void OnActivated() {
            base.OnActivated();
 
            // Perform various tasks depending on the target View.
        }

 

        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            List<ChoiceActionItem> l = new List<ChoiceActionItem>();
            ReplaceTable replaceTable = View.CurrentObject as ReplaceTable;
            Boolean b = false;
          if (replaceTable!=null)
            foreach (var r in singleChoiceAction1.Items) {
             
                switch (replaceTable.Status) {
                    case Status.CREATED:
                        b = ((Status)r.Data) == Status.REJECTED || ((Status)r.Data) == Status.PASSED || ((Status)r.Data) == Status.CREATED;
                        break;
                    case Status.CANCELED:
                        singleChoiceAction1.Items.Clear();
                        return;
                    case Status.APPLIED:
                            b = ((Status)r.Data) == Status.REJECTED || ((Status)r.Data) == Status.APPLIED;
                        break;
                    case Status.REJECTED:
                        singleChoiceAction1.Items.Clear();
                        return;
                    case Status.PASSED:
                            b = ((Status)r.Data) == Status.REJECTED || ((Status)r.Data) == Status.APPLIED || ((Status)r.Data) == Status.PASSED;
                        break;

                }
                    if (replaceTable.OldId == replaceTable.NewId) {
                        singleChoiceAction1.Items.Clear();
                        return;
                    }
                if (!b) {
                    l.Add(r);
                }
            }
            foreach (var r in l) {
                singleChoiceAction1.Items.Remove(r);
            }
            // Access and customize the target View control.
        }
        protected override void OnDeactivated() {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        Type type = null;
        private void parametrizedAction1_Execute(object sender, ParametrizedActionExecuteEventArgs e) {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            string paramValue = e.ParameterCurrentValue as string;
    
            IModelBOModel m = Application.Model.BOModel;
            IModelClass modelClass = null;
            List<IModelNode> nodes = new List<IModelNode>();
            foreach (var r in m) {
                if (r.Name.ToUpper().Contains(paramValue.ToUpper())) {
                    modelClass = r;
                }
            }
            type = Type.GetType(modelClass.Name);
            
            ReferenceTable t = Logic.FindAllRef( Application.CreateObjectSpace(), e.CurrentObject);
            
            DetailView detailView = Application.CreateDetailView(objectSpace, t);
            detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            e.ShowViewParameters.CreatedView = detailView;



        }

        private void popupWindowShowAction1_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            string noteListViewId = Application.FindLookupListViewId(typeof(ReplaceTable));
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(ReplaceTable), noteListViewId);


            type = View.CurrentObject.GetType();
  

            ReferenceTable t = Logic.FindAllRef( objectSpace, View.CurrentObject);
          
            ReplaceTable replace = objectSpace.CreateObject<ReplaceTable>();
          
            replace.Items.AddRange(t.Items);
            replace.Objects.AddRange(t.Objects);
            e.View = Application.CreateDetailView(objectSpace, replace);

        }

        private void Session_ObjectsLoaded(object sender, ObjectsManipulationEventArgs e) {
            throw new NotImplementedException();
        }

        private void Session_ObjectLoading(object sender, ObjectManipulationEventArgs e) {
            throw new NotImplementedException();
        }

        private void popupWindowShowAction1_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
            

            //e.ShowViewParameters.CreatedView = p.
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
            var u = e.PopupWindowViewCurrentObject.GetType().GetProperty(x.Name).GetValue(e.PopupWindowViewCurrentObject);
            replaceTable.NewName = u.ToString();
            
        }

        private void TestAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {

        }

        //private void TestAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {

        //    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //    string noteListViewId = Application.FindLookupListViewId(typeof(Party));
        //    CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Party), noteListViewId);

        //    IModelNode n = Application.Model.Views;
        //    IModelListView p = n.GetNode("Party_LookupListView") as IModelListView;
        //    Type t = View.CurrentObject.GetType();
        //    e.View = Application.CreateListView(p, collectionSource, false);

        //}

        private void simpleTestAction1_Execute(object sender, SimpleActionExecuteEventArgs e) {
           
            BaseObject bo = e.CurrentObject as BaseObject;
            if (bo == null) {
                return;
            }

            

        ITypeInfo inf = XafTypesInfo.CastTypeToTypeInfo(bo.Session.GetType());
            var f = bo.Session.GetType().GetField("_StateStack", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
          var L =  bo.Session.GetType().GetProperty("IsObjectsLoading");
            bool b = ((SessionState.GetObjectsNonReenterant & SessionState.LoadingStates) != SessionState.Empty);
         
            f.SetValue(bo.Session, SessionState.GetObjectsNonReenterant);
            
            Type t1 = e.CurrentObject.GetType();
           


            IObjectSpace objectSpace = Application.CreateObjectSpace();
            string noteListViewId = Application.FindLookupListViewId(typeof(ReplaceTable));
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(ReplaceTable), noteListViewId);
            type = View.CurrentObject.GetType();
            ReferenceTable t = Logic.FindAllRef( objectSpace, View.CurrentObject);
           
            ReplaceTable replaceTable = objectSpace.CreateObject<ReplaceTable>();
            //singleChoiceAction1.Items.Remove(singleChoiceAction1.Items.Last());
            List<ReferenceItem> items = new List<ReferenceItem>();
            items.AddRange(t.Items);
            replaceTable.Items.AddRange(items);
            List<ObjItem> objs = new List<ObjItem>();
            objs.AddRange(t.Objects);
            replaceTable.Objects.AddRange(objs);
            replaceTable.DateCreate = t.DateCreate;
            replaceTable.CurrentType = t1;
            //replaceTable.Items.AddRange(items);
       
          
            //найти ключевое свойство через метаданные
            ITypeInfo info = XafTypesInfo.CastTypeToTypeInfo(t1);

            IMemberInfo m = info.KeyMember;
            String nameKey = m.Name;
            //найти значение поля с таким именем
            
            View v = Application.CreateDetailView(objectSpace, replaceTable);
            var y = e.CurrentObject.GetType().GetProperty(nameKey).GetValue(e.CurrentObject);
            replaceTable.OldId = y.ToString();
            var x = info.DefaultMember;
            var u = e.CurrentObject.GetType().GetProperty(x.Name).GetValue(e.CurrentObject);
            replaceTable.OldName = u.ToString();

            e.ShowViewParameters.CreatedView = v;
        }
       

        private void ShowLookupAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {


            ITypeInfo info = XafTypesInfo.CastTypeToTypeInfo(typeof(Test.Module.BusinessObjects.Party));
            var w = info.OwnMembers;
            foreach (var q in w) {
                var mt = q.IsPersistent;
                var last = q.LastMember;
                //и што
            }

            ReplaceTable replaceTable = View.CurrentObject as ReplaceTable;

            Type t1 = replaceTable.CurrentType;
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            string noteListViewId = Application.FindLookupListViewId(t1);
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, t1, noteListViewId);


            IModelNode n = Application.Model.Views;
           
            IModelListView p = n.GetNode(noteListViewId) as IModelListView;
            Type t = View.CurrentObject.GetType();
            e.View = Application.CreateListView(p, collectionSource,false);
         
        }

        private void simpleAction1_Execute(object sender, SimpleActionExecuteEventArgs e) {
            //ПРИМЕНИТЬ
            ReplaceTable replaceTable = View.CurrentObject as ReplaceTable;
            if (replaceTable.OldId == replaceTable.NewId) {
                throw new Exception("Оригинальный объект и объект для замены совпадают");
                
                
            }
            if (replaceTable.NewId == null) {
               
            }
            replaceTable.DateApply = DateTime.Now;
            replaceTable.Status = Status.APPLIED;
            if (replaceTable.NewId != null) {
                var g = System.Guid.Parse(replaceTable.NewId);
                //найти Гришу
                var p = View.ObjectSpace.GetObjectByKey(replaceTable.CurrentType, g);
           
                if (p != null)
                {
                    replaceTable.Session.GetType().GetField("_StateStack", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(replaceTable.Session, SessionState.GetObjectsNonReenterant);
                    foreach (var r in replaceTable.Objects)
                    {
                        var parentType = r.Obj.GetType();
                        //if (r.IsProperty) {


                        //    while (parentType.GetProperty(r.NameProp, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | BindingFlags.Public) == null) {


                        //        var info = XafTypesInfo.CastTypeToTypeInfo(parentType);

                        //        if (!(info.Base.IsPersistent)) {
                        //            break;
                        //        }
                        //        else parentType = parentType.BaseType;


                        //    }
                        //    var prop = parentType.GetProperty(r.NameProp, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | BindingFlags.Public);
                        //    if (prop != null) {
                        //        if (prop.SetMethod == null) {

                        //        }
                        //        prop.SetValue(View.ObjectSpace.GetObject(r.Obj), p);
                        //    }
                        //}
                        //else {


                            //это было поле


                            while (parentType.GetField(r.NameProp, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | BindingFlags.Public) == null) {


                                var info = XafTypesInfo.CastTypeToTypeInfo(parentType);


                                if (!(info.Base.IsPersistent)) {
                                    break;
                                }
                                else parentType = parentType.BaseType;



                            }
                            //throw new Exception(r.NameProp + " " + parentType.ToString());
                            parentType.GetField(r.NameProp, System.Reflection.BindingFlags.NonPublic | BindingFlags.Public | System.Reflection.BindingFlags.Instance).SetValue(View.ObjectSpace.GetObject(r.Obj), p);

                            
                        
                        //else
                        //{
                        //   r.Session.GetType().GetField("_StateStack", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(replaceTable.Session, SessionState.GetObjectsNonReenterant);
                        //   var parentType = r.Obj.GetType();
                 
                        //        while(parentType.GetField(r.NameProp, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | BindingFlags.Public) == null)
                        //    {
                        //        parentType = parentType.BaseType;
                        //    }
                        //    parentType.GetField(r.NameProp, System.Reflection.BindingFlags.NonPublic | BindingFlags.Public | System.Reflection.BindingFlags.Instance).SetValue(View.ObjectSpace.GetObject(r.Obj), p);

                        //}
                        //if (r.Obj.GetType().GetProperty(r.NameProp, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) == null)
                        //{
                        //   r.Session.GetType().GetField("_StateStack", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(replaceTable.Session, SessionState.GetObjectsNonReenterant);
                        //    r.Obj.GetType().GetField(r.NameProp, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(View.ObjectSpace.GetObject(r.Obj), p);

                        //}
                        //else
                        //{
                        //    r.Obj.GetType().GetProperty(r.NameProp, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(View.ObjectSpace.GetObject(r.Obj), p);
                        //}

                       

                    }

                    replaceTable.Session.GetType().GetField("_StateStack", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(replaceTable.Session, SessionState.Empty);
                   
                    foreach (var q in replaceTable.Objects)
                    {
                        //((BaseObject)q.obj).Session.mo
                        View.ObjectSpace.SetModified(q.Obj);
                    }

                    //BoolList allowEdit = new BoolList(false, BoolListOperatorType.And);
                    //var y = View.GetType();
                    //Type d = typeof(View);

                    //var f =  d.GetField("allowEdit", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    //  f.SetValue(View, allowEdit);
                    //  View.Refresh();
                    //  View.ObjectSpace.CommitChanges();
                }
            }
        }

      

        private void showOldAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
           
        }

        private void showOldAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            ReplaceTable replaceTable = View.CurrentObject as ReplaceTable;
            String id = replaceTable.OldId;
            var g = System.Guid.Parse(id);
            //var o = View.ObjectSpace.GetObjectByKey(replaceTable.CurrentType, g);
            IObjectSpace os = Application.CreateObjectSpace();
            var o = os.GetObjectByKey(replaceTable.CurrentType, g);
            e.View = Application.CreateDetailView(os, o, false);
        }

        private void showNewAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            
            ReplaceTable replaceTable = View.CurrentObject as ReplaceTable;
            if (replaceTable.NewId == null) { return; }
            String id = replaceTable.NewId;
            var g = System.Guid.Parse(id);
            //var o = View.ObjectSpace.GetObjectByKey(replaceTable.CurrentType, g);
            IObjectSpace os = Application.CreateObjectSpace();
            var o = os.GetObjectByKey(replaceTable.CurrentType, g);
            e.View = Application.CreateDetailView(os, o, false);
        }

        public SimpleAction ApplyAction {
            get { return simpleAction1; }
        }
        private void singleChoiceAction1_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            IObjectSpace objectSpace = View is ListView ?
         Application.CreateObjectSpace() : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
           
                foreach (Object obj in objectsToProcess) {
                    ReplaceTable objInNewObjectSpace = (ReplaceTable)objectSpace.GetObject(obj);
                    objInNewObjectSpace.Status = (Status)e.SelectedChoiceActionItem.Data;
                }
            
            
            //if (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View) {
            //    objectSpace.CommitChanges();
            //}
            if (View is ListView) {
                View.Refresh();
            }

        }
    }
}
