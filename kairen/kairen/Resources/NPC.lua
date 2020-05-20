--[[ Meta-Data
Version: 1.0
]]--
-- NPC Class
_G.NPCs = { };
NPC = {};
function NPC.new(_nameSafe, _owner)
    local _sender = "NPC.new"
    if _owner == nil then _owner = "free" end
    co(_sender, "co_enter", _nameSafe .. ", " .. _owner)
    -- This class should set up references to _ghost ONLY
    -- Spawn_NPC should fill this class, in turn filling the ghost.
    local self = {};
    
    
    --Find Usable Ghost and assign it for use
    local i_ghosts
    i_ghosts = 1
    loops = 1
    co(_sender, "co_debugoutput", "Looping for Free Ghost...")
    while loops <= NumberofGhosts do
        if Ghosts[i_ghosts].isFree == true then
            Ghosts[i_ghosts].isFree = false
            _ghost = Ghosts[i_ghosts]
            co(_sender, "co_debugoutput", "Will use Ghosts[" .. i_ghosts .. "]")
            loops = 25
        else
            i_ghosts = i_ghosts + 1
        end
        loops = loops + 1
    end
    if loops == (NumberofGhosts + 1) then
        co(_sender, "co_debugoutput", "No Free Ghost Found to spawn: " .. _nameSafe)
    end
    loops = 0
    
    self.myGhost = _ghost
    
    -- Properties / Variables
    self.nameSafe = _nameSafe
    self.supType = "supType NPC"
    self.Type = "baseType NPC" -- This gets overwritten by the subtype
    self.isFreeOrReplaceable = false
    self.Owner = _owner

    -- Sets This NPC to be references to _ghost
    function self.nameGet()
        co("self.nameGet","co_enter","")
        local value = Read_BitArrayToString(_ghost.nameOverhead, "TypeCharacterName")
        --local value = Read_BitArrayToString(_ghost.nameTarget, "TypeCharacterName")
        co("self.nameGet","co_debugoutput","value = " .. value)
        co("self.nameGet","co_exit","")
        return value
    end
    function self.nameSet(_value)
        co("self.nameSet", "co_enter", _value)
        Write_StringToBitArrayAddress(_value, _ghost.nameOverhead, 1)
        Write_StringToBitArrayAddress(_value, _ghost.nameTarget, 1)
        co("self.nameSet", "co_exit", _value)
    end
    function self.ghostIDGet()
        co("self.ghostIDGet","co_enter","")
        local value = readString(_ghost.ghostID)
        co("self.ghostIDGet","co_debugoutput","value = " .. value)
        co("self.ghostIDGet","co_exit","")
        return value
    end
    function self.ghostIDSet(_value)
        co("self.ghostIDSet", "co_enter", _value)
        writeString(_ghost.ghostID, _value)
        co("self.ghostIDSet","co_exit","")
    end
    function self.nameOverheadColorGet()
        co("self.nameOverheadColorGet","co_enter","")
        local value = readInteger(_ghost.nameOverheadColor)
        co("self.nameOverheadColorGet","co_debugoutput","value = " .. value)
        co("self.nameOverheadColorGet","co_exit","")
        return value
    end
    function self.nameOverheadColorSet(_value)
        co("self.nameOverheadColorSet", "co_enter", _value)
        writeBytes(_ghost.nameOverheadColor, _value)
        co("self.nameOverheadColorSet","co_exit","")
    end
    function self.XGet()
        co("self.XGet","co_enter","")
        local value = readInteger(_ghost.X)
        co("self.XGet","co_debugoutput","value = " .. value)
        co("self.XGet","co_exit","")
        return value
    end
    function self.XSet(_value)
        co("self.XSet", "co_enter", _value)
        co("self.XSet", "co_debugoutput", _ghost.X)
        writeInteger(_ghost.X, _value)
        co("self.XSet","co_exit","")
    end
    function self.YGet()
        co("self.YGet","co_enter","")
        local value = readInteger(_ghost.Y)
        co("self.YGet","co_debugoutput","value = " .. value)
        co("self.YGet","co_exit","")
        return value
    end
    function self.YSet(_value)
        co("self.YSet", "co_enter", _value)
        writeInteger(_ghost.Y, _value)
        co("self.YSet","co_exit","")
    end
    function self.ZGet()
        co("self.ZGet","co_enter","")
        local value = readInteger(_ghost.Z)
        co("self.ZGet","co_debugoutput","value = " .. value)
        co("self.ZGet","co_exit","")
        return value
    end
    function self.ZSet(_value)
        co("self.ZSet", "co_enter", _value)
        writeInteger(_ghost.Z, _value)
        co("self.ZSet","co_exit","")
    end
    function self.FGet()
        co("self.FGet","co_enter","")
        local value = readInteger(_ghost.F)
        co("self.FGet","co_debugoutput","value = " .. value)
        co("self.FGet","co_exit","")
        return value
    end
    function self.FSet(_value)
        co("self.FSet", "co_enter", _value)
        writeInteger(_ghost.F, _value)
        co("self.FSet","co_exit","")
    end
    function self.levelGet()
        co("self.levelGet","co_enter","")
        local value = readByte(_ghost.level)
        co("self.levelGet","co_debugoutput","value = " .. value)
        co("self.levelGet","co_exit","")
        return value
    end
    function self.levelSet(_value)
        co("self.levelSet", "co_enter", _value)
        writeBytes(_ghost.level, _value)
        co("self.levelSet","co_exit","")
    end
    function self.hpGet()
        co("self.hpGet","co_enter","")
        local value = readByte(_ghost.hp)
        co("self.hpGet","co_debugoutput","value = " .. value)
        co("self.hpGet","co_exit","")
        return value
    end
    function self.hpSet(_value)
        co("self.hpSet", "co_enter", _value)
        writeBytes(_ghost.hp, _value)
        co("self.hpSet","co_exit","")
    end

    -- Functions
    function self.Speak()
        co("self.Speak", "co_enter", "I am " .. self.safeName .. ", of the " .. self.Type)
        print("#?| I am " .. self.safeName .. ", of the " .. self.Type)
        co("self.Speak", "co_exit", "")
    end

    return self;
end

function FindNPC(_by, _data)
    if _by == "Name" then
        i = 1
        while i <= NumberofGhosts do
            if NPCs[i].nameGet() == _data then
                return NPCs[i]
            else
                i = i + 1
            end
        end
        -- No NPC Found
        return nil
    else
        -- Unrecognized Find Type
        return nil
    end
end