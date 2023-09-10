use std::rc::Rc;
use std::collections::HashMap;
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
    fn to_remove(&mut self, entity_id: u64);
}

pub struct CAGame {
    pub m_systems: Vec<Rc<RefCell<dyn IGameSystem>>>,
    add_queue: LinkedList<*mut dyn IEntity>,
    remove_queue: LinkedList<u64>,
    entity_table: HashMap<u64, *mut dyn IEntity>,
    time_count: u64,
    pub ref_self: Option<*mut CAGame>,
    entity_count: u64,
}

impl CAGame {
    pub fn new(systems: Vec<Rc<RefCell<dyn IGameSystem>>>) -> CAGame {
        return CAGame {
            m_systems: systems,
            time_count: 0,
            remove_queue: LinkedList::new(),
            add_queue: LinkedList::new(),
            entity_table: HashMap::new(),
            ref_self: None,
            entity_count: 0,
        }
    }
}

impl IGame for CAGame {
    fn setup(&mut self,
        entities: Vec<*mut dyn IEntity>) {
        println!("setup");
        for entity in entities {
            self.add(entity);
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
    fn to_remove(&mut self, entity_id: u64) {
        self.remove_queue.push_back(entity_id);
    }
}

impl CAGame {
    fn add(&mut self, entity: *mut dyn IEntity) {
        if let Some(myref) = &self.ref_self {
            unsafe {
                self.entity_table.insert(self.entity_count, entity);
                (*entity).set_entity_id(self.entity_count);
                (*entity).set_game_delegate(*myref);
                self.entity_count += 1;
            }
        }
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
                self.add(entity);
            }
        }
        while !self.remove_queue.is_empty() {
            if let Some(entity) = self.remove_queue.pop_front() {
                self.remove(self.entity_table[&entity]);
            }
        }
    }
}
