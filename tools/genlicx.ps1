get-childitem ..\* -include *.csproj -recurse | foreach-object {
if (select-string -pattern "Properties\\licenses.licx" -path $_ -quiet) {$dir=$_.directoryname+"\Properties\licenses.licx"
$null > $dir}}