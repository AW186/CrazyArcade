use crate::game::game_system::output_system::serializable::ISerializable;
use crate::constant::entity_type;
use crate::constant::entity_traits;
use crate::game::IGameDelegate;
use std::vec::Vec;

use super::EntityTraits;
use super::IEntity;

pub trait IBlock: ISerializable {
    fn get_block_type(&self) -> u8;
    fn get_x(&self) -> u8;
    fn get_y(&self) -> u8;
}

pub struct Block {
    entity_id: u64,
    type_id: u8,
    x: u8,
    y: u8,
    id: u8,
}

impl Block {
    pub fn new(type_id: u8, x: u8, y: u8) -> Block {
        return Block {
            entity_id: 0,
            type_id: type_id,
            x: x,
            y: y,
            id: 0,
        };
    }
}

impl ISerializable for Block {
    fn get_type(&self) -> u8 {
        return entity_type::BLOCK;
    }
    fn get_id(&self) -> u8 {
        return self.id;
    }
    fn set_id(&mut self, id: u8) {
        self.id = id;
    }
    fn serialize(&self) -> Vec<u8> {
        return vec![self.type_id, self.x, self.y];
    }
}

impl IBlock for Block {
    fn get_block_type(&self) -> u8 {
        return self.type_id;
    }
    fn get_x(&self) -> u8 {
        return self.x;
    }
    fn get_y(&self) -> u8 {
        return self.y;
    }
}

impl IEntity for Block {
    fn down_cast(&mut self, to: u8) -> EntityTraits {
        return match to {
            entity_traits::SERIALIZABLE_TRAIT => EntityTraits::ESerializable(self),
            entity_traits::BLOCK_TRAIT => EntityTraits::EBlock(self),
            _ => EntityTraits::Nil,
        }
    }
    fn set_game_delegate(&mut self, _: *mut dyn IGameDelegate){

    }
    fn get_entity_id(&self) -> u64 {
        self.entity_id
    }
    fn set_entity_id(&mut self, id: u64) {
        self.entity_id = id;
    }
}


