
use byteorder::{BigEndian, WriteBytesExt};
use super::IEntity;
use super::bomb::Bomb;
use crate::game::game_system::player_interaction_system::IPlayer;
use crate::{constant, cpp_new};
use crate::game::IGameDelegate;
use crate::game::game_system::bomb_system::IExplosionCollidable;
use crate::game::game_system::input_system::IInputListener;
use crate::game::game_system::output_system::serializable::ISerializable;
use crate::EntityTraits;
use crate::constant::entity_traits;


// x and y is 1/1024 times in scale of blocks
pub struct Player {
    x: u32,
    y: u32,
    direction: u8,
    bomb_capacity: u8,
    blast_len: u8,
    bomb_used: u8,
    game_delegate: Option<*mut dyn IGameDelegate>,
    entity_id: u64,
    obj_id: u8,
}

impl Player {
    pub fn new(x_in: u8, y_in: u8) -> Player {
        Player {
            x: (x_in as u32) << 10,
            y: (y_in as u32) << 10,
            direction: 0,
            bomb_capacity: 2,
                blast_len: 2,
            bomb_used: 0,
            game_delegate: None,
            entity_id: 0,
            obj_id: 0,
        }
    }
}

impl IEntity for Player {
    fn down_cast(&mut self, to: u8) -> super::EntityTraits {
        match to {
            entity_traits::EXPLO_COLLIDE => return EntityTraits::EExploCollidable(self),
            entity_traits::SERIALIZABLE_TRAIT => return EntityTraits::ESerializable(self),
            entity_traits::PLAYER => return EntityTraits::EPlayer(self),
            entity_traits::INPUT_LISTENER => return EntityTraits::EInputListener(self),
            _ => return EntityTraits::Nil,
        }
    }
    fn set_game_delegate(&mut self, delegate: *mut dyn IGameDelegate) {
        self.game_delegate = Some(delegate);
    }
    fn get_entity_id(&self) -> u64 {
        self.entity_id
    }
    fn set_entity_id(&mut self, id: u64) {
        self.entity_id = id;
    }
}

impl IExplosionCollidable for Player {
    fn get_x(&self) -> u8 {
        return (self.x >> 10) as u8;
    }
    fn get_y(&self) -> u8 {
        return (self.y >> 10) as u8;
    }
    fn collide(&mut self) -> bool {
        println!("collide with player");
        if let Some(delegate) = &self.game_delegate {
            unsafe {
                (**delegate).to_remove(self.entity_id);
            }
        }
        return false;
    }
}

const SPEED: u32 = 10;
const MAX: u32 = 10;
const MIN: u32 = 1;

impl IInputListener for Player {
    fn up(&mut self) {
        self.y -= SPEED;
        if (self.y >> 10) < MIN {
            self.y = MIN << 10;
        }
        self.direction = 1;
    }
    fn down(&mut self) {
        self.y += SPEED;
        if (self.y >> 10) > MAX {
            self.y = ((MAX + 1) << 10) - 1;
        }
        self.direction = 2;
    }
    fn left(&mut self) {
        self.x -= SPEED;
        if (self.x >> 10) < MIN {
            self.x = MIN << 10;
        }
        self.direction = 3;
    }
    fn right(&mut self) {
        self.x += SPEED;
        if (self.x >> 10) > MAX {
            self.x = ((MAX + 1) << 10) - 1;
        }
        self.direction = 4;
    }
    fn bomb(&mut self) {
        if self.bomb_capacity <= self.bomb_used {
            return;
        }
        if let Some(delegate) = &self.game_delegate {
            unsafe {
                (**delegate).to_add(cpp_new(Bomb::new(self.get_x(), self.get_y(), self.blast_len, &mut self.bomb_used as *mut u8)));
                self.bomb_used += 1;
            }
        }
    }
}

impl IPlayer for Player {
    fn update(&mut self) {
    }
}

impl ISerializable for Player {
    fn get_id(&self) -> u8 {
        self.obj_id
    }
    fn set_id(&mut self, id: u8) {
        self.obj_id = id;
    }
    fn get_type(&self) -> u8 {
        constant::entity_type::PLAYER
    }
    fn serialize(&self) -> Vec<u8> {
        let mut stream = vec![];
        if let Err(err) = stream.write_u32::<BigEndian>(self.x) {
            println!("Write err: {}", err);
        }
        if let Err(err) = stream.write_u32::<BigEndian>(self.y) {
            println!("Write err: {}", err);
        }
        stream.push(self.direction);
        return stream;
    }
}








