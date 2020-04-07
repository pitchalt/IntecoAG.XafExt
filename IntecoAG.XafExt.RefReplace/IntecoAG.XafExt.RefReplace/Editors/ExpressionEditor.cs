using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors.Repository;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;

namespace IntecoAG.XafExt.Editors {
   
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [PropertyEditor(typeof(Expression), true)]

    public class ExEditor : DXPropertyEditor { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        Type targetType = typeof(String); Boolean buid = true;
        private void Recurs( TreeListNode node )
        {
        
           if (node.Nodes[0].GetValue(0) != null) { return; }
         
            tree.DeleteNode(node.Nodes[0]);
            bool fi(ITypeInfo obj)
            {

                foreach (var c in obj.GetDependentTypes(filter))
                {
                    var n = node.GetValue(0);
                 var br =   obj.FindAttribute<BrowsableAttribute>();
                    bool a = br == null?  true : br.Browsable;
                    Boolean b = c.FullName.Contains(((ITypeInfo)node.GetValue(0)).Name) && obj.IsPersistent&& a;
                    return b;
                }


                return false;
            }


           
            var info = node.GetValue(0) as ITypeInfo;
          

            var reqTypes = info.GetRequiredTypes(fi);
            ITypeInfo morkovka = null;

            Boolean buid = false;
            TreeListNode childNode = null;
            foreach (var t in reqTypes) {

                node = childNode == null ? node : childNode;
                var value = node.GetValue(0);
               
         
            
                    info = node.GetValue(0) as ITypeInfo;

                foreach (var m in info.OwnMembers) {
                    buid = true;
                    var types = XafTypesInfo.CastTypeToTypeInfo(m.MemberType).GetRequiredTypes(findReqTypes);

                    if (types.Contains(XafTypesInfo.CastTypeToTypeInfo(targetType))||m.MemberType==targetType) {
                        // строить эту ветку
                        buid = true;
                    }
                    if (buid) {

                        var br = m.FindAttribute<BrowsableAttribute>();
                        var b = br == null ? true : br.Browsable;
                        if (m.IsPersistent && m.IsPublic && b) {
                            childNode = tree.AppendNode("n", node);


                            childNode.SetValue(0, XafTypesInfo.CastTypeToTypeInfo(m.MemberType));
                            childNode.SetValue(1, m.Name);
                            //поиск локализованного имени
                            var brw = t.FindAttribute<BrowsableAttribute>();
                            if (t.IsPersistent) {


                                if (brw == null || brw.Browsable) {
                                    var bo = this.Model.Application.BOModel.GetNode(info.FullName);
                                    var v = bo.GetNode("OwnMembers");
                                    var h = v.GetNode(m.Name);
                                    var l = h.GetValue<String>("Caption");



                                    childNode.SetValue(2, l);
                                }
                            }

                            tree.AppendNode("c", childNode);
                        }

                    }
            }
              

            }
            

        }

        private bool lol(ITypeInfo obj) {
            return true;
        }

        private bool findReqTypes(ITypeInfo obj)
        {
            if (obj.Name.Contains("TrwPartyParty")) {

            }
            bool b = obj.Name.Contains(targetType.Name);
            return b;
            //foreach (var c in obj.GetDependentTypes(filter))
            //{

            //    Boolean b = c.Name.Contains(targetType.Name);
            //    return b;
            //}


          
        }
        PopupContainerEdit editor = null;
        Type sourseType = null;
      
        
        TreeList tree = null;
  
        protected override object CreateControlCore() {
            editor = new PopupContainerEdit();
          
            PopupContainerControl popupControl = new PopupContainerControl();
            popupControl.SizeChanged += PopupControl_SizeChanged;
            editor.QueryResultValue += Editor_QueryResultValue;
            editor.QueryCloseUp += Editor_QueryCloseUp;
             tree = new TreeList();

            tree.OptionsBehavior.Editable = false;
     
      
            tree.AfterFocusNode += Tree_AfterFocusNode;
           
            editor.Popup += Editor_Popup;
            tree.BeforeExpand += Tree_BeforeExpand;
           
         
            var column = new TreeListColumn();
          column.Caption = "Тип";
    column.FieldName = "Type";
            column.Name = "Type";
           column.Visible = true;
            column.VisibleIndex = 0;
            //=====column2===
            var column2 = new TreeListColumn();
            column2.Caption = "Имя свойства";
            column2.FieldName = "NameProp";
            column2.Name = "NameProp";
            column2.Visible = true;
            column2.VisibleIndex = 0;
            //=====column3===
            var column3 = new TreeListColumn();
            column3.Caption = "Локализованное имя";
            column3.FieldName = "LocalName";
            column3.Name = "LocalName";
            column3.Visible = true;
            column3.VisibleIndex = 0;

            popupControl.Controls.Add(tree);
            //editor.Controls.Add(new TreeList());

            tree.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            column, column2, column3 });

            editor.Properties.PopupControl = popupControl;
            //Type sourseType = typeof(Contract);
            var n = tree.AppendNode("lol", null);
            n.SetValue(0, XafTypesInfo.CastTypeToTypeInfo(sourseType));
            
            var c = tree.AppendNode("clean", n);
            //var info = XafTypesInfo.CastTypeToTypeInfo(sourseType);
            //var m = info.FindMember("RequiredTypes");
            //var p = sourseType.GetProperty("RequiredTypes", System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);
            //var reqTypes = info.GetRequiredTypes(fi);

            //foreach (var t in reqTypes) {

            //    var c = tree.AppendNode("c", n);
            //    c.SetValue(0, t.FullName);
            //}


           
            return editor;
        }

      
        private void Editor_QueryCloseUp(object sender, CancelEventArgs e) {
            throw new NotImplementedException();
        }

        private void Editor_QueryResultValue(object sender, DevExpress.XtraEditors.Controls.QueryResultValueEventArgs e) {
            Expression ex = new Expression();
            ex.TargetType = targetType;
            ex.SourseType = sourseType;
            e.Value = ex;
        }

        private void PopupControl_SizeChanged(object sender, EventArgs e) {
            tree.Height = editor.Properties.PopupControl.Height;
            tree.Width = editor.Properties.PopupControl.Width;
            
        }

       
        private void Editor_Popup(object sender, EventArgs e) {

        }


        private void Tree_AfterFocusNode(object sender, NodeEventArgs e) {
            //если есть TargetType, то можно выбрать только его
            if (targetType != null) {
                editor.Text = targetType.ToString() == e.Node.GetValue(0).ToString() ? targetType.ToString() : null;
            }
            else {
                editor.Text = e.Node.GetValue(0).ToString();
         
            }
            
        }

        private void Tree_BeforeExpand(object sender, BeforeExpandEventArgs e)
        {
            //подгрузить ссылки первого уровня
            Type t = e.Node.GetValue(0) as Type;
            var info = XafTypesInfo.CastTypeToTypeInfo(t);
            Recurs( e.Node);

        }

        private bool filter(ITypeInfo obj)
        {
            var b = !(obj.FullName.Contains("System") || obj.FullName.Contains("DevExpress"));
            return b;
        }

    
      

        protected override void OnControlCreated() {
            base.OnControlCreated();
          
        }
        public ExEditor(Type objectType, IModelMemberViewItem info)
            : base(objectType, info) {
        }

        //protected override RepositoryItem CreateRepositoryItem() {


        //    RepositoryItemPopupContainerEdit edit = new RepositoryItemPopupContainerEdit();

        //    edit.PopupControl = new PopupContainerControl();






        //    return edit;
        //}

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}