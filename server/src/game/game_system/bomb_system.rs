
use crate::game::IGameSystem;
use crate::IEntity;
use std::rc::Rc;
use std::cell::RefCell;
use std::vec::Vec;

static DIR: [[i8; 2]; 4] = [
    [0, 1],
    [0, -1],
    [1, 0],
    [-1, 0]
];

pub trait IExplosionCollidable {
    fn collide(&self) -> bool;
}

pub trait IIgniteDelegate {
    fn ignite(&mut self, x: u8, y: u8, len: u8);
}

pub trait IExplodable {
    fn set_ignite_delegate(&self, delegate: Rc<RefCell<dyn IIgniteDelegate>>);
    fn try_explode(&self);
}

struct BombSystem {
    grid: Vec<Vec<*mut dyn IExplosionCollidable>>,
    explodables: Vec<*mut dyn IExplodable>,
}

impl BombSystem {
    fn detect(&mut self, x: i8, y: i8, len: u8, dx: i8, dy: i8) {
        let mut i: i8 = 0;
        while i < len.try_into().unwrap() {
            let x: usize = (x+dx*i).try_into().unwrap();
            let y: usize = (y+dy*i).try_into().unwrap();
            unsafe {
                if (*self.grid[x][y]).collide() {
                    break;
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

    }
    fn remove(&mut self, entity: *mut dyn IEntity) {

    }
}