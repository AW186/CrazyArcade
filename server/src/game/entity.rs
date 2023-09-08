pub mod block;
use crate::game::game_system::input_system::IInputListener;
use crate::game::game_system::output_system::serializable::ISerializable;
use crate::game::entity::block::IBlock;
use std::rc::Rc;
use std::cell::RefCell;
use crate::game::IGameDelegate;

pub trait IEntity {
    fn down_cast(&mut self, to: u8) -> EntityTraits; // cast to another down stream trait
    fn set_game_delegate(&mut self, delegate: Rc<RefCell<dyn IGameDelegate>>);
}

pub enum EntityTraits {
    EInputListener(*mut dyn IInputListener),
    ESerializable(*mut dyn ISerializable),
    EBlock(*mut dyn IBlock),
    Nil,
}

