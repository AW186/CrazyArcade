use std::rc::Rc;
use std::cell::RefCell;
use std::collections::LinkedList;

pub mod entity;
pub mod game_system;
use crate::game::entity::IEntity;
use crate::game::game_system::IGameSystem;

pub trait IGame {    
    fn setup(&mut self, entities: Vec<*mut dyn IEntity>);
    fn update(&mut self);
}

pub trait IGameDelegate {
    fn to_add(&mut self, entity: *mut dyn IEntity);
    fn to_remove(&mut self, entity: *mut dyn IEntity);
}

pub struct CAGame {
    pub m_systems: Vec<Rc<RefCell<dyn IGameSystem>>>,
    add_queue: LinkedList<*mut dyn IEntity>,
    remove_queue: LinkedList<*mut dyn IEntity>,
    time_count: u64,
}

impl CAGame {
    pub fn new(systems: Vec<Rc<RefCell<dyn IGameSystem>>>) -> CAGame {
        return CAGame {
            m_systems: systems,
            time_count: 0,
            remove_queue: LinkedList::new(),
            add_queue: LinkedList::new(),
        }
    }
}

impl IGame for CAGame {
    fn setup(&mut self,
        entities: Vec<*mut dyn IEntity>) {
        println!("setup");
        for entity in entities {
            for sys in &self.m_systems {
                sys.borrow_mut().add(entity);
            }
        }
    }
    fn update(&mut self) {
        self.time_count += 1;
        self.update_content();
        for sys in &self.m_systems {
            sys.borrow_mut().update();
        }
    }
}

impl IGameDelegate for CAGame {
    fn to_add(&mut self, entity: *mut dyn IEntity) {
        self.add_queue.push_back(entity);
    }
    fn to_remove(&mut self, entity: *mut dyn IEntity) {
        self.remove_queue.push_back(entity);
    }
}

impl CAGame {
    fn add(&self, entity: *mut dyn IEntity) {
        for sys in &self.m_systems {
            sys.borrow_mut().add(entity);
        }
    }
    fn remove(&self, entity: *mut dyn IEntity) {
        for sys in &self.m_systems {
            sys.borrow_mut().remove(entity);
        }
    }
    pub fn update_content(&mut self) {
        while !self.add_queue.is_empty() {
            if let Some(entity) = self.add_queue.pop_front() {
                for sys in &self.m_systems {
                    sys.borrow_mut().add(entity);
                }
            }
        }
        while !self.remove_queue.is_empty() {
            if let Some(entity) = self.remove_queue.pop_front() {
                for sys in &self.m_systems {
                    sys.borrow_mut().remove(entity);
                }
            }
        }
    }
}
