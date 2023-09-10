pub mod block;
pub mod bomb;
use crate::game::game_system::bomb_system::*;
use crate::game::game_system::input_system::IInputListener;
use crate::game::game_system::output_system::serializable::ISerializable;
use crate::game::entity::block::IBlock;
use crate::game::IGameDelegate;

use super::game_system::bomb_system::IExplosionCollidable;

pub trait IEntity {
    fn down_cast(&mut self, to: u8) -> EntityTraits; // cast to another down stream trait
    fn set_game_delegate(&mut self, delegate: *mut dyn IGameDelegate);
    fn get_entity_id(&self) -> u64;
    fn set_entity_id(&mut self, id: u64);
}

pub enum EntityTraits {
    EInputListener(*mut dyn IInputListener),
    ESerializable(*mut dyn ISerializable),
    EBlock(*mut dyn IBlock),
    EExploCollidable(*mut dyn IExplosionCollidable),
    EExplodable(*mut dyn IExplodable),
    Nil,
}

