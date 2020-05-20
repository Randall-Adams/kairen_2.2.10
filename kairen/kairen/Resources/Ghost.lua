--[[ Meta-Data
Version: 1.0
]]--
-- Ghost Class
_G.Ghosts = { };
Ghost = {};
function Ghost.new(_nameSafe)
    local _sender = "Ghost.new"
    --co(_sender, "co_enter", "_nameSafe = \"" .. _nameSafe .. "\"")
    -- This class should set up references to Pointers ONLY
    -- Spawn_NPC should fill the references, in turn filling the Pointers
    
    local self = {};
    
    fileOfGhost = Folder_Ghosts .. _nameSafe .. Extension_GhostFiles

    --co(_sender, "co_debugoutput", "fileOfGhost = \"" .. fileOfGhost .. "\"")

    if File_Exists(fileOfGhost) == true then
        --  2(_sender, "co_debugoutput", "File_Exists(" .. fileOfGhost .. ") == True")
        file = io.open(fileOfGhost,"r+");
        
        -- Sets This Ghost to point to the Pointers
        local fileversion
        fileversion = IO_ReadNextLine(file, "%-%-")
        if fileversion == "0.1.1" then
            self.ghostID = IO_ReadNextLine(file, "%-%-")
            self.nameOverhead = IO_ReadNextLine(file, "%-%-") 
            self.nameOverheadColor = IO_ReadNextLine(file, "%-%-")
            self.nameTarget = IO_ReadNextLine(file, "%-%-")
            self.X = IO_ReadNextLine(file, "%-%-")
            self.Y = IO_ReadNextLine(file, "%-%-")
            self.Z = IO_ReadNextLine(file, "%-%-")
            self.F = IO_ReadNextLine(file, "%-%-")
            self.level = IO_ReadNextLine(file, "%-%-")
            self.hp = IO_ReadNextLine(file, "%-%-")
        else
            co(_sender, "co_debugoutput", "Error: File Version Not Supported : " .. fileversion)
        end
        file:close();

        -- Properties / Variables
        self.Type = "baseType Ghost"
        self.isFree = false
    else
        co(_sender, co_debugoutput, "Error: File Does Not Exist: " .. fileOfGhost)
    end
    
    return self;
end