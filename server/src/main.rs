mod game;
mod udp_server;

use crate::game::CAGame;
use crate::game::IGame;
use crate::game::game_system::input_system::InputSystem;
use crate::game::game_system::output_system::OutputSystem;
use crate::udp_server::CAUdpServer;
use core::time;
use std::sync::mpsc;
use std::cell::RefCell;
use std::rc::Rc;
use std::thread;
use std::vec::Vec;

fn main() {
    println!("hello");
    let (send_from_server, recv_from_server) = mpsc::channel::<u8>();
    let server: CAUdpServer = CAUdpServer::new(String::from("127.0.0.1:34254")).unwrap();
    let make_server_send = server.run(send_from_server);
    let game: Rc<RefCell<dyn IGame>> = Rc::new(RefCell::new(CAGame::new(vec![
        Rc::new(RefCell::new(InputSystem::new(recv_from_server))),
        Rc::new(RefCell::new(OutputSystem::new(make_server_send)))
    ])));
    let millis = time::Duration::from_millis(1);
    loop {
        thread::sleep(millis);
        game.borrow_mut().update();
    }
}

/*

xxxx xxxx //type (size)
xxxx xxxx
xxxx xxxx
xxxx xxxx
xxxx xxxx

xxxx xxxx









*/