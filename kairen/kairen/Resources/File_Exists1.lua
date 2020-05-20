--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: File_Exists
Code-Type: LUA Function
Code-Version: 1.0
Code-Description: Returns 'true' if the file exists, or 'false' if it does not exist.
Code-Author: Robert Randazzio
]]--
return (
function(_file)
    local f=io.open(_file,"r")
    if f~=nil then io.close(f) return true else return false end
end
);
