--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: String.Right
Code-Type: LUA Function
Code-Version: 1.0
Code-Description: Returns right half of a string, starting at _spaces
Code-Author: Robert Randazzio
]]--
return (
function (_string, _spaces)
    placetostart = string.len(_string) - _spaces + 1
    output = string.sub(_string, placetostart)
    return output
end
);
