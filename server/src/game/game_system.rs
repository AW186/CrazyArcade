pub mod output_system;
pub mod input_system;
pub mod bomb_system;
use crate::game::entity::IEntity;

pub trait IGameSystem {
    fn setup(&self);
    fn update(&mut self);
    fn add(&mut self, entity: *mut dyn IEntity);
    fn remove(&mut self, entity: *mut dyn IEntity);
}