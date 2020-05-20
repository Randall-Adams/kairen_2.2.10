--[[ Meta-Data
Version: 1.0
]]--
-- Area Class
_G.Areas = { }
Area = {};
function Area.new(_filepath)
    local _sender = "Area.new"
    co(_sender,"co_enter","_filepath: " .. _filepath)

    local self = {};
    self.filepath = _filepath
    self.supType = "supType Area"
    self.Type = "baseType Area" -- This gets overwritten by the subtype
    _file = io.open(_filepath .. Extension_NPC_Area_Maker);
    self.fileVersion = IO_ReadNextLine(_file, "%-%-") -- File Version
    co(_sender, "co_debugoutput", "self.fileVersion == " .. self.fileVersion)
    if self.fileVersion == "1.0" then
        self.nameSafe = IO_ReadNextLine(_file, "%-%-") -- SafeName of Area
        self.Zone = IO_ReadNextLine(_file, "%-%-") -- Name of Zone
        self.Xmin = IO_ReadNextLine(_file, "%-%-")
        self.Xmax = IO_ReadNextLine(_file, "%-%-")
        self.Ymin = IO_ReadNextLine(_file, "%-%-")
        self.Ymax = IO_ReadNextLine(_file, "%-%-")
        self.Zmin = IO_ReadNextLine(_file, "%-%-")
        self.Zmax = IO_ReadNextLine(_file, "%-%-")
        _file:close();
    elseif self.fileVersion == "1.1" then
        self.nameSafe = IO_ReadNextLine(_file, "%-%-") -- SafeName of Area
        self.Zone = IO_ReadNextLine(_file, "%-%-") -- Name of Zone
        self.zoneSub = IO_ReadNextLine(_file, "%-%-") -- SubZone
        self.Xmin = IO_ReadNextLine(_file, "%-%-")
        self.Xmax = IO_ReadNextLine(_file, "%-%-")
        self.Ymin = IO_ReadNextLine(_file, "%-%-")
        self.Ymax = IO_ReadNextLine(_file, "%-%-")
        self.Zmin = IO_ReadNextLine(_file, "%-%-")
        self.Zmax = IO_ReadNextLine(_file, "%-%-")
        _file:close();
    else
        co(_sender, "co_debugoutput", "Error: File Version Not Supported: " .. self.fileVersion)
    end

    return self;
end
