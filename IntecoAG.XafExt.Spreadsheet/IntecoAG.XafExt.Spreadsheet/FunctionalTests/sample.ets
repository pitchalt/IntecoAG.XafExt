;#DropDB IntecoAG.XafExt.SpreadsheetEasyTest

#Application IntecoAG.XafExt.Spreadsheet.Win
#Application IntecoAG.XafExt.Spreadsheet.Web
#IfDef IntecoAG.XafExt.Spreadsheet.Win

*Action About...
 
*Action OK

#EndIf

#IfDef IntecoAG.XafExt.Spreadsheet.Web
!Action Navigation(My Details)
#EndIf
