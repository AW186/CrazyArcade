pub mod block;
use crate::game::game_system::input_system::IInputListener;
use crate::game::game_system::output_system::serializable::ISerializable;
use crate::game::entity::block::IBlock;

pub trait IEntity {
    fn down_cast(&mut self, to: u8) -> EntityTraits; // cast to another down stream trait
}

pub enum EntityTraits {
    EInputListener(*mut dyn IInputListener),
    ESerializable(*mut dyn ISerializable),
    EBlock(*mut dyn IBlock),
    Nil,
}

