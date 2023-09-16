mod game;
mod udp_server;
mod constant;

extern crate libc; // 0.2.65
use crate::game::CAGame;
use crate::game::IGame;
use crate::game::game_system::input_system::InputSystem;
use crate::game::game_system::output_system::OutputSystem;
use crate::game::game_system::bomb_system::BombSystem;
use crate::game::game_system::player_interaction_system::PlayerInteractionSystem;
use crate::udp_server::CAUdpServer;
use crate::game::entity::*;
use core::time;
use std::sync::mpsc;
use std::cell::RefCell;
use std::rc::Rc;
use std::thread;
use std::mem;

unsafe fn cpp_new<T>(obj: T) -> *mut T {
    let res = libc::malloc(mem::size_of::<T>() as libc::size_t) as *mut T;
    *res = obj;
    res
}
fn main() {
    println!("hello");
    let (send_from_server, recv_from_server) = mpsc::channel::<u8>();
    let server: CAUdpServer = CAUdpServer::new(String::from("0.0.0.0:8080")).unwrap();
    let make_server_send = server.run(send_from_server);
    let game: *mut CAGame;
    unsafe {
        game = cpp_new(
            CAGame::new(vec![
                        Rc::new(RefCell::new(InputSystem::new(recv_from_server))),
                        Rc::new(RefCell::new(OutputSystem::new(make_server_send))),
                        Rc::new(RefCell::new(BombSystem::new())),
                        Rc::new(RefCell::new(PlayerInteractionSystem::new())),
        ]));
        (*game).ref_self = Some(game);
    }
    let mut bomb_count: u8 = 3;
    unsafe {
        (*game).setup(vec![
            cpp_new(block::Block::new(0, 0, 0)),
            cpp_new(block::Block::new(0, 1, 0)),
            cpp_new(block::Block::new(0, 2, 0)),
            cpp_new(block::Block::new(0, 3, 0)),
            cpp_new(block::Block::new(0, 4, 0)),
            cpp_new(block::Block::new(0, 5, 0)),
            cpp_new(block::Block::new(0, 6, 0)),
            cpp_new(block::Block::new(0, 7, 0)),
            cpp_new(block::Block::new(0, 8, 0)),
            cpp_new(block::Block::new(0, 9, 0)),
            cpp_new(block::Block::new(0, 10, 0)),
            cpp_new(block::Block::new(0, 11, 0)),

            cpp_new(block::Block::new(0, 11, 0)),
            cpp_new(block::Block::new(0, 11, 1)),
            cpp_new(block::Block::new(0, 11, 2)),
            cpp_new(block::Block::new(0, 11, 3)),
            cpp_new(block::Block::new(0, 11, 4)),
            cpp_new(block::Block::new(0, 11, 5)),
            cpp_new(block::Block::new(0, 11, 6)),
            cpp_new(block::Block::new(0, 11, 7)),
            cpp_new(block::Block::new(0, 11, 8)),
            cpp_new(block::Block::new(0, 11, 9)),
            cpp_new(block::Block::new(0, 11, 10)),
            cpp_new(block::Block::new(0, 11, 11)),

            cpp_new(block::Block::new(0, 0, 1)),
            cpp_new(block::Block::new(0, 0, 2)),
            cpp_new(block::Block::new(0, 0, 3)),
            cpp_new(block::Block::new(0, 0, 4)),
            cpp_new(block::Block::new(0, 0, 5)),
            cpp_new(block::Block::new(0, 0, 6)),
            cpp_new(block::Block::new(0, 0, 7)),
            cpp_new(block::Block::new(0, 0, 8)),
            cpp_new(block::Block::new(0, 0, 9)),
            cpp_new(block::Block::new(0, 0, 10)),
            cpp_new(block::Block::new(0, 0, 11)),
 
            cpp_new(block::Block::new(0, 1, 11)),
            cpp_new(block::Block::new(0, 2, 11)),
            cpp_new(block::Block::new(0, 3, 11)),
            cpp_new(block::Block::new(0, 4, 11)),
            cpp_new(block::Block::new(0, 5, 11)),
            cpp_new(block::Block::new(0, 6, 11)),
            cpp_new(block::Block::new(0, 7, 11)),
            cpp_new(block::Block::new(0, 8, 11)),
            cpp_new(block::Block::new(0, 9, 11)),
            cpp_new(block::Block::new(0, 10, 11)),

            cpp_new(player::Player::new(2, 2)),
            cpp_new(player::Player::new(5, 5)),
        ]);
    }
    
    let millis = time::Duration::from_millis(1);
    loop {
        thread::sleep(millis);
        unsafe {
            (*game).update();
        }
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
