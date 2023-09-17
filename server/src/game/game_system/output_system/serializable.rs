use std::vec::Vec;
use crate::game::entity::IEntity;

pub trait ISerializable: IEntity {
    fn serialize(&self) -> Vec<u8>;
    fn get_type(&self) -> u8;
    fn get_id(&self) -> u8;
    fn set_id(&mut self, id: u8);
}