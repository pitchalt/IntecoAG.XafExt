using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using IntecoAG.XafExt;

//namespace IntecoAG.XafExt {

//    public interface ICollectionDictionary<Tk, Ti> : IReadOnlyDictionary<Tk, Ti>
//        //where Ti : ICollectionDictionaryItem<Tk>
//        where Tk : IEquatable<Tk> {

//        void OnChanged(Tk oldkey, Object item);

//    }

//    public interface ICollectionDictionaryItem<Tk>
//        where Tk : IEquatable<Tk> {

//        ICollectionDictionary<Tk, ICollectionDictionaryItem<Tk>> Dict { get; set; }
//        Tk Key { get; }
//    }

//}

namespace IntecoAG.XpoExt {

    //    public class XPCollectionDictionary<Tk, Ti> : XPCollection<Ti>, ICollectionDictionary<Tk, Ti>
    ////        where Ti : ICollectionDictionaryItem<Tk>
    //        where Tk : IEquatable<Tk> 
    //        {

    //        public class KeyAlreadyExist : Exception {

    //            public KeyAlreadyExist() : base() {
    //            }

    //            public KeyAlreadyExist(String message) : base(message) {
    //            }
    //        }

    //        private Dictionary<Tk, Ti> _Dict;
    //        protected IDictionary<Tk, Ti> Dict;

    //        public IEnumerable<Tk> Keys => Dict.Keys;

    //        public IEnumerable<Ti> Values => Dict.Values;

    //        int IReadOnlyCollection<KeyValuePair<Tk, Ti>>.Count => Dict.Count;

    //        public Ti this[Tk key] => Dict[key];

    //        public XPCollectionDictionary(Session session, object theOwner, XPMemberInfo refProperty) {
    //            _Dict = new Dictionary<Tk, Ti>();
    //        }

    //        public override int BaseAdd(object newObject) {
    //            var item = (ICollectionDictionaryItem<Tk>)newObject;
    //            Tk key = item.Key;
    //            if (!key.Equals(default(Tk))) {
    //                if (Dict.ContainsKey(key)) {
    //                    if (!Object.ReferenceEquals(Dict[key], item)) {
    //                        throw new KeyAlreadyExist($"Key: {key} already exist");
    //                    }
    //                }
    //                else {
    //                    Dict[key] = (Ti)item;
    //                }
    //            }
    //            return base.BaseAdd(newObject);
    //        }

    //        public override bool BaseRemove(object theObject) {
    //            var item = (ICollectionDictionaryItem<Tk>)theObject;
    //            if (Dict.ContainsKey(item.Key))
    //                Dict.Remove(item.Key);
    //            return base.BaseRemove(theObject);
    //        }

    //        public void OnChanged(Tk oldkey, Object obj) {
    //            var item = (ICollectionDictionaryItem<Tk>)obj;
    //            if (Dict.ContainsKey(oldkey)) {
    //                Dict.Remove(oldkey);
    //            }
    //            Dict[item.Key] = (Ti)item;
    //        }

    //        public bool ContainsKey(Tk key) {
    //            return Dict.ContainsKey(key);
    //        }

    //        public bool TryGetValue(Tk key, out Ti value) {
    //            return Dict.TryGetValue(key, out value);
    //        }

    //        IEnumerator<KeyValuePair<Tk, Ti>> IEnumerable<KeyValuePair<Tk, Ti>>.GetEnumerator() {
    //            return Dict.GetEnumerator();
    //        }

    //    }

    [NonPersistent]
    [DefaultProperty(nameof(Name))]
    public abstract class IagBaseObject : XPObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        public static bool IsXpoProfiling = false;

        private bool _IsDefaultPropertyAttributeInit;
        private XPMemberInfo _DefaultPropertyMemberInfo;

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public String Name {
            get { return ToString(); }
        }

        protected IagBaseObject(Session session)
            : base(session) {
        }

        protected new T GetPropertyValue<T>([CallerMemberName] String name = null) =>
            base.GetPropertyValue<T>(name);

        protected Boolean SetPropertyValue<T>(ref T holder, T value, [CallerMemberName] String name = null) =>
            SetPropertyValue(name, ref holder, value);

        protected new XPCollection<T> GetCollection<T>([CallerMemberName] String name = null)
            where T : class
            => base.GetCollection<T>(name);

        protected new object EvaluateAlias([CallerMemberName] string name = null)
            => base.EvaluateAlias(name);

        public override string ToString() {
            if (!IsXpoProfiling) {
                if (!_IsDefaultPropertyAttributeInit) {
                    string default_property_name = string.Empty;
                    XafDefaultPropertyAttribute xaf_def_property_attribute = XafTypesInfo.Instance.FindTypeInfo(GetType()).FindAttribute<XafDefaultPropertyAttribute>();
                    if (xaf_def_property_attribute != null) {
                        default_property_name = xaf_def_property_attribute.Name;
                    }
                    else {
                        DefaultPropertyAttribute default_property_attribute = XafTypesInfo.Instance.FindTypeInfo(GetType()).FindAttribute<DefaultPropertyAttribute>();
                        if (default_property_attribute != null) {
                            default_property_name = default_property_attribute.Name;
                        }
                    }
                    if (!string.IsNullOrEmpty(default_property_name)) {
                        _DefaultPropertyMemberInfo = ClassInfo.FindMember(default_property_name);
                    }
                    _IsDefaultPropertyAttributeInit = true;
                }
                if (_DefaultPropertyMemberInfo != null && _DefaultPropertyMemberInfo.Name != nameof(Name)) {
                    object obj = _DefaultPropertyMemberInfo.GetValue(this);
                    if (obj != null) {
                        return obj.ToString();
                    }
                }
            }
            return base.ToString();
        }
    }
}