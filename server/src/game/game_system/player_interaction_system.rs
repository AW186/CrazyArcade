
use crate::constant;
use std::collections::HashSet;
use crate::IEntity;
use crate::EntityTraits;
use super::IGameSystem;

pub trait IPlayer: IEntity{
    fn update(&mut self); 
}

pub struct PlayerInteractionSystem {
    players: HashSet<*mut dyn IPlayer>,
}

impl PlayerInteractionSystem {
    pub fn new() -> PlayerInteractionSystem {
        PlayerInteractionSystem {
            players: HashSet::new(),
        }
    }
}

impl IGameSystem for PlayerInteractionSystem {
    fn setup(&self) {

    }
    fn update(&mut self) {
        for player in &self.players {
            unsafe {
                (**player).update();
            }
        }
    }
    fn add(&mut self, entity: *mut dyn IEntity) {
        unsafe {
            if let EntityTraits::EPlayer(e) = (*entity).down_cast(constant::entity_traits::PLAYER) {
                self.players.insert(e);
            }
        }
    }
    fn remove(&mut self, entity: *mut dyn IEntity) {
        unsafe {
            if let EntityTraits::EPlayer(e) = (*entity).down_cast(constant::entity_traits::PLAYER) {
                self.players.remove(&e);
            }
        }
    }
}

