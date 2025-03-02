use std::sync::mpsc::{Receiver, channel};
use std::thread;
use std::{sync::mpsc::Sender, io};
use std::net::{UdpSocket, SocketAddr};
use std::collections::HashMap;


pub struct CAUdpServer {
    m_sock: UdpSocket,
}

impl CAUdpServer {
    pub fn new(addr: String) -> Result<CAUdpServer, io::Error> {
        let udpsock = UdpSocket::bind(addr)?;
        Ok(CAUdpServer {
            m_sock: udpsock,
        })
    }
    fn run_udp_input(&self, out: Sender<u8>, socket_out: Sender<Vec<u8>>, prep_sender: Sender<SocketAddr>) {
        let sock = match self.m_sock.try_clone() {
            Ok(udpsock) => udpsock,
            Err(err) => panic!("Problem clone socket: {:?}", err),
        };
        thread::spawn(move || {
            //preparation state
            let mut count: u8 = 0;
            let mut players: HashMap<SocketAddr, u8> = HashMap::new();
            while players.len() < 2 {
                let mut buf: [u8; 50] = [0;50];
                let (_, src_addr) = sock.recv_from(&mut buf)
    .expect("Didn't receive data");
                //println!("received data from {}", src_addr);
                if let Some(id) = players.get(&src_addr) {
                    socket_out.send(vec![*id]).unwrap();
                } else {
                    players.insert(src_addr, count);
                    socket_out.send(vec![count]).unwrap();
                    prep_sender.send(src_addr).unwrap();
                    count += 1;
                }
            }
            //game starts here
            loop {
                //println!("game start");
                let mut buf: [u8; 1] = [0;1];
                let (_, src_addr) = sock.recv_from(&mut buf)
    .expect("Didn't receive data");                
                let input = (buf[0] & 0b00011111) + (players[&src_addr] << 5);
                println!("receive from client {}, {}", src_addr, input);
                out.send(input).unwrap();
            }
        });
    }
    fn run_udp_output(&self, prep_recv: Receiver<SocketAddr>) -> Sender<Vec<u8>> {
        let sock = match self.m_sock.try_clone() {
            Ok(udpsock) => udpsock,
            Err(err) => panic!("Problem clone socket: {:?}", err),
        };
        let (sender, receiver) = channel::<Vec<u8>>();
       
        thread::spawn(move || {
            let mut playerlist: Vec<SocketAddr> = Vec::new();
            loop {
                let data = match receiver.recv() {
                    Ok(data) => data,
                    Err(err) => panic!("Problem receive: {:?}", err),
                };
                // preparation state
                if let Ok(addr) = prep_recv.try_recv() {
                    playerlist.push(addr);
                }
                if data.len() == 1 {
                    let idx: usize = data[0].into();
                    if idx >= playerlist.len() {
                        continue;
                    }
                    if let Err(err) = sock.send_to(&data[..], playerlist[idx]) {
                        println!("Sock send error: {}", err);
                    }
                    continue;
                }
                // Game state
                for player_addr in &playerlist {
                    if let Err(err) = sock.send_to(&data, player_addr) {
                        print!("Send to client Err {}", err);
                    }
                }
            }
        });
        return sender;
    }
    pub fn run(&self, out: Sender<u8>) -> Sender<Vec<u8>> {
        let (prep_sender, prep_receiver) = channel::<SocketAddr>();
        let res = self.run_udp_output(prep_receiver);
        self.run_udp_input(out, res.clone(), prep_sender);
        return res;
    }
    
}
