--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: Read_FileToStringArray
Code-Type: LUA Function
Code-Version: 1.1
Code-Description: Reads the non-comment lines of the specified file to an array.
Code-Author: Robert Randazzio
]]--
return(
function(_filepath, _commentString)
    if FAPI.IO.File_Exists(_filepath) then
        _file = io.open(_filepath, "r+");
        local i = 1
        local _lines = {};
        if _commentString == nil then
            _lines[i] = _file:read()
            while _lines[i] ~= nil do
                i = i + 1
                _lines[i] = _file:read()
            end
        else        
            _lines[i] = FAPI.IO.ReadNextLine(_file, _commentString)
            while _lines[i] ~= nil do
                i = i + 1
                _lines[i] = FAPI.IO.ReadNextLine(_file, _commentString)
            end
        end
        _file:close();
        _lines[0] = i
        return _lines
    else
        -- File Does Not Exist
        return nil
    end
end
);
