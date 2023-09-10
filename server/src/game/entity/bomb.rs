
use crate::IEntity;
use crate::EntityTraits;
use crate::constant::*;
use crate::game::game_system::bomb_system::*;
use crate::game::IGameDelegate;
use crate::game::game_system::output_system::serializable::ISerializable;
use std::time::Instant;

pub struct Bomb {
    entity_id: u64,
    obj_id: u8,
    x: u8,
    y: u8,
    len: u8,
    ignite_delegate: Option<*mut dyn IIgniteDelegate>,
    game_delegate: Option<*mut dyn IGameDelegate>,
    delay: Instant,
    exploded: bool,
}

impl Bomb {
    pub fn new(in_x: u8, in_y: u8, in_len: u8) -> Bomb {
        return Bomb {
            entity_id: 0,
            obj_id: 0,
            x: in_x,
            y: in_y,
            len: in_len,
            ignite_delegate: None,
            game_delegate: None,
            delay: Instant::now(),
            exploded: false,
        };
    }
    unsafe fn explode(&mut self) {
        if self.exploded {
            return;
        }
        print!("explode\n");
        self.exploded = true;
        if let Some(delegate) = &self.ignite_delegate {
            (**delegate).ignite(self.x, self.y, self.len);
            self.delay = Instant::now();
        }
    }
}

impl ISerializable for Bomb {
    fn get_type(&self) -> u8 {
        entity_type::BOMB
    }
    fn get_id(&self) -> u8 {
        self.obj_id
    }
    fn set_id(&mut self, id: u8) {
        self.obj_id = id;
    }
    /* Encode rule:
     * 1st Byte: x
     * 2nd Byte: y
     * 3rd Byte: | exploded |           length          |
     *             1st bit  |           7 bits          |
     */
    fn serialize(&self) -> Vec<u8> {
        let explode_encode: u8 = if self.exploded { 0b10000000 } else { 0 };
        return vec![self.x, self.y, self.len | explode_encode];
    }
}

impl IEntity for Bomb {
    fn set_game_delegate(&mut self, delegate: *mut dyn IGameDelegate) {
        self.game_delegate = Some(delegate);   
    }
    fn down_cast(&mut self, to: u8) -> EntityTraits {
        match to {
            entity_traits::EXPLO_COLLIDE => return EntityTraits::EExploCollidable(self),
            entity_traits::EXPLODABLE => return EntityTraits::EExplodable(self),
            entity_traits::SERIALIZABLE_TRAIT => return EntityTraits::ESerializable(self),
            _ => return EntityTraits::Nil,
        }
    }
    fn get_entity_id(&self) -> u64 {
        self.entity_id
    }
    fn set_entity_id(&mut self, id: u64) {
        self.entity_id = id;
    }

}

impl IExplodable for Bomb {
    fn set_ignite_delegate(&mut self, delegate: *mut dyn IIgniteDelegate) {
        self.ignite_delegate = Some(delegate);
    }
    fn try_explode(&mut self) {
        if self.delay.elapsed().as_secs() >= 5 && !self.exploded {
            unsafe {
                self.explode();
            }
        }
        if self.exploded && self.delay.elapsed().as_millis() > 500 {
            if let Some(delegate) = self.game_delegate {
                unsafe {
                    println!("delete");
                    (*delegate).to_remove(self.entity_id);    
                }
            } else {
                println!("no delegate");
            }
        }
    }
}

impl IExplosionCollidable for Bomb {
    fn collide(&mut self) -> bool {
        if !self.exploded { 
            unsafe {
                self.explode();
            }
        }
        return true;
    }
    fn get_x(&self) -> u8 {
        self.x
    }
    fn get_y(&self) -> u8 {
        self.y
    }
}





