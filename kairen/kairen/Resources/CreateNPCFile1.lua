--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: CreateNPCFile
Code-Type: LUA Function
Code-Version: 1.0
Code-Description: Creates an NPC File.
Code-Author: Robert Randazzio
]]--

return(
function(_safeName, _gameName, _gender, _race, _level, _x, _y, _z, _f, _folder, _hp, _mp, _ac, _folder)
    FileAsArray = {};
    FileAsArray[1] = "0.1.1"
    if _safeName == nil or _gameName == nil then
        FileAsArray[2] = _safeName
        FileAsArray[3] = _gameName  
    end    
    --or _gender == nil or _race == nil
    if _x == nil or _y == nil or _z == nil or _f == nil then
        --grab current location since a proper one was not supplied
    else
        FileAsArray[4] = _x
        FileAsArray[5] = _y
        FileAsArray[6] = _z
        FileAsArray[7] = _f
    end
    if _race == nil then
        FileAsArray[8] = 1
    else
        FileAsArray[8] = _race
    end
    if _gender == nil then
        FileAsArray[9] = 1
    else
        FileAsArray[9] = _gender
    end
    FileAsArray[10] = "deprecated class field"
    
    if _level == nil then
        FileAsArray[11] = 1
    else
        FileAsArray[11] = _level
    end
    
    if _hp == nil then
        FileAsArray[12] = 1
    else
        FileAsArray[12] = _hp
    end
    
    if _mp == nil then
        FileAsArray[13] = 1
    else
        FileAsArray[13] = _mp
    end
    
    if _ac == nil then
        FileAsArray[14] = 1
    else
        FileAsArray[14] = _ac
    end
    local FilePath
    if _folder == nil then
        FilePath = RF.EQOA.Custom_Data.NPC_Maker.self  .. _safeName .. Extension_NPCFiles
    else
        FilePath = _folder .. _safeName .. Extension_NPCFiles
    end
    
    FAPI.IO.Write_StringArrayToFile(FilePath, FileAsArray)
    
end
);
