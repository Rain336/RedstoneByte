mod var_int;
mod frame;
mod compression;

pub use var_int::*;
use std::net::SocketAddr;
use tokio::prelude::*;

pub struct Connection {
    address: SocketAddr,
    compression: Option<compression::CompressionInfo>
}

impl Connection {
    pub fn new(address: SocketAddr) -> Self {
        Connection {
            address: address,
            compression: None
        }
    }

    pub async fn run(mut self, mut read: impl AsyncRead + Unpin) -> Result<(), std::io::Error> {
        loop {
            let mut packet = frame::frame_decoder(&mut read).await?;
            if let Some(c) = self.compression.as_mut() {
                packet = c.decompress_packet(packet).await?;
            }
        }
    }

    fn set_compression_threshold(&mut self, value: u32) {
        match self.compression.as_mut() {
            Some(c) => c.set_threshold(value),
            None => self.compression = Some(compression::CompressionInfo::new(value))
        }
    }
}