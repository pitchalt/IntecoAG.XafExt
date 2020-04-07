using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    public static class MdfCoreTableLogic {

        public static void Render(this MdfCoreTable _this, IObjectSpace os) {
            foreach (var table_axis in _this.TableAxiss) {
                table_axis.Axis?.Render(os);
            }
            RenderCells(_this, os);
            RenderCalcs(_this, os);
        }

        public static void RenderCalcs(this MdfCoreTable _this, IObjectSpace os) {
            foreach (var calc in _this.Calcs) {
                calc.Update();
            }
        }

        public static void RenderCells(this MdfCoreTable _this, IObjectSpace os) {

            IList<IList<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>>> axis_mult_list =
                    new List<IList<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>>>();
            foreach (var table_axis in _this.TableAxiss) {
                if (table_axis.Axis != null)
                    axis_mult_list = RenderCellAxis(table_axis.Axis, axis_mult_list);
            }
            //IDictionary<Guid, MdfCoreDimension> dims = new Dictionary<Guid, MdfCoreDimension>();
            //foreach (var dim in _this.Container.Dimensions) {
            //    dims[dim.Guid] = dim;
            //}
            //IDictionary<String, MdfCoreCategory> cat_types = new Dictionary<String, MdfCoreCategory>();
            //foreach (var cat_type in _this.Container.Categorys) {
            //    String cat_type_key = CategoryTypeKeyMake(cat_type.CategoryFields.Select(x => x.Dimension));
            //    cat_types[cat_type_key] = cat_type;
            //}
            //IDictionary<String, MdfCoreDataPoint> data_points = new Dictionary<String, MdfCoreDataPoint>();
            //foreach (var data_point in _this.Container.DataPoints) {
            //    IDictionary<MdfCoreDimension, MdfCoreDimensionMember> data_point_dict = new Dictionary<MdfCoreDimension, MdfCoreDimensionMember>();
            //    foreach (var field in data_point.CategoryMember.CategoryMemberFields) {
            //        data_point_dict[field.CategoryTypeField.Dimension] = field.DimensionMember;
            //    }
            //    String data_point_key = DataPointKeyMake(data_point_dict);
            //    data_points[data_point_key] = data_point;
            //}
            IDictionary<String, MdfCoreTableCell> cells = new Dictionary<String, MdfCoreTableCell>();
            IList<MdfCoreTableCell> old_cells = _this.Cells.ToList();
            IDictionary<MdfCoreTableCell, Boolean> new_cells = new Dictionary< MdfCoreTableCell, Boolean>(axis_mult_list.Count);
            foreach (var cell in _this.Cells) {
                String cell_key = CellKeyMake(cell.AxisOrdinates.Where(x => x != null && x.Axis != null ));
                cells[cell_key] = cell;
                old_cells.Add(cell);
            }
            foreach (var axis_mult_item in axis_mult_list) {
                IDictionary<MdfCoreDimension, MdfCoreDimensionMember> cell_dict =
                    RenderCellAxisDictIntersect(axis_mult_item.Select(x => x.Item2));
                String data_point_key = ((IReadOnlyDictionary<MdfCoreDimension, MdfCoreDimensionMember>)cell_dict).CategoryMemberKeyGet();
                MdfCoreDataPoint data_point = _this.Container.DataPointGet((IReadOnlyDictionary<MdfCoreDimension, MdfCoreDimensionMember>)cell_dict);
                String cell_key = CellKeyMake(axis_mult_item.Select(x => x.Item1));
                MdfCoreTableCell cell = null;
                if (cells.ContainsKey(cell_key)) {
                    cell = cells[cell_key];
                }
                else {
                    cell = os.CreateObject<MdfCoreTableCell>();
                    _this.Cells.Add(cell);
                    cells[cell_key] = cell;
                    foreach (var table_axis in _this.TableAxiss) {
                        var axis_ordinate = axis_mult_item.Select(x => x.Item1)
                            .FirstOrDefault(x => Object.ReferenceEquals(table_axis.Axis, x.Axis));
                        switch (table_axis.AxisIndex) {
                            case 0:
                                cell.AxisOrdinate0Set(axis_ordinate);
                                break;
                            case 1:
                                cell.AxisOrdinate1Set(axis_ordinate);
                                break;
                            case 2:
                                cell.AxisOrdinate2Set(axis_ordinate);
                                break;
                        }
                    }
                }
                cell.DataPoint = data_point;
                new_cells[cell] = true;
            }
            foreach (var old_cell in old_cells) {
                if (!new_cells.ContainsKey(old_cell))
                    os.Delete(old_cell);
            }
        }

        ////public static MdfCoreCategory CategoryTypeLocate (
        ////        MdfCoreContainer container, IObjectSpace os, IEnumerable<MdfCoreDimension> dim_list, 
        ////        IDictionary<String, MdfCoreCategory> cat_types) {
        ////    String cat_type_key = CategoryTypeKeyMake(dim_list);
        ////    if (cat_types.ContainsKey(cat_type_key)) {
        ////        return cat_types[cat_type_key];
        ////    }
        ////    MdfCoreCategory cat_type = os.CreateObject<MdfCoreCategory>();
        ////    container.Categorys.Add(cat_type);
        ////    cat_types[cat_type_key] = cat_type;
        ////    foreach (var dim in dim_list) {
        ////        var cat_type_field = os.CreateObject<MdfCoreCategoryField>();
        ////        cat_type.CategoryFields.Add(cat_type_field);
        ////        cat_type_field.Dimension = dim;
        ////    }
        ////    return cat_type;
        ////}

        public static String CellKeyMake(IEnumerable<MdfCoreAxisOrdinate> ord_list) {
            String result = $@"({String.Join(",", ord_list.OrderBy(x => x.Axis.Guid).Select(x => $@"{x.Axis.CodeOrGuid}={x.CodeOrGuid}"))})";
            return result;
        }

        ////public static String DataPointKeyMake(IDictionary<MdfCoreDimension, MdfCoreDimensionMember> data_point_dict) {
        ////    String result = $@"<{ String.Join(",", data_point_dict.Keys.OrderBy(x => x.Oid)
        ////            .Select(x => $@"{x.Oid}={data_point_dict[x].Oid}"))}>";
        ////    return result;
        ////}

        ////public static String CategoryTypeKeyMake(IEnumerable<MdfCoreDimension> dim_list) {
        ////    String result = $@"<{String.Join(",", dim_list.OrderBy(x => x.Oid).Select(x => x.Oid))}>";
        ////    return result;
        ////}

        public static IList<IList<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>>> RenderCellAxis
                    (MdfCoreAxis axis, IList<IList<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>>> axis_mult_list) {
            IList<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember >>> axis_linear = RenderCellAxisLinear(axis);
            if (axis_linear.Count != 0) {
                var result = new List<IList<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>>>(16);
                if (axis_mult_list.Count == 0) {
                    foreach (var axis_item in axis_linear) {
                        var res_item = new List<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>>(1) {
                            axis_item
                        };
                        result.Add(res_item);
                    }
                }
                else {
                    foreach (var axis_mult_item in axis_mult_list) {
                        foreach (var axis_item in axis_linear) {
                            var res_item = new List<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>>(axis_mult_item) {
                                //axis_mult_item.Add(.CopyTo(res_item,0);
                                axis_item
                            };
                            result.Add(res_item);
                        }
                    }
                }
                return result;
            }
            else {
                return axis_mult_list;
            } 
        }

        public static IList<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>> RenderCellAxisLinear(MdfCoreAxis axis) {
            var result = new List<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>>(128);
            foreach (var ord in axis.OrdinateLine) {
                var dict = new Dictionary<MdfCoreDimension, MdfCoreDimensionMember>();
                if (ord.CategoryMember != null) {
                    foreach (var field in ord.CategoryMember.CategoryMemberFields) {
                        dict[field.CategoryTypeField.Dimension] = field.DimensionMember;
                    }
                }
                result.Add(new Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>(ord, dict));
            }
            return result;
        }
        //public static IList<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>> RenderCellAxisLinear(MdfCoreAxis axis) {
        //    IList<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>> result = new List<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>>(128);
        //    IDictionary<MdfCoreDimension, MdfCoreDimensionMember> cur_dict = new Dictionary<MdfCoreDimension, MdfCoreDimensionMember>();
        //    RenderCellAxisLinear(axis.Root, cur_dict, result);
        //    return result;
        //}
        //public static void RenderCellAxisLinear(MdfCoreAxisOrdinate ord, 
        //    IDictionary<MdfCoreDimension, MdfCoreDimensionMember> top_dict,
        //    IList<Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>> result) {
        //    IDictionary<MdfCoreDimension, MdfCoreDimensionMember> cur_dict = new Dictionary<MdfCoreDimension, MdfCoreDimensionMember>(top_dict);
        //    if (ord.CategoryMember != null) {
        //        foreach (var cat_field in ord.CategoryMember.CategoryMemberFields) {
        //            if (cat_field.CategoryTypeField.Dimension != null)
        //                cur_dict[cat_field.CategoryTypeField.Dimension] = cat_field.DimensionMember;
        //        }
        //    }
        //    if (ord.Downs.Count != 0) {
        //        foreach (var down_ord in ord.Downs) {
        //            RenderCellAxisLinear(down_ord, cur_dict, result);
        //        }
        //    }
        //    else {
        //        result.Add(new Tuple<MdfCoreAxisOrdinate, IDictionary<MdfCoreDimension, MdfCoreDimensionMember>>(ord, cur_dict));
        //    }
        //}

        public static IDictionary<MdfCoreDimension, MdfCoreDimensionMember> RenderCellAxisDictIntersect(IEnumerable<IDictionary<MdfCoreDimension, MdfCoreDimensionMember>> axis_dict_list) {
            IDictionary<MdfCoreDimension, MdfCoreDimensionMember> result = new Dictionary<MdfCoreDimension, MdfCoreDimensionMember>();
            foreach (var axis_dict in axis_dict_list) {
                foreach (var dim in axis_dict.Keys) {
                    result[dim] = axis_dict[dim];
                }
            }
            return result; 
        }

        public static void Render(this MdfCoreAxis _this, IObjectSpace os) {
            IList<MdfCoreAxisLevel> levels = new List<MdfCoreAxisLevel>(10) {
                //IList<Int32> levels_index = new List<Int32>(0);
                _this.LevelGet(0)
            };
            //levels_index.Add(0);
            Int32 index = 0;
            _this.Root.Render(os, 0, levels, ref index);
            foreach (MdfCoreAxisLevel level in _this.Levels.ToList()) {
                if (level.Ordinates.Count == 0) {
                    os.Delete(level);
                }
            }
        }

        public static void Render(this MdfCoreAxisOrdinate _this, IObjectSpace os, Int32 level, IList<MdfCoreAxisLevel> levels, ref Int32 index) {
            if (_this.Axis == null)
                _this.Axis = _this.Up.Axis;
            _this.CategoryMemberUpdate();
            if (_this.IsIntegrated) {
                _this.Level = null;
                _this.LevelLengthSet(0);
                _this.LevelIndexSet(0);
                _this.OrderSet(0);
                foreach (MdfCoreAxisOrdinate sub_ord in _this.Downs.OrderBy(x => x.SortOrder)) {
                    sub_ord.Render(os, level, levels, ref index);
                }
            }
            else {
                Int32 new_level = level + 1;
                if (new_level == levels.Count) {
                    levels.Add(_this.Axis.LevelGet(new_level));
 //                   levels_index.Add(0);
                }
                if (_this.Downs.Count > 0) {

                    Int32 length = 0;
                    Int32 old_index = index;
                    foreach (MdfCoreAxisOrdinate sub_ord in _this.Downs.OrderBy(x => x.SortOrder)) {
                        sub_ord.Render(os, new_level, levels, ref index);
                    }
                    length = index - old_index;
                    _this.LevelIndexSet(old_index);
                    _this.LevelLengthSet(length);
                    _this.OrderSet(0);
                    _this.Axis.OrdinateLine.Remove(_this);
                }
                else {
                    _this.Axis.OrdinateLine.Add(_this);
                    _this.OrderSet(index);
                    _this.LevelIndexSet(index++);
                    _this.LevelLengthSet(1);
                }
                _this.Level = levels[level];
            }
        }

    }
}
