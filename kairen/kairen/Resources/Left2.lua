--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: String.Left
Code-Type: LUA Function
Code-Version: 1.0
Code-Description: Returns left half of a string, ending at _spaces
Code-Author: Robert Randazzio
]]--
return (
function (_string, _spaces)
    output = string.sub(_string, 1, _spaces)
    return output
end
);
