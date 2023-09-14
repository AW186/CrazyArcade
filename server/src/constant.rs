
pub mod entity_traits {
    pub const SERIALIZABLE_TRAIT: u8    = 0;
    pub const INPUT_LISTENER: u8        = 1;
    pub const BLOCK_TRAIT: u8           = 2;
    pub const EXPLO_COLLIDE: u8         = 3;
    pub const EXPLODABLE: u8            = 4;
    pub const PLAYER: u8                = 5;
}

pub mod entity_type {
    pub const BLOCK: u8 = 0;
    pub const BOMB: u8 = 1;
    pub const PLAYER: u8 = 2;
}
