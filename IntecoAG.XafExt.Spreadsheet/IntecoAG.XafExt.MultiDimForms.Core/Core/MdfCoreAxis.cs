using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using IntecoAG.XpoExt;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    [Persistent("FmMdfCoreAxis")]
    [ModelDefault("IsCloneable", "True")]
    public class MdfCoreAxis : MdfCoreElement, IObjectSpaceLink {

        //protected class OrdinatesCollection : XPCollection<MdfAxisOrdinate> {

        //    private MdfAxis _Axis;

        //    public OrdinatesCollection(Session session, MdfAxis owner, XPMemberInfo property):base(session, owner, property) {
        //        _Axis = owner;
        //    }

        //    public override int BaseAdd(Object newObject) {
        //        Int32 index = base.BaseAdd(newObject)
        //        return index;
        //    }
        //}

        
        private MdfCoreContainer _Container;
        [Association("FmMdfContainer-FmMdfAxis")]
        public MdfCoreContainer Container {
            get { return _Container; }
            set { SetPropertyValue<MdfCoreContainer>(ref _Container, value); }
        }

//        [VisibleInDetailView(false)]
        [Association("MdfAxis-MdfAxisOrdinate")]
        [Aggregated]
        public XPCollection<MdfCoreAxisOrdinate> Ordinates {
            get { return GetCollection<MdfCoreAxisOrdinate>(); }
        }

        [Association("MdfAxis-MdfAxisOrdinateLine")]
        public XPCollection<MdfCoreAxisOrdinate> OrdinateLine {
            get { return GetCollection<MdfCoreAxisOrdinate>(); }
        }
        //protected override XPCollection<T> CreateCollection<T>(XPMemberInfo property) {
        //    //if (property.Name == nameof(Ordinates))
        //    //    return 
        //    return base.CreateCollection<T>(property);
        //}

        [Persistent(nameof(Root))]
        [Aggregated]
        private MdfCoreAxisOrdinate _Root;
        [PersistentAlias(nameof(_Root))]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public MdfCoreAxisOrdinate Root {
            get { return _Root; }
        }
        protected void RootSet(MdfCoreAxisOrdinate value) {
            SetPropertyValue<MdfCoreAxisOrdinate>(ref _Root, value);
        }

        [Association("FmMdfAxis-FmMdfAxisLevel")]
        [Aggregated]
        public XPCollection<MdfCoreAxisLevel> Levels {
            get { return GetCollection<MdfCoreAxisLevel>(); }
        }
        //
        [Browsable(false)]
        public IObjectSpace ObjectSpace { get; set; }

        public MdfCoreAxisLevel LevelGet(Int32 index) {
            XPCollection<MdfCoreAxisLevel> levels = Levels;
            for (Int32 cur = 0; cur < levels.Count; cur++) {
                if (levels[cur].Index == index)
                    return levels[cur];
            }
            return CreateLevel(index);
        }

        protected virtual MdfCoreAxisLevel CreateLevel(Int32 index) {
            MdfCoreAxisLevel res = new MdfCoreAxisLevel(Session);
            Levels.Add(res);
            res.IndexSet(index);
            return res;
        }
        
        public MdfCoreAxis(Session session) : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            RootSet(CreateRoot());
            Ordinates.Add(Root);
        }

        [Action(Caption = "Render")]
        public void RenderAction() {
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                MdfCoreAxis _this = os.GetObject(this);
                _this.Render(os);
                os.CommitChanges();
            }
        }

        protected virtual MdfCoreAxisOrdinate CreateRoot() {
            MdfCoreAxisOrdinate ord = new MdfCoreAxisOrdinate(Session);
//            Ordinates.Add(ord);
            return ord;
        }

        public override string ToString() {
            return $@"{Code} - {NameShort}";
        }
    }

}