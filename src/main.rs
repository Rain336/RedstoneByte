#[macro_use]
extern crate lazy_static;

mod config;
mod networking;

use tokio::net::TcpListener;
use tokio::io::{self, BufStream};
use tokio::prelude::*;
use networking::Connection;

#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    let mut listener = TcpListener::bind(&config::CONFIG.address).await?;

    loop {
        let (socket, address) = listener.accept().await?;

        tokio::spawn(async move {
            let (read, write) = io::split(BufStream::new(socket));

            let conn = Connection::new(address);
            conn.run(read).await.unwrap();
        });
    }
}