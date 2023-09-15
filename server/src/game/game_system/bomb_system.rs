
use crate::constant;
use crate::EntityTraits;
use crate::game::IGameSystem;
use crate::IEntity;
use std::collections::HashSet;

static DIR: [[i8; 2]; 4] = [
    [0, 1],
    [0, -1],
    [1, 0],
    [-1, 0]
];

pub trait IExplosionCollidable {
    fn collide(&mut self) -> bool; // return true to show that explosion could not pass through
                                   // this collision
    fn get_x(&self) -> u8;
    fn get_y(&self) -> u8;
}

pub trait IIgniteDelegate {
    fn ignite(&mut self, x: u8, y: u8, len: u8);
}

pub trait IExplodable {
    fn set_ignite_delegate(&mut self, delegate: *mut dyn IIgniteDelegate);
    fn try_explode(&mut self);
}

pub struct BombSystem {
    collidables: HashSet<*mut dyn IExplosionCollidable>,
    explodables: HashSet<*mut dyn IExplodable>,
}

impl BombSystem {
    pub fn new() -> BombSystem {
        return BombSystem {
            collidables: HashSet::new(),
            explodables: HashSet::new(),
        }
    }
    fn detect(&mut self, x: i8, y: i8, len: u8, dx: i8, dy: i8) {
        let mut i: i8 = 1;
        while i < len.try_into().unwrap() {
            if x < 0 || y < 0 {
                break;
            }
            let x: u8 = (x+dx*i).try_into().unwrap();
            let y: u8 = (y+dy*i).try_into().unwrap();
            unsafe {
                for collidable in &self.collidables {
                    println!("getting collidable");
                    if (**collidable).get_x() == x && (**collidable).get_y() == y && (**collidable).collide() {
                        break;
                    }
                }
            }
            i += 1;
        }
    }
}

impl IIgniteDelegate for BombSystem {
    fn ignite(&mut self, x: u8, y: u8, len: u8) {
        for i in 0..4 {
            self.detect(x.try_into().unwrap(), y.try_into().unwrap(), len, DIR[i][0], DIR[i][1]);
        }
        unsafe {
            for collidable in &self.collidables {
                if (**collidable).get_x() == x && (**collidable).get_y() == y && (**collidable).collide() {
                    break;
                }
            }
        }
    }
}

impl IGameSystem for BombSystem {
    fn setup(&self) {

    }
    fn update(&mut self) {
        for explodable in &self.explodables {
            unsafe {
                (**explodable).try_explode();
            }
        }
    }
    fn add(&mut self, entity: *mut dyn IEntity) {
        unsafe {
            if let EntityTraits::EExploCollidable(e) = (*entity).down_cast(constant::entity_traits::EXPLO_COLLIDE) {
                self.collidables.insert(e);
            }
            if let EntityTraits::EExplodable(e) = (*entity).down_cast(constant::entity_traits::EXPLODABLE) {
                let delegate = self as *mut dyn IIgniteDelegate;
                (*e).set_ignite_delegate(delegate);
                self.explodables.insert(e);
            }
        }
    }
    fn remove(&mut self, entity: *mut dyn IEntity) {
        unsafe {
            if let EntityTraits::EExploCollidable(e) = (*entity).down_cast(constant::entity_traits::EXPLO_COLLIDE) {
                self.collidables.remove(&e);
            }
            if let EntityTraits::EExplodable(e) = (*entity).down_cast(constant::entity_traits::EXPLODABLE) {
                self.explodables.remove(&e);
            }
        }

    }
}
