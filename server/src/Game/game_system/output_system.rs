pub mod serializable;

use crate::game::game_system::IGameSystem;
use crate::game::entity::IEntity;
use std::cell::RefCell;
use std::rc::Rc;
use std::sync::mpsc::Sender;
use byteorder::{BigEndian, WriteBytesExt};
use std::io;


pub struct OutputSystem {
    make_server_send: Sender<Vec<u8>>,
    state_id: u64,
}

impl OutputSystem {
    pub fn new(sender: Sender<Vec<u8>>) -> OutputSystem {
        return OutputSystem {
            make_server_send: sender,
            state_id: 0,
        }
    }
}

impl IGameSystem for OutputSystem {
    fn setup(&self) {

    }
    fn update(&mut self) {
        self.state_id += 1;
        let mut stream = vec![];
        stream.write_u64::<BigEndian>(self.state_id);
        println!("sending {}", self.state_id);
        self.make_server_send.send(stream);
    }

    fn add(&self, entity: Rc<RefCell<dyn IEntity>>) {

    }
}