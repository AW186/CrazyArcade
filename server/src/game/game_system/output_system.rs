pub mod serializable;

use crate::constant::entity_traits::SERIALIZABLE_TRAIT;
use crate::game::game_system::IGameSystem;
use crate::game::game_system::output_system::serializable::ISerializable;
use crate::game::entity::{IEntity, EntityTraits};
use std::sync::mpsc::Sender;
use byteorder::{BigEndian, WriteBytesExt};
use std::vec::Vec;


pub struct OutputSystem {
    make_server_send: Sender<Vec<u8>>,
    state_id: u64,
    output_objs: Vec<Option<*mut dyn ISerializable>>,
    free_list: Vec<u8>,
    head: u8,
}

impl OutputSystem {
    pub fn new(sender: Sender<Vec<u8>>) -> OutputSystem {
        let mut res = OutputSystem {
            make_server_send: sender,
            state_id: 0,
            output_objs: vec![None; 255],
            free_list: vec![255; 256],
            head: 0,
        };
        let mut idx: u8 = 0;
        for i in &mut res.free_list {
            if idx != 255 {
                idx += 1;
            }
            *i = idx;
        }
        return res;
    }
    
}

impl IGameSystem for OutputSystem {
    fn setup(&self) {

    }
    fn update(&mut self) {
        self.state_id += 1;
        let mut stream = vec![];
        if let Err(err) = stream.write_u64::<BigEndian>(self.state_id) {
            println!("Write err: {}", err);
        }
        let mut idx: u8 = 0;
        // println!("obj len: {}", self.output_objs.len());
        for obj in &self.output_objs {
            if let Some(obj) = obj {
                stream.push(idx);   //object id
                unsafe { 
                    stream.push((**obj).get_type()); //object type
                    stream.extend((**obj).serialize()) //object data
                }; 
            }
            idx += 1;
        }
        println!("sending {} with size {}", self.state_id, stream.len());
        for b in &stream[8..] {
            print!("{}, ", b);
        }
        if let Err(err) = self.make_server_send.send(stream) {
            println!("send error: {}", err);
        }
    }

    fn add(&mut self, entity: *mut dyn IEntity) {
        if self.head == 255 {
            panic!("Memory is full");
        }
        unsafe {
            if let EntityTraits::ESerializable(entity) = (*entity).down_cast(SERIALIZABLE_TRAIT) {
                let idx: usize = self.head.into();
                (*entity).set_id(self.head);
                self.output_objs[idx] = Some(entity);
                self.head = self.free_list[idx];
            }
        }
    }

    fn remove(&mut self, entity: *mut dyn IEntity) {
        unsafe {
            if let EntityTraits::ESerializable(entity) = (*entity).down_cast(SERIALIZABLE_TRAIT) {
                print!("remove in output");
                let idx: usize = (*entity).get_id().into();
                self.output_objs[idx] = None;
                self.free_list[idx] = self.head;
                self.head = (*entity).get_id();
            }
        }
    }
}
