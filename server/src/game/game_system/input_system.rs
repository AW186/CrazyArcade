
use std::vec;
use std::time::SystemTime;
use std::sync::mpsc::Receiver;

use crate::game::IGameSystem;
use crate::game::entity::{IEntity, EntityTraits};
use crate::constant;

/* 
By default we only have 2 users, but say if we have 8, 
We would use three bits to store player id. We also have
4 directions and stand still, which two directions can't be at same time.
This means we would use another three bits on directions.
Finally we would use one bit to tell if player used a bomb or not.
That is, say the first three bit is used for player id. Last three used
for direction. And the fourth bit from right used for bomb, we would have
the following bit map:

|PID|PID|PID|   |B  |DIR|DIR|DIR|

(Dir stands for direction, B stands for bomb, PID stands for player id)
One empty bits may used in future or we can leave it used.
*/ 

const STAND_STILL:  u8 = 0;
const UP:           u8 = 1;
const DOWN:         u8 = 2;
const LEFT:         u8 = 3;
const RIGHT:        u8 = 4;
const BOMB_MASK:    u8 = 0b1000;


pub trait IInputListener: IEntity {
    fn up(&mut self);
    fn down(&mut self);
    fn left(&mut self);
    fn right(&mut self);
    fn bomb(&mut self);
}

pub struct InputSystem {
    inputs_time: Vec<SystemTime>,
    inputs: Vec<u8>,
    recv: Receiver<u8>,
    listeners: Vec<*mut dyn IInputListener>,
}

impl InputSystem {
    pub fn new(recver: Receiver<u8>) -> InputSystem {
        return InputSystem {
            inputs_time: vec![SystemTime::now(); 8],
            inputs: vec![0; 8],
            recv: recver,
            listeners: Vec::new(),
        };
    }
    unsafe fn update_listener(&mut self, id: usize) {
        let dir = self.inputs[id] & 0b00000111;
        match dir {
            UP => {
                (*self.listeners[id]).up();
            }
            DOWN => {
                (*self.listeners[id]).down();
            }
            LEFT => {
                (*self.listeners[id]).left();
            }
            RIGHT => {
                (*self.listeners[id]).right();
            }
            _ => {
                
            }
        }
        if (self.inputs[id] & BOMB_MASK) != 0 {
            (*(self.listeners[id])).bomb();
        }
    }
}

impl IGameSystem for InputSystem {
    fn setup(&self) {
        
    }
    fn update(&mut self) {
        loop {
            match self.recv.try_recv() {
                Err(_) => {
                    break;
                }
                Ok(value) => {
                    //println!("input system received: {}", value);
                    let id = value >> 5;
                    let id: usize = id.try_into().unwrap();
                    self.inputs[id] = value;
                    self.inputs_time[id] = SystemTime::now();
                    //println!("id: {}, val: {}", id, self.inputs[id])
                }
            }
        }
        for id in 0..self.listeners.len() {
            let id: usize = id.try_into().unwrap();
            match self.inputs_time[id].elapsed() {
                Ok(elapsed) => unsafe {
                    if elapsed.as_millis() < 500 {
                        self.update_listener(id);
                    }
                }
                Err(err) => {
                    println!("time error: {}", err);
                }
            }
        }
    }
    fn add(&mut self, entity: *mut dyn IEntity) {
        unsafe {
            if let EntityTraits::EInputListener(listener) = (*entity).down_cast(constant::entity_traits::INPUT_LISTENER) {
                self.listeners.push(listener);
            }
        }
    }
    fn remove(&mut self, _entity: *mut dyn IEntity) {
               
    }
}

