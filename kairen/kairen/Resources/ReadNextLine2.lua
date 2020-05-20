--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: ReadNextLine
Code-Type: LUA Function
Code-Version: 1.1
Code-Description: Returns the next non-comment line from the specified already opened file.
Code-Author: Robert Randazzio
]]--
return (
function(_ioObject, _commentString)
    _line = _ioObject:read() -- reads line for first processing
    if _line ~= nil then
        if _commentString ~= false then -- if comment removal should happen
            redoRead = true
            while redoRead == true do
                _start, _end = string.find(_line, _commentString) -- checks if comment
                if _start == 1 and _end == 2 then -- if is a comment then ..
                    _line = _ioObject:read() -- reads next line for processing
                    if _line ~= nil then
                        redoRead = true -- read the next line.
                    else
                        redoRead = false
                    end
                else -- if not a comment ..
                    redoRead = false -- don't read more lines, and return current line.
                end
            end
        end
    end
    if _line == nil then
        return nil
    else
        return _line
    end
end
);
